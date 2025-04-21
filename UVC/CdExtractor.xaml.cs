using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace AudioCDExtractor
{
    public partial class MainWindow : Window
    {
        private List<CDDrive> drives;
        private List<TrackInfo> tracks;

        public MainWindow()
        {
            InitializeComponent();
            drives = new List<CDDrive>();
            tracks = new List<TrackInfo>();
            LoadDrives();
        }

        private void LoadDrives()
        {
            DriveComboBox.Items.Clear();
            drives.Clear();

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom && drive.IsReady)
                {
                    drives.Add(new CDDrive { DriveLetter = drive.Name[0] });
                    DriveComboBox.Items.Add(drive.Name);
                }
            }

            if (drives.Count > 0)
                DriveComboBox.SelectedIndex = 0;
        }

        private void DriveComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DriveComboBox.SelectedIndex >= 0)
            {
                tracks.Clear();
                TrackListBox.Items.Clear();

                try
                {
                    using (var reader = new CdReader(drives[DriveComboBox.SelectedIndex].DriveLetter))
                    {
                        for (int i = 1; i <= reader.NumberOfTracks; i++)
                        {
                            var duration = reader.GetTrackLength(i);
                            tracks.Add(new TrackInfo
                            {
                                TrackNumber = i,
                                Duration = TimeSpan.FromSeconds(duration / 75.0).ToString(@"mm\:ss")
                            });
                            TrackListBox.Items.Add(tracks[tracks.Count - 1]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading CD: {ex.Message}");
                }
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = "Select Folder",
                Filter = "Folder|*.folder",
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true
            };

            if (dialog.ShowDialog() == true)
            {
                OutputPathTextBox.Text = Path.GetDirectoryName(dialog.FileName);
            }
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrackListBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a track to extract");
                return;
            }

            if (!Directory.Exists(OutputPathTextBox.Text))
            {
                MessageBox.Show("Please select a valid output directory");
                return;
            }

            var selectedTrack = (TrackInfo)TrackListBox.SelectedItem;

            try
            {
                using (var reader = new CdReader(drives[DriveComboBox.SelectedIndex].DriveLetter))
                {
                    var outputPath = Path.Combine(OutputPathTextBox.Text,
                        $"Track_{selectedTrack.TrackNumber:D2}.wav");

                    using (var waveFile = new WaveFileWriter(outputPath, new WaveFormat(44100, 16, 2)))
                    {
                        var buffer = new byte[2352 * 10];
                        long totalBytes = reader.GetTrackLength(selectedTrack.TrackNumber) * 2352;
                        long bytesRead = 0;

                        reader.Position = reader.GetTrackStartPosition(selectedTrack.TrackNumber);

                        while (bytesRead < totalBytes)
                        {
                            int bytesToRead = (int)Math.Min(buffer.Length, totalBytes - bytesRead);
                            int bytes = reader.Read(buffer, 0, bytesToRead);
                            waveFile.Write(buffer, 0, bytes);
                            bytesRead += bytes;
                        }
                    }
                }

                MessageBox.Show("Track extracted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error extracting track: {ex.Message}");
            }
        }
    }

    public class CDDrive
    {
        public char DriveLetter { get; set; }
    }

    public class TrackInfo
    {
        public int TrackNumber { get; set; }
        public string Duration { get; set; }
    }

    public class CdReader : IDisposable
    {
        private readonly IntPtr handle;
        private bool disposed;

        public CdReader(char driveLetter)
        {
            string devicePath = $@"\\.\{driveLetter}:";
            handle = NativeMethods.CreateFile(devicePath,
                FileAccess.Read,
                FileShare.Read,
                IntPtr.Zero,
                FileMode.Open,
                0,
                IntPtr.Zero);

            if (handle == IntPtr.Zero || handle.ToInt64() == -1)
                throw new IOException("Unable to open CD drive");
        }

        public int NumberOfTracks
        {
            get
            {
                // Simplified: In real implementation, use IOCTL_CDROM_READ_TOC
                return 99; // Maximum possible tracks
            }
        }

        public long GetTrackStartPosition(int track)
        {
            // Simplified: In real implementation, use CDROM_TOC
            return 0;
        }

        public int GetTrackLength(int track)
        {
            // Simplified: In real implementation, use CDROM_TOC
            return 150; // Dummy value
        }

        public long Position { get; set; }

        public int Read(byte[] buffer, int offset, int count)
        {
            // Simplified: In real implementation, use ReadFile
            return count;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (handle != IntPtr.Zero)
                    NativeMethods.CloseHandle(handle);
                disposed = true;
            }
        }
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateFile(string lpFileName, FileAccess dwDesiredAccess,
            FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
            int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr hObject);
    }
}
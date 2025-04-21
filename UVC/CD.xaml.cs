using AudioCDPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UVC
{
    /// <summary>
    /// Логика взаимодействия для CD.xaml
    /// </summary>
    public partial class CD : Window
    {
        private MciPlayer mciPlayer;
        private string currentDrive;
        private bool isPaused;
        public CD()
        {
            InitializeComponent();
            mciPlayer = new MciPlayer();
            LoadCDDrives();
        }
        private void LoadCDDrives()
        {
            DriveList.Items.Clear();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.CDRom)
                {
                    DriveList.Items.Add(drive.Name);
                }
            }
        }

        private void DriveList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DriveList.SelectedItem != null)
            {
                currentDrive = DriveList.SelectedItem.ToString();
                LoadTracks();
            }
        }

        private void LoadTracks()
        {
            TrackList.Items.Clear();
            try
            {
                string[] files = Directory.GetFiles(currentDrive, "*.cda");
                for (int i = 0; i < files.Length; i++)
                {
                    TrackList.Items.Add($"Track {i + 1}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tracks: " + ex.Message);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrackList.SelectedItem == null) return;

            try
            {
                if (isPaused)
                {
                    mciPlayer.Resume();
                    isPaused = false;
                }
                else
                {
                    int trackNumber = TrackList.SelectedIndex + 1;
                    string driveLetter = currentDrive.Substring(0, 1);
                    mciPlayer.PlayCDAudio(driveLetter, trackNumber);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing track: " + ex.Message);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mciPlayer.Pause();
            isPaused = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mciPlayer.Stop();
            isPaused = false;
        }

        private void TrackList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            StopButton_Click(null, null);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            StopButton_Click(null, null);
            base.OnClosing(e);
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}

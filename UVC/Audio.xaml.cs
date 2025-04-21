using System;
using System.Windows;
using Microsoft.Win32;
using NAudio.Wave;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;

namespace WpfApp2
{
    public class AudioTrack
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
    public partial class Audio : Window
    {
        private WaveOutEvent _waveOutEvent;
        private AudioFileReader _audioFileReader;
        private ObservableCollection<AudioTrack> _tracks;
        private string selectedTrack;

        public Audio()
        {
            InitializeComponent();
            _tracks = new ObservableCollection<AudioTrack>();
            TrackList.ItemsSource = _tracks;
        }


        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 Files|*.mp3|WAV Files|*.wav|All Files|*.*",
                Title = "Open Audio File"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(filePath);
                _tracks.Add(new AudioTrack { FileName = fileName, FilePath = filePath });
            }
        }

        private void TrackList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrackList.SelectedItem is AudioTrack track)
            {
                selectedTrack = track.FilePath;
            }
        }

        private void LoadAndPlayAudio(string filePath)
        {
            try
            {
                StopAudio();
                _audioFileReader = new AudioFileReader(filePath);
                _waveOutEvent = new WaveOutEvent();
                _waveOutEvent.Init(_audioFileReader);
                _waveOutEvent.PlaybackStopped += WaveOutEvent_PlaybackStopped;
                _waveOutEvent.Play();
            }
            catch (Exception)
            {
                // Handle exception silently
            }
        }

        private void WaveOutEvent_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Playback stopped handler
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTrack != null)
            {
                if (_waveOutEvent?.PlaybackState == PlaybackState.Paused)
                {
                    _waveOutEvent.Play();
                }
                else
                {
                    LoadAndPlayAudio(selectedTrack);
                }
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_waveOutEvent != null && _waveOutEvent.PlaybackState == PlaybackState.Playing)
            {
                _waveOutEvent.Pause();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopAudio();
        }

        private void StopAudio()
        {
            if (_waveOutEvent != null && _waveOutEvent.PlaybackState != PlaybackState.Stopped)
            {
                _waveOutEvent.Stop();
                _audioFileReader?.Dispose();
                _waveOutEvent.Dispose();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
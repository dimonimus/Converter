using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace UVC
{
    public partial class VideoPlayerWindow : Window
    {
        public VideoPlayerWindow()
        {
            InitializeComponent();
            videoPlayer.MediaEnded += VideoPlayer_MediaEnded;
            videoPlayer.MediaOpened += VideoPlayer_MediaOpened;
            CompositionTarget.Rendering += UpdateSlider;
        }

        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            timeSlider.Maximum = videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Stop();
            timeSlider.Value = 0;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите видео файл",
                Filter = "Видео файлы (*.mp4;*.avi;*.wmv;*.mkv)|*.mp4;*.avi;*.wmv;*.mkv|Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                videoPlayer.Source = new Uri(openFileDialog.FileName);
                videoPlayer.Play();
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Pause();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            videoPlayer.Play();
        }

        private void TimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)
            {
                videoPlayer.Position = TimeSpan.FromSeconds(timeSlider.Value);
            }
        }

        private void UpdateSlider(object sender, EventArgs e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)
            {
                timeSlider.Value = videoPlayer.Position.TotalSeconds;
            }
        }
        private void FullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}

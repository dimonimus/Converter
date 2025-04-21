using System.Windows;
using System.Windows.Controls;

namespace UVC
{
    public partial class BitrateInputWindow : Window
    {
        public int? Bitrate { get; private set; }
        public string SelectedFormat { get; set; } // Новый параметр для передачи формата

        public BitrateInputWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCodecOptions(); // Обновляем список кодеков при загрузке окна
        }

        private void UpdateCodecOptions()
        {
            CodecComboBox.Items.Clear();

            // Условие для доступных кодеков в зависимости от формата
            if (SelectedFormat == "mov" || SelectedFormat == "flv")
            {
                CodecComboBox.Items.Add(new ComboBoxItem { Content = "libx264" });
            }
            else
            {
                CodecComboBox.Items.Add(new ComboBoxItem { Content = "libx264" });
                CodecComboBox.Items.Add(new ComboBoxItem { Content = "libx265" });
                CodecComboBox.Items.Add(new ComboBoxItem { Content = "mpeg4" });
                CodecComboBox.Items.Add(new ComboBoxItem { Content = "copy" });
            }

            // Выбор первого элемента по умолчанию
            if (CodecComboBox.Items.Count > 0)
                CodecComboBox.SelectedIndex = 0;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(BitrateTextBox.Text, out int bitrate))
                {
                    Bitrate = bitrate;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите допустимый битрейт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedCodec = (CodecComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (string.IsNullOrEmpty(selectedCodec))
                {
                    MessageBox.Show("Пожалуйста, выберите кодек.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                this.Tag = selectedCodec;

                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void QualitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (QualityLabel != null && BitrateTextBox != null)
            {
                switch ((int)e.NewValue)
                {
                    case 1:
                        QualityLabel.Content = "360p";
                        BitrateTextBox.Text = "500";
                        break;
                    case 2:
                        QualityLabel.Content = "720p";
                        BitrateTextBox.Text = "1500";
                        break;
                    case 3:
                        QualityLabel.Content = "1080p";
                        BitrateTextBox.Text = "3000";
                        break;
                }
            }
        }
    }
}

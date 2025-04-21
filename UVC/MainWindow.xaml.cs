using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using UVC;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Net.NetworkInformation;
using NAudio.Wave;
using NAudio.Lame;
using UVC.Properties;
using WpfApp2;




namespace UltimateVideoConverter
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer autoScrollTimer;
        private bool userScrolling = false;
        private DateTime lastScrollTime;
        public MainWindow()
        {
            InitializeComponent();
        }
        private StringBuilder keySequence = new StringBuilder();

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private void FindFFmpeg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Найдите ffmpeg.exe",
                Filter = "Executable Files (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Settings.Default.FfmpegPath = openFileDialog.FileName;
                Settings.Default.Save();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void NextScreen_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayerWindow videoPlayer = new VideoPlayerWindow();
            videoPlayer.Show();
            this.Close();
        }

        private void ConvertToMp4_Click(object sender, RoutedEventArgs e)
        {
            ConvertVideo("mp4");
        }

        private void ConvertToAvi_Click(object sender, RoutedEventArgs e)
        {
            ConvertVideo("avi");
        }

        private void ConvertToWmv_Click(object sender, RoutedEventArgs e)
        {
            ConvertVideo("wmv");
        }

        private void ConvertToMkv_Click(object sender, RoutedEventArgs e)
        {
            ConvertVideo("mkv");
        }
        private void ConvertToMov_Click(object sender, RoutedEventArgs e)
        {
            ConvertVideo("mov");
        }
        private void ConvertToFlv_Click(object sender, RoutedEventArgs e)
        {
            ConvertVideo("flv");
        }
        private void ConvertWAVToMP3_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("mp3");
        }

        private void ConvertAACToMP3_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("mp3");
        }

        private void ConvertOGGToMP3_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("mp3");
        }

        private void ConvertFLACToWAV_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("wav");
        }

        private void ConvertWAVToAAC_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("aac");
        }

        private void ConvertWAVToOGG_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("ogg");
        }

        private void ConvertWAVToFLAC_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("flac");
        }

        private void ConvertAACToWAV_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("wav");
        }

        private void ConvertAACToOGG_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("ogg");
        }

        private void ConvertAACToFLAC_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("flac");
        }

        private void ConvertOGGToWAV_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("wav");
        }

        private void ConvertOGGToAAC_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("aac");
        }

        private void ConvertOGGToFLAC_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("flac");
        }

        private void ConvertFLACToAAC_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("aac");
        }

        private void ConvertFLACToOGG_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("ogg");
        }
        private void ConvertFLACToMP3_Click(object sender, RoutedEventArgs e)
        {
            ConvertAudio("mp3");
        }

        private void ConvertVideo(string format)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите видео файл",
                Filter = "Видео файлы (*.mp4;*.avi;*.wmv;*.mkv;*.mov;*.flv)|*.mp4;*.avi;*.wmv;*.mkv;*.mov;*.mov;*.flv|Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string inputFilePath = openFileDialog.FileName;

                BitrateInputWindow bitrateInputWindow = new BitrateInputWindow
                {
                    SelectedFormat = format
                };

                if (bitrateInputWindow.ShowDialog() == true && bitrateInputWindow.Bitrate.HasValue)
                {
                    int bitrate = bitrateInputWindow.Bitrate.Value;
                    string codec = bitrateInputWindow.Tag?.ToString();

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Title = "Сохранить файл",
                        Filter = $"{format.ToUpper()} Файлы|*.{format}|Все файлы|*.*"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string outputFilePath = saveFileDialog.FileName;
                        string ffmpegPath = Settings.Default.FfmpegPath ?? string.Empty;

                        if (string.IsNullOrEmpty(ffmpegPath))
                        {
                            OutputTextBox.AppendText("Пожалуйста, укажите путь к ffmpeg.exe в настройках.\n");
                            return;
                        }

                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = ffmpegPath,
                            Arguments = $"-i \"{inputFilePath}\" -c:v {codec} -b:v {bitrate}k -c:a copy \"{outputFilePath}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        try
                        {
                            Process process = new Process
                            {
                                StartInfo = processStartInfo,
                                EnableRaisingEvents = true
                            };

                            process.OutputDataReceived += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Data))
                                {
                                    Dispatcher.Invoke(() => OutputTextBox.AppendText(e.Data + "\n"));
                                }
                            };

                            process.ErrorDataReceived += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Data))
                                {
                                    Dispatcher.Invoke(() => OutputTextBox.AppendText(" " + e.Data + "\n"));
                                }
                            };

                            process.Exited += (sender, e) =>
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    if (process.ExitCode == 0)
                                    {
                                        OutputTextBox.AppendText($"Конвертация завершена! Выходной файл: {outputFilePath}\n");
                                        OutputTextBox.AppendText("Конвертация успешно завершена. Вы можете продолжить работу.\n");
                                    }
                                    else
                                    {
                                        OutputTextBox.AppendText($"Произошла ошибка при конвертации. Код выхода: {process.ExitCode}\n");
                                    }
                                });
                            };

                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                        }
                        catch (Exception ex)
                        {
                            OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
                        }
                    }
                }
            }
        }

        private void ConvertAudio(string format)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите аудио файл",
                Filter = "Аудио файлы (*.wav;*.aac;*.flac;*.ogg)|*.wav;*.aac;*.flac;*.ogg|Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string inputFilePath = openFileDialog.FileName;

                BitrateInputWindow bitrateInputWindow = new BitrateInputWindow
                {
                    SelectedFormat = format
                };

                if (bitrateInputWindow.ShowDialog() == true && bitrateInputWindow.Bitrate.HasValue)
                {
                    int bitrate = bitrateInputWindow.Bitrate.Value;
                    string codec = GetCodecForFormat(format);

                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Title = "Сохранить файл",
                        Filter = $"{format.ToUpper()} Файлы|*.{format}|Все файлы|*.*"
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string outputFilePath = saveFileDialog.FileName;
                        string ffmpegPath = Settings.Default.FfmpegPath ?? string.Empty;

                        if (string.IsNullOrEmpty(ffmpegPath))
                        {
                            OutputTextBox.AppendText("Пожалуйста, укажите путь к ffmpeg.exe в настройках.\n");
                            return;
                        }

                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = ffmpegPath,
                            Arguments = $"-i \"{inputFilePath}\" -c:a {codec} -b:a {bitrate}k \"{outputFilePath}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        try
                        {
                            Process process = new Process
                            {
                                StartInfo = processStartInfo,
                                EnableRaisingEvents = true
                            };

                            process.OutputDataReceived += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Data))
                                {
                                    Dispatcher.Invoke(() => OutputTextBox.AppendText(e.Data + "\n"));
                                }
                            };

                            process.ErrorDataReceived += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Data))
                                {
                                    Dispatcher.Invoke(() => OutputTextBox.AppendText(" " + e.Data + "\n"));
                                }
                            };

                            process.Exited += (sender, e) =>
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    if (process.ExitCode == 0)
                                    {
                                        OutputTextBox.AppendText($"Конвертация завершена! Выходной файл: {outputFilePath}\n");
                                        OutputTextBox.AppendText("Конвертация успешно завершена. Вы можете продолжить работу.\n");
                                    }
                                    else
                                    {
                                        OutputTextBox.AppendText($"Произошла ошибка при конвертации. Код выхода: {process.ExitCode}\n");
                                    }
                                });
                            };

                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                        }
                        catch (Exception ex)
                        {
                            OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
                        }
                    }
                }
            }
        }


        private string GetCodecForFormat(string format)
        {
            switch (format.ToLower())
            {
                case "mp3":
                    return "libmp3lame";
                case "aac":
                    return "aac";
                case "flac":
                    return "flac";
                case "wav":
                    return "pcm_s16le";
                case "ogg":
                    return "libvorbis";
                default:
                    throw new InvalidOperationException("Неизвестный формат: " + format);
            }
        }



        private void ConvertToNokia_Click(object sender, RoutedEventArgs e)
        {
            ConvertToNokia();
        }

        private void ConvertToNokia()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите видео файл",
                Filter = "Видео файлы (*.mp4;*.avi;*.wmv;*.mkv;*.mov;*.flv)|*.mp4;*.avi;*.wmv;*.mkv;*.mov;*.flv|Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string inputFilePath = openFileDialog.FileName;
                int bitrate = 1000;
                string codec = "libx264";

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Сохранить файл",
                    Filter = "MP4 Файлы|*.mp4|Все файлы|*.*"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string outputFilePath = saveFileDialog.FileName;

                    string ffmpegPath = Settings.Default.FfmpegPath ?? string.Empty;

                    if (string.IsNullOrEmpty(ffmpegPath))
                    {
                        OutputTextBox.AppendText("Пожалуйста, укажите путь к ffmpeg.exe в настройках.\n");
                        return;
                    }

                    ProcessStartInfo processStartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = $"-i \"{inputFilePath}\" -c:v {codec} -b:v {bitrate}k -c:a aac -strict experimental -s 854x480 \"{outputFilePath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    try
                    {
                        Process process = new Process
                        {
                            StartInfo = processStartInfo,
                            EnableRaisingEvents = true
                        };

                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                Dispatcher.Invoke(() => OutputTextBox.AppendText(e.Data + "\n"));
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                Dispatcher.Invoke(() => OutputTextBox.AppendText(" " + e.Data + "\n"));
                            }
                        };

                        process.Exited += (sender, e) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                if (process.ExitCode == 0)
                                {
                                    OutputTextBox.AppendText($"Конвертация завершена! Выходной файл: {outputFilePath}\n");
                                    OutputTextBox.AppendText("Конвертация успешно завершена. Вы можете продолжить работу.\n");
                                }
                                else
                                {
                                    OutputTextBox.AppendText($"Произошла ошибка при конвертации. Код выхода: {process.ExitCode}\n");
                                }
                            });
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                    }
                    catch (Exception ex)
                    {
                        OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
                    }
                }
            }
        }

        private void mP4ToAVIToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConvertWithFFmpeg("mp4", "avi");
        }

        private void mKVToMP4ToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConvertWithFFmpeg("mkv", "mp4");
        }

        private void mP4ToMKVToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ConvertWithFFmpeg("mp4", "mkv");
        }

        private void ConvertWithFFmpeg(string inputFormat, string outputFormat)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = $"Выберите файл {inputFormat.ToUpper()}",
                Filter = $"{inputFormat.ToUpper()} Файлы|*.{inputFormat}|Все файлы|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string inputFilePath = openFileDialog.FileName;
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = $"Сохранить файл {outputFormat.ToUpper()}",
                    Filter = $"{outputFormat.ToUpper()} Файлы|*.{outputFormat}|Все файлы|*.*"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string outputFilePath = saveFileDialog.FileName;
                    string ffmpegPath = Settings.Default.FfmpegPath ?? string.Empty;

                    if (string.IsNullOrEmpty(ffmpegPath) || !System.IO.File.Exists(ffmpegPath))
                    {
                        OutputTextBox.AppendText("Пожалуйста, укажите правильный путь к ffmpeg.exe в настройках.\n");
                        return;
                    }

                    ProcessStartInfo processStartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = $"-i \"{inputFilePath}\" -c:v copy -c:a copy \"{outputFilePath}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    try
                    {
                        using (Process process = new Process())
                        {
                            process.StartInfo = processStartInfo;
                            process.OutputDataReceived += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Data))
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        OutputTextBox.AppendText(e.Data + "\n");
                                        OutputTextBox.ScrollToEnd();
                                    });
                                }
                            };

                            process.ErrorDataReceived += (sender, e) =>
                            {
                                if (!string.IsNullOrEmpty(e.Data))
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        OutputTextBox.AppendText("Ошибка: " + e.Data + "\n");
                                        OutputTextBox.ScrollToEnd();
                                    });
                                }
                            };

                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                            process.WaitForExit();

                            if (process.ExitCode == 0)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    OutputTextBox.AppendText($"Конвертация завершена! Выходной файл: {outputFilePath}\n");
                                    OutputTextBox.AppendText("Конвертация успешно завершена. Вы можете продолжить работу.\n");
                                    OutputTextBox.ScrollToEnd();
                                });
                            }
                            else
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    OutputTextBox.AppendText($"Ошибка при конвертации. Код выхода: {process.ExitCode}\n");
                                    OutputTextBox.ScrollToEnd();
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
                    }
                }
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

        

        private async void ConvertCDAudioToMP3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives()
                    .Where(d => d.DriveType == DriveType.CDRom).ToArray();

                if (!drives.Any())
                {
                    OutputTextBox.AppendText("CD-ROM привод не найден.\n");
                    return;
                }

                var dialog = new SaveFileDialog
                {
                    Title = "Выберите папку для сохранения MP3 файлов",
                    FileName = "Выберите папку",
                    CheckPathExists = true
                };

                if (dialog.ShowDialog() != true)
                    return;

                string outputDir = Path.GetDirectoryName(dialog.FileName);

                foreach (DriveInfo drive in drives)
                {
                    if (!drive.IsReady)
                    {
                        OutputTextBox.AppendText($"Привод {drive.Name} не готов.\n");
                        continue;
                    }

                    OutputTextBox.AppendText($"Извлечение аудио с диска в приводе {drive.Name}...\n");

                    try
                    {
                        using (var reader = new NAudio.Wave.WaveInEvent())
                        {
                            reader.WaveFormat = new WaveFormat(44100, 16, 2);
                            int trackNumber = 1;

                            reader.DataAvailable += async (s, args) =>
                            {
                                string outputFile = Path.Combine(outputDir, $"track{trackNumber:D2}.mp3");

                                using (var writer = new LameMP3FileWriter(outputFile, reader.WaveFormat, LAMEPreset.STANDARD))
                                {
                                    await writer.WriteAsync(args.Buffer, 0, args.BytesRecorded);
                                }

                                await Dispatcher.InvokeAsync(() =>
                                {
                                    OutputTextBox.AppendText($"Трек {trackNumber} извлечен и сохранен как MP3.\n");
                                    OutputTextBox.ScrollToEnd();
                                });

                                trackNumber++;
                            };

                            reader.StartRecording();


                            await Task.Delay(30000);

                            reader.StopRecording();
                        }

                        OutputTextBox.AppendText($"Извлечение аудио завершено! Файлы сохранены в: {outputDir}\n");
                    }
                    catch (Exception ex)
                    {
                        OutputTextBox.AppendText($"Ошибка при извлечении аудио: {ex.Message}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
            }
        }

        private async void ExtractGifFrames_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите GIF файл",
                Filter = "GIF файлы (*.gif)|*.gif|Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Выберите папку для сохранения кадров",
                FileName = "Выберите папку",
                CheckPathExists = true
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            string inputFile = openFileDialog.FileName;
            string outputDir = Path.GetDirectoryName(saveFileDialog.FileName);
            string outputPattern = Path.Combine(outputDir, "frame_%04d.png");
            string ffmpegPath = Settings.Default.FfmpegPath;

            if (string.IsNullOrEmpty(ffmpegPath) || !File.Exists(ffmpegPath))
            {
                OutputTextBox.AppendText("Пожалуйста, укажите правильный путь к ffmpeg.exe в настройках.\n");
                return;
            }

            try
            {
                IsEnabled = false;
                OutputTextBox.AppendText("Начало извлечения кадров...\n");

                await Task.Run(() =>
                {
                    using (Process process = new Process())
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = ffmpegPath,
                            Arguments = $"-i \"{inputFile}\" -vsync 0 \"{outputPattern}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        process.StartInfo = processStartInfo;
                        process.OutputDataReceived += (s, args) =>
                        {
                            if (!string.IsNullOrEmpty(args.Data))
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    OutputTextBox.AppendText(args.Data + "\n");
                                    OutputTextBox.ScrollToEnd();
                                });
                            }
                        };

                        process.ErrorDataReceived += (s, args) =>
                        {
                            if (!string.IsNullOrEmpty(args.Data))
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    OutputTextBox.AppendText("Ошибка: " + args.Data + "\n");
                                    OutputTextBox.ScrollToEnd();
                                });
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();

                        Dispatcher.Invoke(() =>
                        {
                            if (process.ExitCode == 0)
                            {
                                OutputTextBox.AppendText($"Извлечение кадров завершено! Файлы сохранены в: {outputDir}\n");
                            }
                            else
                            {
                                OutputTextBox.AppendText($"Ошибка при извлечении кадров. Код выхода: {process.ExitCode}\n");
                            }
                            OutputTextBox.ScrollToEnd();
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
            }
            finally
            {
                IsEnabled = true;
            }
        }
        private async void ExtractVideoFrames_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите видео файл",
                Filter = "Видео файлы (*.mp4;*.avi;*.wmv;*.mkv)|*.mp4;*.avi;*.wmv;*.mkv|Все файлы (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Выберите папку для сохранения кадров",
                FileName = "Выберите папку",
                CheckPathExists = true
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            string inputFile = openFileDialog.FileName;
            string outputDir = Path.GetDirectoryName(saveFileDialog.FileName);
            string outputPattern = Path.Combine(outputDir, "frame_%04d.jpg");

            string ffmpegPath = Settings.Default.FfmpegPath;
            if (string.IsNullOrEmpty(ffmpegPath) || !File.Exists(ffmpegPath))
            {
                OutputTextBox.AppendText("Пожалуйста, укажите правильный путь к ffmpeg.exe в настройках.\n");
                return;
            }

            try
            {
                IsEnabled = false;
                OutputTextBox.AppendText("Начало извлечения кадров из видео...\n");

                await Task.Run(() =>
                {
                    using (Process process = new Process())
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = ffmpegPath,
                            Arguments = $"-i \"{inputFile}\" -vf fps=1 -frame_pts 1 \"{outputPattern}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        process.StartInfo = processStartInfo;
                        process.OutputDataReceived += (s, args) =>
                        {
                            if (!string.IsNullOrEmpty(args.Data))
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    OutputTextBox.AppendText(args.Data + "\n");
                                    OutputTextBox.ScrollToEnd();
                                });
                            }
                        };

                        process.ErrorDataReceived += (s, args) =>
                        {
                            if (!string.IsNullOrEmpty(args.Data))
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    OutputTextBox.AppendText(args.Data + "\n");
                                    OutputTextBox.ScrollToEnd();
                                });
                            }
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();

                        Dispatcher.Invoke(() =>
                        {
                            if (process.ExitCode == 0)
                            {
                                OutputTextBox.AppendText($"Извлечение кадров завершено! Файлы сохранены в: {outputDir}\n");
                            }
                            else
                            {
                                OutputTextBox.AppendText($"Ошибка при извлечении кадров. Код выхода: {process.ExitCode}\n");
                            }
                            OutputTextBox.ScrollToEnd();
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                OutputTextBox.AppendText($"Произошла ошибка: {ex.Message}\n");
            }
            finally
            {
                IsEnabled = true;
            }
        }


        private void OpenCDAudioPlayer_Click(object sender, RoutedEventArgs e)
        {
            CD cD = new CD();
            cD.ShowDialog();
        }
        private void OpenAudioPlayer_Click(object sender, RoutedEventArgs e)
        {
            Audio audio = new Audio();
            audio.ShowDialog();
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Cipher_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

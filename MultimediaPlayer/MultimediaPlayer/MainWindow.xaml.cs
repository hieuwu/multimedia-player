using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MultimediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        int _lastIndex = -1;
        bool _isPlaying = false;
        DispatcherTimer _timer;
        public MainWindow()
        {
            InitializeComponent();
            mediaPlayer.MediaEnded += _player_MediaEnded;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayList.ItemsSource = _fullPaths;

        }
        public class Song
        {
            public string SongName { get; set; }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                var filename = _fullPaths[_lastIndex].Name;
                var converter = new NameConverter();
                var shortname = converter.Convert(filename, null, null, null);
                var currentPos = mediaPlayer.Position.ToString(@"mm\:ss");
                var duration = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                Title = String.Format($"{currentPos} / {duration} - {shortname}");
            }
            else
                Title = "No file selected...";
        }

        private void BtnPlayClick(object sender, RoutedEventArgs e)
        {
            if (_fullPaths.Count <= 0)
            {
                return;
            }
            if (PlayList.SelectedIndex >= 0)
            {
                _lastIndex = PlayList.SelectedIndex;
                PlaySelectedIndex(_lastIndex);
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Hidden;
                NowPlayingInfo.Visibility = Visibility.Visible;
            }
            else
            {
                System.Windows.MessageBox.Show("No file selected!");
                return;
            }
        }

        private void PlaySelectedIndex(int i)
        {
            string filename = _fullPaths[i].FullName;
            mediaPlayer.Open(new Uri(filename));
            mediaPlayer.Play();
            _isPlaying = true;
            _timer.Start();
        }

        private void _player_MediaEnded(object sender, EventArgs e)
        {
            _lastIndex++;
            PlaySelectedIndex(_lastIndex);
        }

        private void BtnPauseClick(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
        }

        private void BtnPreClick(object sender, RoutedEventArgs e)
        {
            if (_lastIndex == 0)
            {
                return;
            }
            else
            {
                _lastIndex--;
                PlaySelectedIndex(_lastIndex);
            }
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            if (_lastIndex == _fullPaths.Count - 1)
            {
                return;
            }
            else
            {
                _lastIndex++;
                PlaySelectedIndex(_lastIndex);
            }
        }

        private void BtnShuffleEnableClick(object sender, RoutedEventArgs e)
        {
            btnShuffleEnable.Visibility = Visibility.Hidden;
            btnShuffleDisable.Visibility = Visibility.Visible;
        }

        private void BtnShuffleDisableClick(object sender, RoutedEventArgs e)
        {
            btnShuffleDisable.Visibility = Visibility.Hidden;
            btnShuffleEnable.Visibility = Visibility.Visible;
        }

        private void BtnLoopAllClick(object sender, RoutedEventArgs e)
        {
            btnLoopAll.Visibility = Visibility.Hidden;
            btnLoopOne.Visibility = Visibility.Visible;
        }

        private void BtnLoopDisableClick(object sender, RoutedEventArgs e)
        {
            btnLoopDisable.Visibility = Visibility.Hidden;
            btnLoopAll.Visibility = Visibility.Visible;
        }

        private void BtnLoopOneClick(object sender, RoutedEventArgs e)
        {
            btnLoopOne.Visibility = Visibility.Hidden;
            btnLoopDisable.Visibility = Visibility.Visible;
        }

        BindingList<FileInfo> _fullPaths = new BindingList<FileInfo>();
        private void BtnOpenAudioFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
            if (openFileDialog.ShowDialog() == true)
            {
                var info = new FileInfo(openFileDialog.FileName);
                _fullPaths.Add(info);
            }
        }

        private void BtnPlayAllClick(object sender, RoutedEventArgs e)
        {
            if (_fullPaths.Count <= 0)
            {
                return;
            }
            if (PlayList.SelectedIndex >= 0)
            {
                _lastIndex = PlayList.SelectedIndex;
                PlaySelectedIndex(_lastIndex);
            }
            else
            {
                System.Windows.MessageBox.Show("No file selected!");
                return;
            }

            _isPlaying = true;
        
            btnPlayAll.Visibility = Visibility.Hidden;
            btnStopAll.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;

        }

        private void BtnStopClick(object sender, RoutedEventArgs e)
        {
            btnStopAll.Visibility = Visibility.Hidden;
            btnPlayAll.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
            if (_isPlaying)
            {
                mediaPlayer.Stop();
            }
            else
            {
                mediaPlayer.Play();
            }
            _isPlaying = !_isPlaying;
        }

        private void btnDeleteOneSongClick(object sender, RoutedEventArgs e)
        {
            var aSong = (FileInfo)PlayList.SelectedItem;
            _fullPaths.Remove(aSong);
            MessageBox.Show("Removed ");
        }

        private void BtnSavePlaylistClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "MyPlaylist.txt";
            save.DefaultExt = "txt";
            save.FilterIndex = 2;
            save.Filter = "Text File | *.txt";

            if (save.ShowDialog() == true)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                for (int i = 0; i < _fullPaths.Count; i++)
                {
                    writer.WriteLine(_fullPaths[i].FullName);
                }
                writer.Dispose();
                writer.Close();
            }

        }

        private void BtnLoadplaylistClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string play; string[] separatingStrings = {"\n" };
            string[] words;

            if (openFileDialog.ShowDialog() == true)
            {
                play = File.ReadAllText(openFileDialog.FileName);
                words = play.Split(separatingStrings,
                                    System.StringSplitOptions.RemoveEmptyEntries);
                FileInfo fileInfo;
                for (int i = 0; i< words.Length; i++)
                {
                     fileInfo = new FileInfo(words[i].Replace("\r", ""));
                    _fullPaths.Add(fileInfo);
                }
            }

        }

        private void BtnSaveStateClick(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnLoadStateClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

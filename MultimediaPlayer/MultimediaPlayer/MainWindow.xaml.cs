using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        static BindingList<Song> _playlist = null;
        static public List<string> _ListToPlay = _ListToPlay = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            PlayList.ItemsSource = _playlist;
            mediaPlayer.MediaEnded += _player_MediaEnded;

        }
        public class Song 
        {
            public string SongName {get; set;
            
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                var filename = _ListToPlay[_lastIndex];
                string shortname = _playlist[_lastIndex].SongName;
                var currentPos = mediaPlayer.Position.ToString(@"mm\:ss");
                var duration = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                Title = String.Format($"{currentPos} / {duration} - {shortname}");
            }
            else
                Title = "No file selected...";
        }
        private void BtnPlayClick(object sender, RoutedEventArgs e)
        {
            if (_ListToPlay.Count <= 0)
            {
                return;
            }
            else
            {
                mediaPlayer.Play();
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Hidden;
                NowPlayingInfo.Visibility = Visibility.Visible;
            }
           
        }

        private void PlaySelectedIndex(int i)
        {
            string filename = _ListToPlay[i];
            mediaPlayer.Open(new Uri(filename));
            mediaPlayer.Play();
            _isPlaying = true;
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
            //mediaPlayer.Stop();
            //CURRENT_SONG_INDEX--;
            //mediaPlayer = new MediaPlayer();
            //mediaPlayer.Open(new Uri(_ListToPlay.ElementAt(CURRENT_SONG_INDEX)));
            //mediaPlayer.Play();
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            if (_lastIndex == _ListToPlay.Count - 1)
            {
                return;
            }
            else {
                _lastIndex++;
                PlaySelectedIndex(_lastIndex);
            }
            //mediaPlayer.Stop();
            //CURRENT_SONG_INDEX++;
            //mediaPlayer = new MediaPlayer();
            //mediaPlayer.Open(new Uri(_ListToPlay.ElementAt(CURRENT_SONG_INDEX)));
            //mediaPlayer.Play();
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

        private void BtnOpenAudioFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Open(new Uri(openFileDialog.FileName));
                _playlist = new BindingList<Song>();
              
                _ListToPlay.Add(openFileDialog.FileName);
                Song aSong = new Song()
                {
                    SongName = openFileDialog.SafeFileName.ToString(),
                };
                PlayList.Items.Add(aSong);
            }
        }

        private void BtnPlayAllClick(object sender, RoutedEventArgs e)
        {
            if (_ListToPlay.Count <= 0)
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

            btnPlayAll.Visibility = Visibility.Hidden;
            btnStopAll.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Hidden;
            btnPause.Visibility = Visibility.Visible;
          //  mediaPlayer.Play();
            _isPlaying = true;

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

            //myStoryboard = (Storyboard) FindResource("AnimatedRotateTransform");
            //myStoryboard.Stop();
        }

       
    }
}

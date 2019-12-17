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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            PlayList.ItemsSource = _playlist;
        }
        public  int CURRENT_SONG_INDEX = 0;
        public class Song 
        {
            public string SongName {get; set;
            
            }
        }
        static BindingList<Song> _playlist = null;
        static public List<string> _ListToPlay = _ListToPlay = new List<string>();
        private void BtnPlayClick(object sender, RoutedEventArgs e)
        {
            TotalSongNumber.Text = _ListToPlay.Count.ToString();
            SongNamePlaying.Text = _ListToPlay.ElementAt(CURRENT_SONG_INDEX);
            btnPause.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Hidden;
            NowPlayingInfo.Visibility = Visibility.Visible;
            mediaPlayer.Play();
        }

        private void BtnPauseClick(object sender, RoutedEventArgs e)
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;
            mediaPlayer.Pause();
        }

        private void BtnPreClick(object sender, RoutedEventArgs e)
        {
            if (CURRENT_SONG_INDEX == 0)
            {
                return;
            }
            mediaPlayer.Stop();
            CURRENT_SONG_INDEX--;
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(_ListToPlay.ElementAt(CURRENT_SONG_INDEX)));
            mediaPlayer.Play();
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            if (CURRENT_SONG_INDEX == _ListToPlay.Count - 1)
            {
                return;
            }
            mediaPlayer.Stop();
            CURRENT_SONG_INDEX++;
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(_ListToPlay.ElementAt(CURRENT_SONG_INDEX)));
            mediaPlayer.Play();
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
            for (int i = 0; i < _ListToPlay.Count; i++)
            {
                CURRENT_SONG_INDEX = i;
                mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(_ListToPlay[i]));
                mediaPlayer.Play();
            }

        }

        private void BtnStopClick(object sender, RoutedEventArgs e)
        {

        }
    }
}

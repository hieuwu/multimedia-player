using Gma.System.MouseKeyHook;
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
using System.Windows.Forms;

namespace MultimediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool userIsDraggingSlider = false;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        int _lastIndex = -1;
        bool _isPlaying = false;
        DispatcherTimer _timer;
        int shuffleMode = 0;
        // shuffleMode = 0 => disabled
        // shuffleMode = 1 => play random song
        int loopMode = 0;
        //loopMode = 0 => loop disabled
        //loopMode = 1 => loop all list
        //loopMode = 2 => loop one song

        //hook
        private IKeyboardMouseEvents _hook;

        public MainWindow()
        {
            InitializeComponent();
            mediaPlayer.MediaEnded += _player_MediaEnded;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;

            //dang ki su kien hook
            _hook = Hook.GlobalEvents();
            _hook.KeyUp += KeyUp_hook;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayList.ItemsSource = _fullPaths;

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            }
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                var duration = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                totalTime.Text = duration;
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
            if (_isPlaying == true)
            {
                mediaPlayer.Play();
                TotalSongNumber.Text = _fullPaths.Count.ToString();
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Hidden;
                return;
            }
            if (PlayList.SelectedIndex >= 0)
            {
                TotalSongNumber.Text = _fullPaths.Count.ToString();
                btnStopAll.Visibility = Visibility.Visible;
                _lastIndex = PlayList.SelectedIndex;
                PlaySelectedIndex(_lastIndex);
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Hidden;
                NowPlayingInfo.Visibility = Visibility.Visible;
            }
            else
            {
                TotalSongNumber.Text = _fullPaths.Count.ToString();
                btnStopAll.Visibility = Visibility.Visible;
                _lastIndex = 0;
                PlaySelectedIndex(_lastIndex);
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Hidden;
                NowPlayingInfo.Visibility = Visibility.Visible;
            }
        }

        private void PlaySelectedIndex(int i)
        {
            string filename = _fullPaths[i].FullName;
            var shortFileName = _fullPaths[i].Name;
            var converter = new NameConverter();
            var shortname = converter.Convert(shortFileName, null, null, null);
            SongNamePlaying.Text = shortname.ToString();
            mediaPlayer.Open(new Uri(filename));
            PlayList.SelectedIndex = _lastIndex;
            mediaPlayer.Play();
            _isPlaying = true;
           
            _timer.Start();
        }

        List<int> playedSong = new List<int>();
        private void _player_MediaEnded(object sender, EventArgs e)
        {
            if (loopMode == 2)
            {
                PlaySelectedIndex(_lastIndex);
            }
            else
            {
                if (shuffleMode == 0)
                {
                    if (loopMode == 0)
                    {
                        //Play Playlist one time
                        if (_lastIndex == _fullPaths.Count - 1)
                        {
                            return;
                        }
                        _lastIndex++;
                        PlaySelectedIndex(_lastIndex);
                    }
                    if (loopMode == 1)
                    {
                        //Play all list continually
                        if (_lastIndex == _fullPaths.Count - 1)
                        {
                            _lastIndex = -1;
                        }
                        _lastIndex++;
                        PlaySelectedIndex(_lastIndex);
                    }

                }
                else  
                {
                    if (loopMode == 0)
                    {
                        //Play random in list - if meet last song then stop
                        if (_lastIndex == _fullPaths.Count - 1)
                        {
                            return;
                        }
                        Random randomNumber = new Random();
                        _lastIndex = randomNumber.Next(0, _fullPaths.Count);
                        while (playedSong.Contains(_lastIndex))
                        {
                            _lastIndex = randomNumber.Next(0, _fullPaths.Count);
                        }
                        if (playedSong.Count == _fullPaths.Count)
                        {
                            playedSong.Clear();
                        }
                        playedSong.Add(_lastIndex);
                        PlaySelectedIndex(_lastIndex);
                    }
                    else if (loopMode == 1)
                    {
                        //Play random all list continually
                        Random randomNumber = new Random();
                        _lastIndex = randomNumber.Next(0, _fullPaths.Count);
                        PlaySelectedIndex(_lastIndex);
                    }
                }
            }
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
            if (_lastIndex +1 >= _fullPaths.Count )
            {
                return;
            }
            else
            {
                _timer.Stop();
                _lastIndex++;
                PlaySelectedIndex(_lastIndex);
            }
        }

        //When clicked on Shuffle enable => Switch to shuffle disabled
        private void BtnShuffleEnableClick(object sender, RoutedEventArgs e)
        {
            shuffleMode = 0;
            System.Windows.MessageBox.Show(shuffleMode+"");
            btnShuffleEnable.Visibility = Visibility.Hidden;
            btnShuffleDisable.Visibility = Visibility.Visible;
        }

        //When clicked on Shuffle disable => Switch to shuffle enabled
        private void BtnShuffleDisableClick(object sender, RoutedEventArgs e)
        {
            shuffleMode = 1;
            System.Windows.MessageBox.Show(shuffleMode + "");
            btnShuffleDisable.Visibility = Visibility.Hidden;
            btnShuffleEnable.Visibility = Visibility.Visible;
        }

        //When clicked on Loop All => Switch to Loop One feature
        private void BtnLoopAllClick(object sender, RoutedEventArgs e)
        {
            loopMode = 2;
            System.Windows.MessageBox.Show(loopMode + "");
            btnLoopAll.Visibility = Visibility.Hidden;
            btnLoopOne.Visibility = Visibility.Visible;
        }

        //When clicked on Loop Disable => Switch to Loop All List feature
        private void BtnLoopDisableClick(object sender, RoutedEventArgs e)
        {
            loopMode = 1;
            System.Windows.MessageBox.Show(loopMode + "");
            btnLoopDisable.Visibility = Visibility.Hidden;
            btnLoopAll.Visibility = Visibility.Visible;
        }

        //When clicked on Loop One => Switch to Loop Disable
        private void BtnLoopOneClick(object sender, RoutedEventArgs e)
        {
            loopMode = 0;
            System.Windows.MessageBox.Show(loopMode + "");
            btnLoopOne.Visibility = Visibility.Hidden;
            btnLoopDisable.Visibility = Visibility.Visible;
        }

        BindingList<FileInfo> _fullPaths = new BindingList<FileInfo>();
        private void BtnOpenAudioFileClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3";
            if (openFileDialog.ShowDialog() == true)
            {
                var info = new FileInfo(openFileDialog.FileName);
                _fullPaths.Add(info);
            }
        }

        private void BtnStopClick(object sender, RoutedEventArgs e)
        {
            btnStopAll.Visibility = Visibility.Hidden;
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
            if (_isPlaying == true)
            {
                System.Windows.MessageBox.Show("Can not remove while playing ");

            }
            else
            {
                var aSong = (FileInfo)PlayList.SelectedItem;
                _fullPaths.Remove(aSong);
                System.Windows.MessageBox.Show("Removed ");
            }
        }

        private void BtnSavePlaylistClick(object sender, RoutedEventArgs e)
        {
            var save = new Microsoft.Win32.SaveFileDialog();
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
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
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
            var save = new Microsoft.Win32.SaveFileDialog();
            save.FileName = "MyNowPlaying.txt";
            save.DefaultExt = "txt";
            save.FilterIndex = 2;
            save.Filter = "Text File | *.txt";

            if (save.ShowDialog() == true)
            {
                StreamWriter writer = new StreamWriter(save.OpenFile());
                writer.WriteLine(_lastIndex);
                for (int i = 0; i < _fullPaths.Count; i++)
                {
                    writer.WriteLine(_fullPaths[i].FullName);
                }
                writer.Dispose();
                writer.Close();
            }
        }

        private void BtnLoadStateClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            string play; string[] separatingStrings = { "\n" };
            string[] words;

            if (openFileDialog.ShowDialog() == true)
            {
                play = File.ReadAllText(openFileDialog.FileName);
                words = play.Split(separatingStrings,
                                    System.StringSplitOptions.RemoveEmptyEntries);
                FileInfo fileInfo;
                _lastIndex = int.Parse(words[0].Replace("\r", ""));
                for (int i = 1; i < words.Length; i++)
                {
                    fileInfo = new FileInfo(words[i].Replace("\r", ""));
                    _fullPaths.Add(fileInfo);
                }
                PlayList.SelectedIndex = _lastIndex;
            }
        }

        private void sliProgress_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"mm\:ss");
        }

        private void KeyUp_hook(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //hook key up for play song
            if (e.Control && e.Shift && (e.KeyCode == Keys.Q))
            {
                if (_fullPaths.Count <= 0)
                {
                    return;
                }
                if (_isPlaying == true)
                {
                    mediaPlayer.Play();
                    TotalSongNumber.Text = _fullPaths.Count.ToString();
                    btnPause.Visibility = Visibility.Visible;
                    btnPlay.Visibility = Visibility.Hidden;
                    return;
                }
                if (PlayList.SelectedIndex >= 0)
                {
                    TotalSongNumber.Text = _fullPaths.Count.ToString();
                    btnStopAll.Visibility = Visibility.Visible;
                    _lastIndex = PlayList.SelectedIndex;
                    PlaySelectedIndex(_lastIndex);
                    btnPause.Visibility = Visibility.Visible;
                    btnPlay.Visibility = Visibility.Hidden;
                    NowPlayingInfo.Visibility = Visibility.Visible;
                }
                else
                {
                    TotalSongNumber.Text = _fullPaths.Count.ToString();
                    btnStopAll.Visibility = Visibility.Visible;
                    _lastIndex = 0;
                    PlaySelectedIndex(_lastIndex);
                    btnPause.Visibility = Visibility.Visible;
                    btnPlay.Visibility = Visibility.Hidden;
                    NowPlayingInfo.Visibility = Visibility.Visible;

                    //System.Windows.MessageBox.Show("No file selected!");
                    //return;
                }
            }
        
            //hook key up for pause song
            if(e.Control && e.Shift && (e.KeyCode == Keys.W))
            {
                if (btnPlay.Visibility == Visibility.Hidden)
                {
                    mediaPlayer.Pause();
                    btnPlay.Visibility = Visibility.Visible;
                    btnPause.Visibility = Visibility.Hidden;
                }
            }

            //hook key up for previous song
            if (e.Control && e.Shift && (e.KeyCode == Keys.E))
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

            //hook key up for next song
            if (e.Control && e.Shift && (e.KeyCode == Keys.R))
            {
                if (_lastIndex + 1 >= _fullPaths.Count)
                {
                    return;
                }
                else
                {
                    _timer.Stop();
                    _lastIndex++;
                    PlaySelectedIndex(_lastIndex);
                }
            }


        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _hook.KeyUp -= KeyUp_hook;
            _hook.Dispose();
        }
    }
}

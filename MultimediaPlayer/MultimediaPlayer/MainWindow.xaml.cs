using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPlayClick(object sender, RoutedEventArgs e)
        {
            btnPause.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Hidden;
        }

        private void BtnPauseClick(object sender, RoutedEventArgs e)
        {
            btnPlay.Visibility = Visibility.Visible;
            btnPause.Visibility = Visibility.Hidden;

        }

        private void BtnPreClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnNextClick(object sender, RoutedEventArgs e)
        {

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
    }
}

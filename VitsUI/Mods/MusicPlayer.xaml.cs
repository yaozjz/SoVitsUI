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
using System.Windows.Threading;

namespace VitsUI.Mods
{
    /// <summary>
    /// MusicPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class MusicPlayer : UserControl
    {
        DispatcherTimer timer1;
        private void timer_tick(object sender, EventArgs e)
        {
            SliderPosition.Value = MediaPlay.Position.TotalSeconds;
            var now_time = MediaPlay.Position;
            PlayTime.Text = now_time.ToString(@"mm\:ss");
        }

        public MusicPlayer()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 播放键点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayOrStop_Music(object sender, RoutedEventArgs e)
        {
            if (PlayMusic.Content.ToString() == "播放")
            {
                PlayMusic.Content = "暂停";
                MediaPlay.Play();
            }
            else
            {
                PlayMusic.Content = "播放";
                MediaPlay.Pause();
                timer1.Stop();
            }
        }

        private void Music_Opened(object sender, RoutedEventArgs e)
        {
            timer1 = new DispatcherTimer();
            // 获取音频总长度
            SliderPosition.Maximum = MediaPlay.NaturalDuration.TimeSpan.TotalSeconds;
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += new EventHandler(timer_tick);
            timer1.Start();
        }

        private void SliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var _nowTime = TimeSpan.FromSeconds(SliderPosition.Value);
            MediaPlay.Position = _nowTime;
            PlayTime.Text = _nowTime.ToString(@"mm\:ss");
        }
    }
}

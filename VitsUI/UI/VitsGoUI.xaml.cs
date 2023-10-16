using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace VitsUI.UI
{
    /// <summary>
    /// VitsGoUI.xaml 的交互逻辑
    /// </summary>
    public partial class VitsGoUI : Page
    {
        async void ShowMsg(string msg)
        {
            SaveDone_msg.Text = msg;
            SaveDone_msg.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                Thread.Sleep(3000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SaveDone_msg.Visibility = Visibility.Collapsed;
                });
            });
        }

        public VitsGoUI()
        {
            InitializeComponent();
            init_config();
        }

        void init_config()
        {
            //主模型参数
            Model_name.Text = Properties.Settings.Default.sovitsModName;
            Config_name.Text = Properties.Settings.Default.sovitsConfig;
            //扩散模型参数
            Diff_model_name.Text = Properties.Settings.Default.sovitsDiffMods;
            Diff_Config_name.Text = Properties.Settings.Default.sovitsDiffConfig;
            IsEnableDiff.IsChecked = Properties.Settings.Default.isEnableDiff;
            DiffStep.Value = Properties.Settings.Default.Diff_step;
            //特征检索模型
            IsEnableFeature.IsChecked = Properties.Settings.Default.isEnableFeature;
            Feature_mod_name.Text = Properties.Settings.Default.Feature_model_name;
            FeatureArg.Text = Properties.Settings.Default.FeatureArg;
            //其他参数
            IsEnableAutoKey.IsChecked = Properties.Settings.Default.isEnableAutoKey;
            IsEnableNSF.IsChecked = Properties.Settings.Default.isEnableNSF;
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_config_Click(object sender, RoutedEventArgs e)
        {
            //主模型
            Properties.Settings.Default.sovitsModName = Model_name.Text.Trim();
            Properties.Settings.Default.sovitsConfig = Config_name.Text.Trim();
            //扩散模型
            Properties.Settings.Default.sovitsDiffMods = Diff_model_name.Text.Trim();
            Properties.Settings.Default.sovitsDiffConfig = Diff_Config_name.Text.Trim();
            Properties.Settings.Default.isEnableDiff = (bool)IsEnableDiff.IsChecked;
            Properties.Settings.Default.Diff_step = DiffStep.Value;
            //特征检索模型
            Properties.Settings.Default.isEnableFeature = (bool)IsEnableFeature.IsChecked;
            Properties.Settings.Default.Feature_model_name = Feature_mod_name.Text.Trim();
            Properties.Settings.Default.FeatureArg = FeatureArg.Text.Trim();
            //其他参数
            Properties.Settings.Default.isEnableNSF = (bool)IsEnableNSF.IsChecked;
            Properties.Settings.Default.isEnableAutoKey = (bool)IsEnableAutoKey.IsChecked;

            Properties.Settings.Default.Save();
            ShowMsg("配置保存成功");
        }
        /// <summary>
        /// 开始推理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunVits_Click(object sender, RoutedEventArgs e)
        {
            ShowMsg("开始推理！");
        }
        //===================音乐播放
        DispatcherTimer timer1;
        private void timer_tick(object sender, EventArgs e)
        {
            SliderPosition.Value = MusicPlayer.Position.TotalSeconds;
            var now_time = MusicPlayer.Position;
            PlayTime.Text = now_time.ToString(@"mm\:ss");
        }
        /// <summary>
        /// 浏览并加载音乐文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_music_Path(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "音频文件 (*.wav, *.flac, *.mp3)|*.wav;*.flac;*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Input_music_path.Text = openFileDialog.FileName;
                MusicPlayer.Source = new Uri(Input_music_path.Text, UriKind.Relative);
            }
        }
        /// <summary>
        /// 播放或暂停音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayOrStop_Music(object sender, RoutedEventArgs e)
        {
            if(PlayMusic.Content.ToString() == "播放")
            {
                PlayMusic.Content = "暂停";
                MusicPlayer.Play();
            }
            else
            {
                PlayMusic.Content = "播放";
                MusicPlayer.Pause();
                timer1.Stop();
            }
        }

        private void Music_Opened(object sender, RoutedEventArgs e)
        {
            timer1 = new DispatcherTimer();
            // 获取音频总长度
            SliderPosition.Maximum = MusicPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer1.Interval = TimeSpan.FromSeconds(1);
            timer1.Tick += new EventHandler(timer_tick);
            timer1.Start();
        }

        private void SliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var _nowTime = TimeSpan.FromSeconds(SliderPosition.Value);
            MusicPlayer.Position = _nowTime;
            PlayTime.Text = _nowTime.ToString(@"mm\:ss");
        }
    }
}

using Microsoft.Win32;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;
using System.Collections.Generic;

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
        void AddLogs(string msg)
        {
            OutPutLogs.AppendText(msg + "\r");
        }
        /// <summary>
        /// 查找及更新
        /// </summary>
        void Cheked_Foder(ComboBox CB, string filePath, string rule)
        {
            var files = Mods.ToolsFunc.Get_Folder(filePath, rule);
            List<string> name = new List<string>();
            if (files != null)
            {
                foreach (var file_name in files)
                {
                    name.Add(Path.GetFileName(file_name));
                }
                CB.ItemsSource = name;
                CB.SelectedIndex = 0;
            }
            else
            {
                AddLogs($"{filePath}下没有找到合适文件.");
            }
        }

        void ReadSpeeker()
        {
            //配置文件读取
            try
            {
                string de_config = Path.Combine(Properties.Settings.Default.Config_path, Config_name.SelectedItem.ToString());
                if (!File.Exists(de_config))
                {
                    AddLogs("找不到配置文件");
                    return;
                }
                string json = File.ReadAllText(de_config);
                JObject objs = JObject.Parse(json);
                var speeker_value = objs["spk"] as JObject;
                foreach (var speeker in speeker_value)
                {
                    Now_Speeker.Text = speeker.Key;
                }
            }
            catch (Exception ex)
            {
                AddLogs($"配置文件读取失败.{ex.Message}");
            }
        }

        public VitsGoUI()
        {
            InitializeComponent();
            //更新输入源列表
            Cheked_Foder(Input_music_path, Properties.Settings.Default.MusicInputPath, "*.wav");
            //刷新主模型和配置文件
            string model_path = Properties.Settings.Default.Model_path;
            string config_path = Properties.Settings.Default.Config_path;
            Cheked_Foder(Model_name, model_path, "*.pth");
            Cheked_Foder(Config_name, config_path, "*.json");
            Cheked_Foder(Diff_Config_name, config_path, "*.yaml");
            Cheked_Foder(Diff_model_name, Properties.Settings.Default.DiffPath, "*.pt");
            Cheked_Foder(Feature_mod_name, model_path, "*.pkl");
            Mods.ToolsFunc.CheckFile(OutPutLogs);
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
            //F0预测器
            F0_Index.SelectedIndex = Properties.Settings.Default.f0_predictor;
            //其他参数
            IsEnableAutoKey.IsChecked = Properties.Settings.Default.isEnableAutoKey;
            IsEnableNSF.IsChecked = Properties.Settings.Default.isEnableNSF;

            ReadSpeeker();
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
            //F0预测器
            Properties.Settings.Default.f0_predictor = F0_Index.SelectedIndex;
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
            string envs = $"{Properties.Settings.Default.Python_env_path} ";
            string inference_case;
            string config_file = Path.Combine(Properties.Settings.Default.Config_path, Config_name.Text);
            inference_case = $"inference_main.py -m {Path.Combine(Properties.Settings.Default.Model_path, Model_name.Text)} -c {config_file} -n \"{Path.GetFileNameWithoutExtension(Input_music_path.Text)}\" -t {KeyNum.Value} -s {Now_Speeker.Text}" +
                $" -f0p {F0_Index.Text} -sd {slice_db.Text}";
            if (IsEnableDiff.IsChecked == true)
                //启用浅扩散模型
                inference_case += $" -shd -dm {Path.Combine(Properties.Settings.Default.DiffPath, Diff_model_name.Text)} -ks {DiffStep.Value}";
            if (IsEnableNSF.IsChecked == true)
                inference_case += " -eh";
            if (IsEnableFeature.IsChecked == true)
                //启用特征检索模型
                inference_case += $" --feature_retrieval -cm {Path.Combine(Properties.Settings.Default.Model_path, Feature_mod_name.Text)} -cr {FeatureArg.Text}";
            if (IsEnableAutoKey.IsChecked == true)
                inference_case += " -a";
            inference_case += $" -cl {Clip.Text} -lg {linear_gradient.Text} -wf {outFomat.Text}";

            var cmdDoc = new Mods.PythonRunning() { TB_view = OutPutLogs };
            //Thread t = new Thread(cmdDoc.RunPython) { IsBackground = true };
            //t.Start(new Mods.ModelClass() { Path = Properties.Settings.Default.Python_env_path, Args = inference_case });
            cmdDoc.SendCommand(new string[] { envs + inference_case });

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
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Multiselect = true;
            //openFileDialog.Filter = "音频文件 (*.wav, *.flac, *.mp3)|*.wav;*.flac;*.mp3|All files (*.*)|*.*";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    Input_music_path.Text = openFileDialog.FileName;
            //    MusicPlayer.Source = new Uri(Input_music_path.Text, UriKind.Relative);
            //}
            Cheked_Foder(Input_music_path, Properties.Settings.Default.MusicInputPath, "*.wav");
        }
        /// <summary>
        /// 输入音频播放或暂停音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayOrStop_Music(object sender, RoutedEventArgs e)
        {
            string play_path = Path.Combine(Properties.Settings.Default.MusicInputPath, Input_music_path.Text);
            if (!File.Exists(play_path))
            {
                AddLogs($"不存在文件{play_path}.");
                return;
            }
            MusicPlayer.Source = new Uri(play_path, UriKind.Relative);
            if (PlayInput.Content.ToString() == "播放输入音频")
            {
                PlayInput.Content = "暂停";
                MusicPlayer.Play();
            }
            else
            {
                PlayInput.Content = "播放输入音频";
                MusicPlayer.Pause();
                timer1.Stop();
            }
        }

        private void Music_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                timer1 = new DispatcherTimer();
                // 获取音频总长度
                SliderPosition.Maximum = MusicPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                timer1.Interval = TimeSpan.FromSeconds(1);
                timer1.Tick += new EventHandler(timer_tick);
                timer1.Start();
            }
            catch (Exception ex)
            {
                AddLogs(ex.Message);
            }
        }

        private void SliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var _nowTime = TimeSpan.FromSeconds(SliderPosition.Value);
            MusicPlayer.Position = _nowTime;
            PlayTime.Text = _nowTime.ToString(@"mm\:ss");
        }

        private void LogsChange(object sender, TextChangedEventArgs e)
        {
            //防止内存泄漏
            if (OutPutLogs.LineCount > 3001)
            {
                OutPutLogs.Text = OutPutLogs.Text.Substring(OutPutLogs.GetLineText(0).Length + 1);
            }
            OutPutLogs.ScrollToEnd();
        }
        /// <summary>
        /// 打开输入文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnpenInput_Path(object sender, RoutedEventArgs e)
        {
            Mods.ToolsFunc.OpenFolder(Properties.Settings.Default.MusicInputPath);
        }

        private void OnpenOnput_Path(object sender, RoutedEventArgs e)
        {
            Mods.ToolsFunc.OpenFolder(Properties.Settings.Default.MusicOutputPath);
        }

        private void PlayOrStop_OutPut(object sender, RoutedEventArgs e)
        {
            string output_name = $"{Path.GetFileNameWithoutExtension(Input_music_path.Text)}_{KeyNum.Value}key_{Now_Speeker.Text}_{FeatureArg.Text}_sov";
            if (IsEnableDiff.IsChecked == true)
                output_name += "diff";
            else output_name += "its";
            output_name += $"_{F0_Index.Text}.{outFomat.Text}";
            string outputmusic_path = Path.Combine(Properties.Settings.Default.MusicOutputPath, output_name);
            try
            {
                if (!File.Exists(outputmusic_path))
                {
                    AddLogs($"不存在文件{outputmusic_path}。");
                }
                else
                {
                    MusicPlayer.Source = new Uri(outputmusic_path, UriKind.Relative);
                    if (PlayOutput.Content.ToString() == "播放输出音频")
                    {
                        PlayOutput.Content = "暂停";
                        MusicPlayer.Play();
                    }
                    else
                    {
                        PlayOutput.Content = "播放输出音频";
                        MusicPlayer.Pause();
                        timer1.Stop();
                    }
                }
            }
            catch (Exception ex) { AddLogs(ex.Message); }
        }
        /// <summary>
        /// 更换配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeConfigFile(object sender, SelectionChangedEventArgs e)
        {
            ReadSpeeker();
        }
        /// <summary>
        /// 音频文件拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileListView_DragEnter(object sender, DragEventArgs e)
        {
            // 检查拖拽的数据是否包含文件
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void fileListView_Drop(object sender, DragEventArgs e)
        {
            // 获取拖拽的文件路径
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file_path = files[0];
            //检查是否是wav格式文件
            string extension = Path.GetExtension(file_path).ToLowerInvariant();
            if (extension != ".wav")
            {
                AddLogs("你所添加的文件不是wav格式文件！");
                return;
            }
            string file_name = Path.GetFileName(file_path);
            string copy_to = Path.Combine("raw", file_name);
            if (File.Exists(copy_to))
            {
                File.Delete(copy_to);
            }
            try
            {
                File.Copy(file_path, copy_to);
                Input_music_path.Text = file_name;
            }
            catch (Exception ex) { AddLogs(ex.Message); return; }
        }

        private void ComboBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            //允许拖拽
            e.Handled = true;
        }
    }
}

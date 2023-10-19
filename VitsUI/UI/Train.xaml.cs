using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using VitsUI.Mods;

namespace VitsUI.UI
{
    /// <summary>
    /// Train.xaml 的交互逻辑
    /// </summary>
    public partial class Train : Page
    {
        string envs;

        public Train()
        {
            InitializeComponent();
            //envs = ".\\test\\python3.11\\python.exe";
            envs = Properties.Settings.Default.Python_env_path;
        }

        private void Save_config_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SampleData_Click(object sender, RoutedEventArgs e)
        {
            string[] args = new string[] { $"{envs} resample.py",
                $"{envs} preprocess_flist_config.py --speech_encoder {EncoderName.Text} --vol_aug",
                $"{envs} preprocess_hubert_f0.py --f0_predictor {F0_Index.Text} --use_diff" };
            //string[] args = new string[]
            //{
            //    envs + " -u .\\test\\test.py -g 1 -bs 6",
            //    envs + " -u .\\test\\test.py -g 1 -bs 6",
            //    envs + " -u .\\test\\test.py -g 1 -bs 6"
            //};
            var cmdDoc = new Mods.PythonRunning() { TB_view = OutPutLogs };
            cmdDoc.SendCommand(args);
        }
        /// <summary>
        /// Logs文本变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logs_Update(object sender, TextChangedEventArgs e)
        {
            //防止内存泄漏
            if (OutPutLogs.LineCount > 3001)
            {
                OutPutLogs.Text = OutPutLogs.Text.Substring(OutPutLogs.GetLineText(0).Length + 1);
            }
            OutPutLogs.ScrollToEnd();
        }
        //==============
        /// <summary>
        /// 只跑一次代码
        /// </summary>
        /// <param name="arg"></param>
        void RunOneWay(string arg)
        {
            var cmdDoc = new Mods.PythonRunning() { TB_view = OutPutLogs };
            cmdDoc.SendCommand(new string[]
            {
                envs + arg
            });
        }
        /// <summary>
        /// 训练主模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Train_Click(object sender, RoutedEventArgs e)
        {
            RunOneWay($"train.py -c {System.IO.Path.Combine(Properties.Settings.Default.Config_path, Properties.Settings.Default.sovitsConfig)} -m 44k");
        }
        /// <summary>
        /// 训练特征检索模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeatureTrain_Click(object sender, RoutedEventArgs e)
        {
            RunOneWay($"train_index.py -c {System.IO.Path.Combine(Properties.Settings.Default.Config_path, Properties.Settings.Default.sovitsConfig)}");
        }
        /// <summary>
        /// 查看训练记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewLogs_Click(object sender, RoutedEventArgs e)
        {
            RunOneWay(".\\env\\Scripts\\tensorboard.exe --logdir=.\\logs");
            var cmdDoc = new Mods.PythonRunning() { TB_view = OutPutLogs };
            cmdDoc.SendCommand(new string[]
            {
                $"train_diff.py -c {System.IO.Path.Combine(Properties.Settings.Default.Config_path, Properties.Settings.Default.DiffConfig)}"
            });
        }
        /// <summary>
        /// 训练浅扩散模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DiffTrain_Click(object sender, RoutedEventArgs e)
        {
            RunOneWay($"train_diff.py -c {System.IO.Path.Combine(Properties.Settings.Default.Config_path, Properties.Settings.Default.DiffConfig)}");
        }
    }
}

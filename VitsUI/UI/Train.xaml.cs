using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using YamlDotNet.RepresentationModel;

namespace VitsUI.UI
{
    /// <summary>
    /// Train.xaml 的交互逻辑
    /// </summary>
    public partial class Train : Page
    {
        string envs;

        private void AddLogs(string msg)
        {
            OutPutLogs.AppendText(msg + '\r');
        }

        void Init_config()
        {
            var defual_config = System.IO.Path.Combine(Properties.Settings.Default.Config_path, "config.json");
            var def_yaml = System.IO.Path.Combine(Properties.Settings.Default.Config_path, "diffusion.yaml");
            if (!File.Exists(defual_config))
            {
                AddLogs("找不到config.json文件");
                return;

            }
            if (!File.Exists(def_yaml))
            {
                AddLogs("找不到diffusion.yaml文件");
                return;
            }
            try
            {
                var config = Mods.JsonEdit.LoadJsonData(defual_config);
                batch_size.Text = config["train"]["batch_size"].ToString();
                keep_ckpts.Text = config["train"]["keep_ckpts"].ToString();

                var data = Mods.JsonEdit.LoadYaml(def_yaml);
                Diff_batch_size.Text = data.Children["train"]["batch_size"].ToString();
                interval_log.Text = data.Children["train"]["interval_log"].ToString();

            }
            catch (Exception ex)
            {
                AddLogs(ex.Message);
            }
        }

        public Train()
        {
            InitializeComponent();
            //envs = ".\\test\\python3.11\\python.exe";
            envs = Properties.Settings.Default.Python_env_path;
            Mods.ToolsFunc.CheckFile(OutPutLogs);
            Init_config();
        }
        /// <summary>
        /// 数据预处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            Init_config();
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
                $"{envs} {arg}"
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
            RunOneWay($"{System.IO.Path.Combine(System.IO.Path.GetDirectoryName(envs), "Scripts\\tensorboard.exe")} --logdir=.\\logs");
            var cmdDoc = new Mods.PythonRunning() { TB_view = OutPutLogs };
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
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            var defual_config = System.IO.Path.Combine(Properties.Settings.Default.Config_path, "config.json");
            var def_yaml = System.IO.Path.Combine(Properties.Settings.Default.Config_path, "diffusion.yaml");
            try
            {
                var config = Mods.JsonEdit.LoadJsonData(defual_config);
                config["train"]["batch_size"] = int.Parse(batch_size.Text);
                config["train"]["keep_ckpts"] = int.Parse(keep_ckpts.Text);
                Mods.JsonEdit.WirteJson(defual_config, config);
                //yaml修改
                var data = Mods.JsonEdit.LoadYaml(def_yaml);
                var arg_batch_size = data.Children["train"]["batch_size"];
                var arg_interval_log = data.Children["train"]["interval_log"];
                if (arg_batch_size is YamlScalarNode bs)
                    // 修改标量节点的值
                    bs.Value = Diff_batch_size.Text;
                if(arg_interval_log is YamlScalarNode il)
                    il.Value = interval_log.Text;
                Mods.JsonEdit.WriteYaml(def_yaml, data);
                AddLogs("保存成功");
            }
            catch (Exception ex)
            {
                AddLogs(ex.Message);
            }
        }
    }
}

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

namespace VitsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //读取上一次窗口状态
            Width = Properties.Settings.Default.WinWitdh;
            Height = Properties.Settings.Default.WinHeight;

            //初始化窗口页面
            MainContent.Content = new UI.VitsGoUI();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UI.SettingUI();
        }
        /// <summary>
        /// 推理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VitsGo_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UI.VitsGoUI();
        }
        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.WinWitdh = Width;
            Properties.Settings.Default.WinHeight = Height;

            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// 训练
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainVits_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

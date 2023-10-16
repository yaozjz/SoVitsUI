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

namespace VitsUI.UI
{
    /// <summary>
    /// SettingUI.xaml 的交互逻辑
    /// </summary>
    public partial class SettingUI : Page
    {
        public SettingUI()
        {
            InitializeComponent();
            Python_path.Text = Properties.Settings.Default.Python_env_path;
            Model_path.Text = Properties.Settings.Default.Model_path;
            Config_path.Text = Properties.Settings.Default.Config_path;
        }

        private void View_Path(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Python解释器 (*.exe)|*.exe|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Python_path.Text = openFileDialog.FileName;
            }
        }

        private async void Save_config_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Python_env_path = Python_path.Text;
            Properties.Settings.Default.Config_path = Config_path.Text;
            Properties.Settings.Default.Model_path = Model_path.Text;

            Properties.Settings.Default.Save();
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
    }
}

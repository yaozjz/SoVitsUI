using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VitsUI.Mods
{
    class ToolsFunc
    {

        static public void OpenFolder(string folderPath)
        {
            Process.Start("explorer.exe", Path.GetFullPath(folderPath));
        }
        static public IEnumerable<string>? Get_Folder(string FilePath, string rule)
        {
            if (Directory.Exists(FilePath))
            {

                var imageFile = Directory.GetFiles(FilePath, rule);
                return imageFile;
            }
            else
            {
                //MessageBox.Show("文件夹不存在!");
                return null;
            }
        }
        static public void CheckFile(TextBox textBox)
        {
            string[] files = new string[]
            {
                "pretrain\\checkpoint_best_legacy_500.pt",
                "pretrain\\hubert-soft-0d54a1f4.pt",
                "pretrain\\rmvpe.pt",
                "pretrain\\nsf_hifigan\\model",
                "pretrain\\nsf_hifigan\\config.json"
            };
            foreach (string i in files)
            {
                if (!File.Exists(i))
                    textBox.AppendText($"找不到文件{i}{Environment.NewLine}");
            }
        }
    }
}

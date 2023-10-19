using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

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

                var imageFile = Directory.GetFiles(FilePath,rule);
                return imageFile;
            }
            else
            {
                //MessageBox.Show("文件夹不存在!");
                return null;
            }
        }
    }
}

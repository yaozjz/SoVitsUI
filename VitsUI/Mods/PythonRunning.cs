using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Shapes;

namespace VitsUI.Mods
{
    class PythonRunning
    {
        public TextBox TB_view { get; set; }
        Process process;
        /// <summary>
        /// 单次执行
        /// </summary>
        /// <param name="arr"></param>
        public void RunPython(object arr)
        {
            ModelClass msg = arr as ModelClass;
            process = new Process();
            try
            {
                process.StartInfo.FileName = msg.Path;
                process.StartInfo.Arguments = msg.Args;
                Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText($"{msg.Path} {msg.Args}\r"); });
                process.Start();

                process.WaitForExit();  //等待程序执行完退出进程
                process.Close();
                Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText($"程序运行完毕.\r"); });
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText(e.Message + "\r"); });
            }
        }
        private void ProcessOutputHandler(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText(e.Data + '\r'); });
            process.StandardInput.AutoFlush = true;
        }

        /// <summary>
        /// 依次执行多个程序
        /// </summary>
        /// <param name="arr"></param>
        public void RunPythonInit(object arr)
        {
            //List<ModelClass> args = arr as List<ModelClass>;
            //TB_view = args[0].TB;
            string[] args = arr as string[];
            process = new();
            try
            {
                process.StartInfo.RedirectStandardInput = true;  // 接受来自调用程序的输入信息
                process.StartInfo.FileName = "cmd.exe";
                process.Start();
                foreach (var arg in args)
                {
                    process.StandardInput.WriteLine(arg); //向cmd窗口发送输入信息
                }
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText(e.Message + "\r"); });
            }

            Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText("运行结束.\r"); });
        }

        //指令发送函数，tclCommand为需要执行的cmd指令
        public void SendCommand(string[] tclCommand)
        {
            Thread t = new Thread(RunPythonInit) { IsBackground = true };
            t.Start(tclCommand);
        }

        public void Close()
        {
            //process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
        }

    }
}

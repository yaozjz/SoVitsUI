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
using System.Windows.Interop;

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
            TB_view = msg.TB;
            process = new Process();
            try
            {
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.UseShellExecute = false;
                //process.StartInfo.CreateNoWindow = true;
                //process.StartInfo.RedirectStandardInput = true;
                //process.EnableRaisingEvents = true;
                process.StartInfo.FileName = msg.Path;
                process.StartInfo.Arguments = msg.Args;

                process.Start();
                //process.StandardInput.AutoFlush = true;
                //while (!process.StandardOutput.EndOfStream)
                //{
                //    string line = process.StandardOutput.ReadLine();
                //    Application.Current.Dispatcher.Invoke(delegate { msg.TB.AppendText($"{line}\r"); });
                //    // do something with line
                //}

                process.WaitForExit();  //等待程序执行完退出进程
                process.Close();
                Application.Current.Dispatcher.Invoke(delegate { msg.TB.AppendText($"{msg.Args}运行完毕.\r"); });
            }
            catch (Exception e)
            {
                Application.Current.Dispatcher.Invoke(delegate { msg.TB.AppendText(e.Message + "\r"); });
            }
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
                    Application.Current.Dispatcher.Invoke(delegate { TB_view.AppendText($"{arg}运行完毕.\r"); });
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
            process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
        }
    }
}

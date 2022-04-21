using RootNS.View;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RootNS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            RunningCheck();
        }
        private void RunningCheck()
        {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                MessageBoxResult dr = MessageBox.Show("程序运行中，是否强制重新运行？\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (dr == MessageBoxResult.OK)
                {
                    foreach (Process p in Process.GetProcessesByName(thisProc.ProcessName))
                    {
                        if (p.Id != thisProc.Id)
                        {
                            //强制Kill其他同名进程
                            p.Kill();
                        }
                    }
                }
                if (dr == MessageBoxResult.Cancel)
                {
                    //本程序结束，不影响其他进程
                    Application.Current.Shutdown();
                }
            }
        }
    }
}

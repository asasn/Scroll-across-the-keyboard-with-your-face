using RootNS.Helper;
using RootNS.Model;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace RootNS.View
{
    /// <summary>
    /// InfoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VersionWindow : Window
    {
        public VersionWindow()
        {
            InitializeComponent();

            if (Gval.CurrentVersion == Gval.LatestVersion)
            {
                labelTip.Content = "已是最新版本";
                BtnCheck.Visibility = Visibility.Hidden;
            }
            else if (Gval.LatestVersion != "网络错误！" && Gval.LatestVersion != "未检查")
            {
                labelTip.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                labelTip.Content = "有新版本，请打开GitHub仓库以更新";
                BtnCheck.Visibility = Visibility.Hidden;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            BtnCheck.Visibility = Visibility.Hidden;
            StreamReader reader = WebHelper.GetHtmlReaderObject("https://api.github.com/repos/asasn/Scroll-across-the-keyboard-with-your-face/releases/latest");
            if (reader == null)
            {
                Gval.LatestVersion = "网络错误！";
                labelTip.Content = "网络错误，请稍等片刻之后再次尝试！";
                (sender as Button).IsEnabled = true;
                BtnCheck.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                string text = reader.ReadToEnd();
                Dictionary<string, object> latestInfo = JsonHelper.JsonToObject<Dictionary<string, object>>(text);
                string versionName = latestInfo["name"].ToString();
                Match match = Regex.Match(text, "\\d+\\.\\d+\\.\\d+\\.\\d+");
                if (match.Success)
                {
                    Gval.LatestVersion = System.Web.HttpUtility.UrlDecode(match.Value);
                }
            }
            if (Gval.CurrentVersion == Gval.LatestVersion)
            {
                labelTip.Content = "已是最新版本";
            }
            else if (Gval.LatestVersion != "网络错误！" && Gval.LatestVersion != "未检查")
            {
                labelTip.Foreground = new SolidColorBrush(Colors.DodgerBlue);
                labelTip.Content = "有新版本，请打开GitHub仓库以更新";
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }
    }
}

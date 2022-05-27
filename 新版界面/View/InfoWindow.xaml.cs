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
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        class LatestInfo
        {
            public string html_url { get; set; }
            public string tag_name { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = WebHelper.GetHtmlText("https://github.com/asasn/Scroll-across-the-keyboard-with-your-face");
            if (string.IsNullOrEmpty(text))
            {
                Gval.LatestVersion = "Error";
                return;
            }
            Match match = Regex.Match(text, "(?<=releases/tag/)([\\s\\S]+?)(?=\">)");
            if (match.Success)
            {
                Gval.LatestVersion = System.Web.HttpUtility.UrlDecode(match.Value);
            }
            //LatestInfo latestInfo = JsonHelper.JsonToObject<LatestInfo>(text);
            //Gval.LatestVersion = latestInfo.tag_name;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(((Hyperlink)sender).NavigateUri.ToString());
        }
    }
}

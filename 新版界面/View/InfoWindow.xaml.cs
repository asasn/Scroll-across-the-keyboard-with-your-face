using RootNS.Helper;
using RootNS.Model;
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
using System.Windows.Shapes;

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
            string text = WebHelper.GetHtmlText("https://api.github.com/repos/asasn/Scroll-across-the-keyboard-with-your-face/releases/latest");
            LatestInfo latestInfo = JsonHelper.JsonToObject<LatestInfo>(text);
            Gval.CurrentVersion = latestInfo.tag_name;
        }
    }
}

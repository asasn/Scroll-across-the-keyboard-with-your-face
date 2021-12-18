using System;
using System.Collections.Generic;
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
using 脸滚键盘.公共操作类;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// HansWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HansToolWindow : Window
    {
        public HansToolWindow()
        {
            InitializeComponent();
        }

        private void BtnGet_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbHans.Text))
            {
                return;
            }
            string ttt = " ";
            string urlStr = "https://www.zdic.net/hans/" + TbHans.Text;
            string htmlText = WebOperate.GetHtmlText(urlStr);
            //string pattern = "<div class=\"zdict\">([\\s\\S]+)(?=<div class=\"zdict\">)";
            string pattern = "<main>([\\s\\S]+)</main>";
            Match matchRet = Regex.Match(htmlText, pattern, RegexOptions.Multiline);
            string pattern2 = ".*<script([\\s\\S]+?)</script>.*";
            ttt = Regex.Replace(matchRet.Value, pattern2, "", RegexOptions.Multiline);
            ttt = Regex.Replace(ttt, "<h2><span class=\"z_ts2\">条目</span> <strong>(.+?)</strong>([\\s\\S]+?)</h2>", "", RegexOptions.Multiline);
            ttt = Regex.Replace(ttt, "<div id='gg_([\\s\\S]+?)</div>", "", RegexOptions.Multiline);
            ttt = Regex.Replace(ttt, "<div class=\"nr-box nr-box-shiyi wytl\"([\\s\\S]+?)<div class=\"res_c_right\">", "", RegexOptions.Multiline);
            ttt = Regex.Replace(ttt, "<div class=\"div copyright\">(.+?)</div>", "</br><hr/></br>", RegexOptions.Multiline);
            Console.WriteLine(ttt);
            //MatchCollection matchRets = Regex.Matches(matchRet.Value, pattern2, RegexOptions.Multiline);
            //
            //if (matchRets.Count > 0)
            //{
            //    foreach (Match item in matchRets)
            //    {                     
            //        ttt += item.Value + "\n";
            //    }
            //}
            if (string.IsNullOrWhiteSpace(ttt))
            {
                return;
            }
            webBrowser.NavigateToString(WebOperate.ConvertExtendedASCII(ttt));
        }

        private void TbHans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnGet.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}

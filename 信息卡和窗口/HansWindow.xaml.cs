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
    public partial class HansWindow : Window
    {
        public HansWindow()
        {
            InitializeComponent();
        }

        private void BtnGet_Click(object sender, RoutedEventArgs e)
        {
            string ttt = " ";
            string urlStr = "https://www.zdic.net/hans/" + TbHans.Text;
            string htmlText = WebOperate.GetHtmlText(urlStr);
            string pattern = "<div class=\"dictlink\">([\\s\\S]+)<div class=\"div copyright\"> (.+?) </div>";
            Match matchRet = Regex.Match(htmlText, pattern, RegexOptions.Multiline);
            string pattern2 = "<script ([\\s\\S]+?)</script>";
            ttt = Regex.Replace(matchRet.Value, pattern2, "", RegexOptions.Multiline);

            //MatchCollection matchRets = Regex.Matches(matchRet.Value, pattern2, RegexOptions.Multiline);
            //
            //if (matchRets.Count > 0)
            //{
            //    foreach (Match item in matchRets)
            //    {                     
            //        ttt += item.Value + "\n";
            //    }
            //}
            Console.WriteLine(ttt);
            webBrowser.NavigateToString(WebOperate.ConvertExtendedASCII(ttt));
        }
    }
}

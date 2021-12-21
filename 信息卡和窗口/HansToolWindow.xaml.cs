using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 脸滚键盘.公共操作类;
using System.Windows.Navigation;
using System.Reflection;
using System.Windows.Controls;
using mshtml;

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
            string urlZdic = "https://www.zdic.net/hans/" + TbHans.Text;
            string urlBaidu = "https://baike.baidu.com/item/" + TbHans.Text + "?force=1";
            NavigateZdicToString(urlZdic);
        }

        void Navigate(string urlStr)
        {
            webBrowser.Navigate(new Uri(urlStr)); ;
        }

        void NavigateBaikeToString(string urlStr)
        {
            string htmlText = WebOperate.GetHtmlText(urlStr);
            MatchCollection matches = Regex.Matches(htmlText, "<div class=\"content\">([\\s\\S]+?)(?=<div class=\"side-content\">)");
            string match1 = string.Empty;
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    match1 += match.Value + "</div></div></div></div></div></div></div></div></div></div>";
                }
                match1 = Regex.Replace(match1, "<div class=\"nr-box nr-box-shiyi wytl\"([\\s\\S]+)</div>", "", RegexOptions.Multiline);
            }
            htmlText = Regex.Replace(htmlText, "<body([\\s\\S]+?)</body>", "<body>" + match1 + "</body>", RegexOptions.Multiline);
            htmlText = Regex.Replace(htmlText, "href=\"/item/", "href=\"https://baike.baidu.com/item/", RegexOptions.Multiline);
            NavigateToString(htmlText);
        }

        void NavigateZdicToString(string urlStr) 
        {
            string htmlText = WebOperate.GetHtmlText(urlStr);

            //htmlText = Regex.Replace(htmlText, "href=\"/style.css\"", "href=\"https://www.zdic.net/style.css\"", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "href=\"/zi.css?v=1.3\"", "href=\"https://www.zdic.net/zi.css?v=1.3\"", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<script([\\s\\S]+?)</script>", "", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<footer([\\s\\S]+?)</footer>", "", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<header([\\s\\S]+?)</header>", "", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<nav([\\s\\S]+?)</nav>", "", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<div class='res_c_left res_s res_t'>([\\s\\S]+?)(?=<div class=\"zdict\">)", "", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<ins([\\s\\S]+?)</ins>", "", RegexOptions.Multiline);
            //htmlText = Regex.Replace(htmlText, "<div class=\"topslot_container\">([\\s\\S]+?)</div>", "", RegexOptions.Multiline);
            MatchCollection matches = Regex.Matches(htmlText, "<div class=\"nr-box nr-box-shiyi([\\s\\S]+?)(?=<div class=\"res_c_right\">)");
            string match1 = string.Empty;
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    match1 += match.Value + "</div></div></div></div></div></div></div></div></div></div>";
                }
                match1 = Regex.Replace(match1, "<div class=\"nr-box nr-box-shiyi wytl\"([\\s\\S]+)</div>", "", RegexOptions.Multiline);
                match1 = Regex.Replace(match1, "<div class=\"zi-b-container zib-title\">([\\s\\S]+?)</div>", "", RegexOptions.Multiline);
                match1 = Regex.Replace(match1, "<div class=\"zi-b-container res_hos res_hot res_hod zib-title\">([\\s\\S]+?)</div>", "", RegexOptions.Multiline);
                match1 = Regex.Replace(match1, "<ins([\\s\\S]+?)</ins>", "", RegexOptions.Multiline);
                match1 = Regex.Replace(match1, "<div class=\"h_line(.+?)</div>", "<hr/><br/>", RegexOptions.Multiline);
            }
            //Match match2 = Regex.Match(htmlText, "<div class=\"res_c_right\">([\\s\\S]+?)(?=</main>)");
            htmlText = Regex.Replace(htmlText, "<head([\\s\\S]+?)</head>", "<head><meta charset=\"utf-8\"><link type=\"text/css\" rel=\"stylesheet\" media=\"screen\" href=\"https://www.zdic.net/style.css\" /></head>", RegexOptions.Multiline);
            htmlText = Regex.Replace(htmlText, "<body([\\s\\S]+?)</body>", "<body>" + match1 + "</body>", RegexOptions.Multiline);
            htmlText = Regex.Replace(htmlText, "=\"//img.zdic.net/", "=\"https://img.zdic.net/", RegexOptions.Multiline);
            htmlText = Regex.Replace(htmlText, "='//img.zdic.net/", "='https://img.zdic.net/", RegexOptions.Multiline);

            NavigateToString(htmlText);
            
        }

        void NavigateToString(string htmlText)
        {
            if (string.IsNullOrWhiteSpace(htmlText))
            {
                webBrowser.Navigate("about:blank");
            }

            webBrowser.NavigateToString(htmlText);
        }


        #region 浏览器加载过程中：屏蔽弹出脚本错误窗口
        public void SuppressScriptErrors(WebBrowser webBrowser, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }
        void webBrowser1_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SuppressScriptErrors(webBrowser, true);
        }

        /// <summary>
        /// 在浏览器加载时绑定扩展方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigating += webBrowser1_Navigating;
        }

        //导航中
        private void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        //导航完成
        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
        }
        #endregion


        private void TbHans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnGet.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }


    }
}

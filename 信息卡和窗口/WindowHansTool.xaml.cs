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
    public partial class WindowHansTool : Window
    {
        public WindowHansTool()
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
            NavigateZdicToString(urlZdic);
            //string urlBaidu = "https://baike.baidu.com/item/" + TbHans.Text;
            //NavigateBaikeToString(urlBaidu);
        }

        void Navigate(string urlStr)
        {
            UiWebBrowser.Navigate(new Uri(urlStr)); ;
        }

        string GetBaikePage(string htmltext)
        {
            string html2 = string.Empty;
            htmltext = Regex.Replace(htmltext, "href=\"/item/", "href=\"https://baike.baidu.com/item/", RegexOptions.Multiline);
            MatchCollection matches2 = Regex.Matches(htmltext, "(?<=<a target=_blank href=\")(.+?)(?=\" data-lemmaid)");
            string matchTemp = string.Empty;
            if (matches2.Count > 0)
            {
                foreach (Match match in matches2)
                {
                    matchTemp += WebOperate.GetHtmlText(match.Value);
                }
                html2 = Regex.Replace(matchTemp, "<dl([\\s\\S]+?)</dl>", "", RegexOptions.Multiline);
            }
            return html2;
        }

        string PickUpBaikeContent(string htmlText, bool tag = false)
        {
            string matchTemp = string.Empty;
            MatchCollection matches = Regex.Matches(htmlText, "<div class=\"main-content J-content\">([\\s\\S]+?)(?=<div id=\"J-main-content-end-dom\">)");
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    if (tag == true)
                    {
                        matchTemp += "<div style=\"border: 3px double #000000;padding:3px\">" + match.Value + "</div></div></div></div></div></div></div>" + "<br/>";
                    }
                    else
                    {
                        matchTemp += match.Value + "</div></div></div></div></div></div></div>" + "<br/>";
                    }
                }
                matchTemp = Regex.Replace(matchTemp, "<dl([\\s\\S]+?)</dl>", "", RegexOptions.Multiline);
                matchTemp = Regex.Replace(matchTemp, "<div class=\"top-tool([\\s\\S]+?)(?=<div style)", "", RegexOptions.Multiline);
                matchTemp = Regex.Replace(matchTemp, "<a class=\"audio-play part-audio-play([\\s\\S]+?)(?=</a>)", "", RegexOptions.Multiline);
                return matchTemp;
            }
            else
            {
                return string.Empty;
            }
        }

        void NavigateBaikeToString(string urlStr)
        {
            string html1 = WebOperate.GetHtmlText(urlStr);
            string html2;
            string htmlText = string.Empty;
            if (html1.Contains("lemmaWgt-subLemmaListTitle"))
            {
                //html2 = GetBaikePage(html1);
                //htmlText += "<div style=\"background-color: #ddffff!important;padding: 14px;border-left: 6px solid #ccc!important;border-color: #2196F3!important;\">" + PickUpBaikeContent(html1) + "</div>";
                //htmlText += PickUpBaikeContent(html2, true);

                html1 = Regex.Replace(html1, "href=\"/item/", "href=\"https://baike.baidu.com/item/", RegexOptions.Multiline);
                string url2 = Regex.Match(html1, "(?<=<a target=_blank href=\")(.+?)(?=\" data-lemmaid)").Value;
                html2 = WebOperate.GetHtmlText(url2);
                htmlText += Regex.Match(html2, "<div class=\"polysemantList-header\">([\\s\\S]+?)(?=<div class=\"content-wrapper\">)").Value + "</div></div></div></div></div><br/>";
                htmlText += PickUpBaikeContent(html2, true);
                htmlText = Regex.Replace(htmlText, "href='/item/", "href='https://baike.baidu.com/item/", RegexOptions.Multiline);
            }
            else
            {
                htmlText += Regex.Match(html1, "<ul class=\"polysemantList-wrapper cmn-clearfix\"([\\s\\S]+?)(</ul>)").Value + "</div></div></div></div></div><br/>";
                htmlText += PickUpBaikeContent(html1, true);
                htmlText = Regex.Replace(htmlText, "href='/item/", "href='https://baike.baidu.com/item/", RegexOptions.Multiline);
            }
            htmlText = Regex.Replace(html1, "<body([\\s\\S]+?)</body>", "<body>" + htmlText + "</body>", RegexOptions.Multiline);
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
                UiWebBrowser.Navigate("about:blank");
            }
            else
            {
                UiWebBrowser.NavigateToString(htmlText);
            }
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
        void WebBrowser1_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SuppressScriptErrors(UiWebBrowser, true);
        }

        /// <summary>
        /// 在浏览器加载时绑定扩展方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            UiWebBrowser.Navigating += WebBrowser1_Navigating;
        }

        //导航中
        private void WebBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        //导航完成
        private void WebBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            //WebBrowser wb = (WebBrowser)sender;
        }
        #endregion


        private void TbHans_KeyDown(object sender, KeyEventArgs e)
        {
 
        }


    }
}

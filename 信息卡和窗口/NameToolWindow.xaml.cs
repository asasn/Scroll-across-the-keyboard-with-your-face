using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using static System.Windows.Forms.Control;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// NameToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NameToolWindow : Window
    {
        public NameToolWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //遍历词库文件
            DirectoryInfo theFolder = new DirectoryInfo(Gval.Path.Resourse + "/语料");
            FileInfo[] thefileInfo = theFolder.GetFiles("*.txt", SearchOption.TopDirectoryOnly);

            foreach (FileInfo NextFile in thefileInfo) //遍历文件
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(NextFile.FullName);
                if (name != "百家姓" && name != "玄幻百家姓")
                {
                    WrapPanel wp = new WrapPanel() { Width = 200, Name = name, Uid = NextFile.FullName };
                    wp.Children.Add(new Label() { Content = name + " - " + GetListFormTXT(wp.Uid).Count.ToString(), BorderThickness = new Thickness(), Background = (Brush)new BrushConverter().ConvertFromString("#f5f5f5") });
                    wp.Children.Add(new CheckBox());
                    WpWordBank.Children.Add(wp);
                }
            }
        }

        /// <summary>
        /// 点击事件：生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbGenerate_Click(object sender, RoutedEventArgs e)
        {
            //结果列表初始化
            WpResults.Children.Clear();

            List<List<string>> nameLists = new List<List<string>>();
            foreach (WrapPanel wp in WpWordBank.Children)
            {
                if ((wp.Children[1] as CheckBox).IsChecked == true)
                {
                    //载入挂接的词库
                    nameLists.Add(GetListFormTXT(wp.Uid));
                }
            }

            //获取姓氏长度选项值
            int valueSurnameLength = GetCheckedBoxID(GridSurnameLength);
            //获取姓氏风格选项值
            int valueSurnameReality = GetCheckedBoxID(GridSurnameReality);
            //获取名字选择值
            int valueName = GetCheckedBoxID(GridNameLength);

            //选择姓氏字典
            List<string> surnameList = new List<string>();
            if (valueSurnameReality == 0)
            {
                surnameList = GetListFormTXT(Gval.Path.Resourse + "/语料/百家姓.txt");
            }
            if (valueSurnameReality == 1)
            {
                surnameList = GetListFormTXT(Gval.Path.Resourse + "/语料/玄幻百家姓.txt");
            }

            //合并选择的名字字典
            List<string> nameList = GetUnionList(nameLists);

            for (int i = 0; i < 80; i++)
            {
                GenerateWpItems(valueSurnameLength, valueName, surnameList, nameList);
            }

            webBrowser.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 生成一个WrapPanel的子项目
        /// </summary>
        /// <param name="valueSurname"></param>
        /// <param name="valueName"></param>
        void GenerateWpItems(int valueSurname, int valueName, List<string> surnameList, List<string> nameList)
        {
            //生成姓氏
            string surname = string.Empty;

            //生成名字
            string name = string.Empty;

            surname = GetSurname(valueSurname + 1, surnameList);
            if (nameList.Count == 0)
            {
                name = GetAName(valueName + 1);
            }
            else
            {
                name = GetAName(valueName + 1, nameList);
            }
            //生成新文本框
            TextBox tb = new TextBox() { IsReadOnly = true, Text = surname + name, Margin = new Thickness(2) };
            tb.GotFocus += Tb_GotFocus;
            WpResults.Children.Add(tb);


        }



        /// <summary>
        /// 生成一个名字
        /// </summary>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        string GetAName(int length, string encoding = "GB2312")
        {
            string name = string.Empty;
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                for (int i = 0; i < length; i++)
                {
                    name += GenerateChineseWords(encoding);
                }
            }
            else
            {
                name = tbName.Text;
            }

            return name;
        }

        /// <summary>
        /// 通过集合生成一个名字
        /// </summary>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        string GetAName(int length, List<string> myList)
        {
            string name = string.Empty;
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                for (int i = 0; i < length; i++)
                {
                    name += GetStringFromList(myList);
                }
            }
            else
            {
                name = tbName.Text;
            }

            return name;
        }

        /// <summary>
        /// 生成一个随机汉字
        /// </summary>
        /// <param name="encoding">字符集</param>
        /// <returns></returns>
        private string GenerateChineseWords(string encoding = "GB2312")
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            Encoding gb = Encoding.GetEncoding(encoding);

            // 获取区码(常用汉字的区码范围为16-55)
            int regionCode = random.Next(16, 56);

            // 获取位码(位码范围为1-94 由于55区的90,91,92,93,94为空,故将其排除)
            int positionCode = random.Next(1, regionCode == 55 ? 90 : 95);

            // 转换区位码为机内码
            regionCode += 160;// 160即为十六进制的20H+80H=A0H
            positionCode += 160;// 160即为十六进制的20H+80H=A0H

            byte[] bytes = new byte[] { (byte)regionCode, (byte)positionCode };
            return gb.GetString(bytes);
        }

        /// <summary>
        /// 事件：名字文本框获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Clipboard.SetText(tb.Text);
            HandyControl.Controls.Growl.SuccessGlobal("已复制名字到剪贴板！");

            WpShowWord.Children.Clear();
            foreach (char c in tb.Text)
            {
                WebBrowser webb = new WebBrowser();
                string urlStr = "https://www.zdic.net/hans/" + c.ToString();
                string htmlText = WebOperate.GetHtmlText(urlStr);
                string pattern = "(?<=<span class=\"z_d song\">)([\\s\\S]+?)(?=<span class=\"ptr\">)";
                MatchCollection matchRets = Regex.Matches(htmlText, pattern, RegexOptions.Multiline);
                string pinyin = c.ToString() + "：";
                if (matchRets.Count > 0)
                {
                    foreach (Match item in matchRets)
                    {
                        string p = "[āáǎàēéěèīíǐìōóǒòūúǔùǖǘǚǜüêɑńňɡａ-ｚＡ－ＺA-Za-z\\s∥-]+";
                        Match m = Regex.Match(item.Value, p);
                        if (false == pinyin.Contains(item.Value) && m.Success)
                        {
                            pinyin += item.Value + " ";
                        }
                    }
                }
                TextBox tbw = new TextBox() { Margin = new Thickness(0, 2, 0, 2), Text = pinyin.Trim(), IsReadOnly = true };
                WpShowWord.Children.Add(tbw);
            }
        }



        /// <summary>
        /// 查找某种类型的子控件
        /// </summary>
        public static class FindChirldHelper
        {
            public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
            {
                try
                {
                    List<T> TList = new List<T> { };
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                        if (child != null && child is T)
                        {
                            TList.Add((T)child);
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                        else
                        {
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                    }
                    return TList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取选中的id
        /// </summary>
        /// <param name="uIElement"></param>
        /// <returns></returns>
        int GetCheckedBoxID(UIElement uIElement)
        {
            List<RadioButton> rbList = FindChirldHelper.FindVisualChild<RadioButton>(uIElement);
            int t = 0;
            foreach (RadioButton rb in rbList)
            {
                if (rb.IsChecked == true)
                {
                    break;
                }
                else
                {
                    t++;
                }
            }
            return t;
        }


        /// <summary>
        /// 生成姓氏（前缀）
        /// </summary>
        /// <returns></returns>
        public string GetSurname(int n, List<string> myList)
        {
            string surname = string.Empty;
            if (string.IsNullOrWhiteSpace(tbSurname.Text))
            {
                do
                {
                    surname = GetStringFromList(myList);
                } while (surname.Length != n);
            }
            else
            {
                surname = tbSurname.Text;
            }
            return surname;
        }

        /// <summary>
        /// 从类型列表中随机取出一个字符串
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns></returns>
        string GetStringFromList(List<string> stringList)
        {
            if (stringList.Count == 0)
            {
                return string.Empty;
            }
            Random random = new Random();
            int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, stringList.Count());
            return stringList[index];
        }


        /// <summary>
        /// 合并多个字符串列表
        /// </summary>
        /// <param name="listArray"></param>
        /// <returns></returns>
        List<string> GetUnionList(List<List<string>> listArray)
        {
            List<string> listRet = new List<string>();
            foreach (List<string> listString in listArray)
            {
                listRet = listRet.Union(listString).ToList<string>();
            }
            return listRet;
        }

        static List<string> GetListFormTXT(string fullFilePath)
        {
            List<string> myList = new List<string>();
            FileStream fs = new FileStream(fullFilePath, FileMode.Open);
            StreamReader reader = new StreamReader(fs, UnicodeEncoding.GetEncoding("utf-8"));

            //按行读取
            string strLine = string.Empty;
            while ((strLine = reader.ReadLine()) != null)
            {
                strLine = strLine.Trim().ToString();
                myList.Add(strLine);
            }
            fs.Close();
            reader.Close();
            return myList;
        }


    }
}
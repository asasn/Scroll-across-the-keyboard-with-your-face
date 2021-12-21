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
                if (name.Contains("百家姓"))
                {
                    WrapPanel wp = new WrapPanel() { Width = 200, Name = name, Uid = NextFile.FullName };
                    CheckBox ckBox = new CheckBox() { Name = name };
                    ckBox.Checked += CkBox_Checked;
                    ckBox.Unchecked += CkBox_Unchecked;

                    wp.Children.Add(ckBox);
                    wp.Children.Add(new Label() { Content = name, BorderThickness = new Thickness(), Background = (Brush)new BrushConverter().ConvertFromString("#f5f5f5") });
                    WpSurnameBank.Children.Add(wp);

                    if (name.Contains("常见"))
                    {
                        ckBox.IsChecked = true;
                    }
                }
                else
                {
                    WrapPanel wp = new WrapPanel() { Width = 200, Name = name, Uid = NextFile.FullName };
                    CheckBox ckBox = new CheckBox() { Name = name };
                    ckBox.Checked += CkBox_Checked;
                    ckBox.Unchecked += CkBox_Unchecked;

                    wp.Children.Add(ckBox);
                    wp.Children.Add(new Label() { Content = name, BorderThickness = new Thickness(), Background = (Brush)new BrushConverter().ConvertFromString("#f5f5f5") });
                    WpNameBank.Children.Add(wp);

                    if (name.Contains("常用字2500"))
                    {
                        ckBox.IsChecked = true;
                    }
                }
            }
        }

        /// <summary>
        /// 事件：词库选项框选中时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ckBox = sender as CheckBox;
            WrapPanel wp = ckBox.Parent as WrapPanel;

            List<string> listString = GetListFormTXT(wp.Uid);
            ckBox.DataContext = listString;

            Label lb = FindChirldHelper.FindVisualChild<Label>(wp)[0];
            lb.Content = ckBox.Name + " - " + listString.Count.ToString();
        }

        /// <summary>
        /// 事件：词库选项框取消选中时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ckBox = sender as CheckBox;
            WrapPanel wp = ckBox.Parent as WrapPanel;

            Label lb = FindChirldHelper.FindVisualChild<Label>(wp)[0];
            lb.Content = ckBox.Name;
        }

        List<string> GetListFromBank(WrapPanel wpBank)
        {
            List<List<string>> nameLists = new List<List<string>>();
            foreach (WrapPanel wp in wpBank.Children)
            {
                CheckBox ckBox = FindChirldHelper.FindVisualChild<CheckBox>(wp)[0]; ;
                if (ckBox.IsChecked == true)
                {
                    //载入挂接的词库
                    nameLists.Add(ckBox.DataContext as List<string>);
                }
            }

            //合并选择的名字字典（去除重复项）
            return GetUnionList(nameLists);
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
            List<string> surnameList = GetListFromBank(WpSurnameBank);
            List<string> nameList = GetListFromBank(WpNameBank);

            //获取姓氏选项值
            int valueSurname = GetCheckedBoxID(GridSurname);

            //获取名字选择值
            int isNameSecond = GetCheckedBoxID(GridNameSecond);

            for (int i = 0; i < 80; i++)
            {
                GenerateWpItems(valueSurname, isNameSecond, surnameList, nameList);
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

            surname = GetSurname(valueSurname, surnameList);
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
            string name1 = string.Empty;

            if (ckbName1.IsChecked == true)
            {//名字·其一
                if (string.IsNullOrWhiteSpace(tbNameFirst.Text))
                {
                    name1 = name += GenerateChineseWords(encoding);
                }
                else
                {
                    name1 = name += tbNameFirst.Text;
                }
            }

            //名字·其二
            if (ckbName2.IsChecked == true)
            {
                if (ckbNameRepeat.IsChecked == true)
                {
                    name += name1;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(tbNameSecond.Text))
                    {
                        name += GenerateChineseWords(encoding);
                    }
                    else
                    {
                        name += tbNameSecond.Text;
                    }
                }
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
            string name1 = string.Empty;

            //名字·其一
            if (ckbName1.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(tbNameFirst.Text))
                {
                    name1 = name += GetStringFromList(myList);
                }
                else
                {
                    name1 = name += tbNameFirst.Text;
                }
            }

            //名字·其二
            if (ckbName2.IsChecked == true)
            {
                if (ckbNameRepeat.IsChecked == true)
                {
                    name += name1;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(tbNameSecond.Text))
                    {
                        name += GetStringFromList(myList);
                    }
                    else
                    {
                        name += tbNameSecond.Text;
                    }
                }
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
        public string GetSurname(int valueSurname, List<string> myList)
        {
            if (myList.Count == 0)
            {
                return string.Empty;
            }

            int length = 1;
            if (valueSurname == 2)
            {
                //特殊选项
            }
            else
            {
                if (valueSurname == 1)
                {
                    length = 2;
                }
            }
            string surname = string.Empty;
            if (string.IsNullOrWhiteSpace(tbSurname.Text))
            {
                do
                {
                    surname = GetStringFromList(myList);
                } while (surname.Length != length);
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
using NSMain.Bricks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NSMain.Tools
{
    /// <summary>
    /// NameToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WToolMakeName : Window
    {
        public WToolMakeName()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 事件：窗口载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WpSurnameBank.Children.Clear();
            WpNameBank.Children.Clear();

            string styleName = GetCheckedBoxUidName(GridStyle);
            SetStyle(styleName);
        }

        CheckBox AddBank(WrapPanel wpParent, string name, FileInfo NextFile)
        {
            WrapPanel wp = new WrapPanel() { Width = 200, Name = name, Uid = NextFile.FullName };
            CheckBox ckBox = new CheckBox() { Name = name };
            ckBox.Checked += CkBox_Checked;
            ckBox.Unchecked += CkBox_Unchecked;

            wp.Children.Add(ckBox);
            wp.Children.Add(new Label() { Content = name, BorderThickness = new Thickness(), Background = (Brush)new BrushConverter().ConvertFromString("#f5f5f5") });
            wpParent.Children.Add(wp);

            return ckBox;
        }

        /// <summary>
        /// 挂接词库组（根据输入标志进行选择）
        /// </summary>
        /// <param name="styleName"></param>
        void SetStyle(string styleName)
        {
            if (WpSurnameBank != null && WpNameBank != null)
            {
                LoadBank(styleName, WpSurnameBank);
                LoadBank(styleName, WpNameBank);

                CheckBank(WpSurnameBank, new string[] { "常见", "周易" });
                CheckBank(WpNameBank, new string[] { "常用字2500", "后缀" });
            }
        }

        void LoadBank(string styleName, WrapPanel wpBank)
        {
            DirectoryInfo theFolder = new DirectoryInfo(GlobalVal.Path.Resourses + "/语料/取名工具/" + styleName + "/" + wpBank.Tag.ToString());
            FileInfo[] thefileInfo = theFolder.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            wpBank.Children.Clear();
            foreach (FileInfo NextFile in thefileInfo) //遍历文件
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(NextFile.FullName);
                _ = AddBank(wpBank, name, NextFile);
            }
        }

        void CheckBank(WrapPanel wpBank, string[] tags)
        {
            foreach (WrapPanel wp in wpBank.Children)
            {
                CheckBox cb = FindChirldHelper.FindVisualChild<CheckBox>(wp)[0];
                foreach (string tag in tags)
                {
                    if (cb.Name.Contains(tag))
                    {
                        cb.IsChecked = true;
                    }
                }
            }
        }

        /// <summary>
        /// 事件：风格选项选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonStyle_Checked(object sender, RoutedEventArgs e)
        {
            CkbSurname.IsChecked = true;
            string styleName = GetCheckedBoxUidName(GridStyle);
            SetStyle(styleName);
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
            WrapPanel wpBank = wp.Parent as WrapPanel;
            ScrollViewer sc = wpBank.Parent as ScrollViewer;
            Grid gd = sc.Parent as Grid;
            GroupBox gb = gd.Parent as GroupBox;

            List<string> listString = GetListFormTXT(wp.Uid);
            ckBox.DataContext = listString;

            Label lb = FindChirldHelper.FindVisualChild<Label>(wp)[0];
            lb.Content = ckBox.Name + " - " + listString.Count.ToString();

            gb.DataContext = GetListFromBanks(wpBank);
            gb.Header = gb.Tag.ToString() + " - " + (gb.DataContext as List<string>).Count.ToString();
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
            WrapPanel wpBank = wp.Parent as WrapPanel;
            ScrollViewer sc = wpBank.Parent as ScrollViewer;
            Grid gd = sc.Parent as Grid;
            GroupBox gb = gd.Parent as GroupBox;

            Label lb = FindChirldHelper.FindVisualChild<Label>(wp)[0];
            lb.Content = ckBox.Name;

            gb.DataContext = GetListFromBanks(wpBank);
            gb.Header = gb.Tag.ToString() + " - " + (gb.DataContext as List<string>).Count.ToString();
        }

        List<string> GetListFromBanks(WrapPanel wpBank)
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
            List<string> surnameList = WpSurnameBank.DataContext as List<string>;
            List<string> nameList = WpNameBank.DataContext as List<string>;

            for (int i = 0; i < 80; i++)
            {
                GenerateWpItems(surnameList, nameList);
            }

            webBrowser.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 生成一个WrapPanel的子项目
        /// </summary>
        /// <param name="valueSurname"></param>
        /// <param name="valueName"></param>
        void GenerateWpItems(List<string> surnameList, List<string> nameList)
        {
            string surname = string.Empty;
            string name = string.Empty;


            if (RbStyleNormal.IsChecked == true)
            {
                surname = GetSurname(surnameList);
                if (nameList.Count == 0)
                {
                    //这个是GB2312字库开关，旨在未选择词库时是否仍然能够随即选择一个汉字
                    //name = GetAName();
                }
                else
                {
                    name = GetAName(nameList);
                }
            }
            else
            {
                //姓和名倒置，作为前后缀风格，比如xx真人
                surname = GetAName(surnameList);
                if (CkbSuffix.IsChecked == true)
                {
                    if (string.IsNullOrWhiteSpace(TbSuffix.Text))
                    {
                        name = GetSurname(nameList);
                    }
                    else
                    {
                        name = TbSuffix.Text;
                    }
                }
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
        string GetAName(string encoding = "GB2312")
        {
            string name1 = string.Empty;
            string name2 = string.Empty;

            if (CkbName1.IsChecked == true)
            {//名字·其一
                if (string.IsNullOrWhiteSpace(TbNameFirst.Text))
                {
                    while (name1.Length < CbName1Length.SelectedIndex + 1)
                    {
                        name1 += GenerateChineseWords(encoding);
                    };

                }
                else
                {
                    name1 = TbNameFirst.Text;
                }
            }

            //名字·其二
            if (CkbName2.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(TbNameSecond.Text))
                {
                    if (ckbNameRepeat.IsChecked == true)
                    {
                        name2 = name1;
                    }
                    else
                    {
                        while (name2.Length < cbName2Length.SelectedIndex + 1)
                        {
                            name2 += GenerateChineseWords(encoding);
                        };

                    }
                }
                else
                {
                    name2 = TbNameSecond.Text;
                }
            }

            return name1 + name2;
        }

        /// <summary>
        /// 通过集合生成一个名字
        /// </summary>
        /// <param name="length"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        string GetAName(List<string> myList)
        {
            if (myList.Count == 0)
            {
                return string.Empty;
            }

            string name1 = string.Empty;
            string name2 = string.Empty;

            //名字·其一
            if (CkbName1.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(TbNameFirst.Text))
                {
                    while (name1.Length < CbName1Length.SelectedIndex + 1)
                    {
                        name1 += GetStringFromList(myList);
                    };
                }
                else
                {
                    name1 = TbNameFirst.Text;
                }
            }

            //名字·其二
            if (CkbName2.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(TbNameSecond.Text))
                {
                    if (ckbNameRepeat.IsChecked == true)
                    {
                        name2 = name1;
                    }
                    else
                    {
                        while (name2.Length < cbName2Length.SelectedIndex + 1)
                        {
                            name2 += GetStringFromList(myList);
                        }
                    }
                }
                else
                {
                    name2 = TbNameSecond.Text;
                }
            }

            return name1 + name2;
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
                string pinyin = ReadFromPinyinDict(c.ToString());
                if (false == string.IsNullOrWhiteSpace(pinyin))
                {
                    //尝试在本地字典查找
                }
                else
                {
                    //未在本地字典发现，从网络上查找
                    pinyin = c.ToString() + "：";
                    string urlStr = "https://www.zdic.net/hans/" + c.ToString();
                    string htmlText = WebOperate.GetHtmlText(urlStr);
                    string pattern = "(?<=<span class=\"z_d song\">)([\\s\\S]+?)(?=<span class=\"ptr\">)";
                    MatchCollection matchRets = Regex.Matches(htmlText, pattern, RegexOptions.Multiline);
                    if (matchRets.Count > 0)
                    {
                        foreach (Match item in matchRets)
                        {
                            string p = "[āáǎàēéěèīíǐìōóǒòūúǔùǖǘǚǜüêɑḿm̀ńňɡａ-ｚＡ－ＺA-Za-z\\s∥-]+";
                            Match m = Regex.Match(item.Value, p);
                            if (false == pinyin.Contains(item.Value) && m.Success)
                            {
                                pinyin += item.Value + " ";
                            }
                        }
                        SaveToPinyinDict(c.ToString(), pinyin.Trim());
                    }
                }

                TextBox tbw = new TextBox() { Margin = new Thickness(0, 2, 0, 2), Text = pinyin.Trim(), IsReadOnly = true };
                WpShowWord.Children.Add(tbw);
            }
        }

        /// <summary>
        /// 从字典读取拼音
        /// </summary>
        /// <param name="hanzi"></param>
        /// <returns></returns>
        string ReadFromPinyinDict(string hanzi)
        {
            string filepath = GlobalVal.Path.Resourses + "/语料/拼音字典/PinyinDict.txt";
            String line;
            StreamReader sr = new StreamReader(filepath, new UTF8Encoding(true));
            while ((line = sr.ReadLine()) != null)//按行读取 line为每行的数据
            {
                if (line.Contains(hanzi))
                {
                    sr.Close();
                    return line;
                }
            }
            sr.Close();
            return null;
        }

        /// <summary>
        /// 保存至拼音字典
        /// </summary>
        /// <param name="hanzi"></param>
        /// <param name="pinyin"></param>
        void SaveToPinyinDict(string hanzi, string pinyin)
        {
            string filepath = GlobalVal.Path.Resourses + "/语料/拼音字典/PinyinDict.txt";
            string content = string.Empty;
            String line;
            FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs, new UTF8Encoding(true));
            StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(true));

            while ((line = sr.ReadLine()) != null)//按行读取 line为每行的数据
            {
                content += line + "\n";
            }
            if (false == content.Contains(hanzi))
            {
                content = pinyin + "\n" + content;
            }
            if (File.Exists(filepath))
            {
                fs.SetLength(0); //先清空文件
            }
            sw.Write(content);   //写入字符串
            sw.Close();
            sr.Close();
            fs.Close();
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
        //int GetCheckedBoxID(UIElement uIElement)
        //{
        //    List<RadioButton> rbList = FindChirldHelper.FindVisualChild<RadioButton>(uIElement);
        //    int t = 0;
        //    foreach (RadioButton rb in rbList)
        //    {
        //        if (rb.IsChecked == true)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            t++;
        //        }
        //    }
        //    return t;
        //}

        /// <summary>
        /// 获取选中的Uid（标记名称）
        /// </summary>
        /// <param name="uIElement"></param>
        /// <returns></returns>
        string GetCheckedBoxUidName(UIElement uIElement)
        {
            List<RadioButton> rbList = FindChirldHelper.FindVisualChild<RadioButton>(uIElement);
            foreach (RadioButton rb in rbList)
            {
                if (rb.IsChecked == true)
                {
                    return rb.Uid;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 生成姓氏（前缀）
        /// </summary>
        /// <returns></returns>
        public string GetSurname(List<string> myList)
        {
            if (myList.Count == 0)
            {
                return string.Empty;
            }

            string surname = string.Empty;

            if (CkbSurname.IsChecked == true)
            {
                if (TbSurname.IsEnabled == true && false == string.IsNullOrWhiteSpace(TbSurname.Text))
                {
                    surname += TbSurname.Text;
                }
                if (CkbSuffix.IsChecked == true && false == string.IsNullOrWhiteSpace(TbSuffix.Text))
                {
                    surname += TbSuffix.Text;
                }
                if (false == string.IsNullOrWhiteSpace(surname))
                {
                    return surname;
                }

                if (RbStyleNormal.IsChecked == true)
                {
                    while (surname.Length != CbSurnameLength.SelectedIndex + 1)
                    {
                        surname = GetStringFromList(myList);
                    };
                }
                else
                {
                    while (surname.Length != CbSuffixLength.SelectedIndex + 1)
                    {
                        surname = GetStringFromList(myList);
                    };
                }
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
            StreamReader reader = new StreamReader(fullFilePath, UnicodeEncoding.GetEncoding("utf-8"));

            //按行读取
            string strLine;
            while ((strLine = reader.ReadLine()) != null)
            {
                strLine = strLine.Trim().ToString();
                myList.Add(strLine);
            }
            reader.Close();
            return myList;
        }


    }
}
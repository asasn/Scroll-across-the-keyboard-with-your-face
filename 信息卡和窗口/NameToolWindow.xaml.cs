using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private void TbGenerate_Click(object sender, RoutedEventArgs e)
        {
            //结果列表初始化
            WpResults.Children.Clear();

            for (int i = 0; i < 80; i++)
            {
                GenerateWpItems();
            }
        }

        /// <summary>
        /// 生成一个WrapPanel的子项目
        /// </summary>
        /// <param name="valueSurname"></param>
        /// <param name="valueName"></param>
        void GenerateWpItems()
        {
            //获取姓氏选项值
            int valueSurname = GetCheckedBoxID(GboxSurname);

            //获取名字选择值
            int valueName = GetCheckedBoxID(GboxName);

            //获取姓氏选项值
            int valueGender = GetCheckedBoxID(GboxGender);

            //获取风格选项值
            int valueStyle = GetCheckedBoxID(GboxStyle);

            //生成姓氏
            string surname = string.Empty;

            //生成名字
            string name = string.Empty;

            //仙侠风格
            if (valueStyle == 0)
            {
                List<string>[] surnameLists = { surnameXuanHuan1, surnameXuanHuan2 };
                List<string> surnameList = GetUnionList(surnameLists);
                List<string>[] nameLists = { listZhouYi, listQianZiWen };
                List<string> nameList = GetUnionList(nameLists);

                surname = GetSurname(valueSurname + 1, surnameList);
                name = GetAName(valueName + 1, nameList);
            }

            //玄幻风格
            if (valueStyle == 1)
            {
                List<string>[] surnameLists = { surnameXuanHuan1, surnameXuanHuan2 };
                List<string> surnameList = GetUnionList(surnameLists);
                List<string>[] nameLists = { listZhouYi, listQianZiWen };
                List<string> nameList = GetUnionList(nameLists);

                surname = GetSurname(valueSurname + 1, surnameList);
                name = GetAName(valueName + 1, nameList);
            }

            //现实风格
            if (valueStyle == 2)
            {
                List<string>[] surnameLists = { surname1, surname2 };
                List<string> surnameList = GetUnionList(surnameLists);
                List<string>[] nameLists = { listShiJing, listQuanTangShi, };
                List<string> nameList = GetUnionList(nameLists);

                surname = GetSurname(valueSurname + 1, surnameList);
                name = GetAName(valueName + 1, nameList);

            }

            //古代风格
            if (valueStyle == 3)
            {
                List<string>[] surnameLists = { surname1, surname2 };
                List<string> surnameList = GetUnionList(surnameLists);
                List<string>[] nameLists = { listShiJing};
                List<string> nameList = GetUnionList(nameLists);

                surname = GetSurname(valueSurname + 1, surnameList);
                name = GetAName(valueName + 1, nameList);
            }

            //西方风格
            if (valueStyle == 4)
            {

            }

            //代称风格
            if (valueStyle == 5)
            {

            }

            //龙套风格
            if (valueStyle == 6)
            {
                List<string>[] surnameLists = { surnameXuanHuan1, surnameXuanHuan2 };
                List<string> surnameList = GetUnionList(surnameLists);

                surname = GetSurname(valueSurname + 1, surnameList);
                name = GetAName(valueName + 1, "GB2312");
            }

            //配角风格
            if (valueStyle == 7)
            {
                List<string>[] surnameLists = { surnameXuanHuan1, surnameXuanHuan2 };
                List<string> surnameList = GetUnionList(surnameLists);
                List<string>[] nameLists = { listShiJing, listQianZiWen, };
                List<string> nameList = GetUnionList(nameLists);

                surname = GetSurname(valueSurname + 1, surnameList);
                name = GetAName(valueName + 1, nameList);
            }

            //生成新文本框
            TextBox tb = new TextBox();
            tb.IsReadOnly = true;
            tb.GotFocus += Tb_GotFocus;
            tb.Text = surname + name;
            tb.Margin = new Thickness(2);
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
            Random random = new Random();
            int index = new Random(Guid.NewGuid().GetHashCode()).Next(0, stringList.Count());
            return stringList[index];
        }

        List<string> surnameXuanHuan1 = GetListFormTXT("玄幻百家姓（单姓）");
        List<string> surnameXuanHuan2 = GetListFormTXT("玄幻百家姓（复姓）");
        List<string> surname1 = GetListFormTXT("百家姓（单姓）");
        List<string> surname2 = GetListFormTXT("百家姓（复姓）");
        List<string> listQianZiWen = GetListFormTXT("千字文");
        List<string> listQuanTangShi = GetListFormTXT("全唐诗");
        List<string> listShiJing = GetListFormTXT("诗经");
        List<string> listZhouYi = GetListFormTXT("周易");

        /// <summary>
        /// 合并多个字符串列表
        /// </summary>
        /// <param name="listArray"></param>
        /// <returns></returns>
        List<string> GetUnionList(Array listArray)
        {
            List<string> listRet = new List<string>();
            foreach (List<string> listString in listArray)
            {
                listRet = listRet.Union(listString).ToList<string>();
            }
            return listRet;
        }

        static List<string> GetListFormTXT(string sFileName)
        {
            List<string> myList = new List<string>();
            string sFilePath = Gval.Path.Resourse + "/字表/" + sFileName + ".txt";
            FileStream fs = new FileStream(sFilePath, FileMode.Open);
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Xml;

namespace 脸滚键盘.信息卡模板
{
    /// <summary>
    /// RoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class RoleCard : Window
    {
        public class RoleInfo
        {
            public ArrayList uid = new ArrayList();
            public ArrayList 名称 = new ArrayList();
            public ArrayList 别称 = new ArrayList();
            public ArrayList 身份 = new ArrayList();
            public ArrayList 状态 = new ArrayList();
            public ArrayList 性别 = new ArrayList();
            public ArrayList 所属 = new ArrayList();
            public ArrayList 阶级 = new ArrayList();
            public ArrayList 物品 = new ArrayList();
            public ArrayList 能力 = new ArrayList();
            public ArrayList 履历 = new ArrayList();
            public ArrayList 备注 = new ArrayList();

        }
        TreeViewItem thisItem;
        public RoleCard(TreeViewItem selectedItem, MouseButtonEventArgs e)
        {
            InitializeComponent();

            thisItem = selectedItem;
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            Point pointToWindow = (e.Source as FrameworkElement).PointToScreen(p);//转化为屏幕中的坐标
            this.Left = pointToWindow.X - this.Width / 2;
            this.Top = pointToWindow.Y - this.Height / 2;
            this.WindowStartupLocation = WindowStartupLocation.Manual;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double ln = 0;
            Button b = sender as Button;
            string num = b.Name.Substring(1);
            string wpName = "w" + num;
            WrapPanel wp = gCard.FindName(wpName) as WrapPanel;

            TextBox tb = AddTextBox();
            wp.Children.Add(tb);
        }

        TextBox AddTextBox()
        {
            TextBox tb = new TextBox();
            tb.MinWidth = 30;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "";
            tb.BorderThickness = new Thickness(0, 0, 0, 1);
            tb.Margin = new Thickness(10, 5, 0, 0);
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            return tb;
        }

        private void g6_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("内容改变");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //实际上是以名字为标识符
            if (false == string.IsNullOrEmpty(t1.Text))
            {
                string sql = string.Format("INSERT or IGNORE INTO role (名称, 备注) VALUES ('{0}', '{1}');", t1.Text, t11.Text);
                SqliteOperate.ExecuteNonQuery(sql);
                sql = "select last_insert_rowid() from role;";
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
                int lastuid = 0;
                while (reader.Read())
                {
                    lastuid = reader.GetInt32(0);
                }
                if (true == string.IsNullOrEmpty(t0.Content.ToString()))
                {
                    t0.Content = lastuid.ToString();
                }



                WrapPanel[] wrapPanels = { w2, w3, w6, w7, w8, w9, w10 };

                foreach (WrapPanel wp in wrapPanels)
                {
                    sql = string.Empty;
                    foreach (TextBox tb in wp.Children)
                    {
                        if (false == string.IsNullOrEmpty(tb.Text))
                        {
                            sql += string.Format("INSERT or IGNORE INTO {0} (uid, {0}) VALUES ({1}, '{2}');", wp.Uid, lastuid, tb.Text);

                        }
                    }
                    SqliteOperate.ExecuteNonQuery(sql);
                }

                WrapPanel[] wrapPanels2 = { w4, w5 };
                foreach (WrapPanel wp in wrapPanels2)
                {
                    sql = string.Empty;
                    int n = 0;
                    foreach (RadioButton rb in wp.Children)
                    {
                        if (rb.IsChecked == true)
                        {
                            break;
                        }
                        n++;
                    }
                    sql += string.Format("INSERT or IGNORE INTO {0} (uid, {0}) VALUES ({1}, '{2}');", wp.Uid, lastuid, n);
                    SqliteOperate.ExecuteNonQuery(sql);
                }



            }



            RoleInfo roleInfo = new RoleInfo();

            //foreach (var wp in gCard.Children)
            //{
            //    if (wp.GetType() == typeof(WrapPanel))
            //    {
            //        string num = (wp as WrapPanel).Name.Substring(1);
            //        //string alName = "al" + num;
            //        //ArrayList al = this.FindName(alName) as ArrayList;
            //        //Console.WriteLine(al.ToString());
            //    }
            //}

            //XmlDocument doc = new XmlDocument();
            //XmlElement root = doc.CreateElement("root");

            //FillInBox(w0, roleInfo.uid, doc, root);
            //FillInBox(w1, roleInfo.名称, doc, root);
            //FillInBox(w2, roleInfo.别称, doc, root);
            //FillInBox(w3, roleInfo.身份, doc, root);
            //FillInBox(w4, roleInfo.状态, doc, root);
            //FillInBox(w5, roleInfo.性别, doc, root);
            //FillInBox(w6, roleInfo.所属, doc, root);
            //FillInBox(w7, roleInfo.阶级, doc, root);
            //FillInBox(w8, roleInfo.物品, doc, root);
            //FillInBox(w9, roleInfo.能力, doc, root);
            //FillInBox(w10, roleInfo.履历, doc, root);
            //FillInBox(w11, roleInfo.备注, doc, root);

            //thisItem.DataContext = root;
            //Console.WriteLine(root.InnerXml);

        }

        void FillInBox(WrapPanel wp, ArrayList wpBox, XmlDocument doc, XmlElement root)
        {
            wpBox.Clear();
            foreach (var tb in wp.Children)
            {
                if (tb.GetType() == typeof(TextBox))
                {
                    wpBox.Add((tb as TextBox).Text);
                }
                if (tb.GetType() == typeof(RadioButton))
                {
                    wpBox.Add((tb as RadioButton).IsChecked);
                }
            }

            XmlElement wpEle = doc.CreateElement(wp.Name as string);
            root.AppendChild(wpEle);
            foreach (var item in wpBox)
            {
                XmlElement etb = doc.CreateElement("tb");
                etb.SetAttribute("text", item.ToString());
                wpEle.AppendChild(etb);
            }
        }

    }
}
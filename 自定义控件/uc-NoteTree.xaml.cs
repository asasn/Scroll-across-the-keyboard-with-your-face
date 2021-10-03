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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 脸滚键盘
{
    /// <summary>
    /// uc_NoteTree.xaml 的交互逻辑
    /// </summary>
    public partial class uc_NoteTree : UserControl
    {
        public uc_NoteTree()
        {
            InitializeComponent();
        }



        public string UcTitle
        {
            get { return (string)GetValue(UcTitleProperty); }
            set { SetValue(UcTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTitleProperty =
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_NoteTree), new PropertyMetadata(null));



        public string XmlName
        {
            get { return (string)GetValue(XmlNameProperty); }
            set { SetValue(XmlNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XmlName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XmlNameProperty =
            DependencyProperty.Register("XmlName", typeof(string), typeof(uc_NoteTree), new PropertyMetadata(null));



        /// <summary>
        /// DataContext绑定了当前指向的curItem，因此将其更改事件作为curItem的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Gval.CurrentBook.curBookItem != null)
            {
                tv.Items.Clear();

                //获取当前notes对应的完整xml文件名
                string fullXmlName_notes = Gval.Base.AppPath + "/books/" + Gval.CurrentBook.curBookItem.Header.ToString() + "/" + XmlName;
                if (true == FileOperate.IsFileExists(fullXmlName_notes))
                {
                    TreeOperate.XmlToNoteTree.Show(tv, fullXmlName_notes);
                    uc.IsEnabled = true;
                }
            }
            else
            {
                tv.Items.Clear();
                uc.IsEnabled = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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
using 脸滚键盘.信息卡模板;

namespace 脸滚键盘
{
    /// <summary>
    /// uc_Searcher.xaml 的交互逻辑
    /// </summary>
    public partial class uc_Searcher : UserControl
    {
        public uc_Searcher()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_Searcher), new PropertyMetadata(null));



        public TreeViewItem CurItem
        {
            get { return (TreeViewItem)GetValue(CurItemProperty); }
            set { SetValue(CurItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurItemProperty =
            DependencyProperty.Register("CurItem", typeof(TreeViewItem), typeof(uc_Searcher), new PropertyMetadata(null));



        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(uc_Searcher), new PropertyMetadata(null));


        string FullFileName;
        TreeViewItem curVolumeItem;
        TreeViewItem curBookItem;
        TreeView curTv;

        void RefreshBookItem()
        {
            //如果单只依靠绑定属性来传值，可能会发生DataContext改变了（触发本事件）而依赖属性CurItem未改变的情况
            //所以，使用this.DataContext作为CurItem的值是必需的
            CurItem = this.DataContext as TreeViewItem;

            curBookItem = TreeOperate.GetRootItem(CurItem);
            if (curBookItem != null)
            {
                curTv = curBookItem.Parent as TreeView;
            }
            else
            {
                curTv = null;
            }
            curVolumeItem = TreeOperate.GetItemByLevel(CurItem, 2);
            FullFileName = TreeOperate.GetItemPath(CurItem, UcTag);

        }

        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RefreshBookItem();
            if (null == curBookItem)
            {
                return;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchRetWindow rtWin = new SearchRetWindow(CurItem, UcTag, tbKeyWords.Text);
            rtWin.ShowDialog();
        }


        private void tbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}

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
    /// uc_BookTree.xaml 的交互逻辑
    /// </summary>
    public partial class uc_BookTree : UserControl
    {
        public uc_BookTree()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_BookTree), new PropertyMetadata(null));

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tv_Loaded(object sender, RoutedEventArgs e)
        {
            string workPath = Gval.Base.AppPath + "/books";
            //检查工作目录是否存在
            if (FileOperate.IsFolderExists(workPath))
            {
                TreeOperate.Show(tv);
            }
            else
            {
                //不存在则建立
                FileOperate.CreateFolder(workPath);
                TreeOperate.SaveBooks(tv);
            }

        }

        /// <summary>
        /// 添加新书籍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewBook_Click(object sender, RoutedEventArgs e)
        {
            string bookPath = Gval.Base.AppPath + "/books/新书籍";
            if (false == FileOperate.IsFolderExists(bookPath))
            {
                TreeViewItem newItem = TreeOperate.BookTree.AddNewBook(tv);
                FileOperate.CreateFolder(bookPath);
                TreeOperate.SaveBooks(tv);
                TreeOperate.SaveBook(newItem);
            }
        }

        /// <summary>
        /// 添加新分卷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewVolume_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem.Name == "book")
            {

                TreeViewItem bookItem = TreeOperate.GetBookItem(selectedItem);
                string bookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                string volumePath = bookPath + "/新分卷";
                if (false == FileOperate.IsFolderExists(volumePath))
                {
                    FileOperate.CreateFolder(volumePath);
                    TreeViewItem newItem = TreeOperate.BookTree.AddNewVolume(selectedItem);
                    TreeOperate.SaveBook(bookItem);
                }
            }
        }

        /// <summary>
        /// 添加新章节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewChapter_Click(object sender, RoutedEventArgs e)
        {

            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem.Name == "volume" || selectedItem.Name == "chapter")
            {
                TreeViewItem bookItem = TreeOperate.GetBookItem(selectedItem);
                TreeViewItem volumeItem = TreeOperate.GetVolumeItem(selectedItem);
                string bookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                string volumePath = bookPath + '/' + volumeItem.Header.ToString();
                string fullFileName = volumePath + "/新章节.txt";
                if (false == FileOperate.IsFolderExists(volumePath))
                {
                    FileOperate.CreateFolder(volumePath);
                }
                if (false == FileOperate.IsFileExists(fullFileName))
                {
                    FileOperate.CreateNewDoc(fullFileName);
                    TreeViewItem newItem = TreeOperate.BookTree.AddNewChapter(selectedItem);
                    TreeOperate.SaveBook(bookItem);
                }
            }
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem bookItem = TreeOperate.GetBookItem(selectedItem);
                TreeViewItem volumeItem = TreeOperate.GetVolumeItem(selectedItem);
                string bookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                string volumePath = bookPath + '/' + volumeItem.Header.ToString();

                if (selectedItem.Name == "chapter")
                {
                    string fullFileName = volumePath + '/' + selectedItem.Header.ToString() + ".txt";
                    TreeOperate.DelItem(selectedItem);
                    FileOperate.deleteDoc(fullFileName);
                    TreeOperate.SaveBook(bookItem);
                }

                if (selectedItem.Name == "volume")
                {
                    TreeOperate.DelItem(selectedItem);
                    FileOperate.deleteDir(volumePath);
                    TreeOperate.SaveBook(bookItem);
                }

                if (selectedItem.Name == "book")
                {
                    MessageBoxResult dr = MessageBox.Show("真的要进行删除吗？\n将会不经回收站直接删除，请进行确认！\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                    if (dr == MessageBoxResult.OK)
                    {
                        TreeOperate.DelItem(selectedItem);
                        FileOperate.deleteDir(bookPath);
                        TreeOperate.SaveBooks(tv);
                    }
                }

            }
        }

        private void tv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
                if (selectedItem != null)
                {
                    if (renameBox.Visibility == Visibility.Hidden)
                    {
                        TreeOperate.ReNewCurrentBook(tv);//记录下当前节点的各种信息
                        TreeOperate.ReadyForReName(selectedItem, renameBox);
                    }
                }
            }
        }

        private void renameBox_KeyDown(object sender, KeyEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (e.Key == Key.Enter)
            {
                if (renameBox.Visibility == Visibility.Visible)
                {
                    TreeOperate.EndRename(Gval.CurrentBook.curItem, renameBox);
                    selectedItem.Focus();
                }
            }
        }

        //鼠标左键点击事件：点击在TreeView类型的控件tv上，对应Item来说，相当于点击在空白
        private void tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
            }
            if (renameBox.Visibility == Visibility.Visible)
            {
                TreeOperate.EndRename(Gval.CurrentBook.curItem, renameBox);
                selectedItem.Focus();
            }

        }
    }
}

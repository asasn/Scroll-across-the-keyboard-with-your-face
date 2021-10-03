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



        public TreeViewItem CurItem
        {
            get { return (TreeViewItem)GetValue(CurItemProperty); }
            set { SetValue(CurItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurItemProperty =
            DependencyProperty.Register("CurItem", typeof(TreeViewItem), typeof(uc_BookTree), new PropertyMetadata(null));



        /*

        本自定义控件是一个专门用于关联./books/index.xml文件的树状目录结构控件，用于展示书库和书籍目录
        它包含：
        一，books目录下的index.xml，用于显示书库节点，作为TreeView的根节点
        二，各书籍文件夹下的index.xml，在关联的根节点下遍历节点，显示书籍节点

        */

        //以下开始本控件的事件定义

        /// <summary>
        /// 载入TreeView事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_Loaded(object sender, RoutedEventArgs e)
        {
            string workPath = Gval.Base.AppPath + "/books";
            string booksXml = Gval.Base.AppPath + "/books/index.xml";
            //检查工作目录是否存在
            if (true == FileOperate.IsFolderExists(workPath) && true == FileOperate.IsFileExists(booksXml))
            {
                TreeOperate.XmlToBookTree.Show(tv);
            }
            else
            {
                //不存在则建立
                FileOperate.CreateFolder(workPath);
                TreeOperate.BookTreeToXml.SaveBooks(tv);
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
                TreeOperate.BookTreeToXml.SaveBooks(tv);
                TreeOperate.BookTreeToXml.SaveBook(newItem);
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
            if (selectedItem != null && (selectedItem.Name == "book" || selectedItem.Name == "volume"))
            {

                TreeViewItem bookItem = TreeOperate.BookTree.GetBookItem(selectedItem);
                string bookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                string volumePath = bookPath + "/新分卷";
                if (false == FileOperate.IsFolderExists(volumePath))
                {
                    FileOperate.CreateFolder(volumePath);
                    TreeViewItem newItem = TreeOperate.BookTree.AddNewVolume(selectedItem);
                    TreeOperate.BookTreeToXml.SaveBook(bookItem);
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
            if (selectedItem != null && (selectedItem.Name == "volume" || selectedItem.Name == "chapter"))
            {
                TreeViewItem bookItem = TreeOperate.BookTree.GetBookItem(selectedItem);
                TreeViewItem volumeItem = TreeOperate.BookTree.GetVolumeItem(selectedItem);
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
                    TreeOperate.BookTreeToXml.SaveBook(bookItem);
                }
            }
        }

        /// <summary>
        /// 删除选定节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem bookItem = TreeOperate.BookTree.GetBookItem(selectedItem);
                TreeViewItem volumeItem = TreeOperate.BookTree.GetVolumeItem(selectedItem);
                string bookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                string volumePath = bookPath + '/' + volumeItem.Header.ToString();

                if (selectedItem.Name == "chapter")
                {
                    string fullFileName = volumePath + '/' + selectedItem.Header.ToString() + ".txt";
                    TreeOperate.DelItem.Do(selectedItem);
                    FileOperate.deleteDoc(fullFileName);
                    TreeOperate.BookTreeToXml.SaveBook(bookItem);
                }

                if (selectedItem.Name == "volume")
                {
                    TreeOperate.DelItem.Do(selectedItem);
                    FileOperate.deleteDir(volumePath);
                    TreeOperate.BookTreeToXml.SaveBook(bookItem);
                }

                if (selectedItem.Name == "book")
                {
                    MessageBoxResult dr = MessageBox.Show("真的要进行删除吗？\n将会不经回收站直接删除，请进行确认！\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                    if (dr == MessageBoxResult.OK)
                    {
                        TreeOperate.DelItem.Do(selectedItem);
                        FileOperate.deleteDir(bookPath);
                        TreeOperate.BookTreeToXml.SaveBooks(tv);
                    }
                }

            }
        }

        /// <summary>
        /// TreeView快捷键（包含按F2重命名等）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
                if (selectedItem != null)
                {
                    if (renameBox.Visibility == Visibility.Hidden)
                    {
                        TreeOperate.ReNewCurrent(tv);//记录下当前节点的各种信息
                        TreeOperate.ReadyForReName(selectedItem, renameBox);
                    }
                }
            }
        }

        /// <summary>
        /// 重命名文本框快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// TreeView鼠标左键点击事件：点击在TreeView类型的控件tv上，对应Item来说，相当于点击在空白
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //拖曳事件，按下（按压中）
        private void tv_DragEnter(object sender, DragEventArgs e)
        {
            //无源信息时获取（有则不获取，防止跨控件时多次重复获取）
            if (null == Gval.DragDrop.dragItem)
            {
                Gval.DragDrop.dragUc = this;
                Gval.DragDrop.dragTreeView = tv;
                Gval.DragDrop.dragItem = e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem;
                Gval.DragDrop.dragBookItem = TreeOperate.BookTree.GetBookItem(Gval.DragDrop.dragItem);
                Gval.DragDrop.dragVolumeItem = TreeOperate.BookTree.GetVolumeItem(Gval.DragDrop.dragItem);
                Gval.DragDrop.dragBookPath = Gval.Base.AppPath + "/books/" + Gval.DragDrop.dragBookItem.Header.ToString();
                Gval.DragDrop.dragVolumePath = Gval.DragDrop.dragBookPath + '/' + Gval.DragDrop.dragVolumeItem.Header.ToString();
                Gval.DragDrop.dragTextFullName = Gval.DragDrop.dragVolumePath + '/' + Gval.DragDrop.dragItem.Header.ToString() + ".txt";
            }

            TreeViewItem dropItem = e.Source as TreeViewItem;
            dropItem.IsSelected = true;
            dropItem.IsExpanded = true;
        }

        private void tv_MouseMove(object sender, MouseEventArgs e)
        {
            TreeOperate.DragDropItem.DragMove(tv, e);
        }

        private void tv_Drop(object sender, DragEventArgs e)
        {
            TreeViewItem dropItem = e.Source as TreeViewItem;

            TreeViewItem dropBookItem = TreeOperate.BookTree.GetBookItem(dropItem);
            TreeViewItem dropVolumeItem = TreeOperate.BookTree.GetVolumeItem(dropItem);
            string dropBookPath = Gval.Base.AppPath + "/books/" + dropBookItem.Header.ToString();
            string dropVolumePath = dropBookPath + '/' + dropVolumeItem.Header.ToString();
            //目标的文件名采用原来的Gval.DragDrop.dragItem
            string fullFileName = dropVolumePath + "/" + Gval.DragDrop.dragItem.Header.ToString() + ".txt";


            //源和目标不一致，且目标不存在同名文件，面板类型一致时才能拖曳移动
            if (dropItem != Gval.DragDrop.dragItem && Gval.DragDrop.dragUc.GetType() == this.GetType())
            {
                if (Gval.DragDrop.dragItem.Name == "chapter")
                {
                    if (dropVolumePath != Gval.DragDrop.dragVolumePath)
                    {
                        if (true == FileOperate.IsFileExists(fullFileName))
                        {
                            return;
                        }
                        FileOperate.renameDoc(Gval.DragDrop.dragTextFullName, fullFileName);
                    }
                    if (dropItem.Name == "volume")
                    {
                        TreeOperate.DelItem.Do(Gval.DragDrop.dragItem);
                        TreeOperate.BookTree.AddThisItem(dropItem, Gval.DragDrop.dragItem);
                    }
                    if (dropItem.Name == "chapter")
                    {
                        TreeOperate.DelItem.Do(Gval.DragDrop.dragItem);
                        TreeOperate.BookTree.AddThisItem(dropItem, Gval.DragDrop.dragItem);
                    }
                    if (dropItem.Name == "book")
                    {
                        MessageBox.Show("错误的目标节点，请不要在此放下！", "提醒");
                        return;
                    }

                }

                if (Gval.DragDrop.dragItem.Name == "volume")
                {
                    dropVolumePath = dropBookPath + '/' + Gval.DragDrop.dragVolumeItem.Header.ToString();
                    if (dropBookPath != Gval.DragDrop.dragBookPath)
                    {
                        if (FileOperate.IsFolderExists(dropVolumePath))
                        {
                            return;
                        }
                        FileOperate.renameDir(Gval.DragDrop.dragVolumePath, dropVolumePath);
                    }

                    if (dropItem.Name == "volume")
                    {
                        TreeOperate.DelItem.Do(Gval.DragDrop.dragItem);
                        TreeOperate.BookTree.AddThisItem(dropItem, Gval.DragDrop.dragItem);
                    }
                    if (dropItem.Name == "chapter")
                    {
                        MessageBox.Show("错误的目标节点，请不要在此放下！", "提醒");
                        return;
                    }
                    if (dropItem.Name == "book")
                    {
                        TreeOperate.DelItem.Do(Gval.DragDrop.dragItem);
                        TreeOperate.BookTree.AddThisItem(dropItem, Gval.DragDrop.dragItem);
                    }


                }

                if (Gval.DragDrop.dragItem.Name == "book")
                {
                    dropBookPath = Gval.Base.AppPath + "/books/" + Gval.DragDrop.dragBookItem.Header.ToString();

                    if (dropItem.Name == "volume")
                    {
                        MessageBox.Show("错误的目标节点，请不要在此放下！", "提醒");
                        return;
                    }
                    if (dropItem.Name == "chapter")
                    {
                        MessageBox.Show("错误的目标节点，请不要在此放下！", "提醒");
                        return;
                    }
                    if (dropItem.Name == "book")
                    {
                        TreeOperate.DelItem.Do(Gval.DragDrop.dragItem);
                        TreeOperate.BookTree.AddThisItem(dropItem, Gval.DragDrop.dragItem);
                    }
                    if (dropItem.Name == null)
                    {
                        MessageBox.Show("空！", "提醒");
                        return;
                    }
                    FileOperate.renameDir(Gval.DragDrop.dragBookPath, dropBookPath);
                }


                //保存源Xml，因为可能存在只调整次序的情况，所以要分别保存来源和目标的xml文件，另把书目也保存一下
                TreeOperate.BookTreeToXml.SaveBook(Gval.DragDrop.dragBookItem);
                TreeOperate.BookTreeToXml.SaveBook(dropBookItem);
                TreeOperate.BookTreeToXml.SaveBooks(tv);
            }
            else
            {
                //MessageBox.Show("面板类型不一致，请不要在此放下！", "提醒");
            }
        }

        private void tv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //清空源信息
            Gval.DragDrop.dragUc = null;
            Gval.DragDrop.dragTreeView = null;
            Gval.DragDrop.dragItem = null;
            Gval.DragDrop.dragVolumeItem = null;
            Gval.DragDrop.dragBookItem = null;
            Gval.DragDrop.dragBookPath = null;
            Gval.DragDrop.dragVolumePath = null;
            Gval.DragDrop.dragTextFullName = null;
        }

        /// <summary>
        /// Treeview节点双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeOperate.ReNewCurrent(tv);
                //触发其他控件的绑定变动事件
                CurItem = selectedItem;
            }
        }
    }
}

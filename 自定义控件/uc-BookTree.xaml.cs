using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

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


        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(uc_BookTree), new PropertyMetadata(null));


        public TreeViewItem CurItem
        {
            get { return (TreeViewItem)GetValue(CurItemProperty); }
            set { SetValue(CurItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurItemProperty =
            DependencyProperty.Register("CurItem", typeof(TreeViewItem), typeof(uc_BookTree), new PropertyMetadata(null));



        /* 控件总览说明

        本自定义控件是一个专门用于关联./books/index.xml文件的树状目录结构控件，用于展示书库和书籍目录
        它关联：
        一，books目录下的index.xml，用于显示书库节点，作为TreeView的根节点
        二，各书籍文件夹下的index.xml，在关联的根节点下遍历节点，显示书籍节点

        */

        //以下开始本控件的事件定义

        /// <summary>
        /// 事件：载入TreeView事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_Loaded(object sender, RoutedEventArgs e)
        {

            string ucTag = Gval.Base.AppPath + "/" + UcTag;
            string booksXml = Gval.Base.AppPath + "/" + UcTag + "/" + "index.xml";
            //检查工作目录是否存在
            if (true == FileOperate.IsFolderExists(ucTag) && true == FileOperate.IsFileExists(booksXml))
            {
                tv.Items.Clear();
                SqliteOperate.NewConnection();
                //TreeOperate.Show.ToBookTree.ToCurBook(tv);
                TreeOperate.Show.ToBookTree.BySql(tv, Gval.Current.curBookName);
                if (tv.HasItems)
                {
                    TreeViewItem lastVolume = (tv.Items[tv.Items.Count - 1] as TreeViewItem);
                    lastVolume.IsExpanded = true;
                    ChangeCurItem(lastVolume);
                }
            }
        }

        /// <summary>
        /// 事件：点击添加新书籍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewBook_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新书籍";
            string bookPath = Gval.Base.AppPath + "/" + UcTag + "/" + itemTitle;
            string bookXmlPath = bookPath + "/index.xml";
            if (false == FileOperate.IsFolderExists(bookPath))
            {
                FileOperate.CreateFolder(bookPath);
                TreeOperate.Save.BookTree.AddToBooksXml(itemTitle);
                TreeOperate.Save.BookTree.BuildBookXml(bookXmlPath, "book");
            }
        }

        /// <summary>
        /// 事件：点击添加新分卷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewVolume_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新分卷";
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                string volumePath = Gval.Current.curBookPath + "/" + itemTitle;
                if (false == FileOperate.IsFolderExists(volumePath))
                {
                    TreeOperate.AddItem.RootItem(tv, itemTitle, TreeOperate.ItemType.目录);
                    FileOperate.CreateFolder(volumePath);
                    TreeOperate.Save.BookTree.SaveCurBook(tv);
                    TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                }
            }
        }

        /// <summary>
        /// 事件：点击添加新章节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewChapter_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新章节";
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                int level = TreeOperate.GetLevel(selectedItem);
                if (level == 2)
                {
                    string volumePath = TreeOperate.GetItemPath((selectedItem.Parent as TreeViewItem), UcTag);
                    string fullFileName = volumePath + "/" + itemTitle + ".txt";
                    if (false == FileOperate.IsFileExists(fullFileName))
                    {
                        TreeOperate.AddItem.BrotherItem(selectedItem, itemTitle, TreeOperate.ItemType.文档);
                        FileOperate.CreateNewDoc(fullFileName);
                        TreeOperate.Save.BookTree.SaveCurBook(tv);
                        TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                    }
                }
                if (level == 1)
                {
                    string volumePath = TreeOperate.GetItemPath(selectedItem, UcTag);
                    string fullFileName = volumePath + "/" + itemTitle + ".txt";
                    if (false == FileOperate.IsFolderExists(volumePath))
                    {
                        FileOperate.CreateFolder(volumePath);
                    }
                    if (false == FileOperate.IsFileExists(fullFileName))
                    {
                        TreeOperate.AddItem.ChildItem(selectedItem, itemTitle, TreeOperate.ItemType.文档);
                        FileOperate.CreateNewDoc(fullFileName);
                        TreeOperate.Save.BookTree.SaveCurBook(tv);
                        TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                    }
                }
            }



        }

        /// <summary>
        /// 事件：点击删除选定节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem bookItem = TreeOperate.GetRootItem(selectedItem);
                TreeViewItem volumeItem = TreeOperate.GetItemByLevel(selectedItem, 2);
                string bookPath = Gval.Base.AppPath + "/" + UcTag + "/" + Gval.Current.curBookName;
                string volumePath = bookPath + '/' + volumeItem.Header.ToString();
                TreeViewItem parentItem = null;
                if (selectedItem.Parent as TreeViewItem != null)
                {
                    parentItem = selectedItem.Parent as TreeViewItem;
                }

                //if (TreeOperate.GetLevel(selectedItem) == 1)
                //{
                //    MessageBoxResult dr = MessageBox.Show("真的要进行删除吗？\n将会不经回收站直接删除，请进行确认！\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                //    if (dr == MessageBoxResult.OK)
                //    {
                //        TreeOperate.DelItem.Do(selectedItem);
                //        FileOperate.deleteDir(bookPath);

                //    }
                //}
                //else
                //{
                    if (selectedItem.Name == "doc")
                    {
                        string fullFileName = volumePath + '/' + selectedItem.Header.ToString() + ".txt";
                        TreeOperate.DelItem.Do(selectedItem);
                        FileOperate.deleteDoc(fullFileName);
                        TreeOperate.Save.BookTree.SaveCurBook(tv);
                        TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                    }

                    if (selectedItem.Name == "dir")
                    {
                        TreeOperate.DelItem.Do(selectedItem);
                        FileOperate.deleteDir(volumePath);
                        TreeOperate.Save.BookTree.SaveCurBook(tv);
                        TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                    }
                //}
                ChangeCurItem(parentItem);//更改当前节点指针
            }
        }

        /// <summary>
        /// 事件：TreeView快捷键（包含按F2重命名等）
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
                        TreeOperate.ReNewCurrent(tv, selectedItem, UcTag);//记录下当前节点的各种信息
                        TreeOperate.ReName.Ready(tv, selectedItem, renameBox);
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
                    TreeOperate.ReName.Do(tv, Gval.Current.curItem, renameBox, UcTag);
                    ChangeCurItem(selectedItem);//更改当前节点指针
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
                TreeOperate.ReName.Do(tv, Gval.Current.curItem, renameBox, UcTag);
                ChangeCurItem(selectedItem);//更改当前节点指针
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
                Gval.DragDrop.dragVolumeItem = TreeOperate.GetItemByLevel(Gval.DragDrop.dragItem, 1);
                if (Gval.DragDrop.dragVolumeItem != null)
                {
                    Gval.DragDrop.dragVolumePath = Gval.DragDrop.dragBookPath + '/' + Gval.DragDrop.dragVolumeItem.Header.ToString();
                    Gval.DragDrop.dragTextFullName = Gval.DragDrop.dragVolumePath + '/' + Gval.DragDrop.dragItem.Header.ToString() + ".txt";
                }


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

            TreeViewItem dropVolumeItem = TreeOperate.GetItemByLevel(dropItem, 1);
            string dropVolumePath;
            string fullFileName;

            //源和目标不一致，且目标不存在同名文件，面板类型一致时才能拖曳移动
            if (TreeOperate.GetRootItem(Gval.DragDrop.dragItem) != Gval.DragDrop.dragItem && dropItem != Gval.DragDrop.dragItem && Gval.DragDrop.dragUc.GetType() == this.GetType())
            {

                if (TreeOperate.GetRootItem(dropItem) != TreeOperate.GetRootItem(Gval.DragDrop.dragItem))
                {
                    ////拖动跨越了书籍
                    //if (Gval.DragDrop.dragItem.Name == "doc")
                    //{
                    //    bool tag = TreeOperate.DragDropItem.DoDrop(Gval.DragDrop.dragItem, dropItem);
                    //    if (tag == true)
                    //    {
                    //        //成功进行了移动
                    //        dropBookPath = Gval.Base.AppPath + "/books/" + dropGval.Current.curBookName;
                    //        dropVolumePath = dropBookPath + '/' + dropVolumeItem.Header.ToString();
                    //        //目标的文件名采用原来的Gval.DragDrop.dragItem
                    //        fullFileName = dropVolumePath + "/" + Gval.DragDrop.dragItem.Header.ToString() + ".txt";
                    //        if (false == FileOperate.IsFolderExists(dropVolumePath))
                    //        {
                    //            FileOperate.CreateFolder(dropVolumePath);
                    //        }
                    //        if (false == FileOperate.IsFileExists(fullFileName))
                    //        {
                    //            FileOperate.renameDoc(Gval.DragDrop.dragTextFullName, fullFileName);
                    //        }
                    //        TreeOperate.Save.FromBookTree.SaveCurBook(Gval.DragDrop.dragBookItem);
                    //        TreeOperate.Save.FromBookTree.SaveCurBook(tv);
                    //        
                    //    }
                    //}
                    //if (Gval.DragDrop.dragItem.Name == "dir")
                    //{
                    //    bool tag = TreeOperate.DragDropItem.DoDrop(Gval.DragDrop.dragItem, dropItem);
                    //    if (tag == true)
                    //    {
                    //        //成功进行了移动
                    //        dropBookPath = Gval.Base.AppPath + "/books/" + dropGval.Current.curBookName;
                    //        dropVolumePath = dropBookPath + '/' + Gval.DragDrop.dragVolumeItem.Header.ToString();
                    //        if (false == FileOperate.IsFolderExists(dropVolumePath))
                    //        {
                    //            FileOperate.renameDir(Gval.DragDrop.dragVolumePath, dropVolumePath);
                    //        }
                    //        TreeOperate.Save.FromBookTree.SaveCurBook(Gval.DragDrop.dragBookItem);
                    //        TreeOperate.Save.FromBookTree.SaveCurBook(tv);
                    //        
                    //    }
                    //}
                }
                else
                {
                    //TODO 尚未完成的拖曳事件
                    //同一书籍内部
                    if (dropVolumeItem != Gval.DragDrop.dragVolumeItem)
                    {
                        //不同的分卷
                        if (Gval.DragDrop.dragItem.Name == "doc")
                        {
                            bool tag = TreeOperate.DragDropItem.DoDrop(Gval.DragDrop.dragItem, dropItem);
                            if (tag == true)
                            {
                                //成功进行了移动
                                dropVolumePath = Gval.Current.curBookPath + '/' + dropVolumeItem.Header.ToString();
                                //目标的文件名采用原来的Gval.DragDrop.dragItem
                                fullFileName = dropVolumePath + "/" + Gval.DragDrop.dragItem.Header.ToString() + ".txt";
                                if (false == FileOperate.IsFolderExists(dropVolumePath))
                                {
                                    FileOperate.CreateFolder(dropVolumePath);
                                }
                                if (false == FileOperate.IsFileExists(fullFileName))
                                {
                                    FileOperate.renameDoc(Gval.DragDrop.dragTextFullName, fullFileName);
                                }
                                TreeOperate.Save.BookTree.SaveCurBook(tv);
                                TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                            }
                        }
                        if (Gval.DragDrop.dragItem.Name == "dir")
                        {
                            bool tag = TreeOperate.DragDropItem.DoDrop(Gval.DragDrop.dragItem, dropItem);
                            if (tag == true)
                            {
                                //成功进行了移动
                                dropVolumePath = Gval.Current.curBookPath + '/' + Gval.DragDrop.dragVolumeItem.Header.ToString();
                                if (false == FileOperate.IsFolderExists(dropVolumePath))
                                {
                                    FileOperate.renameDir(Gval.DragDrop.dragVolumePath, dropVolumePath);
                                }
                                TreeOperate.Save.BookTree.SaveCurBook(tv);
                                TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                            }

                        }

                    }
                    else
                    {
                        //同一个分卷内
                        if (Gval.DragDrop.dragItem.Name == "doc")
                        {
                            bool tag = TreeOperate.DragDropItem.DoDrop(Gval.DragDrop.dragItem, dropItem);
                            if (tag == true)
                            {
                                TreeOperate.Save.BookTree.SaveCurBook(tv);
                                TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                            }
                        }
                    }
                }
                ChangeCurItem(Gval.DragDrop.dragItem);//更改当前节点指针
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
                ChangeCurItem(selectedItem);
            }
        }

        void ChangeCurItem(TreeViewItem selectedItem)
        {
            CurItem = null;
            TreeOperate.ReNewCurrent(tv, selectedItem, UcTag);
            //触发其他控件的绑定变动事件
            CurItem = selectedItem;
            selectedItem.Focus();
        }

        /// <summary>
        /// 右键弹出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;

            //实例化菜单
            ContextMenu cm = this.FindResource("tvContextMenu") as ContextMenu;

            //在鼠标所在的位置显现
            cm.Placement = PlacementMode.MousePoint;

            //显示菜单
            cm.IsOpen = true;
            (cm.Items.GetItemAt(0) as MenuItem).IsEnabled = false;
            (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = false;
            (cm.Items.GetItemAt(3) as MenuItem).IsEnabled = false;
            (cm.Items.GetItemAt(5) as MenuItem).IsEnabled = false;
            if (selectedItem != null)
            {
                if (TreeOperate.GetLevel(selectedItem) == 1)
                {
                    (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = true;
                }
                if (TreeOperate.GetLevel(selectedItem) == 2)
                {
                    (cm.Items.GetItemAt(3) as MenuItem).IsEnabled = true;
                }

            }
            else
            {
                (cm.Items.GetItemAt(0) as MenuItem).IsEnabled = true;
            }
        }

        private void importBook_Click(object sender, RoutedEventArgs e)
        {

        }

        private void importDir_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            TreeViewItem bookItem = TreeOperate.GetRootItem(selectedItem);
            TreeViewItem volumeItem = null;

            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();

            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string srcFolder = folder.SelectedPath;

                DirectoryInfo root = new DirectoryInfo(srcFolder);
                string destFolder = System.IO.Path.Combine(TreeOperate.GetItemPath(selectedItem, UcTag), root.Name);
                if (false == FileOperate.IsFolderExists(destFolder))
                {
                    volumeItem = TreeOperate.AddItem.ChildItem(selectedItem, root.Name, TreeOperate.ItemType.目录);
                    FileOperate.CreateFolder(destFolder);
                }
                else
                {
                    foreach (TreeViewItem item in selectedItem.Items)
                    {
                        if (item.Header.ToString() == root.Name)
                        {
                            volumeItem = item;
                            break;
                        }
                    }
                }
                foreach (FileInfo f in root.GetFiles())
                {
                    if (f.Extension == ".txt" || f.Extension == ".book")
                    {
                        string title = System.IO.Path.GetFileNameWithoutExtension(f.FullName);
                        string srcFullFileName = f.FullName;
                        string destFullFileName = System.IO.Path.Combine(destFolder, title + ".txt");
                        if (false == FileOperate.IsFileExists(destFullFileName))
                        {
                            File.Copy(srcFullFileName, destFullFileName);
                            TreeOperate.AddItem.ChildItem(volumeItem, title, TreeOperate.ItemType.文档);
                        }
                    }
                }
                TreeOperate.Save.BookTree.SaveCurBook(tv);
                TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
            }


        }

        private void importDoc_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            TreeViewItem bookItem = TreeOperate.GetRootItem(selectedItem);
            string[] files = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "文本文件(*.txt, *.book)|*.txt;*.book|所有文件(*.*)|*.*";
            dlg.Multiselect = true;

            // 打开选择框选择
            if (dlg.ShowDialog() == true)
            {
                files = dlg.FileNames;
            }
            else
            {
                return;
            }
            foreach (string srcFullFileName in files)
            {
                string title = System.IO.Path.GetFileNameWithoutExtension(srcFullFileName);
                string destFullFileName = System.IO.Path.Combine(TreeOperate.GetItemPath(selectedItem, UcTag), title + ".txt");
                if (false == FileOperate.IsFileExists(destFullFileName))
                {
                    File.Copy(srcFullFileName, destFullFileName);
                    TreeOperate.AddItem.ChildItem(selectedItem, title, TreeOperate.ItemType.文档);
                }
            }
            TreeOperate.Save.BookTree.SaveCurBook(tv);
            TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnItemUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnItemDown_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            DrawerLeftInContainer.IsOpen = !DrawerLeftInContainer.IsOpen;
        }

        private void DrawerLeftInContainer_Loaded(object sender, RoutedEventArgs e)
        {
            string ucTag = Gval.Base.AppPath + "/" + UcTag;
            string booksXml = Gval.Base.AppPath + "/" + UcTag + "/" + "index.xml";
            //检查工作目录是否存在
            if (true == FileOperate.IsFolderExists(ucTag) && true == FileOperate.IsFileExists(booksXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(booksXml);
                XmlNode root = doc.SelectSingleNode("root");
                XmlNodeList rlist = root.ChildNodes;
                foreach (XmlNode ch in rlist)
                {
                    XmlElement rn = (XmlElement)ch;
                    HandyControl.Controls.Card bookCard = new HandyControl.Controls.Card();
                    wpBooks.Children.Add(bookCard);
                    //bookCard.Effect = 
                    bookCard.Margin = new Thickness(10, 10, 0, 0);
                    bookCard.Width = 180;
                    bookCard.Height = 240;
                    bookCard.VerticalAlignment = VerticalAlignment.Stretch;
                    bookCard.HorizontalAlignment = HorizontalAlignment.Stretch;

                    bookCard.Header = rn.GetAttribute("title");

                    string imgPath = Gval.Base.AppPath + "/books/" + rn.GetAttribute("title") + ".jpg";

                    bookCard.Content = FileOperate.GetImgObject(imgPath);

                    bookCard.MouseLeftButtonDown += CardSelected;

                    if (bookCard.Header.ToString() == Gval.Current.curBookName)
                    {
                        ChoseBookChange(bookCard);
                    }
                }
            }
            else
            {
                //不存在则建立
                FileOperate.CreateFolder(ucTag);

            }
        }

        void ChoseBookChange(HandyControl.Controls.Card bookCard)
        {
            bookCard.BorderBrush = Brushes.Blue;
            bookCard.BorderThickness = new Thickness(0, 5, 0, 5);
            UcTitle = "当前书籍——" + Gval.Current.curBookName;
            drawerTbk.Text = "当前书籍：" + Gval.Current.curBookName;
            SqliteOperate.NewConnection();
            tv_Loaded(null, null);
            Gval.ucEditor.IsEnabled = false;
        }

        private void CardSelected(object sender, MouseButtonEventArgs e)
        {
            foreach (HandyControl.Controls.Card item in wpBooks.Children)
            {
                item.BorderBrush = null;
                item.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            HandyControl.Controls.Card bookCard = sender as HandyControl.Controls.Card;
            Gval.Current.curBookName = bookCard.Header.ToString();
            Gval.Current.curBookPath = Gval.Base.AppPath + "/books/" + bookCard.Header.ToString();
            SettingsOperate.SaveSettings("curBookName", bookCard.Header.ToString());
            SettingsOperate.SaveSettings("curBookPath", Gval.Current.curBookPath);
            ChoseBookChange(bookCard);
        }

        private void uc_MouseEnter(object sender, MouseEventArgs e)
        {
            btnBooks.Visibility = Visibility.Visible;
        }

        private void uc_MouseLeave(object sender, MouseEventArgs e)
        {
            btnBooks.Visibility = Visibility.Hidden;
        }

        private void DrawerLeftInContainer_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}

﻿using System;
using System.Collections.Generic;
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

namespace 脸滚键盘
{
    /// <summary>
    /// uc_NoteTree.xaml 的交互逻辑
    /// </summary>
    public partial class uc_MaterialTree : UserControl
    {
        public uc_MaterialTree()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_MaterialTree), new PropertyMetadata(null));



        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(uc_MaterialTree), new PropertyMetadata(null));



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = tv.SelectedItem as TreeViewItem;
            TreeView tvd = TreeOperate.GetTreeView(item);
            Console.WriteLine(tvd);
        }

        private void tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void tv_Loaded(object sender, RoutedEventArgs e)
        {
            string ucTag = Gval.Base.AppPath + "/" + UcTag;
            string materialXml = Gval.Base.AppPath + "/" + UcTag + "/" + "index.xml";
            //检查工作目录是否存在
            if (true == FileOperate.IsFolderExists(ucTag) && true == FileOperate.IsFileExists(materialXml))
            {
                TreeOperate.Show.ToMaterialTree.ShowAll(tv);
            }
            else
            {
                //不存在则建立
                FileOperate.CreateFolder(ucTag);
                TreeOperate.Save.FromMaterialTree.SaveAll(tv);
            }
        }




        private void btnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新文件夹";
            string rootPath = Gval.Base.AppPath + "/" + UcTag + "/" + itemTitle;
            if (false == FileOperate.IsFolderExists(rootPath))
            {
                TreeViewItem newItem = TreeOperate.AddItem.RootItem(tv, itemTitle, TreeOperate.ItemType.目录);
                FileOperate.CreateFolder(rootPath);
                TreeOperate.Save.FromMaterialTree.SaveAll(tv);
            }
        }

        private void btnNewDoc_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新文档";
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem rootItem = TreeOperate.GetRootItem(selectedItem);
                string rootPath = Gval.Base.AppPath + "/" + UcTag + "/" + rootItem.Header.ToString();
                string fullFileName = rootPath + "/" + itemTitle + ".txt";

                int level = TreeOperate.GetLevel(selectedItem);
                if (level == 2)
                {
                    if (false == FileOperate.IsFileExists(fullFileName))
                    {
                        TreeOperate.AddItem.BrotherItem(selectedItem, itemTitle, TreeOperate.ItemType.文档);
                        FileOperate.CreateNewDoc(fullFileName);
                        TreeOperate.Save.FromMaterialTree.SaveAll(tv);
                    }
                }
                if (level == 1)
                {
                    if (false == FileOperate.IsFileExists(fullFileName))
                    {
                        TreeOperate.AddItem.ChildItem(selectedItem, itemTitle, TreeOperate.ItemType.文档);
                        FileOperate.CreateNewDoc(fullFileName);
                        TreeOperate.Save.FromMaterialTree.SaveAll(tv);
                    }
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
                TreeOperate.ReName.Do(tv, curItem, renameBox, UcTag);
                selectedItem.Focus();
            }

        }

        TreeViewItem curItem;
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
                        curItem = selectedItem;//记录下当前节点的各种信息
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
                    TreeOperate.ReName.Do(tv, curItem, renameBox, UcTag);
                    selectedItem.Focus();
                }
            }
        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem dirItem = TreeOperate.GetRootItem(selectedItem);
                string dirPath = Gval.Base.AppPath + "/" + UcTag + "/" + dirItem.Header.ToString();


                if (selectedItem.Name == "doc")
                {
                    string fullFileName = dirPath + '/' + selectedItem.Header.ToString() + ".txt";
                    TreeOperate.DelItem.Do(selectedItem);
                    FileOperate.deleteDoc(fullFileName);
                    TreeOperate.Save.FromMaterialTree.SaveAll(tv);
                }

                if (selectedItem.Name == "dir")
                {
                    MessageBoxResult dr = MessageBox.Show("真的要进行删除吗？\n将会不经回收站直接删除，请进行确认！\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                    if (dr == MessageBoxResult.OK)
                    {
                        TreeOperate.DelItem.Do(selectedItem);
                        FileOperate.deleteDir(dirPath);
                        TreeOperate.Save.FromMaterialTree.SaveAll(tv);
                    }
                }
            }
        }

        /// <summary>
        /// 右键菜单
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

            if (selectedItem != null)
            {
                (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = true;
            }
            else
            {
                (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = false;
            }
        }
    }
}
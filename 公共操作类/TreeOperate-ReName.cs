using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace 脸滚键盘
{
    static partial class TreeOperate
    {
        public static partial class ReName
        {

            /// <summary>
            /// 获取子控件
            /// </summary>
            private static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is childItem)
                        return (childItem)child;
                    else
                    {
                        childItem childOfChild = FindVisualChild<childItem>(child);
                        if (childOfChild != null)
                            return childOfChild;
                    }
                }
                return null;
            }

            static string GOldName;
            static string GNewName;
            /// <summary>
            /// 回填selectedItem和renamebox的信息
            /// </summary>
            private static void FillBcak(TreeViewItem selectedItem, TextBox renameBox, string name)
            {
                selectedItem.Header = name;
                selectedItem.Tag = name;
                selectedItem.Width = Double.NaN;
                renameBox.Text = name;
                if (renameBox.Name != "chapterNameBox")
                {
                    renameBox.Visibility = Visibility.Hidden;
                    renameBox.IsEnabled = false;
                    renameBox.Text = string.Empty;
                    renameBox.Margin = new Thickness(-1, -1, 0, 0);
                }
            }

            /// <summary>
            /// 重命名之前的准备（开始）
            /// </summary>
            public static void Ready(TreeView tv, TreeViewItem selectedItem, TextBox renameBox)
            {
                if (selectedItem != null && renameBox.Visibility == Visibility.Hidden)
                {
                    //TextBox reNameTextBox = FindVisualChild<TextBox>(selectedItem as DependencyObject);
                    //reNameTextBox.Visibility = Visibility.Visible;
                    //在节点位置显现
                    Point p = selectedItem.TranslatePoint(new Point(tv.Margin.Left, tv.Margin.Top), tv);
                    renameBox.Margin = new Thickness(p.X, p.Y, 0, 0);
                    //隐藏节点会导致子节点也跟着隐藏，所以并不采用这个方法，而是把Header改为空值
                    renameBox.Visibility = Visibility.Visible;
                    renameBox.IsEnabled = true;
                    GOldName = string.Empty;
                    GNewName = string.Empty;
                    renameBox.Text = selectedItem.Header.ToString(); //新名字
                    GOldName = selectedItem.Header.ToString(); //旧名字
                    renameBox.Focus();
                    renameBox.SelectAll();
                }
            }

            /// <summary>
            /// 结束重命名状态（完成或者忽略）
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="selectedItem"></param>
            /// <param name="renameBox"></param>
            public static void Do(TreeView tv, TreeViewItem selectedItem, TextBox renameBox, string ucTag)
            {
                if (selectedItem != null && renameBox.Visibility == Visibility.Visible)
                {
                    string oldName = GOldName;
                    string newName = renameBox.Text;

                    //未发生改变的情况
                    if (newName == oldName)
                    {
                        Console.WriteLine("未发生改变的情况");
                        FillBcak(selectedItem, renameBox, oldName);
                        return;
                    }

                    //过滤非法字符
                    newName = FileOperate.ReplaceFileName(newName);

                    //新名字为空或存在非法字符的情况
                    if (newName == string.Empty || newName.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    {
                        Console.WriteLine("新名字为空或存在非法字符");
                        FillBcak(selectedItem, renameBox, oldName);
                        return;
                    }

                    //通过合法性检测，开始处理

                    if (selectedItem.Name == "doc")
                    {
                        //是文档的情况
                        string fOld = GetItemPath(selectedItem.Parent as TreeViewItem, ucTag) + "/" + oldName + ".txt";
                        string fNew = GetItemPath(selectedItem.Parent as TreeViewItem, ucTag) + "/" + newName + ".txt";

                        if (true == FileOperate.IsFileExists(fNew))
                        {
                            Console.WriteLine("对应的章节已经存在");
                            FillBcak(selectedItem, renameBox, oldName);
                            return;
                        }
                        else
                        {
                            //新名称回填
                            FillBcak(selectedItem, renameBox, newName);

                            FileOperate.renameDoc(fOld, fNew);
                            SaveIt(selectedItem, newName, tv, ucTag);
                        }
                    }
                    else
                    {
                        //非文档的情况

                        string oldPath = GetItemPath(selectedItem.Parent as TreeViewItem, ucTag) + "/" + oldName;
                        string newPath = GetItemPath(selectedItem.Parent as TreeViewItem, ucTag) + "/" + newName;

                        if (true == FileOperate.IsFolderExists(newPath))
                        {
                            Console.WriteLine("对应的文件夹已经存在");
                            FillBcak(selectedItem, renameBox, oldName);
                            return;
                        }
                        else
                        {
                            //新名称回填
                            FillBcak(selectedItem, renameBox, newName);

                            FileOperate.renameDir(oldPath, newPath);
                            SaveIt(selectedItem, newName, tv, ucTag);
                            
                        }
                    }


                }
                if (ucTag == "books")
                {
                    //刷新工作区公共变量
                    //【注意】本方法是内容的消费者而非生产者，所以在此更新公共变量时，需要填入DocTree的信息
                    TreeOperate.ReNewCurrent(Gval.Current.curTv, selectedItem, ucTag);
                }

            }

            static void SaveIt(TreeViewItem selectedItem, string newName, TreeView tv, string ucTag)
            {
                if (ucTag == "books")
                {
                    Save.BookTree.SaveCurBook(tv);
                    //TreeOperate.Save.BySql(tv, Gval.Current.curBookName);
                    toTable(selectedItem, newName, Gval.Current.curBookName);
                }
                else
                {
                    Save.ToSingleXml(tv, ucTag);
                    //TreeOperate.Save.BySql(tv, ucTag);
                    toTable(selectedItem, newName, ucTag);
                }
                //TreeOperate.Save.SaveTree(tv);
            }


            public static void toTable(TreeViewItem selectedItem, string newName, string tableName)
            {
                if (tableName == "material")
                {
                    SqliteOperate.NewConnection(Gval.Base.AppPath + "/" + "material", "material.db");
                }

                tableName = "Tree_" + tableName;

                string sql = string.Format("update {0} set Header='{1}' where Uid = '{2}';", tableName, newName, selectedItem.Uid);
                SqliteOperate.ExecuteNonQuery(sql);

                //恢复默认连接
                SqliteOperate.NewConnection();
            }
        }
    }
}

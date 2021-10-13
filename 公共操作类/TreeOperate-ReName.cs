using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace 脸滚键盘
{
    static partial class TreeOperate
    {
        public static partial class ReName
        {
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
                    //在节点位置显现
                    Point p = selectedItem.TranslatePoint(new Point(), tv);
                    renameBox.Margin = new Thickness(p.X + 19, p.Y, 0, 0);
                    //隐藏节点会导致子节点也跟着隐藏，所以并不采用这个方法，而是把Header改为空值
                    renameBox.Visibility = Visibility.Visible;
                    renameBox.IsEnabled = true;
                    renameBox.Text = selectedItem.Header.ToString(); //新名字
                    renameBox.Tag = selectedItem.Header.ToString(); //旧名字
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
                    string oldName = renameBox.Tag.ToString();
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
                            SaveIt(tv, ucTag);
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
                            SaveIt(tv, ucTag);
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

            static void SaveIt(TreeView tv, string ucTag)
            {
                if (ucTag == "books")
                {
                    Save.FromBookTree.SaveCurBook(Gval.Current.curBookItem);
                    if (tv.SelectedItem == Gval.Current.curBookItem)
                    {
                        Save.FromBookTree.SaveRoot(tv);
                    }
                }
                else
                {
                    Save.ToSingleXml(tv, Gval.Current.curBookItem, ucTag);
                }
            }
        }
    }
}

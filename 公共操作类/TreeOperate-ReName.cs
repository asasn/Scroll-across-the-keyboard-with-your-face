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
            public static void Ready(TreeViewItem selectedItem, TextBox renameBox)
            {
                if (selectedItem != null && renameBox.Visibility == Visibility.Hidden)
                {
                    //在节点位置显现
                    Point p = selectedItem.TranslatePoint(new Point(), Gval.CurrentBook.curTv);
                    renameBox.Margin = new Thickness(p.X + 19, p.Y, 0, 0);
                    //隐藏节点会导致子节点也跟着隐藏，所以并不采用这个方法，而是把Header改为空值
                    renameBox.Visibility = Visibility.Visible;
                    renameBox.IsEnabled = true;
                    renameBox.Text = selectedItem.Header.ToString(); //新名字
                    renameBox.Tag = selectedItem.Header.ToString(); //旧名字
                    selectedItem.Header = string.Empty;
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
            public static void Do(TreeViewItem selectedItem, TextBox renameBox)
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

                    int level = GetLevel(selectedItem);

                    if (level == 3)
                    {
                        string fOld = Gval.CurrentBook.curTextFullName;
                        string fNew = Gval.CurrentBook.curVolumePath + "/" + newName + ".txt";

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
                            Save.FromBookTree.SaveCurBook(Gval.CurrentBook.curBookItem);
                        }
                    }

                    if (level == 2)
                    {
                        string oldVolumePath = Gval.CurrentBook.curVolumePath;
                        string newVolumePath = Gval.CurrentBook.curBookPath + "/" + newName;

                        if (true == FileOperate.IsFolderExists(newVolumePath))
                        {
                            Console.WriteLine("对应的分卷已经存在");
                            FillBcak(selectedItem, renameBox, oldName);
                            return;
                        }
                        else
                        {
                            //新名称回填
                            FillBcak(selectedItem, renameBox, newName);

                            FileOperate.renameDir(oldVolumePath, newVolumePath);
                            Save.FromBookTree.SaveCurBook(Gval.CurrentBook.curBookItem);
                        }
                    }

                    if (level == 1)
                    {
                        string oldBookPath = Gval.CurrentBook.curBookPath;
                        string newBookPath = Gval.CurrentBook.BooksPath + "/" + newName;

                        //对应的书籍目录已经存在的情况
                        if (true == FileOperate.IsFolderExists(newBookPath))
                        {
                            Console.WriteLine("对应的书籍已经存在");
                            FillBcak(selectedItem, renameBox, oldName);
                            return;
                        }
                        else
                        {
                            //新名称回填
                            FillBcak(selectedItem, renameBox, newName);

                            FileOperate.renameDir(oldBookPath, newBookPath);
                            TreeOperate.Save.FromBookTree.SaveRoot(Gval.CurrentBook.curTv);
                        }
                    }


                }
                //刷新工作区公共变量
                //【注意】本控件是内容的消费者而非生产者，所以在此更新公共变量时，需要填入DocTree的信息
                TreeOperate.ReNewCurrent(Gval.CurrentBook.curTv);
            }
        }
    }
}

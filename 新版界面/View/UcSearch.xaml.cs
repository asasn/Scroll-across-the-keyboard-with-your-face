using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using RootNS.Helper;
using RootNS.Model;
using RootNS.Workfolw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace RootNS.View
{
    /// <summary>
    /// uc_Searcher.xaml 的交互逻辑
    /// </summary>
    public partial class UcSearch : UserControl
    {
        public UcSearch()
        {
            InitializeComponent();
            Gval.View.Searcher = this;
        }


        /// <summary>
        /// 搜索参数初始化
        /// </summary>
        private List<Node> InitSearch()
        {
            List<Node> nodes = new List<Node>();
            if (CbMaterial.IsChecked == true)
            {
                //搜索资料库
                if (RbAll.IsChecked == true)
                {
                    Gval.MaterialBook.LoadForAllMaterialTabs();
                    nodes = Gval.MaterialBook.GetChapterNodes();
                }
                else
                {
                    List<Node> roots = new List<Node>() { Gval.MaterialBook.BoxExample, Gval.MaterialBook.BoxMaterial };
                    if (roots.Contains(Gval.MaterialBook.SelectedNode.RootNode))
                    {
                        nodes = Gval.MaterialBook.GetThisNodeChilds(Gval.MaterialBook.SelectedNode);
                    }
                }
            }
            else
            {
                //搜索当前书籍
                if (RbAll.IsChecked == true)
                {
                    Gval.CurrentBook.LoadForAllChapterTabs();
                    nodes = Gval.CurrentBook.GetChapterNodes();
                }
                else
                {
                    List<Node> roots = new List<Node>() { Gval.CurrentBook.BoxDraft, Gval.CurrentBook.BoxTemp, Gval.CurrentBook.BoxPublished };
                    if (roots.Contains(Gval.CurrentBook.SelectedNode.RootNode))
                    {
                        nodes = Gval.CurrentBook.GetThisNodeChilds(Gval.CurrentBook.SelectedNode);
                    }
                }
            }
            return nodes;
        }


        #region 执行搜索

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

            //初始化列表框
            ListBoxOfResults.Items.Clear();

            //输入检校（空字符串或者为 * 时退出）
            if (string.IsNullOrEmpty(TbKeyWords.Text.Trim()))
            {
                return;
            }

            //输入检校（正则表达式错误）
            if (CbRegex.IsChecked == true)
            {
                MatchCollection matchRets;
                try
                {
                    matchRets = Regex.Matches("", TbKeyWords.Text);
                }
                catch (Exception)
                {
                    FunctionPack.ShowMessageBox("正则表达式错误！");
                    return;
                }
            }
            List<Node> nodes = InitSearch();
            GetResultMain(nodes);
        }


        private void TbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        /// <summary>
        /// 递归获取结果集主函数
        /// </summary>
        /// <param name="baseNode"></param>
        private void GetResultMain(List<Node> nodes)
        {


            foreach (Node node in nodes)
            {
                AddToListBox(node);
            }
        }

        /// <summary>
        /// 把成功匹配的章节添加进列表框
        /// </summary>
        /// <param name="node"></param>
        void AddToListBox(Node node)
        {
            if (string.IsNullOrEmpty(node.Text))
            {
                return;
            }
            string[] Matches = new string[] { };
            string ListItemName = String.Empty;

            //正则模式
            if (CbRegex.IsChecked == true)
            {
                Matches = GetMatchRets(node.Text);
                ListItemName = string.Join(" ", Matches);
            }

            //与模式
            if (CbAnd.IsChecked == true)
            {
                Matches = GetModeAndRets(node.Text);
                ListItemName = string.Join(" ", Matches);
            }

            //或模式
            if (CbOr.IsChecked == true)
            {
                Matches = GetModeOrRets(node.Text);
                ListItemName = string.Join(" ", Matches);
            }


            if (Matches.Length > 99)
            {
                Console.WriteLine("匹配结果太多，无意义，所以丢弃结果不用");
                return;
            }

            //获取和定义列表项
            if (false == string.IsNullOrEmpty(ListItemName))
            {
                ListBoxItem lbItem = new ListBoxItem
                {
                    Content = node.Title + " >> " + ListItemName,
                    DataContext = node,
                    Tag = Matches
                };
                ListBoxOfResults.Items.Add(lbItem);
                SetItemToolTip(lbItem);
            }
        }

        /// <summary>
        /// 获取正则匹配结果列表
        /// </summary>
        /// <param name="nodeText"></param>
        /// <returns></returns>
        string[] GetMatchRets(string nodeText)
        {
            MatchCollection matchRets;
            try
            {
                matchRets = Regex.Matches(nodeText, TbKeyWords.Text);
            }
            catch (Exception)
            {
                Console.WriteLine("正则表达式错误！");
                return null;
            }

            if (string.IsNullOrEmpty(nodeText))
            {
                return new List<string>().ToArray();
            }


            if (matchRets.Count > 0)
            {
                List<string> listResult = new List<string>();
                foreach (Match item in matchRets)
                {
                    if (false == listResult.Contains(item.Value))
                    {
                        listResult.Add(item.Value);
                    }
                }
                return listResult.ToArray();
            }
            else
            {
                return new List<string>().ToArray();
            }
        }

        /// <summary>
        /// 获取与模式匹配结果列表
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <returns></returns>
        string[] GetModeAndRets(string NodeContent)
        {
            List<string> rets = new List<string>();
            string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();

            bool ret = true;
            foreach (var mystr in keysArray)
            {
                if (false == string.IsNullOrEmpty(mystr))
                {
                    //不匹配时跳出
                    if (false == IsInText(NodeContent, mystr))
                    {
                        ret = false;
                        break;
                    }
                }
            }
            if (ret == true)
            {
                return TbKeyWords.Text.Split(' ');
            }
            else
            {
                return new List<string>().ToArray();
            }
        }

        /// <summary>
        /// 获取或模式匹配结果列表
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <returns></returns>
        string[] GetModeOrRets(string NodeContent)
        {
            List<string> rets = new List<string>();
            string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            foreach (var mystr in keysArray)
            {
                if (false == string.IsNullOrEmpty(mystr))
                {
                    if (IsInText(NodeContent, mystr))
                    {
                        rets.Add(mystr);
                    }
                }
            }
            return rets.ToArray();
        }

        /// <summary>
        /// 判断文本当中是否包含某个字符串
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <param name="mystr"></param>
        /// <returns></returns>
        private bool IsInText(string NodeContent, string mystr)
        {
            //空节点时返回空
            if (string.IsNullOrEmpty(NodeContent))
            {
                return false;
            }

            string text = NodeContent;
            if (text.Contains(mystr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion



        /// <summary>
        /// </summary>
        /// 给列表项添加悬浮内容
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SetItemToolTip(ListBoxItem lbItem)
        {
            TextEditor tEdit = new TextEditor()
            {
                Width = 400,
                WordWrap = true,

                FontFamily = new FontFamily("宋体")
            };
            tEdit.Options.WordWrapIndentation = 4;
            tEdit.Options.InheritWordWrapIndentation = false;
            string lines = GetStrOnLines((lbItem.DataContext as Node).Text);
            tEdit.Text = lines;
            EditorHelper.SetColorRulesForSearchResult(tEdit, lbItem.Tag as string[]);
            ToolTip ttp = new ToolTip
            {
                Content = tEdit
            };
            lbItem.ToolTip = ttp;
        }

        //从当前行当中判断字符串是否存在
        private string GetStrOnLines(string nodeContent)
        {
            int counter = 0;
            string lines = string.Empty;

            string[] sArray = nodeContent.Split(new char[] { '\n' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            //正则模式
            if (CbRegex.IsChecked == true)
            {
                string[] keysArray = GetMatchRets(nodeContent);
                foreach (string line in sArrayNoEmpty)
                {
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            lines += line + "\n";
                            counter++;
                            break;
                        }
                    }
                }
            }

            //与模式
            if (CbAnd.IsChecked == true)
            {
                string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                foreach (string line in sArrayNoEmpty)
                {
                    bool isAll = false;
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            isAll = true;
                            break;
                        }
                    }
                    if (isAll == true)
                    {
                        lines += line + "\n";
                        counter++;
                    }
                }
            }

            //或模式
            if (CbOr.IsChecked == true)
            {
                string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                foreach (string line in sArrayNoEmpty)
                {
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            lines += line + "\n";
                            counter++;
                            break;
                        }
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxOfResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbItem = ListBoxOfResults.SelectedItem as ListBoxItem;
            Node node = lbItem.DataContext as Node;
            WSearchResult rtWin = new WSearchResult(node, lbItem.Tag as string[]);
            rtWin.ShowDialog();
        }
    }
}

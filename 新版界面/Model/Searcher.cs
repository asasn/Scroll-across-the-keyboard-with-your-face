using ICSharpCode.AvalonEdit;
using RootNS.Helper;
using RootNS.Workfolw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace RootNS.Model
{
    internal class Searcher : NotificationObject
    {
        public Searcher()
        {

        }

        public struct Item
        {
            public string Title;
            public string Content;
            public string[] Matches;
            public object ToolTip;
        }

        private ObservableCollection<Item> _results;

        public ObservableCollection<Item> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                RaisePropertyChanged(nameof(Results));
            }
        }


        private string _keyWords;

        public string KeyWords
        {
            get { return _keyWords; }
            set
            {
                _keyWords = value;
                RaisePropertyChanged(nameof(KeyWords));
            }
        }


        private bool _cbMaterial;

        public bool CbMaterial
        {
            get { return _cbMaterial; }
            set
            {
                _cbMaterial = value;
                RaisePropertyChanged(nameof(CbMaterial));
            }
        }

        private bool _rbAll = true;

        public bool RbAll
        {
            get { return _rbAll; }
            set
            {
                _rbAll = value;
                RaisePropertyChanged(nameof(RbAll));
            }
        }

        private bool _rbAnd;

        public bool RbAnd
        {
            get { return _rbAnd; }
            set
            {
                _rbAnd = value;
                RaisePropertyChanged(nameof(RbAnd));
            }
        }

        private bool _rbOr;

        public bool RbOr
        {
            get { return _rbOr; }
            set
            {
                _rbOr = value;
                RaisePropertyChanged(nameof(RbOr));
            }
        }


        private bool _rbRegex;

        public bool RbRegex
        {
            get { return _rbRegex; }
            set
            {
                _rbRegex = value;
                RaisePropertyChanged(nameof(RbRegex));
            }
        }


        enum SearchModel
        {
            与模式,
            或模式,
            正则模式,
        }

        private List<Node> InitSearch()
        {
            List<Node> nodes = new List<Node>();
            if (CbMaterial == true)
            {
                //搜索资料库
                if (RbAll == true)
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
                if (RbAll == true)
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


        private void Search()
        {
            //初始化列表框
            Results.Clear();

            //输入检校（空字符串或者为 * 时退出）
            if (string.IsNullOrEmpty(KeyWords.Trim()))
            {
                return;
            }

            //输入检校（正则表达式错误）
            if (RbRegex == true)
            {
                MatchCollection matchRets;
                try
                {
                    matchRets = Regex.Matches("", KeyWords);
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
            if (RbRegex == true)
            {
                Matches = GetMatchRets(node.Text);
                ListItemName = string.Join(" ", Matches);
            }

            //与模式
            if (RbAnd == true)
            {
                Matches = GetModeAndRets(node.Text);
                ListItemName = string.Join(" ", Matches);
            }

            //或模式
            if (RbOr == true)
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
                Item lbItem = new Item()
                {
                    Title = node.Title + " >> " + ListItemName,
                    Content = node.Text,
                    Matches = Matches
                };
                Results.Add(lbItem);
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
                matchRets = Regex.Matches(nodeText, KeyWords);
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
            string[] keysArray = KeyWords.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();

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
                return KeyWords.Split(' ');
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
            string[] keysArray = KeyWords.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
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




        /// <summary>
        /// </summary>
        /// 给列表项添加悬浮内容
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetItemToolTip(Item lbItem)
        {
            TextEditor tEdit = new TextEditor()
            {
                Width = 400,
                WordWrap = true,

                FontFamily = new FontFamily("宋体")
            };
            tEdit.Options.WordWrapIndentation = 4;
            tEdit.Options.InheritWordWrapIndentation = false;
            string lines = GetStrOnLines(lbItem.Content);
            tEdit.Text = lines;
            EditorHelper.SetColorRulesForSearchResult(tEdit, lbItem.Matches);
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
            if (RbRegex == true)
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
            if (RbAnd == true)
            {
                string[] keysArray = KeyWords.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
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
            if (RbOr == true)
            {
                string[] keysArray = KeyWords.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
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

        private void OpenResultWindow()
        {
        }

    }
}

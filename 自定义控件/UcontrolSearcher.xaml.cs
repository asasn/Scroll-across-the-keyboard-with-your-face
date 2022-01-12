using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using 脸滚键盘.控件方法类;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// uc_Searcher.xaml 的交互逻辑
    /// </summary>
    public partial class UcontrolSearcher : UserControl
    {
        public UcontrolSearcher()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(UcontrolSearcher), new PropertyMetadata(null));



        public TreeViewNode TopNode
        {
            get { return (TreeViewNode)GetValue(TopNodeProperty); }
            set { SetValue(TopNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopNodeProperty =
            DependencyProperty.Register("TopNode", typeof(TreeViewNode), typeof(UcontrolSearcher), new PropertyMetadata(null));




        public TreeViewNode CurNode
        {
            get { return (TreeViewNode)GetValue(CurNodeProperty); }
            set { SetValue(CurNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurNodeProperty =
            DependencyProperty.Register("CurNode", typeof(TreeViewNode), typeof(UcontrolSearcher), new PropertyMetadata(null));





        public string KeyWords
        {
            get { return (string)GetValue(KeyWordsProperty); }
            set { SetValue(KeyWordsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyWords.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyWordsProperty =
            DependencyProperty.Register("KeyWords", typeof(string), typeof(UcontrolSearcher), new PropertyMetadata(string.Empty));




        public string Pattern
        {
            get { return (string)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pattern.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register("Pattern", typeof(string), typeof(UcontrolSearcher), new PropertyMetadata(string.Empty));




        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(UcontrolSearcher), new PropertyMetadata(null));

        /// <summary>
        /// 匹配结果数组（字符串数组）
        /// </summary>
        private string[] Matches;



        /// <summary>
        /// 搜索参数初始化
        /// </summary>
        void InitSearch()
        {
            //初始化列表框
            ListBoxOfResults.Items.Clear();

            //每次搜索的关键词
            KeyWords = TbKeyWords.Text;
            Pattern = TbKeyWords.Text;

            if (CbMaterial.IsChecked == true)
            {
                //搜索资料库
                CurNode = Gval.Uc.TreeMaterial.CurNode;
                TopNode = Gval.Uc.TreeMaterial.TopNode;
            }
            else
            {
                //搜索当前书籍
                CurNode = Gval.Uc.TreeBook.CurNode;
                TopNode = Gval.Uc.TreeBook.TopNode;
            }
        }

        #region 执行搜索

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //搜索参数初始化
            InitSearch();

            //输入检校（空字符串时退出）
            if (string.IsNullOrEmpty(TbKeyWords.Text.Trim()))
            {
                return;
            }



            //生成搜索结果列表
            if (RadButton1.IsChecked == true)
            {
                GetResultMain(CurNode);
            }
            if (RadButton2.IsChecked == true)
            {
                GetResultMain(TopNode);
            }

        }

        private void TbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        #endregion

        #region 在列表框中添加结果集

        /// <summary>
        /// 获取结果集主函数
        /// </summary>
        /// <param name="baseNode"></param>
        private void GetResultMain(TreeViewNode baseNode)
        {
            if (baseNode == null)
            {
                return;
            }
            if (baseNode.IsDir == true)
            {
                foreach (TreeViewNode node in baseNode.ChildNodes)
                {
                    GetResultMain(node);
                }
            }
            else
            {
                AddToListBox(baseNode);
            }
        }

        /// <summary>
        /// 把成功匹配的章节添加进列表框
        /// </summary>
        /// <param name="node"></param>
        void AddToListBox(TreeViewNode node)
        {
            if (string.IsNullOrEmpty(node.NodeContent))
            {
                return;
            }

            string ListItemName = String.Empty;

            //正则模式
            if (CbRegex.IsChecked == true)
            {

                Matches = GetMatchRets(node.NodeContent);
                ListItemName = string.Join(" ", Matches);
            }

            //与模式
            if (CbAnd.IsChecked == true)
            {
                Matches = GetModeAndRets(node.NodeContent);
                ListItemName = string.Join(" ", Matches);
            }

            //或模式
            if (CbOr.IsChecked == true)
            {
                Matches = GetModeOrRets(node.NodeContent);
                ListItemName = string.Join(" ", Matches);
            }

            //获取和定义列表项
            if (false == string.IsNullOrEmpty(ListItemName))
            {
                ListBoxItem lbItem = new ListBoxItem
                {
                    Content = node.NodeName + " >> " + ListItemName,
                    DataContext = node.NodeContent,
                    Tag = Matches
                };
                SetItemToolTip(lbItem);
                ListBoxOfResults.Items.Add(lbItem);
            }
        }

        /// <summary>
        /// 获取正则匹配结果列表
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <returns></returns>
        string[] GetMatchRets(string NodeContent)
        {
            if (string.IsNullOrEmpty(NodeContent))
            {
                return new List<string>().ToArray();
            }

            MatchCollection matchRets = Regex.Matches(NodeContent, Pattern);
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

        #endregion

        #region 关键词变色
        /// <summary>
        /// 关键词变色主函数
        /// </summary>
        private void SetKeyWordsColor()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Resourses/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            //清空文件内的自带规则
            textEditor.SyntaxHighlighting.MainRuleSet.Rules.Clear();
            textEditor.SyntaxHighlighting.MainRuleSet.Spans.Clear();

            IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
            foreach (string word in Matches)
            {
                AddKeyword(rules, word, "搜索");
            }
        }

        /// <summary>
        /// 添加一个变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        void AddKeyword(IList<HighlightingRule> rules, string keyword, string colorName)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }
            HighlightingRule rule = new HighlightingRule
            {
                Color = textEditor.SyntaxHighlighting.GetNamedColor(colorName),
                Regex = new Regex(keyword)
            };
            rules.Add(rule);
        }

        #endregion

        TextEditor textEditor = new TextEditor();
        /// <summary>
        /// 给列表项添加悬浮内容
        /// </summary>
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
            textEditor = tEdit;
            string lines = GetStrOnLines(lbItem.DataContext.ToString());
            textEditor.Text = lines;
            SetKeyWordsColor();

            ToolTip ttp = new ToolTip
            {
                Content = textEditor
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
            if (CbOr.IsChecked == true)
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

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxOfResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbItem = ListBoxOfResults.SelectedItem as ListBoxItem;
            WindowSearchRet rtWin = new WindowSearchRet(lbItem, (string[])lbItem.Tag);
            rtWin.ShowDialog();
        }
    }
}

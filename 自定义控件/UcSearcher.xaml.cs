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
    public partial class UcSearcher : UserControl
    {
        public UcSearcher()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(UcSearcher), new PropertyMetadata(null));



        public TreeViewNode TopNode
        {
            get { return (TreeViewNode)GetValue(TopNodeProperty); }
            set { SetValue(TopNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopNodeProperty =
            DependencyProperty.Register("TopNode", typeof(TreeViewNode), typeof(UcSearcher), new PropertyMetadata(new TreeViewNode { Uid = "", IsDir = true }));




        public TreeViewNode CurNode
        {
            get { return (TreeViewNode)GetValue(CurNodeProperty); }
            set { SetValue(CurNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurNodeProperty =
            DependencyProperty.Register("CurNode", typeof(TreeViewNode), typeof(UcSearcher), new PropertyMetadata(new TreeViewNode()));





        public string KeyWords
        {
            get { return (string)GetValue(KeyWordsProperty); }
            set { SetValue(KeyWordsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyWords.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyWordsProperty =
            DependencyProperty.Register("KeyWords", typeof(string), typeof(UcSearcher), new PropertyMetadata(string.Empty));




        public string Pattern
        {
            get { return (string)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pattern.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register("Pattern", typeof(string), typeof(UcSearcher), new PropertyMetadata(string.Empty));




        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(UcSearcher), new PropertyMetadata(null));


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //初始化列表框
            lb.Items.Clear();

            //每次搜索的关键词
            KeyWords = tbKeyWords.Text;
            Pattern = tbKeyWords.Text;

            if (cbMaterial.IsChecked == true)
            {
                //搜索资料库
                //CurNode = Gval.Uc.TreeMaterial.CurNode;
                //TopNode = Gval.Uc.TreeMaterial.TopNode;
            }
            else
            {
                //搜索当前书籍
                CurNode = Gval.Uc.TreeBook.CurNode;
                TopNode = Gval.Uc.TreeBook.TopNode;
            }

            if (radButton1.IsChecked == true)
            {
                //SearchRetWindow rtWin = new SearchRetWindow(CurNode, UcTag, KeyWords);
                //rtWin.ShowDialog();

                GetResultList(CurNode);

            }
            if (radButton2.IsChecked == true)
            {
                //SearchRetWindow rtWin = new SearchRetWindow(TopNode, UcTag, KeyWords);
                //rtWin.ShowDialog();
                GetResultList(TopNode);
            }


        }

        void AddToListBox(TreeViewNode node)
        {
            string ListItemName = String.Empty;
            ListItemName += ' ';


            if (cbRegex.IsChecked == true)
            {
                //正则模式
                string[] sArray = GetMatchRets(node.NodeContent);
                foreach (var mystr in sArray)
                {
                    if (false == string.IsNullOrEmpty(mystr))
                    {
                        ListItemName += mystr + ' ';
                    }
                }
            }
            else
            {
                //简易模式
                string[] keysArray = KeyWords.Split(new char[] { ' ', '|' });
                foreach (var mystr in keysArray)
                {
                    if (false == string.IsNullOrEmpty(mystr))
                    {
                        if (IsInFile(node, mystr))
                        {
                            ListItemName += mystr + ' ';
                        }
                    }
                }

                string[] keysArray1 = Regex.Split(KeyWords, "&", RegexOptions.IgnoreCase);
                string[] keysArray2 = Regex.Split(KeyWords, "and", RegexOptions.IgnoreCase);
                //List<string> c = new List<string>();
                //c.AddRange(keysArray1);
                //c.AddRange(keysArray2);
                //string[] keysArrayAnd = c.ToArray();
                string[] keysArrayAnd = keysArray1.Union(keysArray2).ToArray<string>(); //剔除重复项 

                bool ret = true;
                foreach (var mystr in keysArrayAnd)
                {
                    if (false == string.IsNullOrEmpty(mystr))
                    {
                        if (false == IsInFile(node, mystr))
                        {
                            ret = false;
                        }
                    }
                }
                if (ret == true)
                {
                    ListItemName += KeyWords + ' ';
                }
            }



            if (ListItemName != " ")
            {
                ListBoxItem lbItem = new ListBoxItem();
                lbItem.Content = node.NodeName + ListItemName;
                lb.Items.Add(lbItem);
                lbItem.DataContext = node.NodeContent;
                SetItemToolTip(lbItem);
            }
        }

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


        //判断文件当中是否包含某个字符串
        private bool IsInFile(TreeViewNode node, string mystr)
        {
            //空节点时返回空
            if (string.IsNullOrEmpty(node.NodeContent))
            {
                return false;
            }

            string text = node.NodeContent;
            if (text.Contains(mystr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetResultList(TreeViewNode baseNode)
        {
            if (baseNode.IsDir == true)
            {
                foreach (TreeViewNode node in baseNode.ChildNodes)
                {
                    GetResultList(node);
                }
            }
            else
            {
                AddToListBox(baseNode);
            }
        }

        private void tbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }


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
            SetKeyWordsColor();

            string lines = GetStrOnLines(lbItem.DataContext.ToString());
            textEditor.Text += lines + "\n";
            ToolTip ttp = new ToolTip();
            ttp.Content = textEditor;
            lbItem.ToolTip = ttp;
        }

        private void SetKeyWordsColor()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Resourse/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            //清空文件内的自带规则
            textEditor.SyntaxHighlighting.MainRuleSet.Rules.Clear();
            textEditor.SyntaxHighlighting.MainRuleSet.Spans.Clear();

            string[] keysArray = KeyWords.Split(' ');
            IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
            foreach (string word in keysArray)
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

            SetRules(rules, keyword, colorName);
        }

        void SetRules(IList<HighlightingRule> rules, string keyword, string colorName)
        {
            HighlightingRule rule = new HighlightingRule();
            rule.Color = textEditor.SyntaxHighlighting.GetNamedColor(colorName);
            rule.Regex = new Regex(keyword);
            rules.Add(rule);
        }



        //从当前行当中判断字符串是否存在
        private string GetStrOnLines(string nodeContent)
        {
            int counter = 0;
            string lines = string.Empty;

            string[] sArray = nodeContent.Split(new char[] { '\n' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            if (cbRegex.IsChecked == true)
            {
                //正则模式
                foreach (string line in sArrayNoEmpty)
                {
                    string[] keysArray = GetMatchRets(nodeContent);
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
            else
            {
                //简易模式
                foreach (string line in sArrayNoEmpty)
                {
                    string[] keysArray = KeyWords.Split(new char[] { ' ', '|' });
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            lines += line + "\n";
                            counter++;
                            break;
                        }
                    }
                    string[] keysArray2 = KeyWords.Split(new char[] { '&' });
                    bool isAll = true;
                    foreach (string mystr in keysArray)
                    {
                        if (false == line.Contains(mystr))
                        {
                            isAll = false;
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

            return lines;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbItem = lb.SelectedItem as ListBoxItem;
            SearchRetWindow rtWin = new SearchRetWindow(TopNode, lbItem, UcTag, KeyWords);
            rtWin.ShowDialog();
        }
    }
}

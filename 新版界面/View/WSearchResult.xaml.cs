using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using RootNS.Helper;
using RootNS.Model;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace RootNS.View
{
    /// <summary>
    /// SearchRetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WSearchResult : Window
    {
        public WSearchResult(Node node, string[] keyWords)
        {
            InitializeComponent();
            this.DataContext = node;
            EditorHelper.SetColorRulesForSearchResult(TextEditor, keyWords);
            TextEditor.Text = (node).Text;
        }
    }
}

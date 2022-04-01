using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using RootNS.Model;

namespace RootNS.Brick
{
    /// <summary>
    /// MyEditor.xaml 的交互逻辑
    /// </summary>
    public partial class MyEditor : UserControl
    {
        public MyEditor()
        {
            InitializeComponent();
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFormat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCopyTitle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveText_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextEditor_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TextEditor_MouseHoverStopped(object sender, MouseEventArgs e)
        {

        }

        private void TextEditor_MouseHover(object sender, MouseEventArgs e)
        {

        }

        private void TextEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void TextEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void TextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextEditor).Text = (this.DataContext as Node).Content;
        }
    }
}

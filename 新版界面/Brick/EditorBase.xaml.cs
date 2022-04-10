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
using RootNS.Behavior;
using RootNS.Model;

namespace RootNS.Brick
{
    /// <summary>
    /// MyEditor.xaml 的交互逻辑
    /// </summary>
    public partial class EditorBase : UserControl
    {
        public EditorBase()
        {
            InitializeComponent();
        }



        public string MainText
        {
            get { return (string)GetValue(MainTextProperty); }
            set { SetValue(MainTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register("MainText", typeof(string), typeof(EditorBase), new PropertyMetadata("　　"));




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

        private void ThisTextEditor_TextChanged(object sender, EventArgs e)
        {
            (this.DataContext as Node).Text = ThisTextEditor.Text;
        }

        private void ThisTextEditor_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void ThisTextEditor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ThisTextEditor_MouseHoverStopped(object sender, MouseEventArgs e)
        {

        }

        private void ThisTextEditor_MouseHover(object sender, MouseEventArgs e)
        {

        }

        private void ThisTextEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ThisTextEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Gval.OpeningDocList.Remove((sender as Button).DataContext as Node);
        }

 

        private void ThisControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Node node = this.DataContext as Node;
            if (string.IsNullOrWhiteSpace(node.Text) == true)
            {
                ThisTextEditor.Text = "　　";
            }
            else
            {
                ThisTextEditor.Text = (this.DataContext as Node).Text;
            }
        }
    }
}

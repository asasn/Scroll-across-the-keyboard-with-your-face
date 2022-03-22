using NSMain.Bricks;
using System.Windows;

namespace NSMain.TreeViewPlus
{
    /// <summary>
    /// MaterialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WMaterial : Window
    {
        public WMaterial()
        {
            InitializeComponent();
        }

        private void UcEditor_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = GlobalVal.Uc.TreeMaterial.CurNode.NodeName;
            MyEditor.DataContext = GlobalVal.Uc.TreeMaterial.CurNode;
            MyEditor.LoadChapter("index", "material");
        }
    }
}
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace RootNS.Brick
{
    /// <summary>
    /// MyEditor.xaml 的交互逻辑
    /// </summary>
    public partial class MyEditor : HandyControl.Controls.TabControl
    {
        public MyEditor()
        {
            InitializeComponent();
        }

        private void EditorTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.EditorTabControl = this;
            Gval.OpeningDocList.CollectionChanged += OpenedDocList_CollectionChanged;
        }

        private void OpenedDocList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Console.WriteLine("从列表中删除，关闭了");
            }
            
        }

        private void ThisControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //(ThisControl.SelectedItem as EditorBase).ThisTextEditor.Text = (ThisControl.SelectedItem as Node).Text;
        }

    }
}

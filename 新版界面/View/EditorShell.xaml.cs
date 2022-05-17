using RootNS.Helper;
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
using System.Windows.Threading;

namespace RootNS.View
{
    /// <summary>
    /// MyEditor.xaml 的交互逻辑
    /// </summary>
    public partial class EditorShell : UserControl
    {
        public EditorShell()
        {
            InitializeComponent();
        }

        private void EditorTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.EditorTabControl = sender as HandyControl.Controls.TabControl;
            Gval.OpeningDocList.CollectionChanged += OpenedDocList_CollectionChanged;
        }

        private void ThisTabControl_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenedDocList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Node stuff = (Node)e.NewItems[0];
                Editorkernel editorBase = new Editorkernel
                {
                    DataContext = stuff,
                };
                HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
                {
                    Uid = stuff.Uid,
                    IsSelected = true,
                    Content = editorBase
                };
                editorBase.Tag = tabItem;//携带父容器对象以供关闭方法调用
                tabItem.Closing += TabItem_Closing;
                tabItem.Closed += TabItem_Closed;
                ThisTabControl.Items.Add(tabItem);//创建新的编辑器页面
                Binding textBinding = new Binding
                {
                    Source = stuff,
                    Path = new PropertyPath("Title"),
                    Mode = BindingMode.TwoWay
                };
                tabItem.SetBinding(HeaderedItemsControl.HeaderProperty, textBinding);//对绑定目标的目标属性进行绑定 

                //因为在TabControl中，每次切换的时候都会触发Loaded事件，故而一些初始化步骤放在这里
                if (string.IsNullOrWhiteSpace(stuff.Text) == true)
                {
                    stuff.Text = "　　";
                }
                EditorHelper.SetColorRulesForCards(editorBase.ThisTextEditor);
                editorBase.ThisTextEditor.Text = stuff.Text;
                EditorHelper.MoveToEnd(editorBase.ThisTextEditor);
                editorBase.BtnSaveDoc.IsEnabled = false;
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //Console.WriteLine("关闭，从列表中删除");
            }
        }

        private void TabItem_Closed(object sender, EventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = sender as HandyControl.Controls.TabItem;
            Editorkernel editorBase = tabItem.Content as Editorkernel;
            Gval.OpeningDocList.Remove(editorBase.DataContext as Node);

            //清空各种显示
            EditorHelper.RefreshIsContainFlagForAllCardsBox(string.Empty);
            (Gval.View.UcShower.DataContext as Shower).Clear();
        }

        private void TabItem_Closing(object sender, EventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = sender as HandyControl.Controls.TabItem;
            Editorkernel editorBase = tabItem.Content as Editorkernel;
            if (editorBase.BtnSaveDoc.IsEnabled == true)
            {
                MessageBoxResult dr = MessageBox.Show("该章节尚未保存\n要在退出前保存更改吗？", "Tip", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (dr == MessageBoxResult.Yes)
                {
                    editorBase.BtnSaveText_Click(null, null);
                }
                if (dr == MessageBoxResult.No)
                {

                }
                if (dr == MessageBoxResult.Cancel)
                {
                    (e as HandyControl.Data.CancelRoutedEventArgs).Cancel = true;
                }
            }
        }


    }
}

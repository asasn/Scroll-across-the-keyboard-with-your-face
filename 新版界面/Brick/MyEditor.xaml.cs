﻿using RootNS.Model;
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
    public partial class MyEditor : UserControl
    {
        public MyEditor()
        {
            InitializeComponent();
        }

        private void EditorTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.EditorTabControl = sender as HandyControl.Controls.TabControl;
            Gval.OpeningDocList.CollectionChanged += OpenedDocList_CollectionChanged;
        }

        private void OpenedDocList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Node stuff = (Node)e.NewItems[0];
                EditorBase editorBase = new EditorBase
                {
                    DataContext = stuff,
                };
                HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
                {
                    Uid = stuff.Uid,
                    IsSelected = true,
                    Content = editorBase
                };
                tabItem.Closing += TabItem_Closing;
                tabItem.Closed += TabItem_Closed;
                ThisTabControl.Items.Add(tabItem);
                Binding textBinding = new Binding
                {
                    Source = stuff,
                    Path = new PropertyPath("Title"),
                    Mode = BindingMode.TwoWay
                };
                tabItem.SetBinding(HeaderedItemsControl.HeaderProperty, textBinding);//对绑定目标的目标属性进行绑定 
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Console.WriteLine("从列表中删除，关闭了");
            }
        }

        private void TabItem_Closed(object sender, EventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = sender as HandyControl.Controls.TabItem;
            EditorBase editorBase = tabItem.Content as EditorBase;
            Gval.OpeningDocList.Remove(editorBase.DataContext as Node);
        }

        private void TabItem_Closing(object sender, EventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = sender as HandyControl.Controls.TabItem;
            EditorBase editorBase = tabItem.Content as EditorBase;
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

using RootNS.Helper;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RootNS.View
{
    /// <summary>
    /// NodeItemForSecens.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForStory : UserControl
    {
        public NodeItemForStory()
        {
            InitializeComponent();
        }


        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (this.DataContext as Node).FinishRename();
                e.Handled = true;//防止触发对应的快捷键
            }
        }

        private void TbReName_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as Node).FinishRename();
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Node node = ((this.DataContext as Node).Extra as Summary).Node;
            if (node != null)
            {
                node.CheckChildNodes();
                node.CheckParentNodes();
            }
        }




        private void TbReName_Loaded(object sender, RoutedEventArgs e)
        {
            ((this.DataContext as Node).Extra as Summary).CanSave = false;
        }


        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.CurrentBook.LoadForAllNoteTabs();
            Summary secen = new Summary
            {
                Node = this.DataContext as Node
            };
            if (NewtonsoftJsonHelper.JsonToObject<Summary>(secen.Node.Summary) != null)
            {
                secen.Json = NewtonsoftJsonHelper.JsonToObject<Summary.JsonData>(secen.Node.Summary);
            }

            secen.Time = secen.Json.Time;
            secen.Place = secen.Json.Place;
            (this.DataContext as Node).Extra = secen;

            foreach (string uid in secen.Json.Secens.ToList())
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    if (uid == node.Uid)
                    {
                        secen.Secens.Add(node);
                    }
                }
            }

        }

        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((this.DataContext as Node).IsChecked == true)
            {
                return;
            }
            StoryWindow storyWindow = new StoryWindow();
            storyWindow.DataContext = this.DataContext as Node;
            storyWindow.GMian.DataContext = (this.DataContext as Node).Extra;
            Workfolw.ViewSet.ForViewPointX(storyWindow, Gval.View.TabNote, -6);
            Workfolw.ViewSet.ForViewPointY(storyWindow, this, 50);
            storyWindow.ShowDialog();
        }
    }
}

using RootNS.Helper;
using RootNS.Model;
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

namespace RootNS.View
{
    /// <summary>
    /// NodeItemForDoc.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForTopic : UserControl
    {
        public NodeItemForTopic()
        {
            InitializeComponent();
        }
        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            Topic topic = new Topic();
            if (JsonHelper.JsonToObj<Topic>((this.DataContext as Node).Summary) != null)
            {
                topic = JsonHelper.JsonToObj<Topic>((this.DataContext as Node).Summary);
            }
            (this.DataContext as Node).Extra = topic;

            topic.Subject.PropertyChanged += PropertyChanged;
            topic.Style.PropertyChanged += PropertyChanged; 
            topic.AwardPoints.PropertyChanged += PropertyChanged;
            topic.Protagonist.PropertyChanged += PropertyChanged;
            topic.IncentiveEvent.PropertyChanged += PropertyChanged;
            topic.Overreaction.PropertyChanged += PropertyChanged;
            topic.Suspenses.PropertyChanged += PropertyChanged;
            topic.EnvObstruction.PropertyChanged += PropertyChanged;
            topic.Volumes.PropertyChanged += PropertyChanged;
            topic.Roles.PropertyChanged += PropertyChanged;
            topic.Goldfingers.PropertyChanged += PropertyChanged;
            topic.Clues.PropertyChanged += PropertyChanged;
            topic.Levels.PropertyChanged += PropertyChanged;
            topic.WorldInfo.PropertyChanged += PropertyChanged;
            topic.Sets.PropertyChanged += PropertyChanged;
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((this.DataContext as Node).Extra as Topic).CanSave = true;
        }

        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            TopicWindow win = new TopicWindow
            {
                DataContext = this.DataContext
            };
            win.GMian.DataContext = (this.DataContext as Node).Extra;
            Workfolw.ViewSet.ForViewPointX(win, this, (int)(-(win.Width / 2)));
            Workfolw.ViewSet.ForViewPointY(win, this, 50);
            win.ShowDialog();
        }

        private void ThisControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as Node).IsSelected = true;
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


    }
}

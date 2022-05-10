using RootNS.Helper;
using RootNS.Model;
using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// TopicWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TopicWindow : Window
    {
        public TopicWindow()
        {
            InitializeComponent();
        }




        private void TbShowTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;
        }

        private void TbShowContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //SaveTopic(GMian.DataContext as Topic, this.DataContext as Node);

            (GMian.DataContext as Topic).Save(this.DataContext as Node, TbShowContent.Text);
        }
        //private void SaveTopic(Topic topic, Node node)
        //{
        //    //清除tip.title为空的项目
        //    Card.Line[] lines = { (topic).Subject, (topic).Style, (topic).Volumes, (topic).Roles, (topic).SellPoints, (topic).Goldfingers, (topic).Clues, (topic).WorldInfo, (topic).Sets };
        //    foreach (Card.Line line in lines)
        //    {
        //        foreach (Card.Tip tip in line.Tips.ToList())
        //        {
        //            if (string.IsNullOrWhiteSpace(tip.Title))
        //            {
        //                line.Tips.Remove(tip);
        //            }
        //        }
        //    }
        //    node.Text = TbShowContent.Text;
        //    string json = JsonHelper.ObjToJson(topic);
        //    DataOut.UpdateNodeProperty(node, nameof(Node.Text), node.Text);
        //    DataOut.UpdateNodeProperty(node, nameof(Node.Summary), json);
        //    topic.CanSave = false;
        //}

        private void TbShowTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && BtnSave.IsEnabled == true)
            {
                BtnSave.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (BtnSave.IsEnabled == true)
            {
                MessageBoxResult dr = MessageBox.Show("有数据尚未保存\n要在退出前保存更改吗？", "Tip", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (dr == MessageBoxResult.Yes)
                {
                    BtnSave_Click(null, null);
                }
                if (dr == MessageBoxResult.No)
                {

                }
                if (dr == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ThisWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BtnSave.IsEnabled = false;
        }
    }
}

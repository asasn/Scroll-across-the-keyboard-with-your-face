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

namespace 脸滚键盘
{
    /// <summary>
    /// uc_Editor.xaml 的交互逻辑
    /// </summary>
    public partial class uc_Editor : UserControl
    {
        public uc_Editor()
        {
            InitializeComponent();
        }

        void LoadFromTextFile()
        {
            if (true == FileOperate.IsFileExists(Gval.Current.curItemPath))
            {
                tb.Text = FileOperate.ReadFromTxt(Gval.Current.curItemPath);
                chapterNameBox.Text = Gval.Current.curItem.Header.ToString();
                volumeNameBox.Text = Gval.Current.curVolumeItem.Header.ToString();
                bookNameBox.Text = Gval.Current.curBookItem.Header.ToString();
            }
        }

        /// <summary>
        /// DataContext绑定了当前指向的curItem，因此将其更改事件作为curItem的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null == Gval.Current.curBookItem)
                return;
            //获取当前文件名
            if (true == FileOperate.IsFileExists(Gval.Current.curItemPath))
            {
                LoadFromTextFile();
                uc.IsEnabled = true;
            }
            else
            {
                uc.IsEnabled = false;
            }
        }

        private void tb_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}

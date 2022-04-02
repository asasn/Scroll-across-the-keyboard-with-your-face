using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public static class Gval
    {
        public static Book CurrentBook { get; set; } = new Book("测试书籍");
        public static Material Material { get; set; } = new Material();

        /// <summary>
        /// 打开文档的集合
        /// </summary>
        public static ObservableCollection<Node> OpenedDocList { set; get; } = new ObservableCollection<Node>();

        public static HandyControl.Controls.TabControl EditorTabControl;
    }
}

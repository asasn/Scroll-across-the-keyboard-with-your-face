using RootNS.Brick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public class BookBase : NotificationObject
    {
        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public void Clear()
        {
            CardRole.ChildNodes.Clear();
            CardOther.ChildNodes.Clear();
            CardWorld.ChildNodes.Clear();
            MapPoints.ChildNodes.Clear();
        }

        private string _uid;
        /// <summary>
        /// 书籍标识码
        /// </summary>
        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                this.RaisePropertyChanged("Uid");
            }
        }

        private string _name;
        /// <summary>
        /// 书名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
                string imgPath = Gval.Path.Books + "/" + _name + ".jpg";
                if (false == CFileOperate.IsFileExists(imgPath))
                {
                    this.CoverPath = Gval.Path.Resourses + "/nullbookface.jpg";
                }
            }
        }

        private string _coverpath;
        /// <summary>
        /// 封面路径
        /// </summary>
        public string CoverPath
        {
            get { return _coverpath; }
            set
            {
                _coverpath = value;
                this.RaisePropertyChanged("CoverPath");
            }
        }

        /// <summary>
        /// 工作区
        /// </summary>
        public enum WorkSpace
        {
            当前 = 0,
            公共 = 1
        }

        /// <summary>
        /// 信息卡片TabItem标志
        /// </summary>
        public enum CardTabName
        {
            角色 = 0,
            其他 = 1,
            世界 = 2
        }

        private int _index;
        /// <summary>
        /// 索引序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                this.RaisePropertyChanged("Index");
            }
        }


        private string _summary;
        /// <summary>
        /// 书籍简介
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                this.RaisePropertyChanged("Summary");
            }
        }

        private Node _selectedNode;
        /// <summary>
        /// 选中的节点
        /// </summary>
        public Node SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                _selectedNode = value;
                this.RaisePropertyChanged("SelectedNode");
            }
        }


        private bool _isDel;

        public bool IsDel
        {
            get { return _isDel; }
            set
            {
                _isDel = value;
                this.RaisePropertyChanged(nameof(IsDel));
            }
        }


        #region 信息卡


        public Node CardRole { set; get; } = new Node();
        public Node CardOther { set; get; } = new Node();
        public Node CardWorld { set; get; } = new Node();
        public Node MapPoints { set; get; } = new Node();
        public void LoadForCardsBox(TabControl tabControl, WorkSpace workSpace)
        {
            CardTabName flag = (CardTabName)tabControl.SelectedIndex;
            string itemName = Enum.GetName(typeof(CardTabName), tabControl.SelectedIndex);
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = CardRole;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = CardOther;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = CardWorld;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)flag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = workSpace.ToString() + flag.ToString() });
                }
            }
        }
        #endregion

    }
}

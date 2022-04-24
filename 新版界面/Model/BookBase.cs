using RootNS.Helper;
using RootNS.View;
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
        public BookBase()
        {
            this.PropertyChanged += BookBase_PropertyChanged;
        }

        private void BookBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.InitRootNodes(this.Name);
            }
        }

        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootNodes(string bookName)
        {
            Card[] rootCards = { this.CardRole, this.CardOther, this.CardWorld };
            foreach (Card card in rootCards)
            {
                card.OwnerName = bookName;
                card.Owner = this;
            }
        }

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




        private string _uid = Guid.NewGuid().ToString();
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
                if (false == IOHelper.IsFileExists(imgPath))
                {
                    this.CoverPath = "../Assets/nullbookface.jpg";
                }
            }
        }

        private long _currentYear;
        /// <summary>
        /// 本书剧情年份
        /// </summary>
        public long CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                this.RaisePropertyChanged("CurrentYear");
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
        /// 信息卡片TabItem标志
        /// </summary>
        public enum CardTabName
        {
            角色,
            其他,
            世界,
        }


        /// <summary>
        /// 地图TabItem标志
        /// </summary>
        public enum MapTabName
        {
            地图,
        }



        #region 信息卡
        public Card CardRole { set; get; } = new Card() { Uid = String.Empty, TabName = CardTabName.角色.ToString() };
        public Card CardOther { set; get; } = new Card() { Uid = String.Empty, TabName = CardTabName.其他.ToString() };
        public Card CardWorld { set; get; } = new Card() { Uid = String.Empty, TabName = CardTabName.世界.ToString() };

        #endregion

        #region 地图
        public Node MapPoints { set; get; } = new Node() { Uid = String.Empty, TabName = MapTabName.地图.ToString() };
        #endregion

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

        private Node _selectedNode = new Node();
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

        /// <summary>
        /// 从根节点中递归载入文档节点（IsDir == false）列表
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="pNode"></param>
        /// <returns></returns>
        protected List<Node> GetTreeNodes(List<Node> nodes, Node pNode)
        {
            foreach (Node node in pNode.ChildNodes)
            {
                if (node.IsDir == false)
                {
                    nodes.Add(node);
                }
                GetTreeNodes(nodes, node);
            }
            return nodes;
        }


        /// <summary>
        /// 从当前节点中递归载入文档节点（IsDir == false）列表
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public List<Node> GetThisNodeChilds(Node node)
        {
            List<Node> nodes = new List<Node>();
            nodes.Add(node);//自身先加上
            GetTreeNodes(nodes, node);
            return nodes;
        }

        /// <summary>
        /// 载入所有信息卡
        /// </summary>
        public void LoadForAllCardTabs()
        {
            if (CardRole.ChildNodes.Count == 0)
            {
                DataIn.FillInCards(CardRole);
            }
            if (CardOther.ChildNodes.Count == 0)
            {
                DataIn.FillInCards(CardOther);
            }
            if (CardWorld.ChildNodes.Count == 0)
            {
                DataIn.FillInCards(CardWorld);
            }
        }
    }
}

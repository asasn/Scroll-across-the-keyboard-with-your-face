using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public enum CardItemFlag
        {
            角色 = 0,
            其他 = 1,
            世界 = 2
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                _itemIndex = value;
                this.RaisePropertyChanged("ItemIndex");
            }
        }



        #region 信息卡
        public Node CardRole { set; get; } = new Node();
        public Node CardOther { set; get; } = new Node();
        public Node CardWorld { set; get; } = new Node();

        public void LoadForCardsBox(WorkSpace workSpace, int index)
        {
            ItemIndex = index;
            if (index == (int)CardItemFlag.角色)
            {
                LoadCardsBox(workSpace, this.CardRole, CardItemFlag.角色);
            }
            if (index == (int)CardItemFlag.其他)
            {
                LoadCardsBox(workSpace, this.CardOther, CardItemFlag.其他);
            }
            if (index == (int)CardItemFlag.世界)
            {
                LoadCardsBox(workSpace, this.CardWorld, CardItemFlag.世界);
            }
        }

        private void LoadCardsBox(WorkSpace workSpace, Node rootNode, CardItemFlag partFlag)
        {
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)partFlag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = workSpace.ToString() + partFlag.ToString() });
                }
            }
        }
        #endregion
    }
}

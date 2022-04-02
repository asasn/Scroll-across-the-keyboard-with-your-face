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
        public BookBase()
        {
            Clear();
        }

        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public void Clear()
        {
            CardRole.Clear();
            CardOther.Clear();
            CardWorld.Clear();
        }

        private string _uid;

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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public enum WorkSpace
        {
            当前 = 0,
            公共 = 1
        }

        public enum CardItemTag
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
        public ObservableCollection<Node> CardRole { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> CardOther { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> CardWorld { set; get; } = new ObservableCollection<Node>();

        public void LoadForCardsBox(WorkSpace workSpace, int index)
        {
            ItemIndex = index;
            if (index == (int)CardItemTag.角色)
            {
                LoadCardsBox(workSpace, this.CardRole, CardItemTag.角色);
            }
            if (index == (int)CardItemTag.其他)
            {
                LoadCardsBox(workSpace, this.CardOther, CardItemTag.其他);
            }
            if (index == (int)CardItemTag.世界)
            {
                LoadCardsBox(workSpace, this.CardWorld, CardItemTag.世界);
            }
        }

        private void LoadCardsBox(WorkSpace workSpace, ObservableCollection<Node> cards, CardItemTag partTag)
        {
            if (cards.Count == 0)
            {
                for (int i = 0; i <= (int)partTag; i++)
                {
                    cards.Add(new Node(workSpace.ToString() + partTag.ToString()));
                }
            }
        }
        #endregion
    }
}

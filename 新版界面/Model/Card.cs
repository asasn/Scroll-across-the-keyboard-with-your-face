using RootNS.Behavior;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Card : Node
    {
        public Card()
        {
            this.PropertyChanged += Card_PropertyChanged;
        }

        private void Card_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
      
        }

        private bool _canSave;
        /// <summary>
        /// 是否可以保存的标记
        /// </summary>
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                _canSave = value;
                this.RaisePropertyChanged(nameof(CanSave));
            }
        }



        private long _bornYear;
        /// <summary>
        /// 诞年
        /// </summary>
        public long BornYear
        {
            get { return _bornYear; }
            set
            {
                _bornYear = value;
                this.RaisePropertyChanged(nameof(BornYear));
            }
        }

        private int _weight;
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                this.RaisePropertyChanged(nameof(Weight));
            }
        }


        private ObservableCollection<string> _nickName;
        /// <summary>
        /// 别称
        /// </summary>
        public ObservableCollection<string> NickName
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                this.RaisePropertyChanged(nameof(NickName));
            }
        }


        /// <summary>
        /// 从当前节点添加子节点
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Card AddChildNode(Card card)
        {
            if (card.Title == null)
            {
                card.Title = "新" + this.TabName;
            }
            this.IsExpanded = true;
            this.ChildNodes.Add(card);
            DataOut.CreateNewCard(card);
            return card;
        }

        public class Tip
        {
            public Tip()
            {
                Pid = this.Uid;
            }
            public string Index { get; set; }
            public string Pid { get; set; }
            public string Uid { get; set; }
            public string Tid { get; set; }
            public string TabName { get; set; }
            public string Title { get; set; }
        }
        public class Line
        {
            public string TabName { get; set; }

            public ObservableCollection<Tip> Tips { get; set; }
        }

        public ObservableCollection<Line> Lines { get; set; }
    }
}

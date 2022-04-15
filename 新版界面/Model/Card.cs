﻿using RootNS.Behavior;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            Lines.CollectionChanged += Lines_CollectionChanged;
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


        private long _realYear;

        public long RealYear
        {
            get { return _realYear; }
            set
            {
                _realYear = value;
                this.RaisePropertyChanged(nameof(RealYear));
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

        private Line _nickNames = new Line();

        public Line NickNames
        {
            get { return _nickNames; }
            set
            {
                _nickNames = value;
                this.RaisePropertyChanged(nameof(NickNames));
            }
        }


        public class Tip
        {
            public Tip()
            {
                Pid = this.Uid;
            }
            public Card.Line Parent { get; set; }
            public int Index { get; set; }
            public string Pid { get; set; }
            public string Uid { get; set; }
            public string Tid { get; set; }
            public string Title { get; set; }
            public string TabName { get; set; }
            public string OwnerName { get; set; }
        }
        public class Line
        {
            public Line()
            {
                Tips.CollectionChanged += Tips_CollectionChanged;
            }

            private void Tips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    Card.Tip tip = (Card.Tip)e.NewItems[0];
                    tip.TabName = this.TabName;
                    tip.OwnerName = this.OwnerName;
                    tip.Pid = this.Pid;
                    tip.Parent = this;
                }
            }
            public Card Parent { get; set; }
            public string Pid { get; set; }
            public string LineTitle { get; set; }
            public string TabName { get; set; }
            public string OwnerName { get; set; }
            public ObservableCollection<Tip> Tips { get; set; } = new ObservableCollection<Tip>();
        }

        public ObservableCollection<Line> Lines { get; set; } = new ObservableCollection<Line>();

        private void Lines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Card.Line line = (Card.Line)e.NewItems[0];
                line.TabName = this.TabName;
                line.OwnerName = this.OwnerName;
                line.Pid = this.Uid;
                line.Parent = this;
            }
        }
    }
}

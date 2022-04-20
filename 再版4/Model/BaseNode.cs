using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class BaseNode : BaseBase
    {

        public BaseNode()
        {
            Childs.CollectionChanged += Childs_CollectionChanged;            
        }

        private void Childs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                BaseNode stuff = (BaseNode)e.NewItems[0];
                stuff.Pid = this.Uid;
                stuff.TabName = this.TabName;
                stuff.Owner = this.Owner;
                stuff.Parent = this;
                stuff.Index = this.Childs.IndexOf(stuff);
                this.WordsCount += 1;
                if (this.IsDel == true)
                {
                    stuff.IsDel = true;
                }
                if (this.Parent == null)
                {
                    stuff.Root = this;
                }
                else
                {
                    stuff.Root = this.Root;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                BaseNode stuff = (BaseNode)e.OldItems[0];
                for (int i = stuff.Index; i < this.Childs.Count; i++)
                {
                    (this.Childs[i] as BaseNode).Index -= 1;
                }
                this.WordsCount -= 1;
            }
        }


        private string? _pid;
        /// <summary>
        /// 父节点的Uid
        /// </summary>
        public string? Pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                RaisePropertyChanged(nameof(Pid));
            }
        }

        private object _root;
        /// <summary>
        /// 根节点对象
        /// </summary>
        public object Root
        {
            get { return _root; }
            set
            {
                _root = value;
                RaisePropertyChanged(nameof(Root));
            }
        }



        private object _owner;
        /// <summary>
        /// 所有者（一般为书籍）
        /// </summary>
        public object Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                RaisePropertyChanged(nameof(Owner));
            }
        }

        private string _tabName;
        /// <summary>
        /// 页面标签名称
        /// <para></para>
        /// （对应数据库中的表名/控件TabControl当中的TabItem标签名）
        /// </summary>
        public string TabName
        {
            get { return _tabName; }
            set
            {
                _tabName = value;
                RaisePropertyChanged(nameof(TabName));
            }
        }


        private object _parent;
        /// <summary>
        /// 父节点对象
        /// </summary>
        public object Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                RaisePropertyChanged(nameof(Parent));
            }
        }


        private ObservableCollection<object> _childs = new();
        /// <summary>
        /// 子节点动态数据集合
        /// </summary>
        public ObservableCollection<object> Childs
        {
            get { return _childs; }
            set
            {
                _childs = value;
                RaisePropertyChanged(nameof(Childs));
            }
        }


        private int _wordsCount;
        /// <summary>
        /// 字数统计结果
        /// </summary>
        public int WordsCount
        {
            get { return _wordsCount; }
            set
            {
                _wordsCount = value;
                RaisePropertyChanged(nameof(WordsCount));
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RootNS.Model
{
    /// <summary>
    /// 书籍类
    /// </summary>
    public class Book : BookBase
    {
        public Book(string name)
        {
            Clear();
            this.Name = name;
        }
        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public new void Clear()
        {
            BoxDraft.Clear();
            BoxTemp.Clear();
            BoxPublished.Clear();
            NoteOutline.Clear();
            NoteMemorabilia.Clear();
            NoteClues.Clear();
            NoteTemplate.Clear();
        }

        public enum PartItemTag
        {
            草稿箱 = 0,
            暂存箱 = 1,
            已发布 = 2,
            大纲 = 0,
        }

        public enum NoteItemTag
        {
            大纲 = 0,
            大事记 = 1,
            线索 = 2,
            文例 = 3
        }
        private double _pirce;

        public double Price
        {
            get { return _pirce; }
            set
            {
                _pirce = value;
                this.RaisePropertyChanged("Price");
            }
        }

        private long _currentYear;

        public long CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                this.RaisePropertyChanged("CurrentYear");
            }
        }

        private string _introduce;

        public string Introduce
        {
            get { return _introduce; }
            set
            {
                _introduce = value;
                this.RaisePropertyChanged("Introduce");
            }
        }

        private string _coverpath;

        public string CoverPath
        {
            get { return _coverpath; }
            set
            {
                _coverpath = value;
                this.RaisePropertyChanged("CoverPath");
            }
        }


        #region 目录树
        public ObservableCollection<Node> BoxDraft { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxTemp { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxPublished { set; get; } = new ObservableCollection<Node>();
        #endregion

        #region 记事板
        public ObservableCollection<Node> NoteOutline { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> NoteMemorabilia { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> NoteClues { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> NoteTemplate { set; get; } = new ObservableCollection<Node>();
        #endregion


        public void LoadForBookPart(int index)
        {
            ItemIndex = index;
            if (index == (int)PartItemTag.草稿箱)
            {
                LoadBookPart(BoxDraft, PartItemTag.草稿箱);
            }
            if (index == (int)PartItemTag.暂存箱)
            {
                LoadBookPart(BoxTemp, PartItemTag.暂存箱);
            }
            if (index == (int)PartItemTag.已发布)
            {
                LoadBookPart(BoxPublished, PartItemTag.已发布);
            }
        }

        public void LoadForBookNote(int index)
        {
            ItemIndex = index;
            if (index == (int)NoteItemTag.大纲)
            {
                LoadBookNote(NoteOutline, NoteItemTag.大纲);
            }
            if (index == (int)NoteItemTag.大事记)
            {
                LoadBookNote(NoteMemorabilia, NoteItemTag.大事记);
            }
            if (index == (int)NoteItemTag.线索)
            {
                LoadBookNote(NoteClues, NoteItemTag.线索);
            }
            if (index == (int)NoteItemTag.文例)
            {
                LoadBookNote(NoteTemplate, NoteItemTag.文例);
            }
        }

        private void LoadBookNote(ObservableCollection<Node> nodes, NoteItemTag partTag)
        {
            if (nodes.Count == 0)
            {
                for (int i = 0; i <= (int)partTag; i++)
                {
                    nodes.Add(new Node(partTag.ToString()));
                }
            }
        }

        private void LoadBookPart(ObservableCollection<Node> nodes, PartItemTag partTag)
        {
            if (nodes.Count == 0)
            {
                for (int i = 0; i <= (int)partTag; i++)
                {
                    nodes.Add(new Node(partTag.ToString()));
                }
            }
        }
    }
}

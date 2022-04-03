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
        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public new void Clear()
        {
            BoxDraft.ChildNodes.Clear();
            BoxTemp.ChildNodes.Clear();
            BoxPublished.ChildNodes.Clear();
            NoteOutline.ChildNodes.Clear();
            NoteMemorabilia.ChildNodes.Clear();
            NoteClues.ChildNodes.Clear();
            NoteTemplate.ChildNodes.Clear();
        }

        /// <summary>
        /// 目录树TabItem标志
        /// </summary>
        public enum PartItemFlag
        {
            草稿箱 = 0,
            暂存箱 = 1,
            已发布 = 2,
            大纲 = 0,
        }

        /// <summary>
        /// 记事板TabItem标志标志
        /// </summary>
        public enum NoteItemFlag
        {
            大纲 = 0,
            大事记 = 1,
            线索 = 2,
            文例 = 3
        }

        private double _pirce;
        /// <summary>
        /// 本书稿酬单价
        /// </summary>
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

        private string _introduce;
        /// <summary>
        /// 书籍简介
        /// </summary>
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


        #region 目录树
        public Node BoxDraft { set; get; } = new Node();
        public Node BoxTemp { set; get; } = new Node();
        public Node BoxPublished { set; get; } = new Node();
        #endregion

        #region 记事板
        public Node NoteOutline { set; get; } = new Node();
        public Node NoteMemorabilia { set; get; } = new Node();
        public Node NoteClues { set; get; } = new Node();
        public Node NoteTemplate { set; get; } = new Node();
        #endregion

        /// <summary>
        /// 载入目录树
        /// </summary>
        /// <param name="index"></param>
        public void LoadForBookPart(int index)
        {
            ItemIndex = index;
            if (index == (int)PartItemFlag.草稿箱)
            {
                LoadBookPart(BoxDraft, PartItemFlag.草稿箱);
            }
            if (index == (int)PartItemFlag.暂存箱)
            {
                LoadBookPart(BoxTemp, PartItemFlag.暂存箱);
            }
            if (index == (int)PartItemFlag.已发布)
            {
                LoadBookPart(BoxPublished, PartItemFlag.已发布);
            }
        }

        /// <summary>
        /// 载入记事板
        /// </summary>
        /// <param name="index"></param>
        public void LoadForBookNote(int index)
        {
            ItemIndex = index;
            if (index == (int)NoteItemFlag.大纲)
            {
                LoadBookNote(NoteOutline, NoteItemFlag.大纲);
            }
            if (index == (int)NoteItemFlag.大事记)
            {
                LoadBookNote(NoteMemorabilia, NoteItemFlag.大事记);
            }
            if (index == (int)NoteItemFlag.线索)
            {
                LoadBookNote(NoteClues, NoteItemFlag.线索);
            }
            if (index == (int)NoteItemFlag.文例)
            {
                LoadBookNote(NoteTemplate, NoteItemFlag.文例);
            }
        }

        private void LoadBookNote(Node rootNode, NoteItemFlag partFlag)
        {
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)partFlag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = partFlag.ToString() });
                }
            }
        }

        private void LoadBookPart(Node rootNode, PartItemFlag partTag)
        {
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= 30; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = partTag.ToString() });
                }
            }
        }
    }
}

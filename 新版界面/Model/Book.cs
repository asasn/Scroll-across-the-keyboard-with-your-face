using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            NoteStory.ChildNodes.Clear();
            NoteMemorabilia.ChildNodes.Clear();
            NoteClues.ChildNodes.Clear();
            NoteTemplate.ChildNodes.Clear();
        }

        /// <summary>
        /// 目录树TabItem标志
        /// </summary>
        public enum ContentTabName
        {
            草稿箱 = 0,
            暂存箱 = 1,
            已发布 = 2,
            大纲 = 0,
        }

        /// <summary>
        /// 记事板TabItem标志标志
        /// </summary>
        public enum NoteTabName
        {
            大事记 = 0,
            故事 = 1,
            场景 = 2,
            线索 = 3,
            文例 = 4
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
        public Node NoteMemorabilia { set; get; } = new Node();
        public Node NoteStory { set; get; } = new Node();
        public Node NoteScenes { get; set; } = new Node();        
        public Node NoteClues { set; get; } = new Node();
        public Node NoteTemplate { set; get; } = new Node();
        #endregion


        /// <summary>
        /// 载入目录树
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="partTag"></param>
        public void LoadBookPart(TabControl tabControl)
        {
            ContentTabName flag = (ContentTabName)tabControl.SelectedIndex;
            string itemName = Enum.GetName(typeof(ContentTabName), tabControl.SelectedIndex);
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = BoxDraft;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = BoxTemp;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = BoxPublished;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)flag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = itemName.ToString() });
                }
            }
        }

        /// <summary>
        /// 载入记事板
        /// </summary>
        /// <param name="index"></param>
        public void LoadBookNote(TabControl tabControl)
        {
            NoteTabName flag = (NoteTabName)tabControl.SelectedIndex;
            string itemName = Enum.GetName(typeof(NoteTabName), tabControl.SelectedIndex);
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = NoteMemorabilia;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = NoteStory;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = NoteScenes;
            }
            if (tabControl.SelectedIndex == 3)
            {
                rootNode = NoteClues;
            }
            if (tabControl.SelectedIndex == 4)
            {
                rootNode = NoteTemplate;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)flag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = itemName.ToString() });
                }
            }
        }


    }
}

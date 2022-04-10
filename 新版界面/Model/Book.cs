using RootNS.Behavior;
using RootNS.Brick;
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
            CardRole.ChildNodes.Clear();
            CardOther.ChildNodes.Clear();
            CardWorld.ChildNodes.Clear();
            MapPoints.ChildNodes.Clear();
        }

        /// <summary>
        /// 目录树TabItem标志
        /// </summary>
        public enum ChapterTabName
        {
            草稿,
            暂存,
            已发布,
        }

        /// <summary>
        /// 记事板TabItem标志标志
        /// </summary>
        public enum NoteTabName
        {
            大事记,
            故事,
            场景,
            线索,
            模板,
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




        #region 目录树
        public Node BoxDraft { set; get; } = new Node() { Uid = String.Empty, TabName = ChapterTabName.草稿.ToString() };
        public Node BoxTemp { set; get; } = new Node() { Uid = String.Empty, TabName = ChapterTabName.暂存.ToString() };
        public Node BoxPublished { set; get; } = new Node() { Uid = String.Empty, TabName = ChapterTabName.已发布.ToString() };
        #endregion

        #region 记事板
        public Node NoteMemorabilia { set; get; } = new Node() { Uid = String.Empty, TabName = NoteTabName.大事记.ToString() };
        public Node NoteStory { set; get; } = new Node() { Uid = String.Empty, TabName = NoteTabName.故事.ToString() };
        public Node NoteScenes { get; set; } = new Node() { Uid = String.Empty, TabName = NoteTabName.场景.ToString() };
        public Node NoteClues { set; get; } = new Node() { Uid = String.Empty, TabName = NoteTabName.线索.ToString() };
        public Node NoteTemplate { set; get; } = new Node() { Uid = String.Empty, TabName = NoteTabName.模板.ToString() };
        #endregion

        #region 信息卡
        public Node CardRole { set; get; } = new Node() { Uid = String.Empty, TabName = CardTabName.角色.ToString() };
        public Node CardOther { set; get; } = new Node() { Uid = String.Empty, TabName = CardTabName.其他.ToString() };
        public Node CardWorld { set; get; } = new Node() { Uid = String.Empty, TabName = CardTabName.世界.ToString() };
        public Node MapPoints { set; get; } = new Node() { Uid = String.Empty, TabName = CardTabName.地图.ToString() };
        #endregion

        /// <summary>
        /// 载入目录树
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="partTag"></param>
        public void LoadBookChapters()
        {
            TabControl tabControl = Gval.SelectedChapterTab;
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = Gval.CurrentBook.BoxDraft;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = Gval.CurrentBook.BoxTemp;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = Gval.CurrentBook.BoxPublished;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                DataJoin.FillInPart(rootNode);
            }
        }

        /// <summary>
        /// 载入记事板
        /// </summary>
        /// <param name="index"></param>
        public void LoadBookNotes()
        {
            TabControl tabControl = Gval.SelectedNoteTab;
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = Gval.CurrentBook.NoteMemorabilia;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = Gval.CurrentBook.NoteStory;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = Gval.CurrentBook.NoteScenes;
            }
            if (tabControl.SelectedIndex == 3)
            {
                rootNode = Gval.CurrentBook.NoteClues;
            }
            if (tabControl.SelectedIndex == 4)
            {
                rootNode = Gval.CurrentBook.NoteTemplate;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                DataJoin.FillInPart(rootNode);
            }
        }

        /// <summary>
        /// 载入信息卡
        /// </summary>
        public void LoadForCards()
        {
            TabControl tabControl = Gval.SelectedCardTab;
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
                DataJoin.FillInPart(rootNode);
            }
        }


    }
}

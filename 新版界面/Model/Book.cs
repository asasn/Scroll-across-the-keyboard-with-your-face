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
        }

        /// <summary>
        /// 目录树TabItem标志
        /// </summary>
        public enum ChapterTabName
        {
            草稿箱,
            暂存箱,
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


        private TabControl _selectedChapterTab;

        public TabControl SelectedChapterTab
        {
            get { return _selectedChapterTab; }
            set
            {
                _selectedChapterTab = value;
            }
        }

        private TabControl _selectedNoteTab;

        public TabControl SelectedNoteTab
        {
            get { return _selectedNoteTab; }
            set
            {
                _selectedNoteTab = value;
            }
        }

        private TabControl _selectedCardTab;

        public TabControl SelectedCardTab
        {
            get { return _selectedCardTab; }
            set
            {
                _selectedCardTab = value;
            }
        }


        #region 目录树
        public Node BoxDraft { set; get; } = new Node() { TabName = ChapterTabName.草稿箱.ToString() };
        public Node BoxTemp { set; get; } = new Node() { TabName = ChapterTabName.暂存箱.ToString() };
        public Node BoxPublished { set; get; } = new Node() { TabName = ChapterTabName.已发布.ToString() };
        #endregion

        #region 记事板
        public Node NoteMemorabilia { set; get; } = new Node() { TabName = NoteTabName.大事记.ToString() };
        public Node NoteStory { set; get; } = new Node() { TabName = NoteTabName.故事.ToString() };
        public Node NoteScenes { get; set; } = new Node() { TabName = NoteTabName.场景.ToString() };
        public Node NoteClues { set; get; } = new Node() { TabName = NoteTabName.线索.ToString() };
        public Node NoteTemplate { set; get; } = new Node() { TabName = NoteTabName.模板.ToString() };
        #endregion


        /// <summary>
        /// 载入目录树
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="partTag"></param>
        public void LoadBookChapters()
        {
            TabControl tabControl = Gval.CurrentBook.SelectedChapterTab;
            ChapterTabName flag = (ChapterTabName)tabControl.SelectedIndex;
            Gval.TableName = flag.ToString();
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
                CSqlitePlus.PoolOperate.Add(Gval.CurrentBook.Name);
                DataJoin.FillInPart(Gval.CurrentBook.Name, null, rootNode);
            }
        }

        /// <summary>
        /// 载入记事板
        /// </summary>
        /// <param name="index"></param>
        public void LoadBookNotes()
        {
            TabControl tabControl = Gval.CurrentBook.SelectedNoteTab;
            NoteTabName flag = (NoteTabName)tabControl.SelectedIndex;
            Gval.TableName = flag.ToString();
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
                CSqlitePlus.PoolOperate.Add(Gval.CurrentBook.Name);
                DataJoin.FillInPart(Gval.CurrentBook.Name, null, rootNode);
            }
        }


    }
}

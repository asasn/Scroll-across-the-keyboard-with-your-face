using RootNS.Helper;
using RootNS.View;
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
        public Book()
        {
            this.PropertyChanged += Book_PropertyChanged;
        }

        private void Book_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                this.InitRootNodes(this.Name);
            }
        }


        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootNodes(string bookName)
        {
            Node[] rootNodes = { this.BoxDraft, this.BoxTemp, this.BoxPublished, this.NoteMemorabilia, this.NoteStory, this.NoteScenes, this.NoteClues, this.NoteTemplate};
            foreach (Node node in rootNodes)
            {
                node.OwnerName = bookName;
                node.Owner = this;
            }
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






        #region 目录树
        public Node BoxDraft { set; get; } = new Node() { Uid = String.Empty, TabName = ChapterTabName.草稿.ToString()};
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

        ///// <summary>
        ///// 载入目录树
        ///// </summary>
        ///// <param name="rootNode"></param>
        ///// <param name="partTag"></param>
        //public void LoadChaptersTab()
        //{
        //    TabControl tabControl = Gval.SelectedChapterTab;
        //    Node rootNode = new Node();
        //    if (tabControl.SelectedIndex == 0)
        //    {
        //        rootNode = Gval.CurrentBook.BoxDraft;
        //    }
        //    if (tabControl.SelectedIndex == 1)
        //    {
        //        rootNode = Gval.CurrentBook.BoxTemp;
        //    }
        //    if (tabControl.SelectedIndex == 2)
        //    {
        //        rootNode = Gval.CurrentBook.BoxPublished;
        //    }
        //    if (rootNode.ChildNodes.Count == 0)
        //    {
        //        DataJoin.FillInNodes(rootNode);
        //    }
        //}

        ///// <summary>
        ///// 载入记事板
        ///// </summary>
        ///// <param name="index"></param>
        //public void LoadNotesTab()
        //{
        //    TabControl tabControl = Gval.SelectedNoteTab;
        //    Node rootNode = new Node();
        //    if (tabControl.SelectedIndex == 0)
        //    {
        //        rootNode = Gval.CurrentBook.NoteMemorabilia;
        //    }
        //    if (tabControl.SelectedIndex == 1)
        //    {
        //        rootNode = Gval.CurrentBook.NoteStory;
        //    }
        //    if (tabControl.SelectedIndex == 2)
        //    {
        //        rootNode = Gval.CurrentBook.NoteScenes;
        //    }
        //    if (tabControl.SelectedIndex == 3)
        //    {
        //        rootNode = Gval.CurrentBook.NoteClues;
        //    }
        //    if (tabControl.SelectedIndex == 4)
        //    {
        //        rootNode = Gval.CurrentBook.NoteTemplate;
        //    }
        //    if (rootNode.ChildNodes.Count == 0)
        //    {
        //        DataJoin.FillInNodes(rootNode);
        //    }
        //}


        /// <summary>
        /// 载入所有章节
        /// </summary>
        /// <param name="index"></param>
        public void LoadForAllChapterTabs()
        {
            if (BoxDraft.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(BoxDraft);
            }
            if (BoxTemp.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(BoxTemp);
            }
            if (BoxPublished.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(BoxPublished);
            }
        }

        /// <summary>
        /// 载入所有记事
        /// </summary>
        /// <param name="index"></param>
        public void LoadForAllNoteTabs()
        {
            if (NoteMemorabilia.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteMemorabilia);
            }
            if (NoteStory.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteStory);
            }
            if (NoteScenes.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteScenes);
            }
            if (NoteClues.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteClues);
            }
            if (NoteTemplate.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteTemplate);
            }
        }

    }
}

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
            RootNodes.Add(ChapterTabName.草稿.ToString(), BoxDraft);
            RootNodes.Add(ChapterTabName.暂存.ToString(), BoxTemp);
            RootNodes.Add(ChapterTabName.已发布.ToString(), BoxPublished);
            RootNodes.Add(NoteTabName.大事记.ToString(), NoteMemorabilia);
            RootNodes.Add(NoteTabName.故事.ToString(), NoteStory);
            RootNodes.Add(NoteTabName.场景.ToString(), NoteScenes);
            RootNodes.Add(NoteTabName.线索.ToString(), NoteClues);
            RootNodes.Add(NoteTabName.模板.ToString(), NoteTemplate);


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
            Node[] rootNodes = { this.BoxDraft, this.BoxTemp, this.BoxPublished, this.NoteMemorabilia, this.NoteStory, this.NoteScenes, this.NoteClues, this.NoteTemplate };
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

        public List<Node> GetChapterNodes()
        {
            List<Node> nodes = new List<Node>();
            List<Node> roots = new List<Node>() { BoxDraft, BoxTemp, BoxPublished };
            foreach (var root in roots) //从根节点遍历即可，不必把根节点也加上
            {
                GetTreeNodes(nodes, root);
            }
            return nodes;
        }
        public List<Node> GetSecenNodes()
        {
            if (NoteScenes.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteScenes);
            }
            List<Node> nodes = new List<Node>();
            GetTreeNodes(nodes, NoteScenes);
            return nodes;
        }

        public List<Node> GetPublishedChapterNodes()
        {
            if (BoxPublished.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(BoxPublished);
            }
            List<Node> nodes = new List<Node>();
            GetTreeNodes(nodes, BoxPublished);
            return nodes;
        }

        public void LoadForChapterTab()
        {
            Node rootNode = RootNodes[Enum.GetName(typeof(ChapterTabName), Gval.SelectedChapterTab.SelectedIndex)];
            if (rootNode.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(rootNode);
            }
        }
        public void LoadForNoteTab()
        {
            Node rootNode = RootNodes[Enum.GetName(typeof(NoteTabName), Gval.SelectedNoteTab.SelectedIndex)];
            if (rootNode.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(rootNode);
            }
        }
        public void LoadForCardTab()
        {
            Card rootCard = RootNodes[Enum.GetName(typeof(CardTabName), Gval.SelectedCardTab.SelectedIndex)] as Card;
            if (rootCard.ChildNodes.Count == 0)
            {
                DataIn.FillInCards(rootCard);
            }
        }

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

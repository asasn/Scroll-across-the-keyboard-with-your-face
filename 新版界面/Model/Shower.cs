using RootNS.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RootNS.Model
{
    public class Shower : NotificationObject
    {
        /// <summary>
        /// 静态事件处理属性更改
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public Shower(Node curChapter, int length = 10)
        {
            GetChapters(curChapter, length);
            LoadPreviousCards(curChapter);
        }

        private void LoadPreviousCards(Node curChapter)
        {
            string text = string.Empty;
            foreach (Node node in Chapters)
            {
                if (node == curChapter)
                {
                    continue;
                }
                text += node.Text + "\n　　\n";
            }
            _pRoles = EditorHelper.RefreshCardsForTab(Gval.CurrentBook.CardRole, text);
            _pOthers = EditorHelper.RefreshCardsForTab(Gval.CurrentBook.CardOther, text);
        }

        /// <summary>
        /// 更新所有章节的匹配内容
        /// </summary>
        public static void RefreshCards()
        {
            string text = string.Empty;
            foreach (Node node in Chapters)
            {
                text += node.Text + "\n　　\n";
            }
            Roles = EditorHelper.RefreshCardsForTab(Gval.CurrentBook.CardRole, text);
            Others = EditorHelper.RefreshCardsForTab(Gval.CurrentBook.CardOther, text);
        }

        /// <summary>
        /// 只更新当前章节匹配内容，前面匹配固定不变。
        /// </summary>
        /// <param name="curText"></param>
        public static void RefreshCards(string curText)
        {
            _cRoles = EditorHelper.RefreshCardsForTab(Gval.CurrentBook.CardRole, curText);
            _cOthers = EditorHelper.RefreshCardsForTab(Gval.CurrentBook.CardOther, curText);
            Roles.Clear();
            Others.Clear();
            _pRoles.Union(_cRoles).ToList().ForEach(t => Roles.Add(t));
            _pOthers.Union(_cOthers).ToList().ForEach(t => Others.Add(t));
        }

        private ObservableCollection<Node> GetChapters(Node curChapter, int length = 10)
        {
            Chapters.Clear();
            int c = Gval.CurrentBook.BoxDraft.ChildNodes.IndexOf(curChapter);
            for (int i = 0; i < length; i++)
            {
                if (c - i >= 0)
                {
                    Chapters.Add(Gval.CurrentBook.BoxDraft.ChildNodes[c - i]);
                }
                else
                {
                    break;
                }
            }
            if (length - Chapters.Count > 0)
            {
                if (Gval.CurrentBook.BoxPublished.ChildNodes.Count == 0)
                {
                    DataIn.FillInNodes(Gval.CurrentBook.BoxPublished);
                }
                List<Node> publishedChaptersList = Gval.CurrentBook.GetPublishedChapterNodes();
                int cc = 0;
                if (publishedChaptersList.Contains(curChapter))
                {
                    cc = publishedChaptersList.IndexOf(curChapter);
                }
                else
                {
                    cc = publishedChaptersList.Count - 1;
                }
                length -= Chapters.Count;
                for (int i = 0; i < length; i++)
                {
                    if (cc - i >= 0)
                    {
                        Chapters.Add(publishedChaptersList[cc - i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return Chapters;
        }




        private static ObservableCollection<Card> _roles = new ObservableCollection<Card>();

        public static ObservableCollection<Card> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Roles)));
            }
        }

        private static ObservableCollection<Card> _others = new ObservableCollection<Card>();

        public static ObservableCollection<Card> Others
        {
            get { return _others; }
            set
            {
                _others = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Others)));

            }
        }

        private static ObservableCollection<Node> _chapters = new ObservableCollection<Node>();

        public static ObservableCollection<Node> Chapters
        {
            get { return _chapters; }
            set
            {
                _chapters = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Chapters)));
            }
        }

        private static ObservableCollection<Card> _pRoles = new ObservableCollection<Card>();
        private static ObservableCollection<Card> _cRoles = new ObservableCollection<Card>();
        private static ObservableCollection<Card> _pOthers = new ObservableCollection<Card>();
        private static ObservableCollection<Card> _cOthers = new ObservableCollection<Card>();
    }
}

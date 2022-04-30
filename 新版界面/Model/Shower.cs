using RootNS.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Shower : NotificationObject
    {
        public Shower(Node curChapter, int length = 10)
        {
            GetChapters(curChapter, length);
            RefreshCards(curChapter);
        }

        private void RefreshCards(Node curChapter)
        {
            string text = string.Empty;
            foreach (Node node in Chapters)
            {
                if (node.Uid == curChapter.Uid)
                {
                    text += curChapter.Text + "\n　　\n";
                }
                text += node.Text + "\n　　\n";
            }
            Roles = EditorHelper.RefreshIsContainFlagForTab(Gval.CurrentBook.CardRole, text);
            Others = EditorHelper.RefreshIsContainFlagForTab(Gval.CurrentBook.CardOther, text);
        }

        private ObservableCollection<Node> GetChapters(Node curChapter, int length = 10)
        {
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


        private ObservableCollection<Card> _roles = new ObservableCollection<Card>();

        public ObservableCollection<Card> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                RaisePropertyChanged(nameof(Roles));
            }
        }

        private ObservableCollection<Card> _others = new ObservableCollection<Card>();

        public ObservableCollection<Card> Others
        {
            get { return _others; }
            set
            {
                _others = value;
                RaisePropertyChanged(nameof(Others));
            }
        }

        private ObservableCollection<Node> _chapters = new ObservableCollection<Node>();

        public ObservableCollection<Node> Chapters
        {
            get { return _chapters; }
            set
            {
                _chapters = value;
                RaisePropertyChanged(nameof(Chapters));
            }
        }

    }
}

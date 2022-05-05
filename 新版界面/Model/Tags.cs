using RootNS.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Tags : NotificationObject
    {
        public Tags()
        {
            
        }

        public Tags(string boxTitle, string tabName)
        {
            BoxTitle = boxTitle;
            TabName = tabName; 
        }

        private string _tabName;

        public string TabName
        {
            get { return _tabName; }
            set
            {
                _tabName = value;
                RaisePropertyChanged(nameof(TabName));
            }
        }


        private string _boxTitle;

        public string BoxTitle
        {
            get { return _boxTitle; }
            set
            {
                _boxTitle = value;
                RaisePropertyChanged(nameof(BoxTitle));
            }
        }

        private ObservableCollection<Tag> _childItems = new ObservableCollection<Tag>();

        public ObservableCollection<Tag> ChildItems
        {
            get { return _childItems; }
            set
            {
                _childItems = value;
                RaisePropertyChanged(nameof(ChildItems));
            }
        }

        public bool HasTag(Tag tag)
        {
            foreach (Tag t in ChildItems)
            {
                if (t.Uid == tag.Uid)
                {
                    return true;
                }
            }
            return false;
        }


        public void Remove(Tags.Tag tag)
        {
            this.ChildItems.Remove(tag);
        }

        public class Tag : NotificationObject
        {
            private string _uid;

            public string Uid
            {
                get { return _uid; }
                set
                {
                    _uid = value;
                    RaisePropertyChanged(nameof(Uid));
                }
            }

            private string _title;

            public string Title
            {
                get { return _title; }
                set
                {
                    _title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }


        }
    }
}

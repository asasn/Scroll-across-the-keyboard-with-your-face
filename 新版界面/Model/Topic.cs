using RootNS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Topic : NotificationObject
    {
        private Card.Line _subject = new Card.Line() { LineTitle = "主旨" };

        public Card.Line Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                RaisePropertyChanged(nameof(Subject));
            }
        }

        private Card.Line _style = new Card.Line() { LineTitle = "风格" };

        public Card.Line Style
        {
            get { return _style; }
            set
            {
                _style = value;
                RaisePropertyChanged(nameof(Style));
            }
        }

        private Card.Line _awardPoints = new Card.Line() { LineTitle = "爽点" };

        public Card.Line AwardPoints
        {
            get { return _awardPoints; }
            set
            {
                _awardPoints = value;
                RaisePropertyChanged(nameof(AwardPoints));
            }
        }

        private Card.Line _volumes = new Card.Line() { LineTitle = "分卷" };

        public Card.Line Volumes
        {
            get { return _volumes; }
            set
            {
                _volumes = value;
                RaisePropertyChanged(nameof(Volumes));
            }
        }


        private Card.Line roles = new Card.Line() { LineTitle = "角色" };

        public Card.Line Roles
        {
            get { return roles; }
            set
            {
                roles = value;
                RaisePropertyChanged(nameof(Roles));
            }
        }

        private Card.Line _clues = new Card.Line() { LineTitle = "线索" };

        public Card.Line Clues
        {
            get { return _clues; }
            set
            {
                _clues = value;
                RaisePropertyChanged(nameof(Clues));
            }
        }



        private Card.Line _goldfingers = new Card.Line() { LineTitle = "金手指" };

        public Card.Line Goldfingers
        {
            get { return _goldfingers; }
            set
            {
                _goldfingers = value;
                RaisePropertyChanged(nameof(Goldfingers));
            }
        }

        private Card.Line _levels = new Card.Line() { LineTitle = "阶级" };

        public Card.Line Levels
        {
            get { return _levels; }
            set
            {
                _levels = value;
                RaisePropertyChanged(nameof(Levels));
            }
        }



        private Card.Line _worldInfo = new Card.Line() { LineTitle = "世界" };

        public Card.Line WorldInfo
        {
            get { return _worldInfo; }
            set
            {
                _worldInfo = value;
                RaisePropertyChanged(nameof(WorldInfo));
            }
        }


        private Card.Line _sets = new Card.Line() { LineTitle = "设定" };

        public Card.Line Sets
        {
            get { return _sets; }
            set
            {
                _sets = value;
                RaisePropertyChanged(nameof(Sets));
            }
        }


        private bool _canSave;

        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                _canSave = value;
                RaisePropertyChanged(nameof(CanSave));
            }
        }

        public void Save(Node node, string showContent)
        {
            //清除tip.title为空的项目
            Card.Line[] lines = { Subject, Style, AwardPoints, Volumes, Roles, Goldfingers, Clues, Levels, WorldInfo, Sets };
            foreach (Card.Line line in lines)
            {
                foreach (Card.Tip tip in line.Tips.ToList())
                {
                    if (string.IsNullOrWhiteSpace(tip.Title))
                    {
                        line.Tips.Remove(tip);
                    }
                }
            }
            node.Text = showContent;
            string json = JsonHelper.ObjectToJson(this);
            DataOut.UpdateNodeProperty(node, nameof(Node.Text), node.Text);
            DataOut.UpdateNodeProperty(node, nameof(Node.Summary), json);
            this.CanSave = false;
        }
    }
}

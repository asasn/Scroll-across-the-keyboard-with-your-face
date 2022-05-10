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
    public class Summary : NotificationObject
    {
        public Summary()
        {
            this.PropertyChanged += Summary_PropertyChanged;
            this.Roles.CollectionChanged += Roles_CollectionChanged;
            this.Origin.CollectionChanged += Origin_CollectionChanged;
            this.Result.CollectionChanged += Result_CollectionChanged;
            this.Secens.CollectionChanged += Secens_CollectionChanged;
        }

        private void Summary_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Time))
            {
                Json.Time = Time;
            }
            if (e.PropertyName == nameof(Place))
            {
                Json.Place = Place;
            }
        }

        private void Secens_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Node node = (Node)e.NewItems[0];
                if (this.Json.Secens.Contains(node.Uid) == false)
                {
                    this.Json.Secens.Add(node.Uid);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Node node = (Node)e.OldItems[0];
                if (this.Json.Secens.Contains(node.Uid) == true)
                {
                    this.Json.Secens.Remove(node.Uid);
                }
            }
            this.Roles.Clear();
            foreach (Node nodeSecen in this.Secens)
            {
                foreach (Card card in (nodeSecen.Extra as Summary).Roles)
                {
                    if (this.Roles.Contains(card) == false)
                    {
                        this.Roles.Add(card);
                    }
                }
            }
            this.CanSave = true;
        }

        private void Result_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Node node = (Node)e.NewItems[0];
                if (this.Json.Result.Contains(node.Uid) == false)
                {
                    this.Json.Result.Add(node.Uid);
                }
                if ((node.Extra as Summary).Origin.Contains(this.Node) == false && node != this.Node)
                {
                    (node.Extra as Summary).Origin.Add(this.Node);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Node node = (Node)e.OldItems[0];
                if (this.Json.Result.Contains(node.Uid) == true)
                {
                    this.Json.Result.Remove(node.Uid);
                }
                if ((node.Extra as Summary).Origin.Contains(this.Node) == true)
                {
                    (node.Extra as Summary).Origin.Remove(this.Node);
                }
            }
            this.CanSave = true;
        }

        private void Origin_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Node node = (Node)e.NewItems[0];
                if (this.Json.Origin.Contains(node.Uid) == false)
                {
                    this.Json.Origin.Add(node.Uid);
                }
                if ((node.Extra as Summary).Result.Contains(this.Node) == false && node != this.Node)
                {
                    (node.Extra as Summary).Result.Add(this.Node);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Node node = (Node)e.OldItems[0];
                if (this.Json.Origin.Contains(node.Uid) == true)
                {
                    this.Json.Origin.Remove(node.Uid);
                }
                if ((node.Extra as Summary).Result.Contains(this.Node) == true)
                {
                    (node.Extra as Summary).Result.Add(this.Node);
                }
            }
            this.CanSave = true;
        }

        private void Roles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Card card = (Card)e.NewItems[0];
                if (this.Json.Roles.Contains(card.Uid) == false)
                {
                    this.Json.Roles.Add(card.Uid);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Card card = (Card)e.OldItems[0];
                if (this.Json.Roles.Contains(card.Uid) == true)
                {
                    this.Json.Roles.Remove(card.Uid);
                }
            }
            this.CanSave = true;
        }

        public class JsonData
        {
            public string Time;
            public string Place;
            public ObservableCollection<string> Roles = new ObservableCollection<string>();
            public ObservableCollection<string> Origin = new ObservableCollection<string>();
            public ObservableCollection<string> Result = new ObservableCollection<string>();
            public ObservableCollection<string> Secens = new ObservableCollection<string>();
        }

        private JsonData _json = new JsonData();

        public JsonData Json
        {
            get { return _json; }
            set
            {
                _json = value;
                RaisePropertyChanged(nameof(Json));
            }
        }



        private Node _node;

        public Node Node
        {
            get { return _node; }
            set
            {
                _node = value;
                RaisePropertyChanged(nameof(Node));
            }
        }

        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged(nameof(Time));
            }
        }

        private string _place;

        public string Place
        {
            get { return _place; }
            set
            {
                _place = value;
                RaisePropertyChanged(nameof(Place));
            }
        }


        private ObservableCollection<object> _roles = new ObservableCollection<object>();

        public ObservableCollection<object> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                RaisePropertyChanged(nameof(Roles));
            }
        }


        private ObservableCollection<object> _origin = new ObservableCollection<object>();

        public ObservableCollection<object> Origin
        {
            get { return _origin; }
            set
            {
                _origin = value;
                RaisePropertyChanged(nameof(Origin));
            }
        }


        private ObservableCollection<object> _result = new ObservableCollection<object>();

        public ObservableCollection<object> Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged(nameof(Result));
            }
        }


        private ObservableCollection<object> _secens = new ObservableCollection<object>();

        public ObservableCollection<object> Secens
        {
            get { return _secens; }
            set
            {
                _secens = value;
                RaisePropertyChanged(nameof(Secens));
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

        public void Save(string content, string time = null, string place = null)
        {
            Time = time;
            Place = place;
            Node.Text = content;
            string json = JsonHelper.ObjToJson(Json);
            DataOut.UpdateNodeProperty(Node, nameof(Node.Text), Node.Text);
            DataOut.UpdateNodeProperty(Node, nameof(Node.Summary), json);
            CanSave = false;
        }

        public void SaveOnlyCollection()
        {
            string json = JsonHelper.ObjToJson(Json);
            DataOut.UpdateNodeProperty(Node, nameof(Node.Summary), json);
            CanSave = false;
        }
    }
}

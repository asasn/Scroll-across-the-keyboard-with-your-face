using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Secen : NotificationObject
    {
        public Secen()
        {

        }


        public class JsonData
        {
            public ObservableCollection<string> Roles = new ObservableCollection<string>();
            public ObservableCollection<string> Origin = new ObservableCollection<string>();
            public ObservableCollection<string> Result = new ObservableCollection<string>();
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

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    internal class Story : NotificationObject
    {
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
        private ObservableCollection<Card> _roles;

        public ObservableCollection<Card> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                RaisePropertyChanged(nameof(Roles));
            }
        }

        private ObservableCollection<Secen> _secens;

        public ObservableCollection<Secen> Secens
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
    }
}

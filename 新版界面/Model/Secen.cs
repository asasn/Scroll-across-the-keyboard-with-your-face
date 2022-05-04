using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Secen : NotificationObject
    {
        public Secen()
        {
            this.Roles.PropertyChanged += Roles_PropertyChanged;
        }

        private void Roles_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
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
        private Tags _roles = new Tags("角色", Book.CardTabName.角色.ToString());

        public Tags Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                RaisePropertyChanged(nameof(Roles));
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

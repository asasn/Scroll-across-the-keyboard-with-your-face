using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Card : Node
    {
        public Card()
        {
            this.PropertyChanged += Card_PropertyChanged;
        }

        private void Card_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.CanSave = true;
        }

        private bool _canSave;
        /// <summary>
        /// 是否可以保存的标记
        /// </summary>
        public bool CanSave
        {
            get { return _canSave; }
            set
            {
                _canSave = value;
                this.RaisePropertyChanged(nameof(CanSave));
            }
        }


        private long _bornYear;
        /// <summary>
        /// 诞年
        /// </summary>
        public long BornYear
        {
            get { return _bornYear; }
            set
            {
                _bornYear = value;
                this.RaisePropertyChanged(nameof(BornYear));
            }
        }

        private int _weight;
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                this.RaisePropertyChanged(nameof(Weight));
            }
        }


        private ObservableCollection<string> _nickName;
        /// <summary>
        /// 别称
        /// </summary>
        public ObservableCollection<string> NickName
        {
            get { return _nickName; }
            set
            {
                _nickName = value;
                this.RaisePropertyChanged(nameof(NickName));
            }
        }


    }
}

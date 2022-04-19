using RootNS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class BaseBook : NotificationObject
    {
        private int _index;
        /// <summary>
        /// 在书库中的序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged(nameof(Index));
            }
        }

        private string _uid = Guid.NewGuid().ToString();
        /// <summary>
        /// 唯一标识码（Guid）
        /// </summary>
        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                RaisePropertyChanged(nameof(Uid));
            }
        }

        private string _name = String.Empty;
        /// <summary>
        /// 书籍名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private bool _isDel;
        /// <summary>
        /// 是否删除的标志
        /// </summary>
        public bool IsDel
        {
            get { return _isDel; }
            set
            {
                _isDel = value;
                RaisePropertyChanged(nameof(IsDel));
            }
        }

    }
}

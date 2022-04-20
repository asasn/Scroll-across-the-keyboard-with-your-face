using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class BaseBase : NotificationObject
    {
        public BaseBase()
        {
          
        }

        private void BaseBase_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
        }

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

        private object _owner;
        /// <summary>
        /// 所有者（一般为书籍）
        /// </summary>
        public object Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                RaisePropertyChanged(nameof(Owner));
            }
        }

        private string _tabName;
        /// <summary>
        /// 页面标签名称
        /// <para></para>
        /// （对应数据库中的表名/控件TabControl当中的TabItem标签名）
        /// </summary>
        public string TabName
        {
            get { return _tabName; }
            set
            {
                _tabName = value;
                RaisePropertyChanged(nameof(TabName));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class BaseBook : BaseBase
    {


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

        private string _summary = String.Empty;
        /// <summary>
        /// 注释，概要
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                RaisePropertyChanged(nameof(Summary));
            }
        }




    }
}

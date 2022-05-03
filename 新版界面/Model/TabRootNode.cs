using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class TabRootNode : Node
    {
        public TabRootNode()
        {

        }

        private bool _isAddFolderEnabled = true;
        /// <summary>
        /// 添加文件夹按钮是否可用
        /// </summary>
        public bool IsAddFolderEnabled
        {
            get { return _isAddFolderEnabled; }
            set
            {
                _isAddFolderEnabled = value;
                RaisePropertyChanged(nameof(IsAddFolderEnabled));
            }
        }

        private bool _isImportExportEnabled;
        /// <summary>
        /// 导入导出是否可用
        /// </summary>
        public bool IsImportExportEnabled
        {
            get { return _isImportExportEnabled; }
            set
            {
                _isImportExportEnabled = value;
                RaisePropertyChanged(nameof(IsImportExportEnabled));
            }
        }

    }
}

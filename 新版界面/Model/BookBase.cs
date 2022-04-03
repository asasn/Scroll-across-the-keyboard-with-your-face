﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public class BookBase : NotificationObject
    {
        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public void Clear()
        {
            CardRole.ChildNodes.Clear();
            CardOther.ChildNodes.Clear();
            CardWorld.ChildNodes.Clear();
        }

        private string _uid;
        /// <summary>
        /// 书籍标识码
        /// </summary>
        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                this.RaisePropertyChanged("Uid");
            }
        }

        private string _name;
        /// <summary>
        /// 书名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// 工作区
        /// </summary>
        public enum WorkSpace
        {
            当前 = 0,
            公共 = 1
        }

        /// <summary>
        /// 信息卡片TabItem标志
        /// </summary>
        public enum CardItemFlag
        {
            角色 = 0,
            其他 = 1,
            世界 = 2
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                _itemIndex = value;
                this.RaisePropertyChanged("ItemIndex");
            }
        }

        private Node _selectedNode;
        /// <summary>
        /// 选中的节点
        /// </summary>
        public Node SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                _selectedNode = value;
                this.RaisePropertyChanged("SelectedNode");
            }
        }

        #region 信息卡


        public Node CardRole { set; get; } = new Node();
        public Node CardOther { set; get; } = new Node();
        public Node CardWorld { set; get; } = new Node();

        public void LoadForCardsBox(TabControl tabControl, WorkSpace workSpace)
        {
            CardItemFlag flag = (CardItemFlag)tabControl.SelectedIndex;
            string itemName = Enum.GetName(typeof(CardItemFlag), tabControl.SelectedIndex);
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = CardRole;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = CardOther;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = CardWorld;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)flag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = workSpace.ToString() + flag.ToString() });
                }
            }
        }
        #endregion

    }
}

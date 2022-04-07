using RootNS.Behavior;
using RootNS.Brick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public class Material : BookBase
    {
        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public new void Clear()
        {
            BoxExample.ChildNodes.Clear();
            BoxMaterial.ChildNodes.Clear();
            NoteTheme.ChildNodes.Clear();
            NoteInspiration.ChildNodes.Clear();
            PublicCardRole.ChildNodes.Clear();
            PublicCardOther.ChildNodes.Clear();
            PublicCardWorld.ChildNodes.Clear();
            PublicMapPoints.ChildNodes.Clear();
        }


        public enum MaterialTabName
        {
            范文 = 0,
            资料 = 1,
            主题 = 2,
            灵感 = 3
        }

        #region 资料库
        public Node BoxExample { set; get; } = new Node() { TabName = MaterialTabName.范文.ToString(), OwnerName = "index" };
        public Node BoxMaterial { set; get; } = new Node() { TabName = MaterialTabName.资料.ToString(), OwnerName = "index" };
        public Node NoteTheme { set; get; } = new Node() { TabName = MaterialTabName.主题.ToString(), OwnerName = "index" };
        public Node NoteInspiration { set; get; } = new Node() { TabName = MaterialTabName.灵感.ToString(), OwnerName = "index" };
        #endregion
        #region 信息卡
        public Node PublicCardRole { set; get; } = new Node() { TabName = CardTabName.角色.ToString(), OwnerName = "index" };
        public Node PublicCardOther { set; get; } = new Node() { TabName = CardTabName.其他.ToString(), OwnerName = "index" };
        public Node PublicCardWorld { set; get; } = new Node() { TabName = CardTabName.世界.ToString(), OwnerName = "index" };
        public Node PublicMapPoints { set; get; } = new Node() { TabName = CardTabName.地图.ToString(), OwnerName = "index" };
        #endregion


        /// <summary>
        /// 载入资料库
        /// </summary>
        /// <param name="index"></param>
        public void LoadForMaterialPart()
        {
            TabControl tabControl = Gval.SelectedMaterialTab;
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = BoxExample;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = BoxMaterial;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = NoteTheme;
            }
            if (tabControl.SelectedIndex == 3)
            {
                rootNode = NoteInspiration;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                CSqlitePlus.PoolOperate.Add(rootNode.OwnerName);
                DataJoin.FillInPart(null, rootNode);
            }
        }

        /// <summary>
        /// 载入信息卡
        /// </summary>
        public void LoadForCards()
        {
            TabControl tabControl = Gval.SelectedCardTab;
            Node rootNode = new Node();
            if (tabControl.SelectedIndex == 0)
            {
                rootNode = PublicCardRole;
            }
            if (tabControl.SelectedIndex == 1)
            {
                rootNode = PublicCardOther;
            }
            if (tabControl.SelectedIndex == 2)
            {
                rootNode = PublicCardWorld;
            }
            if (rootNode.ChildNodes.Count == 0)
            {
                CSqlitePlus.PoolOperate.Add(rootNode.OwnerName);
                DataJoin.FillInPart(null, rootNode);
            }
        }
    }
}

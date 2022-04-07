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
            if (tabControl.SelectedIndex == 0)
            {
                Gval.CurrentRootNode = BoxExample;
            }
            if (tabControl.SelectedIndex == 1)
            {
                Gval.CurrentRootNode = BoxMaterial;
            }
            if (tabControl.SelectedIndex == 2)
            {
                Gval.CurrentRootNode = NoteTheme;
            }
            if (tabControl.SelectedIndex == 3)
            {
                Gval.CurrentRootNode = NoteInspiration;
            }
            if (Gval.CurrentRootNode.ChildNodes.Count == 0)
            {
                CSqlitePlus.PoolOperate.Add(Gval.CurrentRootNode.OwnerName);
                DataJoin.FillInPart(null, Gval.CurrentRootNode);
            }
        }


        public void LoadForCards()
        {
            TabControl tabControl = Gval.SelectedCardTab;
            if (tabControl.SelectedIndex == 0)
            {
                Gval.CurrentRootNode = PublicCardRole;
            }
            if (tabControl.SelectedIndex == 1)
            {
                Gval.CurrentRootNode = PublicCardOther;
            }
            if (tabControl.SelectedIndex == 2)
            {
                Gval.CurrentRootNode = PublicCardWorld;
            }
            if (Gval.CurrentRootNode.ChildNodes.Count == 0)
            {
                CSqlitePlus.PoolOperate.Add(Gval.CurrentRootNode.OwnerName);
                DataJoin.FillInPart(null, Gval.CurrentRootNode);
            }
        }
    }
}

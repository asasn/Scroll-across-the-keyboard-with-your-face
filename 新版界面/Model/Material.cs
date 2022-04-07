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
        }


        public enum MaterialTabName
        {
            范文 = 0,
            资料 = 1,
            主题 = 2,
            灵感 = 3
        }

        #region 资料库
        public Node BoxExample { set; get; } = new Node() { TabName = MaterialTabName.范文.ToString() };
        public Node BoxMaterial { set; get; } = new Node() { TabName = MaterialTabName.资料.ToString() };
        public Node NoteTheme { set; get; } = new Node() { TabName = MaterialTabName.主题.ToString() };
        public Node NoteInspiration { set; get; } = new Node() { TabName = MaterialTabName.灵感.ToString() };
        #endregion

        /// <summary>
        /// 载入资料库
        /// </summary>
        /// <param name="index"></param>
        public void LoadForMaterialPart()
        {
            TabControl tabControl = Gval.SelectedMaterialTab;
            MaterialTabName flag = (MaterialTabName)tabControl.SelectedIndex;
            Gval.TableName = flag.ToString();
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
                CSqlitePlus.PoolOperate.Add(Gval.WorkSpace);
                DataJoin.FillInPart(Gval.WorkSpace, null, rootNode);
            }
        }

    }
}

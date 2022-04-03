using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Material : BookBase
    {
        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public new void Clear()
        {
            BoxModel.ChildNodes.Clear();
            BoxMaterial.ChildNodes.Clear();
            BoxTheme.ChildNodes.Clear();
            BoxInspiration.ChildNodes.Clear();
        }

        public enum MaterialItemTag
        {
            范文 = 0,
            资料 = 1,
            主题 = 2,
            灵感 = 3
        }

        #region 目录树
        public Node BoxModel { set; get; } = new Node();
        public Node BoxMaterial { set; get; } = new Node();
        public Node BoxTheme { set; get; } = new Node();
        public Node BoxInspiration { set; get; } = new Node();
        #endregion

        public void LoadForMaterialPart(int index)
        {
            ItemIndex = index;
            if (index == (int)MaterialItemTag.范文)
            {
                LoadMPart(BoxModel, MaterialItemTag.范文);
            }
            if (index == (int)MaterialItemTag.资料)
            {
                LoadMPart(BoxMaterial, MaterialItemTag.资料);
            }
            if (index == (int)MaterialItemTag.主题)
            {
                LoadMPart(BoxTheme, MaterialItemTag.主题);
            }
            if (index == (int)MaterialItemTag.灵感)
            {
                LoadMPart(BoxInspiration, MaterialItemTag.灵感);
            }
        }


        private void LoadMPart(Node rootNode, MaterialItemTag partFlag)
        {
            if (rootNode.ChildNodes.Count == 0)
            {
                for (int i = 0; i <= (int)partFlag; i++)
                {
                    rootNode.ChildNodes.Add(new Node() { Title = partFlag.ToString() });
                }

            }
        }

    }
}

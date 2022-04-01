using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Material:BookBase
    {
        public Material()
        {
            Clear();
        }

        /// <summary>
        /// 清空各部分根节点
        /// </summary>
        public new void Clear()
        {
            BoxModel.Clear();
            BoxMaterial.Clear();
            BoxTheme.Clear();
            BoxInspiration.Clear();
        }

        public enum MaterialItemTag
        {
            范文 = 0,
            资料 = 1,
            主题 = 2,
            灵感 = 3
        }

        #region 目录树
        public ObservableCollection<Node> BoxModel { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxMaterial { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxTheme { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxInspiration { set; get; } = new ObservableCollection<Node>();
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


        private void LoadMPart(ObservableCollection<Node> nodes, MaterialItemTag partTag)
        {
            if (nodes.Count == 0)
            {
                for (int i = 0; i <= (int)partTag; i++)
                {
                    nodes.Add(new Node(partTag.ToString()));
                }
                
            }
        }

    }
}

using RootNS.Helper;
using RootNS.View;
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
        public Material()
        {
            this.PropertyChanged += Material_PropertyChanged;
            Name = "index";
            this.InitRootNodes(Name);
            this.InitRootCards(Name);
            RootNodes.Add(MaterialTabName.范文.ToString(), BoxExample);
            RootNodes.Add(MaterialTabName.资料.ToString(), BoxMaterial);
            RootNodes.Add(MaterialTabName.题材.ToString(), NoteTopic);
            RootNodes.Add(MaterialTabName.灵感.ToString(), NoteInspiration);
        }

        private void Material_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //
        }

        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootNodes(string bookName)
        {
            Node[] rootNodes = { this.BoxExample, this.BoxMaterial, this.NoteTopic, this.NoteInspiration};
            foreach (Node node in rootNodes)
            {
                node.OwnerName = bookName;
                node.Owner = this;
            }
        }
        /// <summary>
        /// 根卡片初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootCards(string bookName)
        {
            Card[] rootCards = { this.CardRole, this.CardOther, this.CardWorld };
            foreach (Card card in rootCards)
            {
                card.OwnerName = bookName;
            }
        }
        public enum SettingKeyName
        {
            MaterialSelectedIndex,
        }

        public enum MaterialTabName
        {
            范文 = 0,
            资料 = 1,
            题材 = 2,
            灵感 = 3
        }

        #region 资料库
        public Node BoxExample { set; get; } = new Node() { Uid = String.Empty, TabName = MaterialTabName.范文.ToString(), OwnerName = "index" };
        public Node BoxMaterial { set; get; } = new Node() { Uid = String.Empty, TabName = MaterialTabName.资料.ToString(), OwnerName = "index" };
        public Node NoteTopic { set; get; } = new Node() { Uid = String.Empty, TabName = MaterialTabName.题材.ToString(), OwnerName = "index" };
        public Node NoteInspiration { set; get; } = new Node() { Uid = String.Empty, TabName = MaterialTabName.灵感.ToString(), OwnerName = "index" };
        #endregion


        public List<Node> GetChapterNodes()
        {
            List<Node> nodes = new List<Node>();
            List<Node> roots = new List<Node>() { BoxExample, BoxMaterial};
            foreach (var root in roots)
            {
                GetTreeNodes(nodes, root);
            }
            return nodes;
        }
        public void LoadForMaterialTab()
        {
            Node rootNode = RootNodes[Enum.GetName(typeof(MaterialTabName), Gval.SelectedMaterialTab.SelectedIndex)];
            if (rootNode.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(rootNode);
            }
        }
        public void LoadForPublicCardTab()
        {
            Card rootCard = RootNodes[Enum.GetName(typeof(CardTabName), Gval.SelectedPublicCardTab.SelectedIndex)] as Card;
            if (rootCard.ChildNodes.Count == 0)
            {
                DataIn.FillInCards(rootCard);
            }
        }

        /// <summary>
        /// 载入所有资料
        /// </summary>
        /// <param name="index"></param>
        public void LoadForAllMaterialTabs()
        {
            if (BoxExample.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(BoxExample);
            }
            if (BoxMaterial.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(BoxMaterial);
            }
            if (NoteTopic.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteTopic);
            }
            if (NoteInspiration.ChildNodes.Count == 0)
            {
                DataIn.FillInNodes(NoteInspiration);
            }
        }
    }
}

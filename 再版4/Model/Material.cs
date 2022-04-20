using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Model
{
    public class Material : BaseBook
    {
        public Material()
        {
            this.Name = "index";
            InitRootNodes();
        }

        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootNodes()
        {
            Note[] rootNodes = { this.BoxExample, this.BoxMaterial, this.NoteTheme, this.NoteInspiration };
            foreach (Note note in rootNodes)
            {
                note.Owner = this;
            }
        }

        public enum MaterialTabName
        {
            范文 = 0,
            资料 = 1,
            主题 = 2,
            灵感 = 3
        }

        #region 资料库
        public Note BoxExample { set; get; } = new Note() { TabName = MaterialTabName.范文.ToString() };
        public Note BoxMaterial { set; get; } = new Note() { TabName = MaterialTabName.资料.ToString() };
        public Note NoteTheme { set; get; } = new Note() { TabName = MaterialTabName.主题.ToString() };
        public Note NoteInspiration { set; get; } = new Note() { TabName = MaterialTabName.灵感.ToString() };
        #endregion

    }
}

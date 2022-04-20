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
        public BaseBook()
        {
            InitRootNodes();
        }

        /// <summary>
        /// 根节点初始化
        /// </summary>
        /// <param name="bookName"></param>
        private void InitRootNodes()
        {
            Card[] rootCards = { this.CardRole, this.CardOther, this.CardWorld };
            foreach (Card card in rootCards)
            {
                card.Owner = this;
            }
            MapSite.Owner = this;
        }

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


        /// <summary>
        /// 信息卡片TabItem标志
        /// </summary>
        public enum CardTabName
        {
            角色,
            其他,
            世界,
        }


        /// <summary>
        /// 地图TabItem标志
        /// </summary>
        public enum MapTabName
        {
            地图,
        }



        #region 信息卡
        public Card CardRole { set; get; } = new Card() { TabName = CardTabName.角色.ToString() };
        public Card CardOther { set; get; } = new Card() { TabName = CardTabName.其他.ToString() };
        public Card CardWorld { set; get; } = new Card() { TabName = CardTabName.世界.ToString() };

        #endregion

        #region 地图
        public Site MapSite { set; get; } = new Site() { TabName = MapTabName.地图.ToString() };
        #endregion
    }
}

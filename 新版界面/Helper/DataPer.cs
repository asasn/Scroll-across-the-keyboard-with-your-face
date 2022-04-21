using RootNS.View;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace RootNS.Helper
{
    public class DataPer
    {

        /// <summary>
        /// 保存成Json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SaveJson(CardModel cm)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string strJson = jserializer.Serialize(cm);
            string filePath = Gval.Path.Books + "/" + "CardModel.json";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(strJson);
                }
            }
        }
        /// <summary>
        /// 从Json中读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static CardModel LoadJson()
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string filePath = Gval.Path.Books + "/" + "CardModel.json";
            if (IOTool.IsFileExists(filePath) == false)
            {
                CardModel cm = new CardModel();
                SaveJson(cm);
            }
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sw = new StreamReader(fs, Encoding.UTF8))
                {
                    string strJson = sw.ReadToEnd();
                    return jserializer.Deserialize<CardModel>(strJson);
                }
            }
        }

        /// <summary>
        /// 保存成Xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void SaveCardModel(CardModel cm)
        {
            string filePath = Gval.Path.Books + "/" + "CardModel.xml";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CardModel));
                serializer.Serialize(fs, cm);
            }
        }
        /// <summary>
        /// 从Xml中读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static CardModel LoadCardModel()
        {
            string filePath = Gval.Path.Books + "/" + "CardModel.xml";
            if (IOTool.IsFileExists(filePath) == false)
            {
                CardModel cm = new CardModel();
                SaveCardModel(cm);
            }
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CardModel));
                CardModel obj = (CardModel)serializer.Deserialize(fs);
                return obj;
            }
        }

        public class CardModel
        {
            public Dictionary<string, Part> PartDict { get; set; } = new Dictionary<string, Part>()
            {
                {"角色", new Part()},
                {"其他", new Part()},
                {"世界", new Part()},
           };

            public class Part
            {
                public ObservableCollection<string> Tags { get; set; } = new ObservableCollection<string>();

            }

        }
    }
}

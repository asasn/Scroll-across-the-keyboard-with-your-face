using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RootNS.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 转换成Json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string ObjToJson<T>(T obj)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            return jserializer.Serialize(obj);
        }
        /// <summary>
        /// 从Json中读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static T JsonToObj<T>(string strJson)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            try
            {
                return jserializer.Deserialize<T>(strJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("解析错误 - {0}！", ex));
                return default;
            }
        }

    }
}

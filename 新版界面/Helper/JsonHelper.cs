using System;
//using System.Text.Json;
using System.Web.Script.Serialization;

namespace RootNS.Helper
{
    public class JsonHelper
    {
        ///// <summary>
        ///// 序列化为JsonString
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public static string ObjectToJson<T>(T obj)
        //{
        //    return JsonSerializer.Serialize(obj);
        //}
        ///// <summary>
        ///// 反序列化JsonString为对象
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //public static T JsonToObject<T>(string jsonString)
        //{
        //    if (string.IsNullOrWhiteSpace(jsonString))
        //    {
        //        return default;
        //    }
        //    return JsonSerializer.Deserialize<T>(jsonString);
        //}


        /// <summary>
        /// 把Obj对象转换成Json字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string ObjectToJson<T>(T obj)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            return jserializer.Serialize(obj);
        }
        /// <summary>
        /// 从Json字符串中获取对象属性以载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static T JsonToObject<T>(string strJson)
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

using Newtonsoft.Json;

namespace RootNS.Helper
{


    /// <summary>
    /// Newtonsoft.Json的扩展方法类
    /// </summary>
    public static class NewtonsoftJsonHelper
    {
        /// <summary>
        /// 将对象实例序列化为Json字符串——Newtonsoft.Json
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>Json字符串</returns>
        public static string ObjectToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        /// <summary>
        /// 将Json字符串反序列化为对象实例——Newtonsoft.Json
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>对象实例</returns>
        public static T JsonToObject<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }


    }

}

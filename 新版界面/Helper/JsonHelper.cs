using System.Text.Json;

namespace RootNS.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 序列化为JsonString
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string ObjectToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        /// <summary>
        /// 反序列化JsonString为对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static T JsonToObject<T>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(jsonString);
        }

    }
}

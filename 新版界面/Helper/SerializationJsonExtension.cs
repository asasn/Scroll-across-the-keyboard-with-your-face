using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RootNS.Helper
{


    /// <summary>
    /// System.Runtime.Serialization.Json扩展方法类（[DataContract]和[DataMember]联合使用来标记被序列化的字段）
    /// </summary>
    public static class SerializationJsonExtension
    {
        private static Dictionary<Type, DataContractJsonSerializer> serDic = new Dictionary<Type, DataContractJsonSerializer>();

        private static DataContractJsonSerializer GetSerializer(Type type)
        {
            if (!serDic.ContainsKey(type))
            {
                serDic.Add(type, new DataContractJsonSerializer(type));
            }
            return serDic[type];
        }

        /// <summary>
        /// 将Json字符串反序列化为对象实例——System.Runtime.Serialization.Json（[DataContract]和[DataMember]联合使用来标记被序列化的字段）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>对象实例</returns>
        public static T DeserializeObjectFromJson_SJ<T>(this string jsonString)
        {
            var ser = GetSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }

        /// <summary>
        /// 将对象实例序列化为Json字符串——System.Runtime.Serialization.Json（[DataContract]和[DataMember]联合使用来标记被序列化的字段）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>Json字符串</returns>
        public static string SerializeObjectToJson_SJ<T>(this T obj)
        {
            var ser = GetSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);
                ms.Position = 0;
                using (var sr = new StreamReader(ms, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

}

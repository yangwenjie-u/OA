using Newtonsoft.Json;
using System;

namespace BD.Jcbg.Common
{
    public class JsonSerializer
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Serialize(object instance)
        {
            string value = null;

            try
            {
                value = JsonConvert.SerializeObject(instance, new JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(ex);
            }

            return value;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
            T instance = default(T);

            try
            {
                instance = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings());
            }
            catch (Exception ex)
            {
                SysLog4.WriteLog(value, ex);
            }

            return instance;
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string Serialize(object instance, out string msg)
        {
            string value = null;
            msg = string.Empty;

            try
            {
                value = JsonConvert.SerializeObject(instance, new JsonSerializerSettings());
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(ex);
                msg = ex.Message;
            }

            return value;
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value, out string msg)
        {
            T instance = default(T);
            msg = string.Empty;

            try
            {
                instance = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings());
            }
            catch(Exception ex)
            {
                SysLog4.WriteLog(value, ex);
                msg = ex.Message;
            }

            return instance;
        }
    }
}

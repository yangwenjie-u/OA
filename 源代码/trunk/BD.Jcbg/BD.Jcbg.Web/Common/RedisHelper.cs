using System;
using BD.Jcbg.Common;
using StackExchange.Redis;

namespace BD.Jcbg.Web
{
    public class RedisHelper
    {
        private static string _configString = Configs.RedisConnection;
        private static object _locker = new object();
        private static ConnectionMultiplexer _instance = null;

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            if (string.IsNullOrEmpty(_configString))
                            {
                                throw new Exception("redis连接没有配置");
                            }

                            _instance = ConnectionMultiplexer.Connect(_configString);
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return JsonSerializer.Deserialize<T>(GetDatabase().StringGet(key));
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            return GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        public static bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            return GetDatabase().StringSet(key, JsonSerializer.Serialize(value), expiry);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        public static bool Set(string key, string value, TimeSpan? expiry = null)
        {
            return GetDatabase().StringSet(key, value, expiry);
        }

        /// <summary>
        /// 判断在缓存中是否存在该key的缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return GetDatabase().KeyExists(key);
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 实现递增
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long Increment(string key)
        {
            //三种命令模式
            //Sync,同步模式会直接阻塞调用者，但是显然不会阻塞其他线程。
            //Async,异步模式直接走的是Task模型。
            //Fire - and - Forget,就是发送命令，然后完全不关心最终什么时候完成命令操作。
            //即发即弃：通过配置 CommandFlags 来实现即发即弃功能，在该实例中该方法会立即返回，如果是string则返回null 如果是int则返回0.这个操作将会继续在后台运行，一个典型的用法页面计数器的实现：
            return GetDatabase().StringIncrement(key, flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 实现递减
        /// </summary>·
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Decrement(string key, string value)
        {
            return GetDatabase().HashDecrement(key, value, flags: CommandFlags.FireAndForget);
        }
    }
}
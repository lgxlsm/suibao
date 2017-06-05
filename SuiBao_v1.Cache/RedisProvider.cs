using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using SuiBao_v1.Utility;

namespace SuiBao_v1.Cache
{
    /// <summary>
    /// redis服务提供类
    /// </summary>
    public class RedisProvider : ICacheService
    {
        /// <summary>
        /// 连接池的大小
        /// </summary>
        private const int PoolSize = 50;

        /// <summary>
        /// 出队的等待时间，毫秒数
        /// </summary>
        private const int QueueWait = 2000;

        /// <summary>
        /// 存储空间索引
        /// </summary>
        private int _dbIndex = -1;

        /// <summary>
        /// 连接池对象
        /// </summary>
        private static readonly BlockQueue<ConnectionMultiplexer> RedisPool;

        static RedisProvider()
        {
            RedisPool = new BlockQueue<ConnectionMultiplexer>(PoolSize);
        }

        #region redis操作封装

        public string Get(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).StringGet(key);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public T Get<T>(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                RedisValue value = redis.GetDatabase(_dbIndex).StringGet(key);
                if (!value.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                return default(T);
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool KeyExpire(string key, int timeOut)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).KeyExpire(key, TimeSpan.FromSeconds(timeOut));
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool HasKey(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).KeyExists(key);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool RemoveKey(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).KeyDelete(key);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool Set(string key, string value, int timeOut)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    TimeSpan? expire = null;
                    if (timeOut != -1) { expire = TimeSpan.FromSeconds(timeOut); }
                    return redis.GetDatabase(_dbIndex).StringSet(key, value, expire);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool Set<T>(string key, T value, int timeOut)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    TimeSpan? expire = null;
                    if (timeOut != -1) { expire = TimeSpan.FromSeconds(timeOut); }
                    return redis.GetDatabase(_dbIndex).StringSet(key, JsonConvert.SerializeObject(value), expire);
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public long StringIncrement(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).StringIncrement(key);
                }
                else
                {
                    return 0;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public long StringDecrement(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).StringDecrement(key);
                }
                else
                {
                    return 0;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool HashSet(string key, Dictionary<string, string> values)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    List<HashEntry> entry = new List<HashEntry>();
                    foreach (var item in values)
                    {
                        entry.Add(new HashEntry(item.Key, item.Value));
                    }
                    redis.GetDatabase(_dbIndex).HashSet(key, entry.ToArray());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public bool HashSet(string key, string filed, string value)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    redis.GetDatabase(_dbIndex).HashSet(key, filed, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public string HashGet(string key, string filed)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    var result = redis.GetDatabase(_dbIndex).HashGet(key, filed);
                    return result.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public Dictionary<string, string> HashGet(string key)
        {
            var redis = RedisPool.DeQueue(QueueWait);
            try
            {
                if (redis != null && redis.IsConnected)
                {
                    return redis.GetDatabase(_dbIndex).HashGetAll(key).ToStringDictionary();
                }
                else
                {
                    return null;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            finally
            {
                RedisPool.EnQueue(redis);
            }
        }

        public void SetDbIndex(int index)
        {
            _dbIndex = index;
        }

        #endregion


        /// <summary>
        /// 启动服务，初始化redis连接池
        /// </summary>
        public void StartUp()
        {
            ConfigurationOptions config = new ConfigurationOptions()
            {
                AbortOnConnectFail = false,
                ConnectRetry = 10,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                EndPoints = { { ConfigurationManager.AppSettings.Get("RedisHost") } },
                Password = ConfigurationManager.AppSettings.Get("RedisAccessKey"),
                AllowAdmin = true,
                KeepAlive = 180
            };
            TextWriter log = new LogWriter();
            for (int i = 0; i < PoolSize; i++)
            {
                var conn = ConnectionMultiplexer.Connect(config, log);
                conn.ConnectionFailed += Redis_ConnectionFailed;
                conn.ErrorMessage += Redis_ErrorMessage;
                conn.ConnectionRestored += Redis_ConnectionRestored; ;
                RedisPool.EnQueue(conn);
            }
        }


        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Dispose()
        {
            RedisPool.Clear();
            GC.SuppressFinalize(this);
        }

        #region 注册redis状态事件

        private void Redis_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            LogHelper.Info("redis重新连接成功，详细信息：" + e.EndPoint.ToString());
        }

        private void Redis_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            LogHelper.Info("redis服务响应失败，错误信息：" + e.Message);
        }

        private void Redis_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            LogHelper.Info("redis连接失败，错误信息：" + e.Exception.Message);
        }

        #endregion
    }


    /// <summary>
    /// 为redis重写一套log服务
    /// </summary>
    public class LogWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return System.Text.Encoding.UTF8;
            }
        }

        public override void WriteLine(string format, object arg0)
        {
            LogHelper.Info(string.Format(format, arg0));
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            LogHelper.Info(string.Format(format, arg0, arg1));
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            LogHelper.Info(string.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine(string format, params object[] arg)
        {
            LogHelper.Info(string.Format(format, arg));
        }

        public override void WriteLine(string value)
        {
            LogHelper.Info(value);
        }
    }
}

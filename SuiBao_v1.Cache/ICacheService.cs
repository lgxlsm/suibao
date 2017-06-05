using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuiBao_v1.Cache
{
    /// <summary>
    /// 缓存服务的公共接口
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// 启动缓存服务
        /// </summary>
        void StartUp();

        /// <summary>
        /// 设置数据存储区索引
        /// </summary>
        /// <param name="index"></param>
        void SetDbIndex(int index);

        /// <summary>
        /// 关闭服务
        /// </summary>
        void Dispose();

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveKey(string key);

        /// <summary>
        /// 设置缓存项过期时间，秒为单位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        bool KeyExpire(string key, int timeOut);

        /// <summary>
        /// 判断缓存项是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool HasKey(string key);

        /// <summary>
        /// 插入缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut">过期时间，秒为单位</param>
        /// <returns></returns>
        bool Set(string key, string value, int timeOut);

        /// <summary>
        /// 读取缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 插入缓存项-复杂类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value, int timeOut);

        /// <summary>
        /// 缓存值自增
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long StringIncrement(string key);

        /// <summary>
        /// 缓存值自减
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long StringDecrement(string key);

        /// <summary>
        /// 插入缓存项-Hash类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool HashSet(string key, Dictionary<string, string> values);

        /// <summary>
        /// 插入缓存项-Hash类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool HashSet(string key, string filed, string value);

        /// <summary>
        /// 读取Hash缓存项的字段值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        string HashGet(string key, string filed);

        /// <summary>
        /// 读取Hash缓存项的所有键值对
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, string> HashGet(string key);

        /// <summary>
        /// 读取缓存项-复杂类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
    }
}

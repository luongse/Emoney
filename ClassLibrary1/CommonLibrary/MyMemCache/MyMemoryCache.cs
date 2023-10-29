using System;
using System.Runtime.Caching;

namespace MyMemCache
{
    public delegate object MemCallBack(object obj);

    public interface IMyCache
    {
        /// <summary>
        /// Author:  
        /// <para>Date : 19-12-2014</para>
        /// <para>Add new CacheItem</para>
        /// </summary>
        /// <param name="model"></param>
        void Set(MemoryCacheModel model);

        /// <summary>
        /// Author: PhatVT
        /// <para>Date : 23-12-2014</para>
        /// <para>Add new CacheItem</para>
        /// </summary>
        /// <param name="model"></param>
        void Set(MemoryCacheModel model, MemCallBack callBack);

        /// <summary>
        /// Author:  
        /// <para>Date : 19-12-2014</para>
        /// <para>Get Item</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Author: PhatVT
        /// <para>Date : 20-03-2015</para>
        /// <para>Get Item</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// Author:  
        /// <para>Date : 19-12-2014</para>
        /// <para>Xoa CacheItem Data</para>
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }

    internal class MyMemoryCache : IMyCache
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        //private readonly RedisCache _redisCache;
        private readonly CacheConfigModel _model;

        public MyMemoryCache(CacheConfigModel model)
        {
            if (model == null) { throw new Exception("MyMemCache.MyMemoryCache.CacheModel is invalid"); }
            _model = model;
        }

        public void Set(MemoryCacheModel model)
        {
            var cacheItem = new CacheItem(model.Key) { Value = model.Data };
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration =
                    !model.AbsoluteExpiration.HasValue
                        ? ObjectCache.InfiniteAbsoluteExpiration
                        : model.AbsoluteExpiration.GetValueOrDefault(),
                SlidingExpiration =
                    !model.SlidingExpiration.HasValue
                        ? ObjectCache.NoSlidingExpiration
                        : model.SlidingExpiration.GetValueOrDefault()
            };
            if (model.OnRemovedCallback != null)
            {
                cachePolicy.RemovedCallback = model.OnRemovedCallback;
            }

            switch (_model.MemoryCache)
            {
                default:
                    if (!_cache.Add(cacheItem, cachePolicy))
                    {
                        _cache.Set(cacheItem, cachePolicy);
                    }
                    break;
            }
        }

        /// <summary>
        /// Author: PhatVT
        /// <para>Date : 23-12-2014</para>
        /// <para>Add new CacheItem</para>
        /// </summary>
        /// <param name="model"></param>
        public void Set(MemoryCacheModel model, MemCallBack callBack)
        {
            model.Data = callBack;
            Set(model);
        }

        public object Get(string key)
        {
            var result = _cache.Get(key);
            return result;
        }

        public T Get<T>(string key)
        {
            var result = _cache.Get(key);
            return result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}

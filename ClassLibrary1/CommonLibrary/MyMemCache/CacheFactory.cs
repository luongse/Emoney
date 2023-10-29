using System;

namespace MyMemCache
{
    public class CacheFactory
    {
        /// <summary>
        /// <para>Khoi tao Cache</para>
        /// </summary>
        /// <param name="model">Cau hinh su dung Cache</param>
        public void InitCache(CacheConfigModel model)
        {
            if (model == null) { throw new Exception("MyMemCache.CacheFactory.InitCache.CacheModel is invalid"); }
            /* Check model.MemoryCache */
            switch (model.MemoryCache)
            {
                default:
                    MemoryCache = new MyMemoryCache(model);
                    break;
            }
        }

        /// <summary>
        /// <para>Cache in-process</para>
        /// </summary>
        public static IMyCache MemoryCache
        {
            get; set;
        }
    }
}

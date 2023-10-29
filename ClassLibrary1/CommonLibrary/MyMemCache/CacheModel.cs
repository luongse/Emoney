using System;
using System.Runtime.Caching;

namespace MyMemCache
{
    /// <summary>
    /// <para>Cau hinh khoi tao CacheObject</para>
    /// </summary>
    public class CacheConfigModel
    {
        public CacheConfigModel(DateTimeOffset absoluteExpiration, TimeSpan slidingExpiration)
        {
            MemoryCache = MemoryCacheType.MyMemoryCache;
            DefaultAbsoluteExpiration = absoluteExpiration;
            DefaultSlidingExpiration = slidingExpiration;
        }

        /// <summary>
        /// Thoi gian het han cua cache vd: set expire 19-12-2014 => 20-12-2014 Cache will be removed ke ca khi cache dc truy xuat thuong xuyen
        /// <para>Absolute Expiration:  Absolute expiration means It will expire cache after some time period set at the time of activating cache. This will be absolute expiration whether cache will be used or not It will expire the cache. This type of expiration used to  cache data which are not frequently changing.</para>
        /// </summary>
        public DateTimeOffset DefaultAbsoluteExpiration { get; set; }

        /// <summary>
        /// Khoang thoi gian luu giu cache vd: 5p => sau 5p cache se bi remove. neu trong khoang 5p nay cache dc su dung thi thoi gian ton tai se dc reset lai 5p tuong tu session
        /// <para>Sliding Expiration: Sliding expiration means It will expire cache after time period at the time of activating cache if any request is not made during this time period. This type of expiration is useful when there are so many data to cache. So It will put those items in the cache which are frequently used in the application. So it will not going to use unnecessary memory.</para>
        /// </summary>
        public TimeSpan DefaultSlidingExpiration { get; set; }

        public MemoryCacheType MemoryCache { get; set; }
    }

    /// <summary>
    /// <para>Tham so tao cache</para>
    /// </summary>
    public class MemoryCacheModel
    {
        /// <summary>
        /// Unique Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Data need to cache
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Thoi gian het han cua cache vd: set expire 19-12-2014 => 20-12-2014 Cache will be removed ke ca khi cache dc truy xuat thuong xuyen
        /// <para>Absolute Expiration:  Absolute expiration means It will expire cache after some time period set at the time of activating cache. This will be absolute expiration whether cache will be used or not It will expire the cache. This type of expiration used to  cache data which are not frequently changing.</para>
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        /// <summary>
        /// Khoang thoi gian luu giu cache vd: 5p => sau 5p cache se bi remove. neu trong khoang 5p nay cache dc su dung thi thoi gian ton tai se dc reset lai 5p tuong tu session
        /// <para>Sliding Expiration: Sliding expiration means It will expire cache after time period at the time of activating cache if any request is not made during this time period. This type of expiration is useful when there are so many data to cache. So It will put those items in the cache which are frequently used in the application. So it will not going to use unnecessary memory.</para>
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; }

        /// <summary>
        /// Method se duoc Execute khi cache removed
        /// <para>Method must have Singnature : OnRemovedCallback(CacheEntryRemovedArguments args)</para>
        /// </summary>
        public CacheEntryRemovedCallback OnRemovedCallback { get; set; }
    }

    public enum MemoryCacheType
    {
        MyMemoryCache,
        RedisCache
    }
}

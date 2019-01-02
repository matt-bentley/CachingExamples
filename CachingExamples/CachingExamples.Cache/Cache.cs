using CachingExamples.Cache.Abstract;
using CachingExamples.Cache.Caches;
using CachingExamples.Cache.Configuration;
using CachingExamples.Cache.Containers;
using System;
using System.Linq;

namespace CachingExamples.Cache
{
    /// <summary>
    /// Wrapper for accessing <see cref="ICache"/> implementations
    /// </summary>
    public static class Cache
    {
        public static ICache Get(CacheType cacheType)
        {
            ICache cache = new NullCache();
            try
            {
                var caches = CacheContainer.GetAll<ICache>();
                cache = caches.Where(c => c.CacheType == cacheType).Last();
            }
            catch (Exception ex)
            {
                //Log.Warn("Failed to instantiate cache of type: {0}, using null cache. Exception: {1}", cacheType, ex);
                cache = new NullCache();
            }
            return cache;
        }

        public static ICache Default
        {
            get
            {
                return Get(CacheConfiguration.Current.DefaultCacheType);
            }
        }

        public static ICache Memory
        {
            get
            {
                return Get(CacheType.Memory);
            }
        }

        public static ICache Disk
        {
            get
            {
                return Get(CacheType.Disk);
            }
        }
    }
}

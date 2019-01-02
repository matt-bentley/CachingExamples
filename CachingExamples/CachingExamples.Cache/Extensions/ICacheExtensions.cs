﻿using CachingExamples.Cache.Abstract;

namespace CachingExamples.Cache.Extensions
{
    /// <summary>
    /// Extensions to <see cref="ICache"/>
    /// </summary>
    public static class ICacheExtensions
    {
        public static T Get<T>(this ICache cache, string key) where T : class
        {
            return cache.Get<T>(key);
        }
    }
}

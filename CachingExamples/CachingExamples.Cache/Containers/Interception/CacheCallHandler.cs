using CachingExamples.Cache.Configuration;
using CachingExamples.Cache.Serialization;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;

namespace CachingExamples.Cache.Containers.Interception
{
    /// <summary>
    /// <see cref="ICallHandler"/> for intercepting method calls and caching the responses
    /// </summary>
    public class CacheCallHandler : ICallHandler
    {
        /// <summary>
        /// Order which the handler should fire
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Returns previously cached response or invokes method and caches response
        /// </summary>
        /// <param name="input"></param>
        /// <param name="getNext"></param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            //if caching is disabled, leave:
            if (!CacheConfiguration.Current.Enabled)
            {
                return Proceed(input, getNext);
            }

            //get the cache settings from the attribute & config:
            var cacheAttribute = GetCacheSettings(input);
            if (cacheAttribute.Disabled)
            {
                return Proceed(input, getNext);
            }

            //if there's no cache provider, leave:
            var cache = Cache.Get(cacheAttribute.CacheType);
            var serializer = Serializer.GetCurrent(cacheAttribute.SerializationFormat);
            if (cache == null || cache.CacheType == CacheType.Null || serializer == null)
            {
                return Proceed(input, getNext);
            }

            // Log cache request here

            var returnType = ((MethodInfo)input.MethodBase).ReturnType;
            var cacheKey = CacheKeyBuilder.GetCacheKey(input, serializer);
            var cachedValue = cache.Get(returnType, cacheKey, cacheAttribute.SerializationFormat);
            if (cachedValue == null)
            {
                // missed the cache
                // Log here for instrumentation

                //call the intended method to set the return value
                var methodReturn = Proceed(input, getNext);
                //only cache if we have a real return value & no exception:
                if (methodReturn != null && methodReturn.ReturnValue != null && methodReturn.Exception == null)
                {
                    var lifespan = cacheAttribute.Lifespan;
                    if (lifespan.TotalSeconds > 0)
                    {
                        cache.Set(cacheKey, methodReturn.ReturnValue, lifespan, cacheAttribute.SerializationFormat);
                    }
                    else
                    {
                        cache.Set(cacheKey, methodReturn.ReturnValue, cacheAttribute.SerializationFormat);
                    }
                }
                return methodReturn;
            }
            else
            {
                // hit the cache
                // Log here for instrumentation
            }
            return input.CreateMethodReturn(cachedValue);
        }

        private static CacheAttribute GetCacheSettings(IMethodInvocation input)
        {
            //get the cache attribute & check if overridden in config:
            var attributes = input.MethodBase.GetCustomAttributes(typeof(CacheAttribute), false);
            var cacheAttribute = (CacheAttribute)attributes[0];
            var cacheKeyPrefix = CacheKeyBuilder.GetCacheKeyPrefix(input);
            var targetConfig = CacheConfiguration.Current.Targets[cacheKeyPrefix];
            if (targetConfig != null)
            {
                cacheAttribute.Disabled = !targetConfig.Enabled;
                cacheAttribute.Days = targetConfig.Days;
                cacheAttribute.Hours = targetConfig.Hours;
                cacheAttribute.Minutes = targetConfig.Minutes;
                cacheAttribute.Seconds = targetConfig.Seconds;
                cacheAttribute.CacheType = targetConfig.CacheType;
                cacheAttribute.SerializationFormat = targetConfig.SerializationFormat;
            }
            if (cacheAttribute.SerializationFormat == SerializationFormat.Null)
            {
                cacheAttribute.SerializationFormat = CacheConfiguration.Current.DefaultSerializationFormat;
            }
            if (cacheAttribute.CacheType == CacheType.Null)
            {
                cacheAttribute.CacheType = CacheConfiguration.Current.DefaultCacheType;
            }
            return cacheAttribute;
        }

        private static IMethodReturn Proceed(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            return getNext()(input, getNext);
        }
    }
}

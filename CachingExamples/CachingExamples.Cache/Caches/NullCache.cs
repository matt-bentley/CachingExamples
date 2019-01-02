using CachingExamples.Cache.Abstract;
using System;

namespace CachingExamples.Cache.Caches
{
    /// <summary>
    /// <see cref="ICache"/> implementation which does nothing
    /// </summary>
    /// <remarks>
    /// Used when real caches are unavailable or disabled
    /// </remarks>
    public class NullCache : CacheBase
    {
        public override CacheType CacheType
        {
            get { return CacheType.Null; }
        }

        protected override void InitialiseInternal() { }

        protected override void SetInternal(string key, object value) { }

        protected override void SetInternal(string key, object value, DateTime expiresAt) { }

        protected override void SetInternal(string key, object value, TimeSpan validFor) { }

        protected override object GetInternal(string key)
        {
            return null;
        }

        protected override void RemoveInternal(string key) { }

        protected override bool ExistsInternal(string key)
        {
            return false;
        }
    }
}

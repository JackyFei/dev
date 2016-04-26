using Common.Utils;
using System;
using System.Runtime.Caching;

namespace Common.Caching.Microsoft
{
    /// <summary>
    /// The memory caching implementation for ICache interface.
    /// </summary>
    public class MsMemoryCache : ICache
    {
        #region Fields & Properties

        private readonly MemoryCache _cache;
        private readonly string _name;
        private bool _disposed;

        #endregion

        #region Constructor

        public MsMemoryCache()
        {
            _name = "default";
            _cache = MemoryCache.Default;
        }

        public MsMemoryCache(string name)
        {
            Guard.ArgumentNotNullOrEmpty(name, "name");

            _name = name;
            _cache = new MemoryCache(name);
        }

        #endregion

        #region ICache
        public string Name
        {
            get { return string.Format("MsMemoryCache-{0}", _name); }
        }

        public bool Add<T>(string key, T value)
        {   
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");
            return _cache.Add(key, value, DateTimeOffset.MaxValue);
        }

        public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");
            return _cache.Add(key, value, absoluteExpiration);
        }

        public bool Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");
            return _cache.Add(key, value,
                new CacheItemPolicy { SlidingExpiration = slidingExpiration, Priority = CacheItemPriority.Default });
        }

        public void Set<T>(string key, T value)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");
            _cache.Set(key, value, DateTimeOffset.MaxValue);
        }

        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");
            _cache.Set(key, value, absoluteExpiration);
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");
            _cache.Set(key, value, new CacheItemPolicy { SlidingExpiration = slidingExpiration, Priority = CacheItemPriority.Default });
        }

        public object Get(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            return _cache.Get(key);
        }

        public T Get<T>(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            return (T)_cache.Get(key);
        }

        public object Remove(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            return _cache.Remove(key);
        }

        public T Remove<T>(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            return (T)_cache.Remove(key);
        }

        public bool Contains(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            return _cache.Contains(key);
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _cache.Dispose();
            }

            _disposed = true;
        }

        ~MsMemoryCache()
        {
            Dispose(false);
        }
        #endregion
    }
}

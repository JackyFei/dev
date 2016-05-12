using System;
using Common.Utils;
using Microsoft.ApplicationServer.Caching;

namespace Common.Caching.Microsoft
{
    /// <summary>
    /// The caching implementation by calling AppFabric caching server.
    /// </summary>
    public class AppFabricCache : ICache
    {
        #region Fields & Properties

        private readonly string _name;
        private readonly DataCacheFactory _dataCacheFactory;
        private readonly DataCache _cache;
        private bool _disposed;
        #endregion

        #region Constructor

        public AppFabricCache(string name)
        {
            Guard.ArgumentNotNullOrEmpty(name, "name");
            _name = name;

            _dataCacheFactory = new DataCacheFactory();
            _cache = _dataCacheFactory.GetCache(name);
        }

        public AppFabricCache() : this("default") { }

        #endregion

        #region ICache
        public string Name
        {
            get { return string.Format("AppFabricCache-{0}", _name); }
        }

        public bool Add<T>(string key, T value)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");

            try
            {
                _cache.Add(key, value);
                return true;
            }
            catch (DataCacheException ex)
            {
                if (ex.ErrorCode == DataCacheErrorCode.KeyAlreadyExists)
                {
                    return false;
                }
                // if other error, just throw exception.
                throw;
            }
        }

        public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");

            try
            {
                var timeout = absoluteExpiration - DateTimeOffset.Now;
                if (timeout <= TimeSpan.Zero)
                {
                    return false;
                }
                _cache.Add(key, value, timeout);
                return true;
            }
            catch (DataCacheException ex)
            {
                if (ex.ErrorCode == DataCacheErrorCode.KeyAlreadyExists)
                {
                    return false;
                }
                // if other error, just throw exception.
                throw;
            }
        }

        public bool Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            throw new NotSupportedException("Appfabric doesn't support slidingExpiration directly.");
        }

        public void Set<T>(string key, T value)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");

            _cache.Put(key, value);
        }

        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            Guard.ArgumentNotNull(value, "value");

            var timeout = absoluteExpiration - DateTimeOffset.Now;
            _cache.Put(key, value, timeout);
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            throw new NotSupportedException("Appfabric doesn't support slidingExpiration directly.");
        }

        public bool Contains(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            var cacheItem = _cache.GetCacheItem(key);
            return cacheItem != null;
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
            var cacheItem = _cache.GetCacheItem(key);
            if (cacheItem == null)
            {
                return null;
            }

            _cache.Remove(key);
            return cacheItem.Value;
        }

        public T Remove<T>(string key)
        {
            Guard.ArgumentNotNullOrEmpty(key, "key");
            var cacheItem = _cache.GetCacheItem(key);
            if (cacheItem == null)
            {
                return default(T);
            }

            _cache.Remove(key);
            return (T)cacheItem.Value;
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
                _dataCacheFactory.Dispose();
            }

            _disposed = true;
        }

        ~AppFabricCache()
        {
            Dispose(false);
        }
        #endregion
    }
}

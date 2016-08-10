using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Exceptions;
using Alachisoft.NCache.Web.Caching;
using Common.Utils;
using System;
using System.Diagnostics;
using ANCache = Alachisoft.NCache.Web.Caching.NCache;

namespace Common.Caching.Alachisoft
{
    public class NCache : ICache
    {
        #region Fields & Properties
        private readonly string _cacheId;
        private readonly string _clientCacheId;
        private readonly Cache _cache;
        private bool _disposed;
        #endregion

        #region Constructors
        public NCache(string cacheId)
        {
            Guard.ArgumentNotNullOrEmpty(cacheId, "cacheId");
            _cacheId = cacheId;
            _cache = ANCache.InitializeCache(cacheId);
        }

        public NCache(string cacheId, string clientCacheId)
        {
            Guard.ArgumentNotNullOrEmpty(cacheId, "cacheId");
            Guard.ArgumentNotNullOrEmpty(clientCacheId, "clientCacheId");
            _cacheId = cacheId;
            _clientCacheId = clientCacheId;
            _cache = ANCache.InitializeCache(cacheId, clientCacheId);
        }
        #endregion

        #region ICache
        public string Name => $"NCache-{_cacheId}-{_clientCacheId}";

        public bool Add<T>(string key, T value)
        {
            try
            {
                var cacheItemVersion = _cache.Add(key, value);
                return true;
            }
            catch (OperationFailedException ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            try
            {
                var cacheItemVersion = _cache.Add(key, value, null, absoluteExpiration.DateTime, Cache.NoSlidingExpiration, CacheItemPriority.Default);
                return true;
            }
            catch (OperationFailedException ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        public bool Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            try
            {
                var cacheItemVersion = _cache.Add(key, value, null, Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.Default);
                return true;
            }
            catch (OperationFailedException ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        public void Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public object Remove(string key)
        {
            return _cache.Remove(key);
        }

        public T Remove<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key);
        }

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

        ~NCache()
        {
            Dispose(false);
        }
        #endregion
        #endregion
    }
}

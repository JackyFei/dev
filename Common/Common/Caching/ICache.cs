using System;

namespace Common.Caching
{
    /// <summary>
    /// The abstraction of caching system which provides basic object caching capability.
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Name of the cache instance.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Add the key/object to the caching system, never expire.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">object value</param>
        /// <returns>true if success, false if the key has already been added.</returns>
        bool Add<T>(string key, T value);

        /// <summary>
        /// Add the key/object to the caching system with absolute expiration.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">object value</param>
        /// <param name="absoluteExpiration">the absolute expiration time.</param>
        /// <returns>true if success, false if the key has already been added.</returns>
        bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration);

        /// <summary>
        /// Add the key/object to the caching system with sliding expiration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">the time span for the sliding</param>
        /// <returns>true if success, false if the key has already been added.</returns>
        bool Add<T>(string key, T value, TimeSpan slidingExpiration);

        /// <summary>
        /// Add if the key is not in the caching system, update if the key has already been added to the caching system with key/object, never expire.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">object value</param>
        void Set<T>(string key, T value);


        /// <summary>
        /// Add if the key is not in the caching system, update if the key has already been added to the caching system with key/object, absolute expiration.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">object value</param>
        /// <param name="absoluteExpiration">absolute expiration time</param>
        void Set<T>(string key, T value, DateTimeOffset absoluteExpiration);

        /// <summary>
        /// Add if the key is not in the caching system, update if the key has already been added to the caching system with key/object, sliding expiration.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">object value</param>
        /// <param name="slidingExpiration">sliding expiration timespan</param>
        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        /// <summary>
        /// Get the object value from caching system by using the key.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>the object in the caching system for the key, if key doesn't exist, null is returned.</returns>
        object Get(string key);

        /// <summary>
        /// Get the T object from caching system.
        /// </summary>
        /// <typeparam name="T">type of the cached item.</typeparam>
        /// <param name="key">key</param>
        /// <returns>returns the T object in the caching system by the key.
        /// If key doesn't exist, null is returned.
        /// If key exists, but the object value cannot be cast to type of T, cast exception is thrown.
        /// </returns>
        T Get<T>(string key);

        /// <summary>
        /// Remove the key/object by the key in the caching system.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>the object which is removed.</returns>
        object Remove(string key);

        /// <summary>
        /// Remove the key/object by the key in the caching system.
        /// </summary>
        /// <typeparam name="T">type of cached object</typeparam>
        /// <param name="key">key</param>
        /// <returns>the T object which is removed, if the obj is not type of T, cast exception is thrown.</returns>
        T Remove<T>(string key);

        /// <summary>
        /// Check if a key exists in caching system or not.
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true if the key is found, false if the key doesn't exist in the caching system.</returns>
        bool Contains(string key);
    }
}

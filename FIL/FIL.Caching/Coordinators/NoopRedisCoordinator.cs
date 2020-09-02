using FIL.Caching.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Caching.Coordinators
{
    /// <summary>
    /// For use when you dont want redis used, but enabled elsewhere
    /// </summary>
    public class NoopRedisCoordinator : ICacheCoordinator
    {
        public Task<T> Get<T>(string cacheKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable
        {
            return apiCall.Invoke();
        }

        public Task<IEnumerable<T>> GetList<T>(Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            return apiCall.Invoke();
        }

        public Task<T> GetOwnedItem<T>(Type ownerType, string ownerKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable
        {
            return apiCall.Invoke();
        }

        public Task<IEnumerable<T>> GetOwnedList<T>(Type ownerType, string ownerKey,
            Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable
        {
            return apiCall.Invoke();
        }

        public void Save<T>(T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
        }

        public void ClearCache<T>(T item) where T : ICacheable
        {
        }

        public void ClearCache<T>(IEnumerable<T> item) where T : ICacheable
        {
        }

        public void Save(string key, string value, TimeSpan? expirationTimeSpan = null)
        {
        }

        public void Delete(string key)
        {
        }

        public string Get(string key)
        {
            return null;
        }
    }
}
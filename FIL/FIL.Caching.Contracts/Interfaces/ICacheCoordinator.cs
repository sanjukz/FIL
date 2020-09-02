using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Caching.Contracts.Interfaces
{
    public interface ICacheCoordinator
    {
        Task<T> Get<T>(string cacheKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null)
            where T : class, ICacheable;

        Task<IEnumerable<T>> GetList<T>(Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null) where T : ICacheable;

        Task<T> GetOwnedItem<T>(Type ownerType, string ownerKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable;

        Task<IEnumerable<T>> GetOwnedList<T>(Type ownerType, string ownerKey, Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable;

        void Save<T>(T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable;

        void ClearCache<T>(T item) where T : ICacheable;

        void ClearCache<T>(IEnumerable<T> item) where T : ICacheable;

        void Save(string key, string value, TimeSpan? expirationTimeSpan = null);

        void Delete(string key);

        string Get(string key);
    }
}
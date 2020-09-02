using System;
using System.Collections.Generic;

namespace FIL.Caching.Contracts.Interfaces
{
    public interface ICacheHelper
    {
        T Get<T>(string cacheKey) where T : class, ICacheable;

        IEnumerable<T> GetList<T>() where T : ICacheable;

        T GetOwnedItem<T>(Type ownerType, string ownerKey) where T : class, ICacheable;

        IEnumerable<T> GetOwnedList<T>(Type ownerType, string ownerKey) where T : class, ICacheable;

        void Save<T>(T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable;

        void SaveList<T>(IEnumerable<T> data, TimeSpan? expirationTimeSpan = null) where T : ICacheable;

        void SaveOwnedItem<T>(T data, Type ownerType, string ownerKey, TimeSpan? expirationTimeSpan = null) where T : ICacheable;

        void SaveOwnedList<T>(IEnumerable<T> data, Type ownerType, string ownerKey, TimeSpan? expirationTimeSpan = null) where T : ICacheable;

        void ClearCache<T>(T item) where T : ICacheable;

        void ClearCache<T>(IEnumerable<T> item) where T : ICacheable;

        void FlushAll();

        void Save(string key, string value, TimeSpan? expirationTimeSpan = null);

        void Delete(string key);

        string Get(string key);
    }
}
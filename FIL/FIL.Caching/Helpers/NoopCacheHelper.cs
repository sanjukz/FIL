using FIL.Caching.Contracts.Interfaces;
using System;
using System.Collections.Generic;

namespace FIL.Caching.Helpers
{
    public class NoopCacheHelper : ICacheHelper
    {
        public T Get<T>(string cacheKey) where T : class, ICacheable
        {
            return null;
        }

        public IEnumerable<T> GetList<T>() where T : ICacheable
        {
            return null;
        }

        public T GetOwnedItem<T>(Type ownerType, string ownerKey) where T : class, ICacheable
        {
            return null;
        }

        public IEnumerable<T> GetOwnedList<T>(Type ownerType, string ownerKey) where T : class, ICacheable
        {
            return null;
        }

        public void Save<T>(T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
        }

        public void SaveList<T>(IEnumerable<T> data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
        }

        public void SaveOwnedItem<T>(T data, Type ownerType, string ownerKey, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
        }

        public void SaveOwnedList<T>(IEnumerable<T> data, Type ownerType, string ownerKey, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
        }

        public void ClearCache<T>(T item) where T : ICacheable
        {
        }

        public void ClearCache<T>(IEnumerable<T> item) where T : ICacheable
        {
        }

        public void FlushAll()
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
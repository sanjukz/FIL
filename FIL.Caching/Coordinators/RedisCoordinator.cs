using FIL.Caching.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Caching.Coordinators
{
    public class RedisCoordinator : ICacheCoordinator
    {
        private readonly ICacheHelper _redisHelper;

        public RedisCoordinator(ICacheHelper redisHelper)
        {
            _redisHelper = redisHelper;
        }

        public virtual async Task<T> Get<T>(string cacheKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable
        {
            var redisResult = _redisHelper.Get<T>(cacheKey);
            if (redisResult != null)
            {
                return redisResult;
            }

            var apiResult = await apiCall.Invoke().ConfigureAwait(false);
            _redisHelper.Save(apiResult, expirationTimeSpan);
            return apiResult;
        }

        public virtual async Task<IEnumerable<T>> GetList<T>(Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            var redisResult = _redisHelper.GetList<T>();
            if (redisResult != null)
            {
                return redisResult;
            }

            var apiResult = (await apiCall.Invoke()).ToList();
            _redisHelper.SaveList(apiResult, expirationTimeSpan);
            return apiResult;
        }

        public virtual async Task<T> GetOwnedItem<T>(Type ownerType, string ownerKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null) where T : class, ICacheable
        {
            var redisResult = _redisHelper.GetOwnedItem<T>(ownerType, ownerKey);
            if (redisResult != null)
            {
                return redisResult;
            }

            var apiResult = await apiCall.Invoke();
            _redisHelper.SaveOwnedItem(apiResult, ownerType, ownerKey, expirationTimeSpan);
            return apiResult;
        }

        public virtual async Task<IEnumerable<T>> GetOwnedList<T>(Type ownerType, string ownerKey, Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null)
            where T : class, ICacheable
        {
            var redisResult = _redisHelper.GetOwnedList<T>(ownerType, ownerKey);
            if (redisResult != null)
            {
                return redisResult;
            }
            var apiResult = (await apiCall.Invoke()).ToList();
            _redisHelper.SaveOwnedList(apiResult, ownerType, ownerKey, expirationTimeSpan);
            return apiResult;
        }

        public void Save<T>(T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            _redisHelper.Save(data, expirationTimeSpan);
        }

        public void ClearCache<T>(T item) where T : ICacheable
        {
            ClearCache(new List<T> { item }.AsEnumerable());
        }

        public void ClearCache<T>(IEnumerable<T> items) where T : ICacheable
        {
            _redisHelper.ClearCache(items);
        }

        public void Save(string key, string value, TimeSpan? expirationTimeSpan = null)
        {
            _redisHelper.Save(key, value, expirationTimeSpan);
        }

        public void Delete(string key)
        {
            _redisHelper.Delete(key);
        }

        public virtual string Get(string key)
        {
            return _redisHelper.Get(key);
        }
    }
}
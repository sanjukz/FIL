using FIL.Caching.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Caching.Coordinators
{
    /// <summary>
    /// For use in write only scenarios, like KITMS, etc. Will ready from DB, but update cache
    /// </summary>
    public class WriteOnlyApiRedisCoordinator : RedisCoordinator
    {
        public WriteOnlyApiRedisCoordinator(ICacheHelper redisHelper)
            : base(redisHelper)
        {
        }

        public override Task<T> Get<T>(string cacheKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null)
        {
            return apiCall.Invoke();
        }

        public override Task<IEnumerable<T>> GetList<T>(Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null)
        {
            return apiCall.Invoke();
        }

        public override Task<T> GetOwnedItem<T>(Type ownerType, string ownerKey, Func<Task<T>> apiCall, TimeSpan? expirationTimeSpan = null)
        {
            return apiCall.Invoke();
        }

        public override Task<IEnumerable<T>> GetOwnedList<T>(Type ownerType, string ownerKey, Func<Task<IEnumerable<T>>> apiCall, TimeSpan? expirationTimeSpan = null)
        {
            return apiCall.Invoke();
        }

        public override string Get(string key)
        {
            return null;
        }
    }
}
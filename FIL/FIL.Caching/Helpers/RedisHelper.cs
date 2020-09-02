using FIL.Caching.Attributes;
using FIL.Caching.Contracts.Interfaces;
using FIL.Caching.Managers;
using FIL.Logging;
using FIL.Logging.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.Caching.Helpers
{
    public class RedisHelper : BaseRedisHelper, ICacheHelper
    {
        private readonly IDictionary<Type, Action<ICacheable, HashSet<string>>> _addOwnerKeysActions =
            new Dictionary<Type, Action<ICacheable, HashSet<string>>>();

        public RedisHelper(IRedisClientManager redisClientManager, ILogFactory logFactory)
            : base(redisClientManager, logFactory)
        {
        }

        public RedisHelper(IRedisClientManager redisClientManager, ILogFactory logFactory, TimeSpan? ttlTimeSpan)
            : base(redisClientManager, logFactory, ttlTimeSpan)
        {
        }

        private static string GetKeyName(ICacheable data)
        {
            return GetKeyName(data.GetType(), data.CacheKey);
        }

        private static string GetKeyName(Type t, string cacheKey)
        {
            return t.Name.ToLower() + ":" + cacheKey;
        }

        private static string GetKeyNameAll(Type t)
        {
            return t.Name.ToLower() + ":all";
        }

        private static string GetKeyNameOwnedItem(Type t, Type ownerType, string ownerKey)
        {
            return ownerType.Name.ToLower() + ":" + ownerKey + ":" + t.Name.ToLower();
        }

        private static string GetKeyNameOwnedList(Type t, Type ownerType, string ownerKey)
        {
            return GetKeyNameOwnedItem(t, ownerType, ownerKey) + ":all";
        }

        private class CacheWrapper<T>
        {
            public T Model { get; set; }
        }

        public T Get<T>(string cacheKey) where T : class, ICacheable
        {
            return GetCacheWrapper<T>(GetKeyName(typeof(T), cacheKey))?.Model;
        }

        private CacheWrapper<T> GetCacheWrapper<T>(string keyName) where T : class
        {
            return RedisGet<CacheWrapper<T>>(keyName);
        }

        public IEnumerable<T> GetList<T>() where T : ICacheable
        {
            return RedisGet<IEnumerable<T>>(GetKeyNameAll(typeof(T)));
        }

        public T GetOwnedItem<T>(Type ownerType, string ownerKey) where T : class, ICacheable
        {
            return GetCacheWrapper<T>(GetKeyNameOwnedItem(typeof(T), ownerType, ownerKey))?.Model;
        }

        public IEnumerable<T> GetOwnedList<T>(Type ownerType, string ownerKey) where T : class, ICacheable
        {
            return GetOwnedListHelper<T>(ownerType, ownerKey);
        }

        private IEnumerable<T> GetOwnedListHelper<T>(Type ownerType, string ownerKey) where T : class, ICacheable
        {
            return RedisGet<IEnumerable<T>>(GetKeyNameOwnedList(typeof(T), ownerType, ownerKey));
        }

        public void Save<T>(T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            ClearCache(data);
            if (data != null)
            {
                SaveCacheWrapper(GetKeyName(data), data, expirationTimeSpan);
            }
        }

        public void SaveList<T>(IEnumerable<T> data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            ClearCache(data);
            if (data != null)
            {
                RedisSave(GetKeyNameAll(typeof(T)), data, expirationTimeSpan);
            }
        }

        public void SaveOwnedItem<T>(T data, Type ownerType, string ownerKey, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            ClearCache(data);
            if (data != null)
            {
                RedisSave(GetKeyNameOwnedItem(typeof(T), ownerType, ownerKey), data, expirationTimeSpan);
            }
        }

        public void SaveOwnedList<T>(IEnumerable<T> data, Type ownerType, string ownerKey, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            ClearCache(data);
            if (data != null)
            {
                RedisSave(GetKeyNameOwnedList(typeof(T), ownerType, ownerKey), data, expirationTimeSpan);
            }
        }

        private void SaveCacheWrapper<T>(string keyName, T data, TimeSpan? expirationTimeSpan = null) where T : ICacheable
        {
            RedisSave(keyName, new CacheWrapper<T>
            {
                Model = data
            }, expirationTimeSpan);
        }

        public void ClearCache<T>(T item) where T : ICacheable
        {
            ClearCache(new List<T> { item }.AsEnumerable());
        }

        public void ClearCache<T>(IEnumerable<T> items) where T : ICacheable
        {
            var itemsList = items.ToList();
            var addOwnerKeysAction = GetAddOwnerKeysAction<T>();
            var relatedKeys = new HashSet<string>
            {
                GetKeyNameAll(typeof(T))
            };
            foreach (var item in itemsList.Where(i => i != null))
            {
                relatedKeys.Add(GetKeyName(item));
                addOwnerKeysAction.Invoke(item, relatedKeys);
            }

            var nullItems = itemsList.Where(i => i == null).ToList();
            if (nullItems.Any())
            {
                Logger.Log(LogCategory.Warn, "ClearCache received at least one null item", new Dictionary<string, object>
                {
                    ["NullItemsCount"] = nullItems.Count,
                    ["ItemType"] = typeof(T)
                });
            }

            VoidWrapper(db =>
            {
                relatedKeys.ToList().ForEach(db.Remove);
            });
        }

        private Action<ICacheable, HashSet<string>> GetAddOwnerKeysAction<T>()
        {
            var t = typeof(T);
            if (_addOwnerKeysActions.ContainsKey(t))
            {
                return _addOwnerKeysActions[t];
            }

            var properties = t.GetProperties();
            var actions = new List<Action<ICacheable, HashSet<string>>>();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(OwnerAttribute), true).Cast<OwnerAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    actions.Add((cacheable, set) =>
                    {
                        set.Add(GetKeyNameOwnedItem(typeof(T), attribute.ParentType,
                            t.GetProperty(property.Name).GetValue(cacheable).ToString()));
                        set.Add(GetKeyNameOwnedList(typeof(T), attribute.ParentType,
                            t.GetProperty(property.Name).GetValue(cacheable).ToString()));
                    });
                }
            }

            void AddOwnerKeys(ICacheable c, HashSet<string> s)
            {
                foreach (var action in actions)
                {
                    action(c, s);
                }
            }

            _addOwnerKeysActions[t] = AddOwnerKeys;
            return AddOwnerKeys;
        }

        public void FlushAll()
        {
            throw new NotImplementedException();
        }

        public void Save(string key, string value, TimeSpan? expirationTimeSpan = null)
        {
            VoidWrapper(db =>
            {
                if (expirationTimeSpan.HasValue || TtlTimeSpan.HasValue)
                {
                    db.Set(key, Encoding.UTF8.GetBytes(value), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expirationTimeSpan ?? TtlTimeSpan
                    });
                }
                else
                {
                    db.Set(key, Encoding.UTF8.GetBytes(value));
                }
            });
        }

        public void Delete(string key)
        {
            VoidWrapper(db =>
            {
                db.Remove(key);
            });
        }

        public string Get(string key)
        {
            return Wrapper(db =>
            {
                var redisValue = Encoding.UTF8.GetString(db.Get(key));
                return string.IsNullOrWhiteSpace(redisValue) ? null : redisValue;
            });
        }
    }

    public class BaseRedisHelper
    {
        protected readonly TimeSpan? TtlTimeSpan;
        private readonly IRedisClientManager _redisClientManager;

        protected readonly ILogger Logger;
        private readonly DefaultContractResolver _contractResolver;

        public BaseRedisHelper(IRedisClientManager redisClientManager, ILogFactory logFactory, TimeSpan? ttlTimeSpan)
        {
            _redisClientManager = redisClientManager;
            Logger = logFactory.GetLogger(GetType());
            _contractResolver = new DefaultContractResolver();
            TtlTimeSpan = ttlTimeSpan;
        }

        public BaseRedisHelper(IRedisClientManager redisClientManager, ILogFactory logFactory)
            : this(redisClientManager, logFactory, TimeSpan.FromMinutes(20))
        {
        }

        protected void VoidWrapper(Action<RedisCache> func)
        {
            var client = _redisClientManager.GetRedisClient();
            if (client != null)
            {
                try
                {
                    func(client);
                }
                catch (RedisException ex)
                {
                    Logger.Log(LogCategory.Warn, new Exception("Redis connection failed", ex));
                }
                catch (Exception ex)
                {
                    Logger.Log(LogCategory.Error, ex);
                }
            }
        }

        protected T Wrapper<T>(Func<RedisCache, T> func) where T : class
        {
            var client = _redisClientManager.GetRedisClient();
            if (client != null)
            {
                try
                {
                    return func(client);
                }
                catch (RedisException ex)
                {
                    Logger.Log(LogCategory.Warn, new Exception("Redis connection failed", ex));
                }
                catch (Exception ex)
                {
                    Logger.Log(LogCategory.Error, ex);
                }
            }
            return null;
        }

        protected void RedisSave(string key, object data, TimeSpan? expirationTimeSpan = null)
        {
            VoidWrapper(db =>
            {
                var value = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ContractResolver = _contractResolver
                });
                if (expirationTimeSpan.HasValue || TtlTimeSpan.HasValue)
                {
                    db.Set(key, Encoding.UTF8.GetBytes(value), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expirationTimeSpan ?? TtlTimeSpan
                    });
                }
                else
                {
                    db.Set(key, Encoding.UTF8.GetBytes(value));
                }
            });
        }

        protected T RedisGet<T>(string key) where T : class
        {
            return Wrapper(db =>
            {
                var redisValue = Encoding.UTF8.GetString(db.Get(key));
                if (string.IsNullOrWhiteSpace(redisValue))
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<T>(redisValue, new JsonSerializerSettings
                {
                    ContractResolver = _contractResolver
                });
            });
        }
    }
}
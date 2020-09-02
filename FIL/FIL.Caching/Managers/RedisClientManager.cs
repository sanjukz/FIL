using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Logging;
using FIL.Logging.Enums;
using Microsoft.Extensions.Caching.Redis;
using System;

namespace FIL.Caching.Managers
{
    public interface IRedisClientManager
    {
        RedisCache GetRedisClient();
    }

    public class RedisClientManager : IRedisClientManager
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private RedisCache _redisClient;
        private DateTime _nextRetryTime;

        public RedisClientManager(ILogger logger, ISettings settings)
        {
            _logger = logger;
            _settings = settings;
            _nextRetryTime = DateTime.MinValue;
        }

        public RedisCache GetRedisClient()
        {
            try
            {
                if (ShouldUseRedis() && DateTime.UtcNow > _nextRetryTime)
                {
                    return _redisClient ?? (_redisClient = new RedisCache(new RedisCacheOptions
                    {
                        Configuration = _settings.GetConfigSetting<string>(SettingKeys.Redis.ConnectionString),
                        InstanceName = _settings.GetConfigSetting<string>(SettingKeys.Redis.InstanceName)
                    }));
                }
            }
            catch (Exception e)
            {
                _nextRetryTime = DateTime.UtcNow.AddMinutes(1);
                _logger.Log(LogCategory.Warn, new Exception("Unable to connect to Redis", e));
            }
            return null;
        }

        protected bool ShouldUseRedis()
        {
            return _settings.GetConfigSetting<bool>(SettingKeys.Redis.Enabled);
        }
    }
}
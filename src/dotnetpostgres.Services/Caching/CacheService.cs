using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace dotnetpostgres.Services.Caching
{
    public class CacheService : ICacheService
    {
        private readonly ICacheService _cache;

        public CacheService(IOptions<Configuration> cacheConfig)
        {
            var cachePrviderType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(p => p.Name == cacheConfig.Value.CacheProvider);

            if (cachePrviderType == null)
            {
                throw new ApplicationException($"{cacheConfig.Value.CacheProvider} CacheProviderNotFound");
            }

            _cache = cachePrviderType.GetProperty("Instance").GetValue(null) as ICacheService;
        }

        public void Add(string key, object item, int expireInMinutes)
        {
            _cache.Add(key, item, expireInMinutes);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public void RemoveAll()
        {
            _cache.RemoveAll();
        }
    }
}

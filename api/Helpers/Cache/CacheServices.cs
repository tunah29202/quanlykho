using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Collections;
namespace Helpers.Cache
{
    public class CacheServices : ICacheServices
    {
        private readonly IMemoryCache cache;
        private const int _timeToLive = 7;
        public CacheServices(IMemoryCache _cache) 
        {
            this.cache = _cache;
        }
        public void Clear(string key)
        {
            cache.Remove(key);
        }
        public Task ClearAllAsync()
        {
            if(cache is MemoryCache memoryCache)
            {
                var percentage = 1.0;
                memoryCache.Compact(percentage);
            }
            return Task.CompletedTask;
        }
        public Task ClearAsync(string key)
        {
            cache.Remove(key);
            return Task.CompletedTask;
        }
        public T Get<T>(string key, Func<T> fallback)
        {
            bool foundInCache = cache.TryGetValue(key, out T value);
            if(foundInCache)
            {
                return value;
            }
            value = fallback();
            if(value != null)
            {
                cache.Set(key, value, TimeSpan.FromDays(_timeToLive));
            }
                return value;
        }
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> callback)
        {
            return await cache.GetOrCreateAsync(key, async m =>
            {
                return await Task.FromResult(await callback());
            });
        }
        public List<string> GetKeys()
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(cache) as ICollection;
            var items = new List<string>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    items.Add(val.ToString());
                }
            }
            return items;
        }
    }
}

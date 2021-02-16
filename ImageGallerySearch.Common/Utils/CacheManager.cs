using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGallerySearch.Common.Utils
{

    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache cache;

        public CacheManager(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public bool TryGetValue<T>(string key, out T value) where T : class
        {
            return this.cache.TryGetValue<T>(key, out value);
        }
        public void Set<T>(string key, T value) where T : class
        {
            this.cache.Set(key, value);
        }
        public void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow) where T : class
        {
            this.cache.Set(key, value, absoluteExpirationRelativeToNow);
        }
        public T Get<T>(string key, T value) where T : class
        {
            return this.cache.Get<T>(key);
        }
    }
    public interface ICacheManager
    {
        bool TryGetValue<T>(string key, out T value) where T : class;
        void Set<T>(string key, T value) where T : class;
        void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow) where T : class;
        T Get<T>(string key, T value) where T : class;
    }
}

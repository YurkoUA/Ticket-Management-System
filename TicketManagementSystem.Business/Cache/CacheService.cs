using System;
using System.Runtime.Caching;

namespace TicketManagementSystem.Business
{
    public class CacheService : ICacheService
    {
        public bool Contains(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

        public T GetItem<T>(string key)
        {
            return (T)MemoryCache.Default.Get(key);
        }

        public bool AddOrReplaceItem<T>(string key, T item)
        {
            return AddOrReplaceItem(key, item, 60);
        }

        public bool AddOrReplaceItem<T>(string key, T item, int timeToStoreMinutes)
        {
            if (timeToStoreMinutes < 1)
                timeToStoreMinutes = 1;

            return MemoryCache.Default.Add(key, item, DateTime.UtcNow.AddMinutes(timeToStoreMinutes));
        }

        public void DeleteItem(string key)
        {
            MemoryCache.Default.Remove(key);
        }
    }
}

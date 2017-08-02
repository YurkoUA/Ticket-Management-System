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
            return MemoryCache.Default.Add(key, item, DateTimeOffset.Now.AddHours(1));
        }

        public void DeleteItem(string key)
        {
            if (Contains(key))
                MemoryCache.Default.Remove(key);
        }
    }
}

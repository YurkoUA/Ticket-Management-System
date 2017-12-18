namespace TicketManagementSystem.Business
{
    public interface ICacheService
    {
        bool Contains(string key);
        T GetItem<T>(string key);
        bool AddOrReplaceItem<T>(string key, T item);
        bool AddOrReplaceItem<T>(string key, T item, int timeToStoreMinutes);
        void DeleteItem(string key);
    }
}

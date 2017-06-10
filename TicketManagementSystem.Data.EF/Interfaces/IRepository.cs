using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagementSystem.Data.EF.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int GetCount();
        bool IsEmpty();
        bool ExistsById(int id);
        bool Contains(T item);
        bool Contains(Func<T, bool> predicate);

        IQueryable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> predicate);
        T GetById(int id);

        T Create(T item);
        void Update(T item);
        void Remove(T item);
        void Remove(int id, string table);
    }
}

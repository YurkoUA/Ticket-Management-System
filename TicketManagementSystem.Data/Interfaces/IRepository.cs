using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketManagementSystem.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int Count { get; }

        bool IsEmpty();
        bool ExistsById(int id);
        bool ExisysById<TEntity>(int id) where TEntity : class;
        bool Contains(T item);
        bool Contains(Func<T, bool> predicate);
        bool Contains<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;

        IQueryable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> predicate);
        T GetById(int id);
        T GetLastItem();

        T Create(T item);
        void Update(T item);
        void Remove(T item);
        void SaveChanges();
    }
}

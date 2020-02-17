using System.Data.Entity;

namespace TicketManagementSystem.AutoTest.Data.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(string connectionString)
        {
            _context = new AppDbContext(connectionString);
            _dbSet = _context.Set<T>();
        }
    }
}

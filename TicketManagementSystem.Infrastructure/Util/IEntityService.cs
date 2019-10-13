using System.Collections.Generic;

namespace TicketManagementSystem.Infrastructure.Util
{
    public interface IEntityService
    {
        TDest Convert<TSource, TDest>(TSource source);
        IEnumerable<TDest> ConvertCollection<TSource, TDest>(IEnumerable<TSource> source);

        void Assign<TSource, TDest>(TSource source, TDest dest);
    }
}

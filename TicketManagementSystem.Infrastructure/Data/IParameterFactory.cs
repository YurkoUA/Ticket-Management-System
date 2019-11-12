using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace TicketManagementSystem.Infrastructure.Data
{
    public interface IParameterFactory
    {
        IEnumerable<IDbDataParameter> CreateParameterList<T>(T obj, params Expression<Func<T, object>>[] properties);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Infrastructure.Data;

namespace TicketManagementSystem.Data
{
    public class SqlParameterFactory : IParameterFactory
    {
        public IEnumerable<IDbDataParameter> CreateParameterList<T>(T obj, params Expression<Func<T, object>>[] properties)
        {
            var parameters = new List<SqlParameter>();
            var props = obj.GetType().GetProperties();

            foreach (var p in props)
            {
                if (properties.Any(v => ((MemberExpression)(v.Body as UnaryExpression).Operand).Member.Name == p.Name))
                {
                    var value = p.GetValue(obj);
                    SqlParameter sqlParam = null;

                    // TODO: Type, size(?).
                    if (value != null)
                    {
                        sqlParam = new SqlParameter(p.Name, value);
                    }
                    else
                    {
                        sqlParam = new SqlParameter(p.Name, DBNull.Value);
                    }
                    parameters.Add(sqlParam);
                }
            }

            return parameters;
        }
    }
}

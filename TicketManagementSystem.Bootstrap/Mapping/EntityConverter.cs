using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TicketManagementSystem.Infrastructure.Util;

namespace TicketManagementSystem.Bootstrap.Mapping
{
    public class EntityConverter : IEntityService
    {
        private readonly IMapper mapper;

        public EntityConverter()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            }).CreateMapper();
        }

        public TDest Convert<TSource, TDest>(TSource source)
        {
            return mapper.Map<TDest>(source);
        }

        public IEnumerable<TDest> ConvertCollection<TSource, TDest>(IEnumerable<TSource> source)
        {
            return mapper.Map<IEnumerable<TDest>>(source);
        }

        public void Assign<TSource, TDest>(TSource source, TDest dest)
        {
            mapper.Map<TSource, TDest>(source);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SF.SGL.API.Util
{
    public static class MappingExtensions
    {
        public static Task<ListaPaginada<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => ListaPaginada<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

    }
}

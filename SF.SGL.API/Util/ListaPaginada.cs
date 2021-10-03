using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SF.SGL.API.Util
{
    public class ListaPaginada<T> : List<T>
    {
        public int NumeroPagina { get; }

        public int TotalPaginas { get; }

        public ListaPaginada(List<T> items, int count, int numeroPagina, int tamanhoPagina)
        {
            NumeroPagina = numeroPagina;
            TotalPaginas = (int)Math.Ceiling(count / (double)tamanhoPagina);

            AddRange(items);
        }

        public bool ExistePaginaAnterior => NumeroPagina > 1;

        public bool ExisteProximaPagina => NumeroPagina < TotalPaginas;

        public static async Task<ListaPaginada<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int count = await source.CountAsync();
            List<T> items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new ListaPaginada<T>(items, count, pageIndex, pageSize);
        }
    }
}

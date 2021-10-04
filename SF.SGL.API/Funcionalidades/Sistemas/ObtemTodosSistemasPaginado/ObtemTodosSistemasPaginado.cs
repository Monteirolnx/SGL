using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SF.SGL.API.Funcionalidades.Excecoes;
using SF.SGL.API.Util;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Infra.Data.Contextos;

namespace SF.SGL.API.Funcionalidades.Sistemas
{
    public class ObtemTodosSistemasPaginado
    {
        public record Query : IRequest<Resultado>
        {
            public string Ordenacao { get; init; }

            public string CurrentFilter { get; init; }

            public string PalavraChave { get; init; }

            public int? NumeroPagina { get; init; }

            public int TamanhoPagina { get; init; }
        }

        public record Resultado
        {
            public string CurrentSort { get; init; }

            public string NameSortParm { get; init; }

            public string DateSortParm { get; init; }

            public string CurrentFilter { get; init; }

            public string SearchString { get; init; }

            public int NumeroPagina { get; internal set; }

            public int TotalPaginas { get; internal set; }

            public bool ExistePaginaAnterior { get; set; }

            public bool ExisteProximaPagina { get; set; }

            public ListaPaginada<Modelo> Resultados { get; init; }
        }
        public record Modelo
        {
            public int Id { get; init; }

            public string Nome { get; init; }

            public string UrlServicoConsultaLog { get; init; }

            public string UsuarioLogin { get; init; }

            public string UsuarioSenha { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EntidadeSistema, Modelo>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Resultado>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public QueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public async Task<Resultado> Handle(Query query, CancellationToken cancellationToken)
            {
                FuncionalidadeSistemasException.Quando(query.TamanhoPagina < 1, "Tamanho da página deve ser maior que 0.");

                string palavraChave = query.PalavraChave ?? query.CurrentFilter;
                IQueryable<EntidadeSistema> sistemas = _sglContexto.Sistema;

                if (!string.IsNullOrEmpty(palavraChave))
                {
                    sistemas = sistemas.Where(s => s.Nome.Contains(palavraChave));
                    //|| s.OutraPropriedade.Contains(searchString)); --exemplo de uso
                }

                sistemas = query.Ordenacao switch
                {
                    "nome_desc" => sistemas.OrderByDescending(s => s.Nome),
                    //"date" => sistemas.OrderBy(s => s.DataCriacao), --exemplo de uso
                    //"date_desc" => sistemas.OrderByDescending(s => s.DataCriacao), --exemplo de uso
                    _ => sistemas.OrderBy(s => s.Nome)
                };

                ListaPaginada <Modelo> resultado = await sistemas
                   .ProjectTo<Modelo>(_configurationProvider)
                   .PaginatedListAsync(query.NumeroPagina ?? 1, query.TamanhoPagina);

                FuncionalidadeSistemasException.Quando(!resultado.Any(), "Não existe resultado para a pesquisa.");
                FuncionalidadeSistemasException.Quando(query.NumeroPagina.HasValue && query.NumeroPagina > resultado.TotalPaginas, "Número da página invalido.");

                Resultado model = new()
                {
                    CurrentSort = query.Ordenacao,
                    NameSortParm = string.IsNullOrEmpty(query.Ordenacao) ? "nome_desc" : "",
                    DateSortParm = query.Ordenacao == "Date" ? "date_desc" : "Date",
                    CurrentFilter = palavraChave,
                    SearchString = palavraChave,
                    NumeroPagina = resultado.NumeroPagina,
                    TotalPaginas = resultado.TotalPaginas,
                    ExistePaginaAnterior = resultado.ExistePaginaAnterior,
                    ExisteProximaPagina = resultado.ExisteProximaPagina,
                    Resultados = resultado,
                };

                return model;
            }

        }
    }
}

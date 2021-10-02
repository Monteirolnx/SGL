using System;
using System.ComponentModel.DataAnnotations;
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
        public record Query : IRequest<Result>
        {
            public string SortOrder { get; init; }

            public string CurrentFilter { get; init; }

            public string PalavraChave { get; init; }

            public int? NumeroPagina { get; init; }

            [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
            public int TamanhoPagina { get; init; }
        }

        public record Result
        {
            public string CurrentSort { get; init; }

            public string NameSortParm { get; init; }

            public string DateSortParm { get; init; }

            public string CurrentFilter { get; init; }

            public string SearchString { get; init; }   
            
            public int PageIndex { get; internal set; }

            public int TotalPages { get; internal set; }

            public PaginatedList<Model> Resultados { get; init; }
        }
        public record Model
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
                CreateMap<EntidadeSistema, Model>();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public QueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                FuncionalidadeSistemasException.Quando(request.TamanhoPagina < 1, "Tamanho da página deve ser maior que 0.");

                string palavraChave = request.PalavraChave ?? request.CurrentFilter;
                IQueryable<EntidadeSistema> sistemas = _sglContexto.Sistema;

                if (!string.IsNullOrEmpty(palavraChave))
                {
                    sistemas = sistemas.Where(s => s.Nome.Contains(palavraChave));
                    //|| s.OutraPropriedade.Contains(searchString)); --exemplo de uso
                }

                sistemas = request.SortOrder switch
                {
                    "name_desc" => sistemas.OrderByDescending(s => s.Nome),
                    //"date" => sistemas.OrderBy(s => s.DataCriacao), --exemplo de uso
                    //"date_desc" => sistemas.OrderByDescending(s => s.DataCriacao), --exemplo de uso
                    _ => sistemas.OrderBy(s => s.Nome)
                };

                int pageSize = request.TamanhoPagina;
                int pageNumber = (request.PalavraChave == null ? request.NumeroPagina : 1) ?? 1;

                PaginatedList<Model> resultado = await sistemas
                   .ProjectTo<Model>(_configurationProvider)
                   .PaginatedListAsync(pageNumber, pageSize);

                FuncionalidadeSistemasException.Quando(!resultado.Any(), "Não existe resultado para a pesquisa.");
                FuncionalidadeSistemasException.Quando(request.NumeroPagina.HasValue && 
                    request.NumeroPagina > resultado.TotalPages, "Número da página invalido.");

                Result model = new()
                {
                    CurrentSort = request.SortOrder,
                    NameSortParm = string.IsNullOrEmpty(request.SortOrder) ? "name_desc" : "",
                    DateSortParm = request.SortOrder == "Date" ? "date_desc" : "Date",
                    CurrentFilter = palavraChave,
                    SearchString = palavraChave,
                    PageIndex = resultado.PageIndex,
                    TotalPages = resultado.TotalPages,
                    Resultados = resultado,
                };

                return model;
            }

        }
    }
}

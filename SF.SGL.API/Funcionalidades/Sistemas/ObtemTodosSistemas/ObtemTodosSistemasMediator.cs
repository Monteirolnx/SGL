using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Infra.Data.Contexto;

namespace SF.SGL.API.Funcionalidades.Sistemas.ObtemTodosSistemas
{
    public class ObtemTodosSistemasMediator
    {
        public record Query : IRequest<Result>
        {

        }

        public record Result
        {
            public IEnumerable<Model> Resultados { get; set; }
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
                CreateMap<SistemaEntidade, Model>();
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
                List<Model> resultado = await _sglContexto.Sistema
                    .ProjectTo<Model>(_configurationProvider)
                    .ToListAsync(cancellationToken);

                SistemasException.Quando(!resultado.Any(), "Não existe resultado para a pesquisa.");

                Result model = new()
                {
                    Resultados = resultado
                };

                return model;
            }
        }
    }
}

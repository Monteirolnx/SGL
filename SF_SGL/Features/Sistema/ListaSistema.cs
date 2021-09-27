using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SF_SGL.Data;

namespace SF_SGL.Features.Sistema
{
    public class ListaSistema
    {
        
        public Result Data { get; set; }

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
                CreateMap<Entidades.Sistema, Model>();
            }
        }

        public record Query : IRequest<Result>
        {

        }

        public record Result
        {
            public List<Model> Resultados { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly SGLContext _sglContext;
            private readonly IConfigurationProvider _configurationProvider;

            public QueryHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
            {
                _sglContext = sglContext;
                _configurationProvider = configurationProvider;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<Entidades.Sistema> sistemas = _sglContext.Sistema;

                List<Model> resultados = await Task.FromResult(sistemas.ProjectTo<Model>(_configurationProvider).ToList());

                Result model = new()
                {
                    Resultados = resultados
                };

                return model;                    
            }
        }


    }
}

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL.Data;

namespace SF_SGL.Features.Sistema
{
    public class DetalhaSistema
    {
        public Model Data { get;  set; }

        public record Model
        {
            public int Id { get; init; }

            public string Nome { get; init; }

            public string UrlServicoConsultaLog { get; init; }

            public string UsuarioLogin { get; init; }

            public string UsuarioSenha { get; init; }
        }

        public class MappingProfile: Profile
        {
            public MappingProfile()
            {
                CreateMap<Entidades.Sistema, Model>();
            }
        }

        public record Query : IRequest<Model>
        {
            public int Id { get; init; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly SGLContext _sglContext;
            private readonly IConfigurationProvider _configurationProvider;

            public Handler(SGLContext sglContext, IConfigurationProvider configurationProvider)
            {
                _sglContext = sglContext;
                _configurationProvider = configurationProvider;
            }

            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                return _sglContext.Sistema.Where(s => s.Id == request.Id)
                    .ProjectTo<Model>(_configurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

    }
}

using System.ComponentModel.DataAnnotations;
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
    public class DeletaSistema
    {
        public Command Data { get; set; }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Entidades.Sistema, Command>();
            }
        }

        public record Query : IRequest<Command>
        {
            public int Id { get; init; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly SGLContext _sglContext;
            private readonly IConfigurationProvider _configurationProvider;

            public QueryHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
            {
                _sglContext = sglContext;
                _configurationProvider = configurationProvider;
            }

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _sglContext.Sistema.Where(s => s.Id == request.Id)
                    .ProjectTo<Command>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);
            }
        }

        public record Command: IRequest
        {
            public int Id { get; init; }

            [Required]
            public string Nome { get; init; }

            [Required]
            public string UrlServicoConsultaLog { get; init; }

            [Required]
            public string UsuarioLogin { get; init; }

            [Required]
            public string UsuarioSenha { get; init; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SGLContext _sglContext;
            public CommandHandler(SGLContext sglContext)
            {
                _sglContext = sglContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _sglContext.Sistema.Remove(await _sglContext.Sistema.FindAsync(request.Id));

                await _sglContext.SaveChangesAsync(cancellationToken);

                return default;
            }
        }

    }
}

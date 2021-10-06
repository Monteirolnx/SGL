using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF.SGL.API.Funcionalidades.Excecoes;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Infra.Data.Contextos;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.DeletaSistema
{
    public class Deleta
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EntidadeSistema, Command>().ReverseMap();
            }
        }

        public record Query : IRequest<Command>
        {
            public int Id { get; init; }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public QueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public async Task<Command> Handle(Query query, CancellationToken cancellationToken)
            {
                Command command = await _sglContexto.Sistema.Where(s => s.Id == query.Id)
                       .ProjectTo<Command>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

                FuncionalidadeSistemasException.Quando(command is null, $"Não existe sistema com o código {query.Id}.");

                return command;
            }
        }

        public record Command : IRequest
        {
            public int Id { get; set; }

            public string Nome { get; set; }

            public string UrlServicoConsultaLog { get; set; }

            public string UsuarioLogin { get; set; }

            public string UsuarioSenha { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IMapper _mapper;

            public CommandHandler(SGLContexto sglContexto, IMapper mapper)
            {
                _sglContexto = sglContexto;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                EntidadeSistema entidadeSistema = _mapper.Map<EntidadeSistema>(command);

                await Task.FromResult(_sglContexto.Sistema.Remove(entidadeSistema));

                await _sglContexto.SaveChangesAsync(cancellationToken);

                return default;
            }
        }
    }
}

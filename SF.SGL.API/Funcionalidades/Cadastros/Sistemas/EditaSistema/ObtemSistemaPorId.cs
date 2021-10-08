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

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.EditaSistema
{
    public class ObtemSistemaPorId
    {
        public record Query : IRequest<Command>
        {
            public int Id { get; init; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EntidadeSistema, Command>();
            }
        }

        public record Command
        {
            public int Id { get; init; }

            public string Nome { get; init; }

            public string UrlServicoConsultaLog { get; init; }

            public string UsuarioLogin { get; init; }

            public string UsuarioSenha { get; init; }
        }

        public class CommandHandler : IRequestHandler<Query, Command>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public CommandHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }
            public async Task<Command> Handle(Query query, CancellationToken cancellationToken)
            {
                Command command = await _sglContexto.EntidadeSistema.Where(s => s.Id == query.Id)
                    .ProjectTo<Command>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

                FuncionalidadeSistemasException.Quando(command is null, $"Não existe sistema com o código {query.Id}.");

                return command;
            }
        }

    }
}

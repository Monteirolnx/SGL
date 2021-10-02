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

namespace SF.SGL.API.Funcionalidades.Sistemas
{
    public class EditaSistema
    {
        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }

        }

        public class Command : IRequest
        {
            public int Id { get; set; }

            public string Nome { get; set; }

            public string UrlServicoConsultaLog { get; set; }

            public string UsuarioLogin { get; set; }

            public string UsuarioSenha { get; set; }
        }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EntidadeSistema, Command>();
            }
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

            public async Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                Command sistema = await _sglContexto.Sistema.Where(s => s.Id == request.Id)
                    .ProjectTo<Command>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

                FuncionalidadeSistemasException.Quando(sistema is null, $"Não existe sistema com o código {request.Id}.");

                return sistema;
            }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SGLContexto _sglContexto;

            public CommandHandler(SGLContexto sglContexto)
            {
                _sglContexto = sglContexto;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                EntidadeSistema sistema = await _sglContexto.Sistema.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

                sistema.Nome = request.Nome;
                sistema.UrlServicoConsultaLog = request.UsuarioLogin;
                sistema.UsuarioLogin = request.UsuarioLogin;
                sistema.UsuarioSenha = request.UsuarioSenha;

                _sglContexto.Sistema.Update(sistema);
                await _sglContexto.SaveChangesAsync(cancellationToken);

                return default;
            }
        }
    }
}

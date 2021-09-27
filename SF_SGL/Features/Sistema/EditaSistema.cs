using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL.Data;

namespace SF_SGL.Features.Sistema
{
    public class EditaSistema
    {      
        public Command Data { get; set; }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Entidades.Sistema, Command>();
        }

        public record Query : IRequest<Command>
        {
            public int? Id { get; init; }
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
                    .ProjectTo<Command>(_configurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }

            public string Nome { get; set; }

            public string UrlServicoConsultaLog { get; set; }

            public string UsuarioLogin { get; set; }

            public string UsuarioSenha { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly SGLContext _sglContext;
            private readonly IConfigurationProvider _configurationProvider;

            public CommandHandler(SGLContext sglContext, IConfigurationProvider configurationProvider)
            {
                _sglContext = sglContext;
                _configurationProvider = configurationProvider;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Entidades.Sistema sistema = await _sglContext.Sistema.FindAsync(request.Id);

                sistema.Nome = request.Nome;
                sistema.UrlServicoConsultaLog = request.UsuarioLogin;
                sistema.UsuarioLogin = request.UsuarioLogin;
                sistema.UsuarioSenha = request.UsuarioSenha;

                _sglContext.Sistema.Update(sistema);
                await _sglContext.SaveChangesAsync(cancellationToken);

                return default;
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Nome).NotNull().Length(1, 50);
                RuleFor(m => m.UrlServicoConsultaLog).NotNull().Length(1, 50);
                RuleFor(m => m.UsuarioLogin).NotNull();
            }
        }
    }
}

﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF.SGL.Dominio.Entidades.Sistema;
using SF.SGL.Infra.Data.Contexto;

namespace SF.SGL.API.Funcionalidades.Sistemas.DeletaSistema
{
    public class DeletaSistemaMediator
    {
        public record Query : IRequest<Command>
        {
            public int Id { get; init; }
        }

        public record Command : IRequest
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

            public async Task<Command> Handle(Query query, CancellationToken cancellationToken)
            {
                Command sistema = await _sglContexto.Sistema.Where(s => s.Id == query.Id)
                       .ProjectTo<Command>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

                FuncionalidadeSistemasException.Quando(sistema is null, $"Não existe sistema com o código {query.Id}.");

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
                _sglContexto.Sistema.Remove(await _sglContexto.Sistema.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken));

                await _sglContexto.SaveChangesAsync(cancellationToken);

                return default;
            }
        }
    }
}
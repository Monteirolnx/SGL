﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF_SGL_Dominio.Entidades;
using SF_SGL_Infra.Contexto;

namespace SF_SGL_API.Funcionalidades.Sistemas
{
    public class ObtemSistemaPorIdMediator
    {
        public record Query : IRequest<Model>
        {
            public int Id { get; init; }
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

        #region Query
        
        public class ObtemSistemaPorIdHandler : IRequestHandler<Query, Model>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public ObtemSistemaPorIdHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }
            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                Model sistema = await _sglContexto.Sistema.Where(s => s.Id == request.Id)
                    .ProjectTo<Model>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

                SistemasException.Quando(sistema is null, $"Não existe sistema com o código {sistema.Id}.");

                return sistema;
            }
        }

        #endregion
    }
}

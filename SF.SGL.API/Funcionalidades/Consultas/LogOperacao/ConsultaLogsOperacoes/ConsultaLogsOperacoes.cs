﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SF.SGL.API.Funcionalidades.Consultas.LogOperacao.Excecoes;
using SF.SGL.Dominio.Entidades;
using SF.SGL.Infra.Data.Contextos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes
{
    public class ConsultaLogsOperacoes
    {
    }
    public class AuxConsultaSistemas
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EntidadeSistema, Sistema>();
            }
        }

        public record Query : IRequest<Resultado>
        {
        }

        public record Resultado
        {
            public IEnumerable<Sistema> Resultados { get; set; }
        }

        public record Sistema
        {
            public int Id { get; init; }

            public string Nome { get; init; }
        }

        public class QueryHandler : IRequestHandler<Query, Resultado>
        {
            private readonly SGLContexto _sglContexto;
            private readonly IConfigurationProvider _configurationProvider;

            public QueryHandler(SGLContexto sglContexto, IConfigurationProvider configurationProvider)
            {
                _sglContexto = sglContexto;
                _configurationProvider = configurationProvider;
            }

            public async Task<Resultado> Handle(Query query, CancellationToken cancellationToken)
            {
                List<Sistema> resultado = await _sglContexto.EntidadeSistema
                    .ProjectTo<Sistema>(_configurationProvider)
                    .ToListAsync(cancellationToken);

                FuncionalidadeLogOperacaoException.Quando(!resultado.Any(), "Não existem sistemas cadastrados.");

                Resultado model = new()
                {
                    Resultados = resultado
                };

                return model;
            }
        }
    }
}
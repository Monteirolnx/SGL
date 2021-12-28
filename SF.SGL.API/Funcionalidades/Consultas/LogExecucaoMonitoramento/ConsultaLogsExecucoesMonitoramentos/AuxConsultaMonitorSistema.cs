using SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.Excecoes;

namespace SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.ConsultaLogsExecucoesMonitoramentos;

public class AuxConsultaMonitorSistema
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeMonitoramento, Monitoramento>().ReverseMap();
        }
    }

    public record Query : IRequest<Resultado>
    {
        public int Id { get; init; }
    }

    public record Resultado
    {
        public IEnumerable<Monitoramento> Monitoramentos { get; set; }
    }

    public record Monitoramento
    {
        public int Id { get; init; }

        public string Nome { get; init; }

        public int SistemaId { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, Resultado>
    {
        private readonly SGLContexto _sglContexto;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public QueryHandler(SGLContexto sglContexto, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _sglContexto = sglContexto;
            _configurationProvider = configurationProvider;
        }

        public async Task<Resultado> Handle(Query query, CancellationToken cancellationToken)
        {
            List<Monitoramento> monitoramentos = await _sglContexto.EntidadeMonitoramento
                .ProjectTo<Monitoramento>(_configurationProvider)
                .Where(x => x.SistemaId == query.Id)
                .OrderBy(x => x.Nome)
                .ToListAsync(cancellationToken);

            FuncionalidadeLogMonitoramentoException.Quando(!monitoramentos.Any(), "Não existem monitoramentos cadastrados para esse sistema.");

            Resultado resultado = new()
            {
                Monitoramentos = monitoramentos
            };

            return resultado;
        }
    }

}

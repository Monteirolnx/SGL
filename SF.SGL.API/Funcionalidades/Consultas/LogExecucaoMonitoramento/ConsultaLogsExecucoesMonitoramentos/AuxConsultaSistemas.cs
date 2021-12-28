using SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.Excecoes;

namespace SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.ConsultaLogsExecucoesMonitoramentos;

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
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public QueryHandler(SGLContexto sglContexto, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _sglContexto = sglContexto;
            _configurationProvider = configurationProvider;
        }

        public async Task<Resultado> Handle(Query query, CancellationToken cancellationToken)
        {
            var consultaDb = await _sglContexto.EntidadeSistema
                .ProjectTo<Sistema>(_configurationProvider)
                .Join(_sglContexto.EntidadeMonitoramento,
                sistema => sistema.Id,
                monitoramento=> monitoramento.SistemaId,
                (sistema, monitoramento) => new
                {
                    SistemaId = sistema.Id,
                    SistemaNome = sistema.Nome
                })
                .Distinct()
                .ToListAsync(cancellationToken);

            FuncionalidadeLogMonitoramentoException.Quando(!consultaDb.Any(), "Não existem sistemas cadastrados.");

            List<Sistema> sistemas = new();
            foreach (var item in consultaDb)
            {
                Sistema sistema = new()
                {
                    Id = item.SistemaId,
                    Nome = item.SistemaNome
                };

                sistemas.Add(sistema);
            }

            Resultado resultado = new()
            {
                Resultados = sistemas.OrderBy(x=> x.Nome)
            };

            return resultado;
        }
    }
}

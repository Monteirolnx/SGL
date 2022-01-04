namespace SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.ConsultaLogsExecucoesMonitoramentos;

public class ConsultaLogsExecMonitoramentos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeExecucaoMonitoramento, LogExecMonitoramento>();
        }
    }

    public record Query : IRequest<RespostaConsultaLogExecMonitoramento>
    {
        public int? SistemaId { get; set; }

        public int? MonitoramentoId { get; set; }

        public DateTime? PeriodoInicial { get; set; }

        public DateTime? PeriodoFinal { get; set; }

        public TimeSpan? HorarioInicial { get; set; }

        public TimeSpan? HorarioFinal { get; set; }

        public bool? Status { get; set; }
    }

    public class RespostaConsultaLogExecMonitoramento
    {
        public List<LogExecMonitoramento> LogsExecsMonitoramentos { get; set; }

        public int QuantidadeTotalRegistrosEncontrados { get; set; }
    }

    public class LogExecMonitoramento
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        public bool Status { get; set; }

        public DateTime Data { get; set; }

        public string Mensagem { get; set; }

        public int MonitoramentoId { get; set; }

        public string MonitoramentoNome { get; set; }

        public string SistemaNome { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, RespostaConsultaLogExecMonitoramento>
    {
        private readonly SGLContexto _sglContexto;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public QueryHandler(SGLContexto sglContexto, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _sglContexto = sglContexto;
            _configurationProvider = configurationProvider;
        }

        public async Task<RespostaConsultaLogExecMonitoramento> Handle(Query query, CancellationToken cancellationToken)
        {
            List<LogExecMonitoramento> logsExecucoesMonitoramentos = new();

            var consultaDb = await _sglContexto.EntidadeExecucaoMonitoramento
             .ProjectTo<LogExecMonitoramento>(_configurationProvider)
             .Join(_sglContexto.EntidadeMonitoramento,
             execucaoMonitoramento => execucaoMonitoramento.MonitoramentoId,
             monitoramento => monitoramento.Id,
             (execucaoMonitoramento, monitoramento) => new
             {
                 execucaoMonitoramento.Id,
                 execucaoMonitoramento.Guid,
                 execucaoMonitoramento.Data,
                 execucaoMonitoramento.Mensagem,
                 execucaoMonitoramento.Status,
                 execucaoMonitoramento.MonitoramentoId,
                 monitoramento.SistemaId,
                 MonitoramentoNome = monitoramento.Nome,
                 SistemaNome = monitoramento.EntidadeSistema.Nome
             })
             .Where(x => (query.SistemaId == null || x.SistemaId == query.SistemaId)
                    && (query.MonitoramentoId == null || x.MonitoramentoId == query.MonitoramentoId))
             .Where(x => query.PeriodoInicial == null || x.Data >= query.PeriodoInicial)
             .Where(x => query.PeriodoFinal == null || x.Data <= query.PeriodoFinal)
             .Where(x=>(query.Status == null || x.Status == query.Status))
             .ToListAsync(cancellationToken);

            foreach (var item in consultaDb)
            {
                LogExecMonitoramento logExecMonitoramento = new()
                {
                    Id = item.Id,
                    Guid = item.Guid,
                    Data = item.Data,
                    Mensagem = item.Mensagem,
                    Status = item.Status,
                    MonitoramentoId = item.MonitoramentoId,
                    MonitoramentoNome = item.MonitoramentoNome,
                    SistemaNome = item.SistemaNome
                };

                logsExecucoesMonitoramentos.Add(logExecMonitoramento);
            }

            logsExecucoesMonitoramentos = logsExecucoesMonitoramentos
                                            .OrderByDescending(x => x.Data).ToList();

            RespostaConsultaLogExecMonitoramento respostaConsultaLogExecMonitoramento = new()
            {
                LogsExecsMonitoramentos = logsExecucoesMonitoramentos,
                QuantidadeTotalRegistrosEncontrados = logsExecucoesMonitoramentos.Count,
            };

            return respostaConsultaLogExecMonitoramento;
        }
    }
}
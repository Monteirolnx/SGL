namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.ConsultaMonitoramentos;

public class ConsultaMonitoramentos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeMonitoramento, Monitoramento>();
            CreateMap<EntidadeSistema, Sistema>();
        }
    }

    public record Query : IRequest<Resultado>
    {

    }

    public record Resultado
    {
        public IEnumerable<Monitoramento> Monitoramentos { get; set; }
    }

    public record Monitoramento
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Acao { get; set; }

        public string Contato { get; set; }

        public int SistemaId { get; set; }

        public Sistema Sistema { get; set; }
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

        public async Task<Resultado> Handle(Query request, CancellationToken cancellationToken)
        {
            var consultaDb = await _sglContexto.EntidadeMonitoramento
                .ProjectTo<Monitoramento>(_configurationProvider)
                .Join(_sglContexto.EntidadeSistema,
                monitoramento => monitoramento.SistemaId,
                sistema => sistema.Id,
                (monitoramento, sistema) => new
                {
                    MonitoramentoId = monitoramento.Id,
                    MonitoramentoGUI = monitoramento.Guid,
                    MonitoramentoNome = monitoramento.Nome,
                    MonitoramentoDescricao = monitoramento.Descricao,
                    MonitoramentoAcao = monitoramento.Acao,
                    MonitoramentoContato = monitoramento.Contato,

                    SistemaId = monitoramento.Id,
                    SistemaNome = sistema.Nome
                })
                .ToListAsync(cancellationToken);

            List<Monitoramento> monitoramentos = new();

            foreach (var item in consultaDb)
            {
                Monitoramento monitoramento = new();
                monitoramento.Id = item.MonitoramentoId;
                monitoramento.Guid = item.MonitoramentoGUI;
                monitoramento.Nome = item.MonitoramentoNome;
                monitoramento.Descricao = item.MonitoramentoDescricao;
                monitoramento.Acao = item.MonitoramentoAcao;
                monitoramento.Contato = item.MonitoramentoContato;

                monitoramento.Sistema = new Sistema
                {
                    Id = item.SistemaId,
                    Nome = item.SistemaNome
                };

                monitoramentos.Add(monitoramento);
            }

            Resultado resultado = new()
            {
                Monitoramentos = monitoramentos.OrderBy(s=>s.Sistema.Nome)
            };

            return resultado;
        }
    }
}

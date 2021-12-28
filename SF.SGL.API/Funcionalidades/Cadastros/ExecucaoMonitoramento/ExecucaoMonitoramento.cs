using Newtonsoft.Json;
using SF.SGL.API.Funcionalidades.Cadastros.ExecucaoMonitoramento.Excecoes;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace SF.SGL.API.Funcionalidades.Cadastros.ExecucaoMonitoramento;

public class ExecucaoMonitoramento
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeExecucaoMonitoramento, Command>().ReverseMap();
            CreateMap<EntidadeMonitoramento, Monitoramento>().ReverseMap();
        }
    }

    public class Command : IRequest<long>
    {
        [Required(ErrorMessage = "Guid do monitoramento é obrigatório.")]
        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Status do monitoramento é obrigatório.")]
        public bool Status { get; set; }

        [JsonIgnore]
        public DateTime Data { get; set; }

        public string Mensagem { get; set; }

        [JsonIgnore]
        public int MonitoramentoId { get; set; }

        //[JsonIgnore]
        //public string MonitoramentoNome { get; set; }

        //[JsonIgnore]
        //public string SistemaNome { get; set; }
    }

    public class MensagemSignal
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        public bool Status { get; set; }

        public DateTime Data { get; set; }

        public string Mensagem { get; set; }

        public int MonitoramentoId { get; set; }

        public string MonitoramentoNome { get; set; }

        public int SistemaId { get; set; }

        public string SistemaNome { get; set; }
    }

    public class Monitoramento
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Acao { get; set; }

        public string Contato { get; set; }

        public int SistemaId { get; set; }

        public string SistemaNome { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, long>
    {
        private readonly SGLContexto _sglContexto;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly SGLHub _mainHub;

        public CommandHandler(SGLContexto sglContexto, IMapper mapper, AutoMapper.IConfigurationProvider configurationProvider, IMemoryCache memoryCache, SGLHub mainHub)
        {
            _sglContexto = sglContexto;
            _mapper = mapper;
            _configurationProvider = configurationProvider;
            _memoryCache = memoryCache;
            _mainHub = mainHub;
        }

        public async Task<long> Handle(Command command, CancellationToken cancellationToken)
        {
            List<Monitoramento> monitoramentosCache = _memoryCache.Get<List<Monitoramento>>("MonitoramentosCache");
            string monitoramentoNome = string.Empty;
            int sistemaId = 0;
            string sistemaNome = string.Empty;

            if (monitoramentosCache is not null && monitoramentosCache.Exists(x => x.Guid == command.Guid))
            {
                command.MonitoramentoId = monitoramentosCache.Where(m => m.Guid == command.Guid)
                                                             .Select(m => m.Id).FirstOrDefault();

                monitoramentoNome = monitoramentosCache.Where(m => m.Guid == command.Guid)
                                                             .Select(m => m.Nome).FirstOrDefault();

                sistemaId = monitoramentosCache.Where(m => m.Guid == command.Guid)
                                                      .Select(m => m.SistemaId).FirstOrDefault();

                sistemaNome = monitoramentosCache.Where(m => m.Guid == command.Guid)
                                                             .Select(m => m.SistemaNome).FirstOrDefault();
            }
            else
            {
                Monitoramento monitoramento = _sglContexto.EntidadeMonitoramento.ProjectTo<Monitoramento>(_configurationProvider).Where(m => m.Guid == command.Guid).FirstOrDefault();

                FuncionalidadeExecucaoMonitoramentoException.Quando(monitoramento is null, $"Guid inválido: {command.Guid}.");

                command.MonitoramentoId = monitoramento.Id;
                monitoramentoNome = monitoramento.Nome;

                monitoramento.SistemaId = sistemaId = monitoramento.SistemaId;
                monitoramento.SistemaNome = sistemaNome = _sglContexto.EntidadeSistema.Where(s => s.Id == monitoramento.SistemaId).Select(s => s.Nome).FirstOrDefault();

                if (monitoramentosCache is null)
                {
                    monitoramentosCache = new();
                }
                monitoramentosCache.Add(monitoramento);

                _memoryCache.Set("MonitoramentosCache", monitoramentosCache, TimeSpan.FromDays(1));
            }

            command.Data = DateTime.Now;

            EntidadeExecucaoMonitoramento entidadeExecucaoMonitoramento = _mapper.Map<EntidadeExecucaoMonitoramento>(command);

            await _sglContexto.AddAsync(entidadeExecucaoMonitoramento, cancellationToken);

            await _sglContexto.SaveChangesAsync(cancellationToken);

            MensagemSignal mensagemSignal = new()
            {
                Id = entidadeExecucaoMonitoramento.Id,
                Guid = command.Guid,
                Status = command.Status,
                Data = command.Data,
                Mensagem = command.Mensagem,
                MonitoramentoId = command.MonitoramentoId,
                MonitoramentoNome = monitoramentoNome,
                SistemaId = sistemaId,
                SistemaNome = sistemaNome
            };

            string mensagem = JsonConvert.SerializeObject(mensagemSignal);
            await _mainHub.EnviarMensagem(mensagem);

            return entidadeExecucaoMonitoramento.Id;
        }
    }
}




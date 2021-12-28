namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.EditaMonitoramento;

public class EditaMonitoramento
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeMonitoramento, Command>().ReverseMap();
        }
    }

    public class Command : IRequest
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Guid Guid { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Acao { get; set; }

        public string Contato { get; set; }

        [JsonIgnore]
        public int SistemaId { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly SGLContexto _sglContexto;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public CommandHandler(SGLContexto sglContexto, IMapper mapper, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _sglContexto = sglContexto;
            _mapper = mapper;
            _configurationProvider = configurationProvider;
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            EntidadeMonitoramento monitoramentoAntigo = await _sglContexto.EntidadeMonitoramento
                .Where(s => s.Id == command.Id)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);


            command.SistemaId = monitoramentoAntigo.SistemaId;
            command.Guid = monitoramentoAntigo.Guid;
            EntidadeMonitoramento entidadeMonitoramento = _mapper.Map<EntidadeMonitoramento>(command);

            _sglContexto.EntidadeMonitoramento.Update(entidadeMonitoramento);
            await _sglContexto.SaveChangesAsync(cancellationToken);

            return default;
        }

    
    }


}

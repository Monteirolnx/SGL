namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.AdicionaMonitoramento;

public class AdicionaMonitoramento
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeMonitoramento, Command>().ReverseMap();
        }
    }

    public class Command : IRequest<int>
    {
        [Required(ErrorMessage = "Guid do monitoramento é obrigatório.")]
        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Nome do monitoramento é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Descrição do monitoramento é obrigatório.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Ação do monitoramento é obrigatória.")]
        public string Acao { get; set; }

        [Required(ErrorMessage = "Contato do monitoramento é obrigatório.")]
        public string Contato { get; set; }

        [Required(ErrorMessage = "Id do sistema é obrigatório.")]
        public int SistemaId { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, int>
    {
        private readonly SGLContexto _sglContexto;
        private readonly IMapper _mapper;

        public CommandHandler(SGLContexto sglContexto, IMapper mapper)
        {
            _sglContexto = sglContexto;
            _mapper = mapper;
        }

        public async Task<int> Handle(Command command, CancellationToken cancellationToken)
        {
            EntidadeMonitoramento entidadeMonitoramento = _mapper.Map<EntidadeMonitoramento>(command);

            await _sglContexto.AddAsync(entidadeMonitoramento, cancellationToken);

            await _sglContexto.SaveChangesAsync(cancellationToken);

            return entidadeMonitoramento.Id;
        }
    }

}

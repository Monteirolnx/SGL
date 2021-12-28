namespace SF.SGL.API.Funcionalidades.Parametizacoes.ExpurgoLogs.AdicionaParametrizacao;

public class AdicionaParametrizacao
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeParametroExpurgo, Command>().ReverseMap();
        }
    }

    public class Command : IRequest<int>
    {
        [Required]
        public int SistemaId { get; set; }

        [Required]
        public int ParametroExpurgoLogOperacao { get; set; }

        [Required]
        public int ParametroExpurgoLogAuditoria { get; set; }
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
            EntidadeParametroExpurgo entidadeParametroExpurgo = _mapper.Map<EntidadeParametroExpurgo>(command);

            await _sglContexto.AddAsync(entidadeParametroExpurgo, cancellationToken);

            await _sglContexto.SaveChangesAsync(cancellationToken);

            return entidadeParametroExpurgo.Id;
        }
    }
}

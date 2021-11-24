namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.EditaSistema;

public class EditaSistema
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeSistema, Command>().ReverseMap();
        }
    }

    public class Command : IRequest
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string UrlServicoConsultaLog { get; set; }

        public string UsuarioLogin { get; set; }

        public string UsuarioSenha { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly SGLContexto _sglContexto;
        private readonly IMapper _mapper;

        public CommandHandler(SGLContexto sglContexto, IMapper mapper)
        {
            _sglContexto = sglContexto;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            EntidadeSistema entidadeSistema = _mapper.Map<EntidadeSistema>(command);

            _sglContexto.EntidadeSistema.Update(entidadeSistema);
            await _sglContexto.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}

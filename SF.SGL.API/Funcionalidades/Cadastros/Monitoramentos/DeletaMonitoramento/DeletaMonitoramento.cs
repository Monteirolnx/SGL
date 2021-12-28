using SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.Excecoes;

namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.DeletaMonitoramento;

public class DeletaMonitoramento
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeMonitoramento, Command>().ReverseMap();
        }
    }

    public record Query : IRequest<Command>
    {
        public int Id { get; init; }
    }
     
    public class QueryHandler : IRequestHandler<Query, Command>
    {
        private readonly SGLContexto _sglContexto;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public QueryHandler(SGLContexto sglContexto, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _sglContexto = sglContexto;
            _configurationProvider = configurationProvider;
        }

        public async Task<Command> Handle(Query query, CancellationToken cancellationToken)
        {
            Command command = await _sglContexto.EntidadeMonitoramento.Where(s => s.Id == query.Id)
                   .ProjectTo<Command>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

            FuncionalidadeMonitoramentoException.Quando(command is null, $"Não existe monitoramento com o código {query.Id}.");

            return command;
        }
    }

    public class Command : IRequest
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Acao { get; set; }

        public string Contato { get; set; }

        public int SistemaId { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly SGLContexto _sglContexto;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CommandHandler(SGLContexto sglContexto, IMapper mapper, IMemoryCache memoryCache)
        {
            _sglContexto = sglContexto;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            EntidadeMonitoramento entidadeMonitoramento = _mapper.Map<EntidadeMonitoramento>(command);

            await Task.FromResult(_sglContexto.EntidadeMonitoramento.Remove(entidadeMonitoramento));

            await _sglContexto.SaveChangesAsync(cancellationToken);

            List<EntidadeMonitoramento> monitoramentos = await _sglContexto.EntidadeMonitoramento
                      .OrderBy(s => s.Guid)
                      .ToListAsync(cancellationToken);


            _memoryCache.Set("MonitoramentosCache", monitoramentos, TimeSpan.FromDays(1));

            return default;
        }
    }
}

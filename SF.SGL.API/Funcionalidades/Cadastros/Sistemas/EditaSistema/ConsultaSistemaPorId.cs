namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.EditaSistema;

public class ConsultaSistemaPorId
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
        public int Id { get; init; }
    }

    public record Resultado
    {
        public Sistema Sistema { get; init; }
    }

    public record Sistema
    {
        public int Id { get; init; }

        public string Nome { get; init; }

        public string UrlServicoConsultaLog { get; init; }

        public string UsuarioLogin { get; init; }

        public string UsuarioSenha { get; init; }
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
            Sistema sistema = await _sglContexto.EntidadeSistema.Where(s => s.Id == query.Id)
                .ProjectTo<Sistema>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

            FuncionalidadeSistemasException.Quando(sistema is null, $"Não existe sistema com o código {query.Id}.");

            Resultado resultado = new()
            {
                Sistema = sistema
            };

            return resultado;
        }
    }
}

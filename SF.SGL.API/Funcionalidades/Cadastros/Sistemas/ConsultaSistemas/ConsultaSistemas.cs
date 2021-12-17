using SF.SGL.API.Funcionalidades.Cadastros.Sistemas.Excecoes;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.ConsultaSistemas;
public class ConsultaSistemas
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
        public IEnumerable<Sistema> Sistemas { get; set; }
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
            List<Sistema> sistemas = await _sglContexto.EntidadeSistema
                .ProjectTo<Sistema>(_configurationProvider)
                .OrderBy(s => s.Nome)
                .ToListAsync(cancellationToken);

            FuncionalidadeSistemasException.Quando(!sistemas.Any(), "Não existem sistemas cadastrados.");

            Resultado resultado = new()
            {
                Sistemas = sistemas
            };

            return resultado;
        }
    }
}

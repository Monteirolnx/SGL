using SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.Excecoes;

namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.AdicionaMonitoramento;

public class AuxConsultaSistemasCadMonit
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
        public IEnumerable<Sistema> Resultados { get; init; }
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

        public async Task<Resultado> Handle(Query query, CancellationToken cancellationToken)
        {
            List<Sistema> sistemas = await _sglContexto.EntidadeSistema
                .ProjectTo<Sistema>(_configurationProvider)
                .OrderBy(x => x.Nome)
                .ToListAsync(cancellationToken);

            FuncionalidadeMonitoramentoException.Quando(!sistemas.Any(), "Não existem sistemas cadastrados.");

            Resultado resultado = new()
            {
                Resultados = sistemas
            };

            return resultado;
        }
    }
}

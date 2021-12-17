using SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.Excecoes;

namespace SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.ConsultaLogsAuditoria;

public class AuxConsultaSistemasLogAudit
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
        public IEnumerable<Sistema> Resultados { get; set; }
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

            FuncionalidadeLogAuditoriaException.Quando(!sistemas.Any(), "Não existem sistemas cadastrados.");

            Resultado resultado = new()
            {
                Resultados = sistemas
            };

            return resultado;
        }
    }
}

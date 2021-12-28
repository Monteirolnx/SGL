using SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.Excecoes;

namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.EditaMonitoramento;

public class AuxConsultaMonitoramentoPorId
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeMonitoramento, Monitoramento>();
        }
    }

    public record Query : IRequest<Resultado>
    {
        public int Id { get; init; }
    }

    public record Resultado
    {
        public Monitoramento Monitoramento { get; init; }
    }

    public record Monitoramento
    {
        public int Id { get; init; }

        public Guid Guid { get; init; }
        
        public string Nome { get; init; }
        
        public string Descricao { get; init; }
        
        public string Acao { get; init; }
        
        public string Contato { get; init; }
        
        public int SistemaId { get; init; }

        public Sistema Sistema { get; init; }
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
            var consultaDb = await _sglContexto.EntidadeMonitoramento
            .ProjectTo<Monitoramento>(_configurationProvider)
            .Join(_sglContexto.EntidadeSistema,
            monitoramento => monitoramento.SistemaId,
            sistema => sistema.Id,
            (monitoramento, sistema) => new
            {
                MonitoramentoId = monitoramento.Id,
                MonitoramentoGUI = monitoramento.Guid,
                MonitoramentoNome = monitoramento.Nome,
                MonitoramentoDescricao = monitoramento.Descricao,
                MonitoramentoAcao = monitoramento.Acao,
                MonitoramentoContato = monitoramento.Contato,
                MonitoramendoSistemaId = monitoramento.SistemaId,

                SistemaId = monitoramento.Id,
                SistemaNome = sistema.Nome
            })
            .Where(m => m.SistemaId == query.Id)
            .SingleOrDefaultAsync(cancellationToken);

            FuncionalidadeMonitoramentoException.Quando(consultaDb is null, $"Não existe monitoramento com o código {query.Id}.");

            Monitoramento monitoramento = new()
            {
                Id = consultaDb.MonitoramentoId,
                Guid = consultaDb.MonitoramentoGUI,
                Nome = consultaDb.MonitoramentoNome,
                Descricao = consultaDb.MonitoramentoDescricao,
                Acao = consultaDb.MonitoramentoAcao,
                Contato = consultaDb.MonitoramentoContato,
                SistemaId = consultaDb.MonitoramendoSistemaId,

                Sistema = new()
                {
                    Id = consultaDb.SistemaId,
                    Nome = consultaDb.SistemaNome
                }
            };
            
            Resultado resultado = new()
            {
                Monitoramento = monitoramento
            };

            return resultado;
        }
    }
}

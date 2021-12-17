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
        public int Id { get; set; }

        public Guid Guid { get; set; }
        
        public string Nome { get; set; }
        
        public string Descricao { get; set; }
        
        public string Acao { get; set; }
        
        public string Contato { get; set; }
        
        public int SistemaId { get; set; }

        public Sistema Sistema { get; set; }
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
            //Monitoramento monitoramento = await _sglContexto.EntidadeMonitoramento.Where(s => s.Id == query.Id)
            //    .ProjectTo<Monitoramento>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

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
            .Where(s => s.SistemaId == query.Id)
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

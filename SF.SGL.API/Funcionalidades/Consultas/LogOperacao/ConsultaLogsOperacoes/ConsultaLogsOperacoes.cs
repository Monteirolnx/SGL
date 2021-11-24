namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes;

public class ConsultaLogsOperacoes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeSistema, Sistema>();
            CreateMap<WCF.Sistema, Sistema>().ReverseMap();
            CreateMap<WCF.ParametroConsultaLogOperacao, Query>().ReverseMap();
        }
    }

    public record Query : IRequest<Resultado>
    {
        public int SistemaId { get; set; }

        public string SistemaNome { get; set; }

        public int? CodigoLogOperacao { get; set; }

        public string CodigoIdentificadorUsuario { get; set; }

        public DateTime? PeriodoInicial { get; set; }

        public DateTime? PeriodoFinal { get; set; }

        public TimeSpan? HorarioInicial { get; set; }

        public TimeSpan? HorarioFinal { get; set; }

        public string Funcionalidade { get; set; }

        public int? TipoRegistro { get; set; }

        public int? SubTipoRegistro { get; set; }

        public string MensagemErro { get; set; }

        public string ExcecaoCapturada { get; set; }

        public string CampoOrdenacao { get; set; }

        public int DirecaoOrdenacao { get; set; }

        public int PaginaAtual { get; set; }

        public int QuantidadeRegistroPagina { get; set; }
    }

    public record Sistema
    {
        public int Id { get; init; }

        public string Nome { get; init; }

        public string UrlServicoConsultaLog { get; init; }

        public string UsuarioLogin { get; init; }

        public string UsuarioSenha { get; init; }
    }

    public class Resultado
    {
        public int CodigoRetorno { get; set; }

        public List<LogOperacao> LogOperacao { get; set; }

        public string MensagemRetorno { get; set; }

        public int QuantidadeTotalRegistrosEncontrados { get; set; }
    }

    public class LogOperacao
    {
        public int CodigoLogOperacao { get; set; }

        public string CodigoIdentificadorUsuario { get; set; }

        public string NomeUsuario { get; set; }

        public string EnderecoIp { get; set; }

        public string CodigoIdentificadorCertificado { get; set; }

        public DateTime DataOcorrencia { get; set; }

        public TimeSpan HoraOcorrencia { get; set; }

        public string NomeFuncionalidade { get; set; }

        public int TipoRegistro { get; set; }

        public int SubTipoRegistro { get; set; }

        public string MensagemErro { get; set; }

        public string ExcecaoCapturada { get; set; }

        public string DetalhesDaExcecao { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, Resultado>
    {
        private readonly SGLContexto _sglContexto;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public QueryHandler(SGLContexto sglContexto, IMapper mapper, AutoMapper.IConfigurationProvider configurationProvider)
        {
            _sglContexto = sglContexto;
            _mapper = mapper;
            _configurationProvider = configurationProvider;
        }

        public async Task<Resultado> Handle(Query query, CancellationToken cancellationToken)
        {
            Sistema sistema = await _sglContexto.EntidadeSistema.Where(s => s.Id == query.SistemaId)
                   .ProjectTo<Sistema>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

            WCF.Sistema sistemaWCF = _mapper.Map<WCF.Sistema>(sistema);
            WCF.ParametroConsultaLogOperacao parametroConsultaLogAuditoriaWCF = _mapper.Map<WCF.ParametroConsultaLogOperacao>(query);
            RepostaConsultaLogOperacaoDTO repostaConsultaLogOperacaoDTO = await WCF.ConsultaLogService.ConsultarLogOperacaoAsync(sistemaWCF, parametroConsultaLogAuditoriaWCF);

            Resultado resultado = MontarResultado(repostaConsultaLogOperacaoDTO);

            return resultado;
        }

        private static Resultado MontarResultado(RepostaConsultaLogOperacaoDTO repostaConsultaLogOperacaoDTO)
        {
            Resultado resultado = new();
            resultado.CodigoRetorno = repostaConsultaLogOperacaoDTO.CodigoRetorno;
            resultado.MensagemRetorno = repostaConsultaLogOperacaoDTO.MensagemRetorno;
            resultado.QuantidadeTotalRegistrosEncontrados = repostaConsultaLogOperacaoDTO.QuantidadeTotalRegistrosEncontrados;

            if (resultado.QuantidadeTotalRegistrosEncontrados > 0)
            {
                resultado.LogOperacao = new List<LogOperacao>();
                foreach (LogOperacaoDTO item in repostaConsultaLogOperacaoDTO.LogOperacao)
                {
                    LogOperacao logOperacaoDTO = new()
                    {
                        CodigoIdentificadorCertificado = item.CodigoIdentificadorCertificado,
                        CodigoIdentificadorUsuario = item.CodigoIdentificadorUsuario,
                        CodigoLogOperacao = item.CodigoLogOperacao,
                        DataOcorrencia = item.DataOcorrencia,
                        DetalhesDaExcecao = item.DetalhesDaExcecao,
                        EnderecoIp = item.EnderecoIp,
                        ExcecaoCapturada = item.ExcecaoCapturada,
                        HoraOcorrencia = item.HoraOcorrencia,
                        MensagemErro = item.MensagemErro,
                        NomeFuncionalidade = item.NomeFuncionalidade,
                        NomeUsuario = item.NomeUsuario,
                        SubTipoRegistro = item.SubTipoRegistro,
                        TipoRegistro = item.TipoRegistro
                    };
                    resultado.LogOperacao.Add(logOperacaoDTO);
                }
            }
            return resultado;
        }
    }
}

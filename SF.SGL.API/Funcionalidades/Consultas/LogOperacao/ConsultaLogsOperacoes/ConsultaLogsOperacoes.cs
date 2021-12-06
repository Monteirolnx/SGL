namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes;

public class ConsultaLogsOperacoes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeSistema, Sistema>();
            CreateMap<WCF.ConsultaLogServices.ConsultaLogOperacaoService.Sistema, Sistema>().ReverseMap();
            CreateMap<WCF.ConsultaLogServices.ConsultaLogOperacaoService.ParametroConsultaLogOperacao, Query>().ReverseMap();
        }
    }

    public record Query : IRequest<RepostaConsultaLogOperacao>
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

    public class RepostaConsultaLogOperacao
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

        public string NomeFuncionalidade { get; set; }

        public int TipoRegistro { get; set; }

        public int SubTipoRegistro { get; set; }

        public string MensagemErro { get; set; }

        public string ExcecaoCapturada { get; set; }

        public string DetalhesDaExcecao { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, RepostaConsultaLogOperacao>
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

        public async Task<RepostaConsultaLogOperacao> Handle(Query query, CancellationToken cancellationToken)
        {
            Sistema sistema = await _sglContexto.EntidadeSistema.Where(s => s.Id == query.SistemaId)
                   .ProjectTo<Sistema>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

            WCF.ConsultaLogServices.ConsultaLogOperacaoService.Sistema sistemaWCF = 
                _mapper.Map<WCF.ConsultaLogServices.ConsultaLogOperacaoService.Sistema>(sistema);

            WCF.ConsultaLogServices.ConsultaLogOperacaoService.ParametroConsultaLogOperacao parametroConsultaLogOperacaoWCF = 
                _mapper.Map<WCF.ConsultaLogServices.ConsultaLogOperacaoService.ParametroConsultaLogOperacao>(query);

            RepostaConsultaLogOperacaoDTO repostaConsultaLogOperacaoDTO = 
                await WCF.ConsultaLogServices.ConsultaLogOperacaoService.ConsultarAsync(sistemaWCF, parametroConsultaLogOperacaoWCF);

            RepostaConsultaLogOperacao resultado = MontarResultado(repostaConsultaLogOperacaoDTO);

            return resultado;
        }        
    }

    private static RepostaConsultaLogOperacao MontarResultado(RepostaConsultaLogOperacaoDTO repostaConsultaLogOperacaoDTO)
    {
        RepostaConsultaLogOperacao repostaConsultaLogOperacao = new();
        repostaConsultaLogOperacao.CodigoRetorno = repostaConsultaLogOperacaoDTO.CodigoRetorno;
        repostaConsultaLogOperacao.MensagemRetorno = repostaConsultaLogOperacaoDTO.MensagemRetorno;
        repostaConsultaLogOperacao.QuantidadeTotalRegistrosEncontrados = repostaConsultaLogOperacaoDTO.QuantidadeTotalRegistrosEncontrados;

        if (repostaConsultaLogOperacao.QuantidadeTotalRegistrosEncontrados > 0)
        {
            repostaConsultaLogOperacao.LogOperacao = new List<LogOperacao>();
            foreach (LogOperacaoDTO item in repostaConsultaLogOperacaoDTO.LogOperacao)
            {
                LogOperacao logOperacaoDTO = new()
                {
                    CodigoIdentificadorCertificado = item.CodigoIdentificadorCertificado,
                    CodigoIdentificadorUsuario = item.CodigoIdentificadorUsuario,
                    CodigoLogOperacao = item.CodigoLogOperacao,
                    DataOcorrencia = RecuperarDataHoraOcorrecia(item.DataOcorrencia, item.HoraOcorrencia),
                    DetalhesDaExcecao = item.DetalhesDaExcecao,
                    EnderecoIp = item.EnderecoIp,
                    ExcecaoCapturada = item.ExcecaoCapturada,
                    MensagemErro = item.MensagemErro,
                    NomeFuncionalidade = item.NomeFuncionalidade,
                    NomeUsuario = item.NomeUsuario,
                    SubTipoRegistro = item.SubTipoRegistro,
                    TipoRegistro = item.TipoRegistro
                };
                repostaConsultaLogOperacao.LogOperacao.Add(logOperacaoDTO);
            }
        }
        return repostaConsultaLogOperacao;
    }

    private static DateTime RecuperarDataHoraOcorrecia(DateTime dataOcorrencia, TimeSpan horaOcorrencia)
    {
        return new DateTime(dataOcorrencia.Year, dataOcorrencia.Month, dataOcorrencia.Day, horaOcorrencia.Hours, horaOcorrencia.Minutes, horaOcorrencia.Seconds);
    }
}

namespace SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.ConsultaLogsAuditoria;

public class ConsultaLogsAuditoria
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EntidadeSistema, Sistema>();
            CreateMap<WCF.ConsultaLogServices.ConsultaLogAuditoriaService.Sistema, Sistema>().ReverseMap();
            CreateMap<WCF.ConsultaLogServices.ConsultaLogAuditoriaService.ParametroConsultaLogAuditoria, Query>().ReverseMap();
        }
    }

    public record Query : IRequest<RepostaConsultaLogAuditoria>
    {
        public int SistemaId { get; set; }

        public string SistemaNome { get; set; }

        public int? CodigoLogAuditoria { get; set; }

        public string CodigoIdentificadorUsuario { get; set; }

        public DateTime? PeriodoInicial { get; set; }

        public DateTime? PeriodoFinal { get; set; }

        public TimeSpan? HorarioInicial { get; set; }

        public TimeSpan? HorarioFinal { get; set; }

        public string Funcionalidade { get; set; }

        public string Operacao { get; set; }

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

    public class RepostaConsultaLogAuditoria
    {
        public int CodigoRetorno { get; set; }

        public string MensagemRetorno { get; set; }

        public List<LogAuditoria> LogAuditoria { get; set; }

        public int QuantidadeTotalRegistrosEncontrados { get; set; }
    }

    public class LogAuditoria
    {
        public int CodigoLogAuditoria { get; set; }

        public string CodigoIdentificadorUsuario { get; set; }

        public string NomeUsuario { get; set; }

        public string EnderecoIp { get; set; }

        public string CodigoIdentificadorCertificado { get; set; }

        public DateTime DataOcorrencia { get; set; }

        public string NomeFuncionalidade { get; set; }

        public string NomeOperacao { get; set; }

        public string Conteudo { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, RepostaConsultaLogAuditoria>
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

        public async Task<RepostaConsultaLogAuditoria> Handle(Query query, CancellationToken cancellationToken)
        {
            Sistema sistema = await _sglContexto.EntidadeSistema.Where(s => s.Id == query.SistemaId)
                   .ProjectTo<Sistema>(_configurationProvider).SingleOrDefaultAsync(cancellationToken);

            WCF.ConsultaLogServices.ConsultaLogAuditoriaService.Sistema sistemaWCF = 
                _mapper.Map<WCF.ConsultaLogServices.ConsultaLogAuditoriaService.Sistema>(sistema);

            WCF.ConsultaLogServices.ConsultaLogAuditoriaService.ParametroConsultaLogAuditoria parametroConsultaLogAuditoriaWCF = 
                _mapper.Map<WCF.ConsultaLogServices.ConsultaLogAuditoriaService.ParametroConsultaLogAuditoria>(query);

            RespostaConsultaLogAuditoriaDTO repostaConsultaLogAuditoriaDTO = 
                await WCF.ConsultaLogServices.ConsultaLogAuditoriaService.ConsultarAsync(sistemaWCF, parametroConsultaLogAuditoriaWCF);

            RepostaConsultaLogAuditoria resultado = MontarResultado(repostaConsultaLogAuditoriaDTO);

            return resultado;
        }

        private static RepostaConsultaLogAuditoria MontarResultado(RespostaConsultaLogAuditoriaDTO repostaConsultaLogAuditoriaDTO)
        {
            RepostaConsultaLogAuditoria respostaConsultaLogAuditoria = new();
            respostaConsultaLogAuditoria.CodigoRetorno = repostaConsultaLogAuditoriaDTO.CodigoRetorno;
            respostaConsultaLogAuditoria.MensagemRetorno = repostaConsultaLogAuditoriaDTO.MensagemRetorno;
            respostaConsultaLogAuditoria.QuantidadeTotalRegistrosEncontrados = repostaConsultaLogAuditoriaDTO.QuantidadeTotalRegistrosEncontrados;

            if (respostaConsultaLogAuditoria.QuantidadeTotalRegistrosEncontrados > 0)
            {
                respostaConsultaLogAuditoria.LogAuditoria = new List<LogAuditoria>();
                foreach (LogAuditoriaDTO item in repostaConsultaLogAuditoriaDTO.LogAuditoria)
                {
                    LogAuditoria logAuditoriaDTO = new()
                    {
                        CodigoLogAuditoria = item.CodigoLogAuditoria,
                        CodigoIdentificadorCertificado = item.CodigoIdentificadorCertificado,
                        CodigoIdentificadorUsuario = item.CodigoIdentificadorUsuario,
                        DataOcorrencia = RecuperarDataHoraOcorrecia(item.DataOcorrencia, item.HoraOcorrencia),
                        EnderecoIp = item.EnderecoIp,
                        NomeFuncionalidade = item.NomeFuncionalidade,
                        NomeUsuario = item.NomeUsuario,
                        NomeOperacao = item.NomeOperacao,
                        Conteudo = item.Conteudo,
                    };
                    respostaConsultaLogAuditoria.LogAuditoria.Add(logAuditoriaDTO);
                }
            }
            return respostaConsultaLogAuditoria;
        }

        private static DateTime RecuperarDataHoraOcorrecia(DateTime dataOcorrencia, TimeSpan horaOcorrencia)
        {
            return new DateTime(dataOcorrencia.Year, dataOcorrencia.Month, dataOcorrencia.Day, horaOcorrencia.Hours, horaOcorrencia.Minutes, horaOcorrencia.Seconds);
        }
    }
}

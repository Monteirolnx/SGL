namespace SF.SGL.WCF.ConsultaLogServices;

public class ConsultaLogOperacaoService
{
    public static async Task<RepostaConsultaLogOperacaoDTO> ConsultarAsync(Sistema sistema, ParametroConsultaLogOperacao parametroConsultaLogOperacao)
    {
        EndpointAddress remoteAddress = new(sistema.UrlServicoConsultaLog);

        BasicHttpBinding binding = new();
        binding.Name = "BasicHttpBinding_IServicoLog";
        binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
        binding.MaxReceivedMessageSize = int.MaxValue;
        binding.MaxBufferSize = int.MaxValue;
        binding.ReaderQuotas.MaxDepth = 32;
        binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
        binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

        using ServicoLogClient proxy = new(binding, remoteAddress);
        proxy.ClientCredentials.UserName.UserName = sistema.UsuarioLogin;
        proxy.ClientCredentials.UserName.Password = sistema.UsuarioSenha;
        proxy.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
        {
            CertificateValidationMode = X509CertificateValidationMode.None,
            RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
        };

        ParametroConsultaLogOperacaoDTO parametros = new();
        parametros.PeriodoInicial = parametroConsultaLogOperacao.PeriodoInicial.HasValue ? parametroConsultaLogOperacao.PeriodoInicial : null;
        parametros.PeriodoFinal = parametroConsultaLogOperacao.PeriodoFinal.HasValue ? parametroConsultaLogOperacao.PeriodoFinal : null;

        parametros.HorarioInicial =
            parametroConsultaLogOperacao.HorarioInicial.HasValue &&
            parametroConsultaLogOperacao.HorarioInicial.Value.Ticks > 0 ? parametroConsultaLogOperacao.HorarioInicial.Value : null;

        parametros.HorarioFinal =
            parametroConsultaLogOperacao.HorarioFinal.HasValue &&
            parametroConsultaLogOperacao.HorarioFinal.Value.Ticks > 0 ? parametroConsultaLogOperacao.HorarioFinal.Value : null;

        parametros.CodigoIdentificadorUsuario = parametroConsultaLogOperacao.CodigoIdentificadorUsuario;

        parametros.Funcionalidade = parametroConsultaLogOperacao.Funcionalidade;
        parametros.MensagemErro = parametroConsultaLogOperacao.MensagemErro;

        parametros.TipoRegistro = parametroConsultaLogOperacao.TipoRegistro;
        parametros.SubTipoRegistro = parametroConsultaLogOperacao.SubTipoRegistro;

        parametros.ExcecaoCapturada = parametroConsultaLogOperacao.ExcecaoCapturada;

        #region Fixos no fonte
        parametros.PaginaAtual = parametroConsultaLogOperacao.PaginaAtual;
        parametros.QuantidadeRegistroPagina = 10000;
        parametros.CampoOrdenacao = "DataOcorrencia";
        parametros.DirecaoOrdenacao = 2;
        #endregion

        RepostaConsultaLogOperacaoDTO resposta = await proxy.ConsultarLogOperacaoAsync(parametros);
        return resposta;
    }

    public record Sistema
    {
        public string UrlServicoConsultaLog { get; init; }

        public string UsuarioLogin { get; init; }

        public string UsuarioSenha { get; init; }
    }

    public record ParametroConsultaLogOperacao
    {
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
}
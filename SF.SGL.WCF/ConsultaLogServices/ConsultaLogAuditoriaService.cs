namespace SF.SGL.WCF.ConsultaLogServices;

public class ConsultaLogAuditoriaService
{
    public static async Task<RespostaConsultaLogAuditoriaDTO> ConsultarAsync(Sistema sistema, ParametroConsultaLogAuditoria parametroConsultaLogAuditoria)
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

        ParametroConsultaLogAuditoriaDTO parametros = new();
        parametros.PeriodoInicial = parametroConsultaLogAuditoria.PeriodoInicial.HasValue ? parametroConsultaLogAuditoria.PeriodoInicial : null;
        parametros.PeriodoFinal = parametroConsultaLogAuditoria.PeriodoFinal.HasValue ? parametroConsultaLogAuditoria.PeriodoFinal : null;

        parametros.HorarioInicial =
            parametroConsultaLogAuditoria.HorarioInicial.HasValue &&
            parametroConsultaLogAuditoria.HorarioInicial.Value.Ticks > 0 ? parametroConsultaLogAuditoria.HorarioInicial.Value : null;

        parametros.HorarioFinal =
            parametroConsultaLogAuditoria.HorarioFinal.HasValue &&
            parametroConsultaLogAuditoria.HorarioFinal.Value.Ticks > 0 ? parametroConsultaLogAuditoria.HorarioFinal.Value : null;

        parametros.Operacao = parametroConsultaLogAuditoria.Operacao;
        parametros.CodigoIdentificadorUsuario = parametroConsultaLogAuditoria.CodigoIdentificadorUsuario;
        parametros.CodigoLogAuditoria = parametroConsultaLogAuditoria.CodigoLogAuditoria;
        parametros.Funcionalidade = parametroConsultaLogAuditoria.Funcionalidade;

        #region Fixos no fonte
        parametros.PaginaAtual = parametroConsultaLogAuditoria.PaginaAtual;
        parametros.QuantidadeRegistroPagina = 10000;
        parametros.CampoOrdenacao = "DataOcorrencia";
        parametros.DirecaoOrdenacao = 2;
        #endregion


        RespostaConsultaLogAuditoriaDTO resposta = await proxy.ConsultarLogAuditoriaAsync(parametros);
        return resposta;
    }

    public record Sistema
    {
        public string UrlServicoConsultaLog { get; init; }

        public string UsuarioLogin { get; init; }

        public string UsuarioSenha { get; init; }
    }

    public record ParametroConsultaLogAuditoria
    {
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
}

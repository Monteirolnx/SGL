namespace SF.SGL.WCF;

public class ConsultaLogService
{
    public static async Task<RepostaConsultaLogOperacaoDTO> ConsultarLogOperacaoAsync(Sistema sistema, ParametroConsultaLogOperacao parametroConsultaLogOperacao)
    {
        EndpointAddress remoteAddress = new(sistema.UrlServicoConsultaLog);

        BasicHttpBinding binding = new();
        binding.Name = "BasicHttpBinding_IServicoLog";
        binding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
        binding.MaxReceivedMessageSize = 20000000;
        binding.MaxBufferSize = 20000000;
        binding.ReaderQuotas.MaxDepth = 32;
        binding.ReaderQuotas.MaxArrayLength = 20000000;
        binding.ReaderQuotas.MaxStringContentLength = 200000000;

        using ServicoLogClient proxy = new(binding, remoteAddress);
        proxy.ClientCredentials.UserName.UserName = sistema.UsuarioLogin;
        proxy.ClientCredentials.UserName.Password = sistema.UsuarioSenha;
        proxy.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
        {
            CertificateValidationMode = X509CertificateValidationMode.None,
            RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
        };

        ParametroConsultaLogOperacaoDTO parametros = new();
        //parametros.CodigoLogOperacao
        //parametros.CodigoIdentificadorUsuario
        //parametros.PeriodoInicial
        //parametros.PeriodoFinal
        //parametros.HorarioInicial
        //parametros.HorarioFinal
        //parametros.Funcionalidade
        parametros.TipoRegistro = parametroConsultaLogOperacao.TipoRegistro;



        parametros.PaginaAtual = parametroConsultaLogOperacao.PaginaAtual;
        parametros.QuantidadeRegistroPagina = 10000;
        parametros.CampoOrdenacao = "DataOcorrencia";
        parametros.DirecaoOrdenacao = 2;

        RepostaConsultaLogOperacaoDTO resposta = await proxy.ConsultarLogOperacaoAsync(parametros);
        return resposta;
    }
}

public record Sistema
{
    public string UrlServicoConsultaLog { get; init; }

    public string UsuarioLogin { get; init; }

    public string UsuarioSenha { get; init; }
}

public class ParametroConsultaLogOperacao
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

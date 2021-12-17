namespace SF.SGL.UI.Pages.Consultas.LogAuditoria.ConsultaLogAuditoria;

public partial class ConsultaLogAuditoria
{
    protected ParametroConsultaLogAuditoria parametroConsultaLogAuditoria;
    protected ErroRetornoAPI erroRetornoAPI;
    protected RepostaConsultaLogAuditoria repostaConsultaLogAuditoria;

    string tipoOperacao;
    List<string> tiposOperacao = new();

    protected IEnumerable<LogAuditoria> LogsAuditoria { get; set; }

    protected List<Sistema> Sistemas { get; set; }

    protected bool DesabilitarBtnPesquisarSistema { get; set; } = false;

    protected bool DesabilitarBtnLimpar { get; set; } = false;

    protected bool DesabilitarBtnConsultar { get; set; } = false;

    protected bool BuscandoRegistros { get; set; } = false;

    protected int TotalRegistrosPesquisa { get; set; }

    #region Injects
    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IConfiguration Configuration { get; set; }

    [Inject]
    protected RadzenDataGrid<LogAuditoria> GridConsultaLogAuditoria { get; set; }

    [Inject]
    protected HttpClient HttpClient { get; set; }
    #endregion

    #region Métodos

    protected override async Task OnInitializedAsync()
    {
        LimparComponentes();
        MontarDropDowns();
        await MontarMemoria();
    }

    protected void LimparComponentes()
    {
        LogsAuditoria = null;
    }

    protected void MontarDropDowns()
    {
        tiposOperacao = new();
        tipoOperacao = TipoOperacao.Todas.ToString();
        Array valoresTipoOperacao = Enum.GetValues(typeof(TipoOperacao));
        foreach (Enum valor in valoresTipoOperacao)
        {
            tiposOperacao.Add(RetornaDescricaoEnum(valor));
        }
    }

    protected static string RetornaDescricaoEnum(Enum valor)
    {
        Type tipo = valor.GetType();
        FieldInfo fieldInfo = tipo.GetField(valor.ToString());
        DescriptionAttribute[] atributos =
        fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                as DescriptionAttribute[];
        if (atributos.Length > 0)
            return atributos[0].Description;
        else
            return string.Empty;
    }

    protected async Task MontarMemoria()
    {
        await Task.FromResult(parametroConsultaLogAuditoria = new());
        HttpResponseMessage httpResponseMessage = await ApiAuxConsultaSistemasLogAudit();
        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            InformarFallhaComunicacaoAPI();
        }
        else if (!httpResponseMessage.IsSuccessStatusCode)
        {
            await InformarErroAPI(httpResponseMessage);
        }
        else
        {
            Sistemas = await httpResponseMessage.Content.ReadFromJsonAsync<List<Sistema>>();
        }
        Recarregar();
    }

    protected void Recarregar()
    {
        InvokeAsync(StateHasChanged);
    }

    protected void DesabilitarBotoes(bool valor)
    {
        DesabilitarBtnPesquisarSistema = valor;
        DesabilitarBtnLimpar = valor;
    }

    protected async Task LimparConsulta()
    {
        await OnInitializedAsync();
    }

    protected async Task EnviarFormulario(ParametroConsultaLogAuditoria parametroConsultaLogAuditoria)
    {
        LogsAuditoria = null;
        TotalRegistrosPesquisa = 0;
        BuscandoRegistros = true;
        DesabilitarBotoes(true);

        RecuperarFiltros(ref parametroConsultaLogAuditoria);
        HttpResponseMessage httpResponseMessage = await ApiConsultarLogOperacao(parametroConsultaLogAuditoria);
        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            InformarFallhaComunicacaoAPI();
        }
        else if (!httpResponseMessage.IsSuccessStatusCode)
        {
            await InformarErroAPI(httpResponseMessage);
        }
        else
        {
            repostaConsultaLogAuditoria = await httpResponseMessage.Content.ReadFromJsonAsync<RepostaConsultaLogAuditoria>();
            if (!string.IsNullOrEmpty(repostaConsultaLogAuditoria.MensagemRetorno) && (repostaConsultaLogAuditoria.MensagemRetorno.Contains("Erro") || repostaConsultaLogAuditoria.MensagemRetorno.Contains("Timeout")))
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = repostaConsultaLogAuditoria.MensagemRetorno });
            }
            else
            {
                LogsAuditoria = repostaConsultaLogAuditoria.LogAuditoria;
                TotalRegistrosPesquisa = repostaConsultaLogAuditoria.QuantidadeTotalRegistrosEncontrados;
            }

        }

        BuscandoRegistros = false;
        DesabilitarBotoes(false);
        Recarregar();
    }

    protected void RecuperarFiltros(ref ParametroConsultaLogAuditoria parametroConsultaLogAuditoria)
    {
        if (tipoOperacao.Equals("Todas"))
            parametroConsultaLogAuditoria.Operacao = string.Empty;
        else
            parametroConsultaLogAuditoria.Operacao = tipoOperacao;
    }
    #endregion

    #region Chamadas Api
    protected async Task<HttpResponseMessage> ApiAuxConsultaSistemasLogAudit()
    {
        try
        {
            string serviceEndpoint = "api/ConsultaLogsAuditoria/AuxConsultaSistemasLogAudit";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task<HttpResponseMessage> ApiConsultarLogOperacao(ParametroConsultaLogAuditoria parametroConsultaLogAuditoria)
    {
        try
        {
            string serviceEndpoint = $"api/ConsultaLogsAuditoria/ConsultarLogAudit";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.PostAsJsonAsync(uriBuilder.Uri, parametroConsultaLogAuditoria);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Eventos

    protected void OnChangeRecuperarFiltroPeriodoInicial(DateTime? periodoInicial)
    {
        parametroConsultaLogAuditoria.PeriodoInicial = periodoInicial ?? null;
        parametroConsultaLogAuditoria.HorarioInicial = periodoInicial.HasValue ? periodoInicial.Value.TimeOfDay : null;
    }

    protected void OnChangeRecuperarFiltroPeriodoFinal(DateTime? periodoFinal)
    {
        parametroConsultaLogAuditoria.PeriodoFinal = periodoFinal ?? null;
        parametroConsultaLogAuditoria.HorarioFinal = periodoFinal.HasValue ? periodoFinal.Value.TimeOfDay : null;
    }
    #endregion

    #region Notificações e Mensagens de erro
    protected void InformarFallhaComunicacaoAPI()
    {
        erroRetornoAPI = new();
        erroRetornoAPI.Message = "Não foi possível realizar a comunicação com a Api SGL.";
        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = erroRetornoAPI.Message });
    }

    protected async Task InformarErroAPI(HttpResponseMessage response)
    {
        erroRetornoAPI = new();
        erroRetornoAPI = await response.Content.ReadFromJsonAsync<ErroRetornoAPI>();
        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = erroRetornoAPI.Message });
    }
    #endregion

    #region Classes
    public class Sistema
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }


    public class ParametroConsultaLogAuditoria
    {
        public int? SistemaId { get; set; }

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

    public record ErroRetornoAPI
    {
        public string Message { get; set; }
    }

    public class RepostaConsultaLogAuditoria
    {
        public int CodigoRetorno { get; set; }

        public List<LogAuditoria> LogAuditoria { get; set; }

        public string MensagemRetorno { get; set; }

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

        public TimeSpan HoraOcorrencia { get; set; }

        public string NomeFuncionalidade { get; set; }

        public string NomeOperacao { get; set; }

        public string Conteudo { get; set; }
    }
    #endregion

    #region Enums
    public enum TipoOperacao
    {
        [Description("Todas")]
        Todas,

        [Description("Consulta")]
        Consulta,

        [Description("Inclusao")]
        Inclusao,

        [Description("Alteracao")]
        Alteracao,

        [Description("Exclusao")]
        Exclusao,

        [Description("Login")]
        Login,

        [Description("Logout")]
        Logout,

        [Description("Integracao")]
        Integracao,

        [Description("Rotina")]
        Rotina,
    }
    #endregion
}

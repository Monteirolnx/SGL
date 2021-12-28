namespace SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao;

public partial class ConsultaLogOperacao
{
    protected ParametroConsultaLogOperacao parametroConsultaLogOperacao;
    protected ErroRetornoAPI erroRetornoAPI;
    protected RespostaConsultaLogOperacao respostaConsultaLogOperacao;

    string tipoRegistro;
    List<string> tipoRegistros = new();

    string subTipoRegistro;
    List<string> subTipoRegistros = new();

    protected List<Sistema> Sistemas { get; set; }
    
    protected IEnumerable<LogOperacao> LogsOperacoes { get; set; }

    protected bool BuscandoRegistros { get; set; } = false;

    protected int TotalRegistrosPesquisa { get; set; }
    
    protected bool DesabilitarCampo{ get; set; } = false;

    protected bool DesabilitarBtnLimpar { get; set; } = false;

    protected bool DesabilitarBtnConsultar { get; set; } = false;
   

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
    protected RadzenDataGrid<LogOperacao> GridConsultaLogOperacao { get; set; }

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
        LogsOperacoes = null;
    }

    protected void MontarDropDowns()
    {
        tipoRegistros = new();
        tipoRegistro = TipoRegistro.Todos.ToString();
        Array valoresTipoRegistro = Enum.GetValues(typeof(TipoRegistro));
        foreach (Enum valor in valoresTipoRegistro)
        {
            tipoRegistros.Add(RetornaDescricaoEnum(valor));
        }

        subTipoRegistros = new();
        subTipoRegistro = SubTipoRegistro.Todos.ToString();
        Array valoresSubTipoRegistro = Enum.GetValues(typeof(SubTipoRegistro));
        foreach (Enum valor in valoresSubTipoRegistro)
        {
            subTipoRegistros.Add(RetornaDescricaoEnum(valor));
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
        await Task.FromResult(parametroConsultaLogOperacao = new());
        HttpResponseMessage httpResponseMessage = await ChamarApiAuxConsultaSistemasLogOper();
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

    protected void DesabilitarComponentes(bool valor)
    {
        DesabilitarCampo = valor;
        DesabilitarBtnLimpar = valor;
        DesabilitarBtnConsultar = valor;
    }

    protected void RecuperarFiltros(ref ParametroConsultaLogOperacao parametroConsultaLogOperacao)
    {
        parametroConsultaLogOperacao.TipoRegistro = tipoRegistro switch
        {
            "Sucesso" => 0,
            "Falha" => 1,
            _ => null,
        };

        parametroConsultaLogOperacao.SubTipoRegistro = subTipoRegistro switch
        {
            "Geral" => 3,
            "Integração" => 2,
            "Processo Batch - Aplicação" => 1,
            "Processo Batch - Banco de Dados" => 0,
            _ => null,
        };
    }

    protected async Task LimparConsulta()
    {
        await OnInitializedAsync();
    }

    protected async Task EnviarFormulario(ParametroConsultaLogOperacao parametroConsultaLogOperacao)
    {
        LogsOperacoes = null;
        TotalRegistrosPesquisa = 0;
        BuscandoRegistros = true;
        DesabilitarComponentes(true);

        RecuperarFiltros(ref parametroConsultaLogOperacao);
        HttpResponseMessage httpResponseMessage = await ChamarApiConsultarLogOperacao(parametroConsultaLogOperacao);
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
            respostaConsultaLogOperacao = await httpResponseMessage.Content.ReadFromJsonAsync<RespostaConsultaLogOperacao>();
            if (!string.IsNullOrEmpty(respostaConsultaLogOperacao.MensagemRetorno) && (respostaConsultaLogOperacao.MensagemRetorno.Contains("Erro") || respostaConsultaLogOperacao.MensagemRetorno.Contains("Timeout")))
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = respostaConsultaLogOperacao.MensagemRetorno });
            }
            else
            {
                LogsOperacoes = respostaConsultaLogOperacao.LogOperacao;
                TotalRegistrosPesquisa = respostaConsultaLogOperacao.QuantidadeTotalRegistrosEncontrados;
            }
        }

        BuscandoRegistros = false;
        DesabilitarComponentes(false);
        Recarregar();
    }
    #endregion

    #region Chamadas Api
    protected async Task<HttpResponseMessage> ChamarApiAuxConsultaSistemasLogOper()
    {
        try
        {
            string serviceEndpoint = "api/ConsultaLogsOperacoes/AuxConsultaSistemasLogOper";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task<HttpResponseMessage> ChamarApiConsultarLogOperacao(ParametroConsultaLogOperacao parametroConsultaLogOperacao)
    {
        try
        {
            string serviceEndpoint = $"api/ConsultaLogsOperacoes/ConsultaLogOper";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.PostAsJsonAsync(uriBuilder.Uri, parametroConsultaLogOperacao);
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
        parametroConsultaLogOperacao.PeriodoInicial = periodoInicial ?? null;
        parametroConsultaLogOperacao.HorarioInicial = periodoInicial.HasValue ? periodoInicial.Value.TimeOfDay : null;
    }

    protected void OnChangeRecuperarFiltroPeriodoFinal(DateTime? periodoFinal)
    {
        parametroConsultaLogOperacao.PeriodoFinal = periodoFinal ?? null;
        parametroConsultaLogOperacao.HorarioFinal = periodoFinal.HasValue ? periodoFinal.Value.TimeOfDay : null;
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

    public class ParametroConsultaLogOperacao
    {
        public int? SistemaId { get; set; }

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

    public record ErroRetornoAPI
    {
        public string Message { get; set; }
    }

    public class RespostaConsultaLogOperacao
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
    #endregion

    #region Enums
    public enum TipoRegistro
    {
        [Description("Todos")]
        Todos,

        [Description("Sucesso")]
        Sucesso,

        [Description("Falha")]
        Falha
    }

    public enum SubTipoRegistro
    {
        [Description("Todos")]
        Todos,

        [Description("Geral")]
        Geral,

        [Description("Integração")]
        Integracao,

        [Description("Processo Batch - Aplicação")]
        Processo_Batch_Aplicacao,

        [Description("Processo Batch - Banco de Dados")]
        Processo_Batch_Banco_de_Dados,
    }
    #endregion
}
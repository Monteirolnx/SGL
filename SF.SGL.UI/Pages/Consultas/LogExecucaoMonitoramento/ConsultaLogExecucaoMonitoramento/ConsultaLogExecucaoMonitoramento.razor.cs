namespace SF.SGL.UI.Pages.Consultas.LogExecucaoMonitoramento.ConsultaLogExecucaoMonitoramento;

public partial class ConsultaLogExecucaoMonitoramento
{
    protected ParametroConsultaLogExecMonitoramento parametroConsultaLogExecMonitoramento;
    protected ErroRetornoAPI erroRetornoAPI;
    protected RespostaConsultaLogExecMonitoramento respostaConsultaLogExecucaoMonitoramento;

    string tipoRegistro;
    List<string> tipoRegistros = new();

    protected List<LogExecMonitoramento> LogsExecucoesMonitoramentos { get; set; }

    protected List<Sistema> Sistemas { get; set; }

    protected List<Monitoramento> Monitoramentos { get; set; }

    protected bool DesabilitarCampo { get; set; } = false;

    protected bool DesabilitarBtnLimpar { get; set; } = false;

    protected bool DesabilitarBtnConsultar { get; set; } = true;

    protected bool BuscandoRegistros { get; set; } = false;

    public bool ModoEscutaHabilitado { get; set; } = true;

    protected int TotalRegistrosPesquisa { get; set; }

    private HubConnection HubConexao { get; set; }

    public bool HubConectado => HubConexao?.State == HubConnectionState.Connected;

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
    protected RadzenDataGrid<LogExecMonitoramento> GridConsultaLogMonitoramento { get; set; }

    [Inject]
    protected HttpClient HttpClient { get; set; }
    #endregion

    #region Métodos

    protected override async Task OnInitializedAsync()
    {
        LimparGridExecucoesMonitoramentos();
        MontarDropDowns();

        if (ModoEscutaHabilitado)
        {
            await AtivarModoEscuta();
        }
        
        await MontarMemoria();
    }

    private async Task AtivarModoEscuta()
    {
        string serviceEndpoint = $"sf_sgl_api_hub";
        UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));

        HubConexao = new HubConnectionBuilder()
           .WithUrl(uriBuilder.Uri)
           .Build();

        HubConexao.On<string>("ExecucaoMonitoramento", (message) =>
        {
            if (ModoEscutaHabilitado)
            {
                if (LogsExecucoesMonitoramentos is null)
                {
                    LogsExecucoesMonitoramentos = new();
                }
                TotalRegistrosPesquisa = 0;
                LogExecMonitoramento logExecMonitoramento = new();
                logExecMonitoramento = JsonConvert.DeserializeObject<LogExecMonitoramento>(message);

                if (
                (parametroConsultaLogExecMonitoramento.SistemaId.HasValue && logExecMonitoramento.SistemaId == parametroConsultaLogExecMonitoramento.SistemaId && parametroConsultaLogExecMonitoramento.MonitoramentoId is null && 
                (parametroConsultaLogExecMonitoramento.Status is null ||logExecMonitoramento.Status == parametroConsultaLogExecMonitoramento.Status)) ||
                (parametroConsultaLogExecMonitoramento.MonitoramentoId.HasValue && (logExecMonitoramento.MonitoramentoId == parametroConsultaLogExecMonitoramento.MonitoramentoId)) && (parametroConsultaLogExecMonitoramento.Status is null || logExecMonitoramento.Status == parametroConsultaLogExecMonitoramento.Status) ||
                (parametroConsultaLogExecMonitoramento.Status.HasValue && (logExecMonitoramento.Status == parametroConsultaLogExecMonitoramento.Status)) ||
                (parametroConsultaLogExecMonitoramento.SistemaId is null && parametroConsultaLogExecMonitoramento.MonitoramentoId is null && parametroConsultaLogExecMonitoramento.Status is null))
                {
                    LogsExecucoesMonitoramentos.Add(logExecMonitoramento);
                }

                LogsExecucoesMonitoramentos = LogsExecucoesMonitoramentos.OrderByDescending(x => x.Data).ToList();
                TotalRegistrosPesquisa = LogsExecucoesMonitoramentos.Count;
                GridConsultaLogMonitoramento.Reset();

                Recarregar();
            }
        });

        await HubConexao.StartAsync();
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
        await Task.FromResult(parametroConsultaLogExecMonitoramento = new());
        if (ModoEscutaHabilitado)
        {
            parametroConsultaLogExecMonitoramento.PeriodoInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            await ConsultarLogExecMonitoramento();
        }        
        await CarregarSistemas();
        
        Recarregar();
    }

    private async Task CarregarSistemas()
    {
        HttpResponseMessage httpResponseMessage = await ChamarApiAuxConsultaSistemasLogExecMonitor();
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
    }

    private async Task ConsultarLogExecMonitoramento()
    {
        HttpResponseMessage httpResponseMessage = await ChamarApiConsultarLogExecMonitoramento();
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
            respostaConsultaLogExecucaoMonitoramento = await httpResponseMessage.Content.ReadFromJsonAsync<RespostaConsultaLogExecMonitoramento>();
            LogsExecucoesMonitoramentos = respostaConsultaLogExecucaoMonitoramento.LogsExecsMonitoramentos;
            TotalRegistrosPesquisa = respostaConsultaLogExecucaoMonitoramento.QuantidadeTotalRegistrosEncontrados;
        }
    }

    protected void Recarregar()
    {
        InvokeAsync(StateHasChanged);
    }

    protected void DesabilitarBotoes(bool valor)
    {
        DesabilitarCampo = valor;
        DesabilitarBtnLimpar = valor;
        DesabilitarBtnConsultar = valor;
    }

    protected async Task LimparConsulta()
    {
        await OnInitializedAsync();
    }

    protected async Task EnviarFormulario()
    {
        if (parametroConsultaLogExecMonitoramento.PeriodoFinal.HasValue && parametroConsultaLogExecMonitoramento.PeriodoFinal < parametroConsultaLogExecMonitoramento.PeriodoInicial)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = "Período final deve ser maior que período inicial." });
        }
        else
        {
            LimparGridExecucoesMonitoramentos();
            BuscandoRegistros = true;
            DesabilitarBotoes(true);

            await ConsultarLogExecMonitoramento();

            BuscandoRegistros = false;
            DesabilitarBotoes(false);
        }
        Recarregar();
    }

    protected void RecuperarFiltros(ref ParametroConsultaLogExecMonitoramento parametroConsultaLogExecMonitoramento)
    {
        parametroConsultaLogExecMonitoramento.Status = tipoRegistro switch
        {
            "Sucesso" => true,
            "Falha" => false,
            _ => null,
        };
    }

    protected async Task ConectarDesconctarHub()
    {
        if (ModoEscutaHabilitado)
        {
            DesabilitarBtnConsultar = true;
            await LimparConsulta();
            await AtivarModoEscuta();
        }
        else
        {
            parametroConsultaLogExecMonitoramento.PeriodoInicial = null;
            DesabilitarBtnConsultar = false;
            await HubConexao.StopAsync();
            await HubConexao.DisposeAsync();
        }
    }
    #endregion

    #region Chamadas Api
    protected async Task<HttpResponseMessage> ChamarApiAuxConsultaSistemasLogExecMonitor()
    {
        try
        {
            string serviceEndpoint = "api/ConsultaLogsExecMonitoramentos/AuxConsultaSistemasLogExecMonitor";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<HttpResponseMessage> ChamarApiAuxConsultaMonitoramentosLogExecMonitor(int sistemaId)
    {
        try
        {
            string serviceEndpoint = $"api/ConsultaLogsExecMonitoramentos/AuxConsultaMonitoramentosLogExecMonitor/{sistemaId}";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task<HttpResponseMessage> ChamarApiConsultarLogExecMonitoramento()
    {
        try
        {
            if (parametroConsultaLogExecMonitoramento.PeriodoInicial is null)
            {
                parametroConsultaLogExecMonitoramento.PeriodoInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddMonths(-2);
            }
            string serviceEndpoint = $"api/ConsultaLogsExecMonitoramentos/ConsultaLogsExecMonitoramentos";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.PostAsJsonAsync(uriBuilder.Uri, parametroConsultaLogExecMonitoramento);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Eventos
    protected async Task OnChangePesquisarMonitoramento(dynamic args)
    {
        Monitoramentos = new();
        parametroConsultaLogExecMonitoramento.SistemaId = args > 0 ? args : null;
        parametroConsultaLogExecMonitoramento.MonitoramentoId = null;
        if (parametroConsultaLogExecMonitoramento.SistemaId is not null)
        {
            HttpResponseMessage httpResponseMessage = await ChamarApiAuxConsultaMonitoramentosLogExecMonitor(args);
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
                Monitoramentos = await httpResponseMessage.Content.ReadFromJsonAsync<List<Monitoramento>>();
            }
        }

        if (ModoEscutaHabilitado)
        {
            await ConsultarLogExecMonitoramento();
            Recarregar();
        }
    }

    protected async Task OnChangeAlternarMonitoramento()
    {
        if (ModoEscutaHabilitado)
        {
            await ConsultarLogExecMonitoramento();
            Recarregar();
        }
    }

    protected async Task OnChangeAlternarTipoRegistro()
    {
        RecuperarFiltros(ref parametroConsultaLogExecMonitoramento);
        if (ModoEscutaHabilitado)
        {
            await ConsultarLogExecMonitoramento();
            Recarregar();
        }
    }

    protected void OnChangeRecuperarFiltroPeriodoInicial(DateTime? periodoInicial)
    {
        LimparGridExecucoesMonitoramentos();
        ModoEscutaHabilitado = false;
        DesabilitarBtnConsultar = false;
        parametroConsultaLogExecMonitoramento.PeriodoInicial = periodoInicial ?? null;
    }

    protected void OnChangeRecuperarFiltroPeriodoFinal(DateTime? periodoFinal)
    {
        LimparGridExecucoesMonitoramentos();
        ModoEscutaHabilitado = false;
        DesabilitarBtnConsultar = false;
        parametroConsultaLogExecMonitoramento.PeriodoFinal = periodoFinal ?? null;
    }

    private void LimparGridExecucoesMonitoramentos()
    {
        TotalRegistrosPesquisa = 0;
        LogsExecucoesMonitoramentos = null;
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

        if (ModoEscutaHabilitado)
        {
            LimparGridExecucoesMonitoramentos();
            Recarregar();
            var x = HubConectado;
        }
    }
    #endregion

    #region Classes
    public class Sistema
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }

    public class Monitoramento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int SistemaId { get; set; }
    }

    public class ParametroConsultaLogExecMonitoramento
    {
        public int? SistemaId { get; set; }

        public int? MonitoramentoId { get; set; }

        public DateTime? PeriodoInicial { get; set; }

        public DateTime? PeriodoFinal { get; set; }

        public bool? Status { get; set; }
    }

    public record ErroRetornoAPI
    {
        public string Message { get; set; }
    }

    public class RespostaConsultaLogExecMonitoramento
    {
        public List<LogExecMonitoramento> LogsExecsMonitoramentos { get; set; }

        public int QuantidadeTotalRegistrosEncontrados { get; set; }
    }

    public class LogExecMonitoramento
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }

        public bool Status { get; set; }

        public DateTime Data { get; set; }

        public string Mensagem { get; set; }

        public int MonitoramentoId { get; set; }

        public string MonitoramentoNome { get; set; }

        public int SistemaId { get; set; }

        public string SistemaNome { get; set; }
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
    #endregion
}

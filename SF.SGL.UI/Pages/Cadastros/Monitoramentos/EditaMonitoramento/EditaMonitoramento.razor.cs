namespace SF.SGL.UI.Pages.Cadastros.Monitoramentos.EditaMonitoramento;

public partial class EditaMonitoramento
{
    [Parameter] public int Id { get; set; }
    protected Monitoramento monitoramento;
    protected ErroRetornoAPI erroRetornoAPI;
    CancellationTokenSource cancellationTokenSource;
    State state;

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
    protected HttpClient HttpClient { get; set; }

    [Inject]
    protected ClipboardService ClipboardService { get; set; }
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync()
    {
        await MontarMemoria();
    }

    protected async Task MontarMemoria()
    {
        HabilitarBotaoCopiar();

        HttpResponseMessage httpResponseMessage = await ChamarApiConsultaMonitoramentoPorId(Id);
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
            monitoramento = await httpResponseMessage.Content.ReadFromJsonAsync<Monitoramento>();
        }
    }

    public void Recarregar()
    {
        InvokeAsync(StateHasChanged);
    }

    protected void NavegarPaginaSistemas()
    {
        NavigationManager.NavigateTo("cadastros/monitoramentos");
    }

    protected async Task CopiarTextoAreaTransferencia()
    {
        State temp = state;
        state.Texto = "Copiado";
        state.ClassName = "oi oi-check";
        state.IsDisabled = true;

        await ClipboardService.WriteTextAsync(Convert.ToString(monitoramento.Guid));
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationTokenSource.Token);
        state = temp;
        HabilitarBotaoCopiar();
    }

    private void HabilitarBotaoCopiar()
    {
        cancellationTokenSource = new();
        state = new();
        state.Texto = "Copiar";
        state.ClassName = "oi oi-clipboard";
        state.IsDisabled = false;
    }

    protected async Task EnviarFormulario()
    {
        HttpResponseMessage httpResponseMessage = await ChamarApiEditaMonitoramento(Id, monitoramento);
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
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Sucesso:", Detail = $"Monitoramentos editado com sucesso." });
            NavigationManager.NavigateTo("cadastros/monitoramentos");
        }
        Recarregar();
    }

    #endregion

    #region Chamadas Api
    private async Task<HttpResponseMessage> ChamarApiConsultaMonitoramentoPorId(int id)
    {
        try
        {
            string serviceEndpoint = $"api/EditaMonitoramento/ConsultaMonitoramentoPorId/{id}";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<HttpResponseMessage> ChamarApiEditaMonitoramento(int id, Monitoramento monitoramento)
    {
        try
        {
            string serviceEndpoint = $"api/EditaMonitoramento/Edita/{id}";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.PutAsJsonAsync(uriBuilder.Uri, monitoramento);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Notificações e Mensagens de erro
    private void InformarFallhaComunicacaoAPI()
    {
        erroRetornoAPI = new();
        erroRetornoAPI.Message = "Não foi possível realizar a comunicação com a Api SGL.";
        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = erroRetornoAPI.Message });
    }

    private async Task InformarErroAPI(HttpResponseMessage response)
    {
        erroRetornoAPI = new();
        erroRetornoAPI = await response.Content.ReadFromJsonAsync<ErroRetornoAPI>();
        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = erroRetornoAPI.Message });
    }
    #endregion

    #region Classes
    public class Monitoramento
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

    public class Sistema
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }

    public record State
    {
        public string Texto { get; set; }

        public string ClassName { get; set; }

        public bool IsDisabled { get; set; }
    }

    public record ErroRetornoAPI
    {
        public string Message { get; set; }
    }
    #endregion
}

namespace SF.SGL.UI.Pages.Cadastros.Sistemas.AdicionaSistema;

public partial class AdicionaSistema
{
    protected Sistema sistema;
    protected ErroRetornoAPI erroRetornoAPI;

    #region Injects
    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

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
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync()
    {
        await MontarMemoria();
    }

    protected async Task MontarMemoria()
    {
        await Task.FromResult(sistema = new());
    }

    public void Recarregar()
    {
        InvokeAsync(StateHasChanged);
    }

    protected void NavegarPaginaSistemas()
    {
        NavigationManager.NavigateTo("cadastros/sistemas");
    }

    protected async Task EnviarFormulario()
    {
        HttpResponseMessage httpResponseMessage = await ChamarApiAdicionaSistema();
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
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Sucesso:", Detail = $"Sistema incluído com sucesso." });
            NavigationManager.NavigateTo("cadastros/sistemas");
        }
        Recarregar();
    }
    #endregion

    #region Chamadas Api
    protected async Task<HttpResponseMessage> ChamarApiAdicionaSistema()
    {
        try
        {
            string serviceEndpoint = "api/AdicionaSistema/Adiciona";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.PostAsJsonAsync(uriBuilder.Uri, sistema);
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
    public class Sistema
    {
        public string Nome { get; set; }

        public string UrlServicoConsultaLog { get; set; }

        public string UsuarioLogin { get; set; }

        public string UsuarioSenha { get; set; }
    }

    public record ErroRetornoAPI
    {
        public string Message { get; set; }
    }
    #endregion
}

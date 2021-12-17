namespace SF.SGL.UI.Pages.Cadastros.Sistemas.EditaSistema;

public partial class EditaSistema
{
    [Parameter] public int Id { get; set; }
    protected Sistema sistema;
    protected ErroRetornoAPI erroRetornoAPI;

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
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync()
    {
        await MontarMemoria();
    }

    protected async Task MontarMemoria()
    {
        HttpResponseMessage httpResponseMessage = await ApiConsultaSistemaPorId(Id);
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
            sistema = await httpResponseMessage.Content.ReadFromJsonAsync<Sistema>();
        }
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
        HttpResponseMessage httpResponseMessage = await ApiEditaSistema(Id, sistema);
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
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Sucesso:", Detail = $"Sistema editado com sucesso." });
            NavigationManager.NavigateTo("cadastros/sistemas");
        }
        Recarregar();
    }

    #endregion

    #region Chamadas Api
    private async Task<HttpResponseMessage> ApiConsultaSistemaPorId(int id)
    {
        try
        {
            string serviceEndpoint = $"api/EditaSistema/ConsultaSistemaPorId/{id}";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<HttpResponseMessage> ApiEditaSistema(int id, Sistema sistema)
    {
        try
        {
            string serviceEndpoint = $"api/EditaSistema/Edita/{id}";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.PutAsJsonAsync(uriBuilder.Uri, sistema);
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
        public int Id { get; set; }

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

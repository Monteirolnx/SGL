namespace SF.SGL.UI.Pages.Cadastros.Monitoramentos.ConsultaMonitoramentos;

public partial class ConsultaMonitoramentos
{
    protected List<Monitoramento> monitoramentos;
    protected ErroRetornoAPI erroRetornoAPI;
    protected bool desabilitaAdicao;

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
    protected RadzenDataGrid<Monitoramento> GridConsultaMonitoramentos { get; set; }
    #endregion

    #region Métodos
    protected async override void OnInitialized()
    {
        await MontarMemoria();
    }

    private async Task MontarMemoria()
    {
        desabilitaAdicao = true;
        HttpResponseMessage httpResponseMessage = await ApiConsultaMonitoramentos();
        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            InformarFallhaComunicacaoAPI();
        }
        else if (!httpResponseMessage.IsSuccessStatusCode)
        {
            await InformarErroAPI(httpResponseMessage);
            desabilitaAdicao = false;
        }
        else
        {
            monitoramentos = await httpResponseMessage.Content.ReadFromJsonAsync<List<Monitoramento>>();
            desabilitaAdicao = false;
        }
        Recarregar();
    }

    public void Recarregar()
    {
        InvokeAsync(StateHasChanged);
    }

    protected void NavegarPaginaAdicionarMonitoramento()
    {
        NavigationManager.NavigateTo("cadastros/monitoramentos/adicionamonitoramento");
    }
    #endregion

    #region Eventos
    protected void GridEditButtonClick(dynamic data)
    {
        NavigationManager.NavigateTo($"cadastros/monitoramentos/editamonitoramento/{data.Id}");
    }

    protected async Task GridDeleteButtonClick(dynamic data)
    {
        try
        {
            if (await DialogService.Confirm($"Deseja excluir o monitoramento: {data.Nome}?", title: "Confirma", new ConfirmOptions() { CancelButtonText = "Cancelar", OkButtonText = "Ok" }) == true)
            {
                HttpResponseMessage response = await ApiDeletaMonitoramento(data.Id);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    InformarFallhaComunicacaoAPI();
                }
                else if (!response.IsSuccessStatusCode)
                {
                    await InformarErroAPI(response);
                }
                else
                {
                    monitoramentos.RemoveAll(x => x.Id == data.Id);

                    await GridConsultaMonitoramentos.Reload();
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Sucesso:", Detail = $"Monitoramento exluído com sucesso." });
                }
            }
        }
        catch (Exception)
        {
            NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = $"Não foi possível deletar o monitoramento." });
        }
    }
    #endregion

    #region Chamadas Api
    protected async Task<HttpResponseMessage> ApiConsultaMonitoramentos()
    {
        try
        {
            string serviceEndpoint = "api/ConsultaMonitoramentos/ConsultaTodos";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.GetAsync(uriBuilder.Uri);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<HttpResponseMessage> ApiDeletaMonitoramento(dynamic id)
    {
        try
        {
            string serviceEndpoint = $"api/DeletaMonitoramento/Deleta/{id}";
            UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
            return await HttpClient.DeleteAsync(uriBuilder.Uri);
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
    public record Monitoramento
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

    public record Sistema
    {
        public int Id { get; init; }

        public string Nome { get; init; }
    }

    public record ErroRetornoAPI
    {
        public string Message { get; set; }
    }
    #endregion
}

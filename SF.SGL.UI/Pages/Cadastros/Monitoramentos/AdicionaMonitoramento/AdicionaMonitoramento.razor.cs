namespace SF.SGL.UI.Pages.Cadastros.Monitoramentos.AdicionaMonitoramento
{
    public partial class AdicionaMonitoramento
    {
        protected ErroRetornoAPI erroRetornoAPI;
        protected Monitoramento monitoramento;
        CancellationTokenSource cancellationTokenSource;
        State state;

        protected List<Sistema> Sistemas { get; set; }

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
            await Task.FromResult(monitoramento = new());
            CriarGUID();

            HttpResponseMessage httpResponseMessage = await ApiAuxConsultaSistemasCadMonitoramento();
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

        public void Recarregar()
        {
            InvokeAsync(StateHasChanged);
        }

        private void CriarGUID()
        {
            monitoramento.Guid = Guid.NewGuid();
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
            HttpResponseMessage httpResponseMessage = await ApiAdicionaMonitoramento();
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
                NavigationManager.NavigateTo("cadastros/monitoramentos");
            }
            Recarregar();
        }

        protected void NavegarPaginaMonitoramentos()
        {
            NavigationManager.NavigateTo("cadastros/monitoramentos");
        }
        #endregion

        #region Chamadas Api
        protected async Task<HttpResponseMessage> ApiAuxConsultaSistemasCadMonitoramento()
        {
            try
            {
                string serviceEndpoint = "api/AdicionaMonitoramento/AuxConsultaSistemasCadMonit";
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.GetAsync(uriBuilder.Uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<HttpResponseMessage> ApiAdicionaMonitoramento()
        {
            try
            {
                string serviceEndpoint = "api/AdicionaMonitoramento/Adiciona";
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.PostAsJsonAsync(uriBuilder.Uri, monitoramento);
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
        public record Sistema
        {
            public int Id { get; set; }

            public string Nome { get; set; }
        }

        public class Monitoramento
        {
            public Guid Guid { get; set; }

            public string Nome { get; set; }

            public string Descricao { get; set; }

            public string Acao { get; set; }

            public string Contato { get; set; }

            public int? SistemaId { get; set; }
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
}

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao
{
    public partial class ConsultaLogOperacao
    {
        protected LogOperacao logOperacao;
        protected List<Sistema> sistemas;
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
            await CargaInicial();
        }

        protected async Task CargaInicial()
        {
            await Task.FromResult(logOperacao = new());
            HttpResponseMessage httpResponseMessage = await ApiAuxConsultaSistemas();
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
                sistemas = await httpResponseMessage.Content.ReadFromJsonAsync<List<Sistema>>();
            }
            StateHasChanged();
        }

        private async Task<HttpResponseMessage> ApiAuxConsultaSistemas()
        {
            try
            {
                string serviceEndpoint = "api/ConsultaLogsOperacoes/AuxConsultaSistemas";
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.GetAsync(uriBuilder.Uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnTxtIdSistemaChange(string data)
        {
            int id = Convert.ToInt32(data);
            Sistema sistema = sistemas.Find(x => x.Id == id);
            if (sistema != null)
            {
                logOperacao.SistemaId = Convert.ToString(sistema.Id);
                logOperacao.SistemaNome = sistema.Nome;
            }
            else
            {
                logOperacao.SistemaId = string.Empty;
                logOperacao.SistemaNome = string.Empty;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = "Sistema não existe." });
            }
            StateHasChanged();
        }

        protected async Task EnvioFormulario(LogOperacao logOperacao)
        {

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
        public class LogOperacao
        {
            public string SistemaId { get; set; }

            public string SistemaNome { get; set; }
        }

        public class Sistema
        {
            public int Id { get; set; }

            public string Nome { get; set; }
        }

        public record ErroRetornoAPI
        {
            public string Message { get; set; }
        }
        #endregion
    }
}

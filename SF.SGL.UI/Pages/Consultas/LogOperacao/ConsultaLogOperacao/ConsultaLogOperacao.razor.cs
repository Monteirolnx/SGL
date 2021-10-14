using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Radzen;
using Radzen.Blazor;
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
            await MontarMemoria();
        }

        protected async Task AbrirPesquisaSistema()
        {
            logOperacao.SistemaId = string.Empty;
            logOperacao.SistemaNome = string.Empty;

            LogOperacao resultadoPesquisa = await DialogService.OpenAsync<AuxPesquisaSistema>($"Pesquisa",
                   new Dictionary<string, object>() { { "Sistemas", sistemas } },
                   new DialogOptions() { Width = "670px", Height = "620px", Resizable = false, Draggable = true });

            if (resultadoPesquisa == null)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = "Nenhum sistema foi selecionado." });
            }
            else
            {
                logOperacao.SistemaId = resultadoPesquisa.SistemaId;
                logOperacao.SistemaNome = resultadoPesquisa.SistemaNome;
                Recarregar();
            }
        }

        protected async Task LimparConsulta()
        {
            await OnInitializedAsync();
        }

        protected async Task MontarMemoria()
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
            Recarregar();
        }

        public void Recarregar()
        {
            InvokeAsync(StateHasChanged);
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

        protected async Task EnviarFormulario(LogOperacao logOperacao)
        {

        }
        #endregion

        #region Eventos
        protected void OnTxtIdSistemaChange(string data)
        {
            logOperacao.SistemaId = string.Empty;
            logOperacao.SistemaNome = string.Empty;

            if (!int.TryParse(data, out int resultado))
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = "Código de sistema inválido." });
            }
            else
            {
                Sistema sistema = sistemas.Find(x => x.Id == resultado);
                if (sistema != null)
                {
                    logOperacao.SistemaId = Convert.ToString(sistema.Id);
                    logOperacao.SistemaNome = sistema.Nome;

                }
                else
                {
                    NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = "Sistema não existe." });
                }
                Recarregar();
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

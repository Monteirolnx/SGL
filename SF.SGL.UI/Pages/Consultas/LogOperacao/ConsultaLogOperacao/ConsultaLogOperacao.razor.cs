using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao
{
    public partial class ConsultaLogOperacao
    {
        protected LogOperacao logOperacao;
        protected List<Sistema> sistemas;
        protected ErroRetornoAPI erroRetornoAPI;

        EnumTipoRegistro tipoRegistro;
        List<EnumTipoRegistro> tipoRegistros;

        EnumSubTipoRegistro subTipoRegistro;
        List<EnumSubTipoRegistro> subTipoRegistros;

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
            MontarDropDown();
            await MontarMemoria();
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

        private void MontarDropDown()
        {
            tipoRegistro = EnumTipoRegistro.Selecione;
            tipoRegistros = Enum.GetValues(typeof(EnumTipoRegistro)).Cast<EnumTipoRegistro>().ToList();

            subTipoRegistro = EnumSubTipoRegistro.Selecione;
            subTipoRegistros = Enum.GetValues(typeof(EnumSubTipoRegistro)).Cast<EnumSubTipoRegistro>().ToList();
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

        protected async Task EnviarFormulario(LogOperacao logOperacao)
        {

        }

        protected async Task LimparConsulta()
        {
            await OnInitializedAsync();
        }
        #endregion

        #region Chamadas Api
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

        #region Enums
        public enum EnumTipoRegistro
        {
            [EnumMember(Value = "0")]
            [Display(Name = "Selecione")]
            [Description("Selecione")]
            Selecione = 0,

            [EnumMember(Value = "1")]
            [Display(Name = "Falha")]
            [Description("Falha")]
            FALHA = 1,

            [EnumMember(Value = "2")]
            [Display(Name = "Sucesso")]
            [Description("Sucesso")]
            SUCESSO = 2
        }

        public enum EnumSubTipoRegistro
        {
            [EnumMember(Value = "0")]
            [Display(Name = "Selecione")]
            [Description("Selecione")]
            Selecione = 0,

            [EnumMember(Value = "1")]
            [Display(Name = "Geral")]
            [Description("Geral")]
            GERAL = 1,

            [EnumMember(Value = "2")]
            [Display(Name = "Integração")]
            [Description("Integração")]
            INTEGRACAO = 2,

            [EnumMember(Value = "3")]
            [Display(Name = "Processo Batch - Aplicação")]
            [Description("Processo Batch - Aplicação")]
            PROCESSO_BATCH_APLICACAO = 3,

            [EnumMember(Value = "4")]
            [Display(Name = "Processo Batch - Banco de Dados")]
            [Description("Processo Batch - Banco de Dados")]
            PROCESSO_BATCH_BANCO_DE_DADOS = 4,
        }
        #endregion
    }
}

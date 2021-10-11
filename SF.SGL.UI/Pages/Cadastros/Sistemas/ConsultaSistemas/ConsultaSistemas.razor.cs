using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SF.SGL.UI.Pages.Cadastros.Sistemas.ConsultaSistemas
{
    public partial class ConsultaSistemas
    {
        protected List<Sistema> sistemas;
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
        protected RadzenDataGrid<Sistema> GridConsultaSistemas { get; set; }
        #endregion

        #region Métodos
        protected async override void OnInitialized()
        {
            await CargaInicial();
        }

        private async Task CargaInicial()
        {
            desabilitaAdicao = true;
            HttpResponseMessage httpResponseMessage = await ApiConsultaSistemas();
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
                sistemas = await httpResponseMessage.Content.ReadFromJsonAsync<List<Sistema>>();
                desabilitaAdicao = false;
            }
            StateHasChanged();
        }

        public void Recarregar()
        {
            InvokeAsync(StateHasChanged);
        }

        protected void NavegarPaginaAdicionarSistema()
        {
            NavigationManager.NavigateTo("Cadastros/Sistemas/adicionasistema");
        }
        #endregion

        #region Eventos
        protected void GridEditButtonClick(dynamic data)
        {
            NavigationManager.NavigateTo($"Cadastros/Sistemas/Editasistema/{data.Id}");
        }

        protected async Task GridDeleteButtonClick(dynamic data)
        {
            try
            {
                if (await DialogService.Confirm($"Deseja excluir o sistema: {data.Nome}?", title: "Confirma", new ConfirmOptions() { CancelButtonText = "Cancelar", OkButtonText = "Ok" }) == true)
                {
                    HttpResponseMessage response = await ApiDeletaSistema(data.Id);
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
                        sistemas.RemoveAll(x => x.Id == data.Id);

                        await GridConsultaSistemas.Reload();
                        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = $"Sucesso:", Detail = $"Sistema exluído com sucesso." });
                    }
                }
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Erro:", Detail = $"Não foi possível deletar o sistema." });
            }
        }
        #endregion

        #region Chamadas Api
        protected async Task<HttpResponseMessage> ApiConsultaSistemas()
        {
            try
            {
                string serviceEndpoint = "api/ConsultaSistemas/ConsultaTodos";
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.GetAsync(uriBuilder.Uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> ApiDeletaSistema(dynamic id)
        {
            try
            {
                string serviceEndpoint = $"api/DeletaSistema/Deleta/{id}";
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
        public record Sistema
        {
            public int Id { get; init; }

            public string Nome { get; init; }

            public string UrlServicoConsultaLog { get; init; }

            public string UsuarioLogin { get; init; }

            public string UsuarioSenha { get; init; }
        }

        public record ErroRetornoAPI
        {
            public string Message { get; set; }
        }
        #endregion
    }
}

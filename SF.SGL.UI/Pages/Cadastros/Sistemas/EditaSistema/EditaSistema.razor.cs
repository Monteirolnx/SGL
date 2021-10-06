using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Radzen;

namespace SF.SGL.UI.Pages.Cadastros.Sistemas.EditaSistema
{
    public partial class EditaSistema : ComponentBase
    {
        public Sistema Sistema { get; set; }

        public ErroRetornoAPI ErroRetornoAPI { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected IConfiguration Configuration { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            HttpResponseMessage httpResponseMessage = await APIObtemSistemaPorId(Id);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Sistema = await httpResponseMessage.Content.ReadFromJsonAsync<Sistema>();
                if (ErroRetornoAPI != null)
                {
                    ErroRetornoAPI.Message = string.Empty;
                }
            }
            else
            {
                ErroRetornoAPI = await httpResponseMessage.Content.ReadFromJsonAsync<ErroRetornoAPI>();
                Sistema = null;
            }
        }

        private async Task<HttpResponseMessage> APIObtemSistemaPorId(int id)
        {
            try
            {
                string serviceEndpoint = $"api/EditaSistema/ObtemSistemaPorId/{id}";
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.GetAsync(uriBuilder.Uri);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task FormSubmit()
        {
            HttpResponseMessage httpResponseMessage = await APIEditaSistema(Id, Sistema);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("cadastros/sistemas");
            }
            else
            {
                ErroRetornoAPI = await httpResponseMessage.Content.ReadFromJsonAsync<ErroRetornoAPI>();
                Sistema = null;
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = ErroRetornoAPI.Message });
            }
            StateHasChanged();
        }

        private async Task<HttpResponseMessage> APIEditaSistema(int id, Sistema sistema)
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

        protected void ButtonCancelClick(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("cadastros/sistemas");
        }
    }
    public class Sistema
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string UrlServicoConsultaLog { get; set; }

        public string UsuarioLogin { get; set; }

        public string UsuarioSenha { get; set; }
    }

    public class ErroRetornoAPI
    {
        public string Message { get; set; }
    }
}

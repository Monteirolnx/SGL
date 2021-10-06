using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Radzen;

namespace SF.SGL.UI.Pages.Cadastros.Sistemas.AdicionaSistema
{
    public partial class AdicionaSistema : ComponentBase
    {
        internal Sistema Sistema { get; set; }

        private ErroRetornoAPI MensagemErro { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        protected async Task Load()
        {
            await Task.FromResult(Sistema = new());
        }

        protected async Task FormSubmit(Sistema sistema)
        {
            HttpResponseMessage response = await APIAdicionaSistema();
            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("cadastros/sistemas");
            }
            else
            {
                MensagemErro = await response.Content.ReadFromJsonAsync<ErroRetornoAPI>();
                Sistema = null;
            }
            StateHasChanged();
        }

        protected async Task<HttpResponseMessage> APIAdicionaSistema()
        {
            try
            {
                string serviceEndpoint = "api/AdicionaSistema/Adiciona";
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.PostAsJsonAsync(uriBuilder.Uri, Sistema);
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

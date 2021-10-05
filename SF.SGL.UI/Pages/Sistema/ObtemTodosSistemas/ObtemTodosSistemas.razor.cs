﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;

namespace SF.SGL.UI.Pages.Sistema.ObtemTodosSistemas
{
    public partial class ObtemTodosSistemas : ComponentBase
    {
        private IEnumerable<Sistema> Sistemas { get; set; }

        private ErroRetornoAPI MensagemErro { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IConfiguration Configuration { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        protected async override void OnInitialized()
        {
            await Load();
        }

        private async Task Load()
        {
            string serviceEndpoint = "api/Sistemas/ObtemTodosSistemas";

            HttpResponseMessage response = await ChamarAPI(serviceEndpoint);
            if (response.IsSuccessStatusCode)
            {
                Sistemas = await response.Content.ReadFromJsonAsync<IEnumerable<Sistema>>();
                if (MensagemErro != null)
                    MensagemErro.Message = string.Empty;
            }
            else
            {
                MensagemErro = await response.Content.ReadFromJsonAsync<ErroRetornoAPI>();
                Sistemas = null;
            }
            StateHasChanged();
        }

        protected void Adicionar(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/sistemas/adiciona");
        }

        protected void GridEditButtonClick(dynamic data)
        {
            NavigationManager.NavigateTo($"/sistemas/edita/{data.Id}");
        }

        protected void GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {


        }

        protected async Task<HttpResponseMessage> ChamarAPI(string serviceEndpoint)
        {
            try
            {
                UriBuilder uriBuilder = new(string.Concat(Configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
                return await HttpClient.GetAsync(uriBuilder.Uri);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    internal record Sistema
    {
        public int Id { get; init; }

        public string Nome { get; init; }

        public string UrlServicoConsultaLog { get; init; }

        public string UsuarioLogin { get; init; }

        public string UsuarioSenha { get; init; }
    }

    internal class ErroRetornoAPI
    {
        public string Message { get; set; }
    }
}

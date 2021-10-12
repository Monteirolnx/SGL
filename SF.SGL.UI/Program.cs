using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace SF.SGL.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            builder.Services.AddSingleton<RadzenDataGrid<Pages.Cadastros.Sistemas.ConsultaSistemas.ConsultaSistemas.Sistema>>();
            builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogOperacao.ConsultaLogOperacao.ConsultaLogOperacao.Sistema>>();

            await builder.Build().RunAsync();
        }
    }
}

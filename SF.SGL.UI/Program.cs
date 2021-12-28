namespace SF.SGL.UI;

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
        builder.Services.AddScoped<ClipboardService>();

        #region Cadastros        
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Cadastros.Monitoramentos.ConsultaMonitoramentos.ConsultaMonitoramentos.Monitoramento>>();
        
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Cadastros.Sistemas.ConsultaSistemas.ConsultaSistemas.Sistema>>();
        #endregion

        #region Consultas
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogAuditoria.ConsultaLogAuditoria.ConsultaLogAuditoria.LogAuditoria>>();

        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogOperacao.ConsultaLogOperacao.ConsultaLogOperacao.LogOperacao>>();

        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogExecucaoMonitoramento.ConsultaLogExecucaoMonitoramento.ConsultaLogExecucaoMonitoramento.LogExecMonitoramento>>();
        #endregion

        await builder.Build().RunAsync();

    }
}

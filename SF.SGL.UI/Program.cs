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

        #region Sistemas
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Cadastros.Sistemas.ConsultaSistemas.ConsultaSistemas.Sistema>>();
        #endregion

        #region Consulta Log Auditoria
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogAuditoria.ConsultaLogAuditoria.ConsultaLogAuditoria.Sistema>>();
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogAuditoria.ConsultaLogAuditoria.ConsultaLogAuditoria.LogAuditoria>>();
        #endregion

        #region Consulta Log Operacao
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogOperacao.ConsultaLogOperacao.ConsultaLogOperacao.Sistema>>();
        builder.Services.AddSingleton<RadzenDataGrid<Pages.Consultas.LogOperacao.ConsultaLogOperacao.ConsultaLogOperacao.LogOperacao>>();
        #endregion

        await builder.Build().RunAsync();
    }
}

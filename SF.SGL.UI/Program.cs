
WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<ClipboardService>();

builder.Services
             .AddHttpClient(Constantes.HttpClients.Server)
             .ConfigureHttpClient(client =>
             {
                 client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/");
             })
         .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddOidcAuthentication(options =>
{
    Console.WriteLine(builder.Configuration["JWT"]);
    options.ProviderOptions.Authority = builder.Configuration["JWT_AUTHORITY"];
    options.ProviderOptions.ClientId = builder.Configuration["JWT_CLIENT_ID"];
    options.ProviderOptions.ResponseType = builder.Configuration["JWT_RESPONSE_TYPE"];
    options.ProviderOptions.ResponseMode = builder.Configuration["JWT_RESPONSE_MODE"];
    options.ProviderOptions.PostLogoutRedirectUri =
    new Uri($"{builder.HostEnvironment.BaseAddress}authentication/logged-out").ToString();
    options.UserOptions.RoleClaim = Constantes.ClaimsTypes.Role;
    options.UserOptions.NameClaim = Constantes.ClaimsTypes.Name;

})
    .AddAccountClaimsPrincipalFactory<CustomClaimsFactory>();

#region Cadastros        
builder.Services.AddScoped<RadzenDataGrid<SF.SGL.UI.Pages.Cadastros.Monitoramentos.ConsultaMonitoramentos.ConsultaMonitoramentos.Monitoramento>>();

builder.Services.AddScoped<RadzenDataGrid<SF.SGL.UI.Pages.Cadastros.Sistemas.ConsultaSistemas.ConsultaSistemas.Sistema>>();
#endregion

#region Consultas
builder.Services.AddScoped<RadzenDataGrid<SF.SGL.UI.Pages.Consultas.LogAuditoria.ConsultaLogAuditoria.ConsultaLogAuditoria.LogAuditoria>>();

builder.Services.AddScoped<RadzenDataGrid<SF.SGL.UI.Pages.Consultas.LogOperacao.ConsultaLogOperacao.ConsultaLogOperacao.LogOperacao>>();

builder.Services.AddScoped<RadzenDataGrid<SF.SGL.UI.Pages.Consultas.LogExecucaoMonitoramento.ConsultaLogExecucaoMonitoramento.ConsultaLogExecucaoMonitoramento.LogExecMonitoramento>>();
#endregion


await builder.Build().RunAsync();

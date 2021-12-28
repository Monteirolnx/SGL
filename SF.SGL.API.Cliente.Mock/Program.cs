// See https://aka.ms/new-console-template for more information
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using SF.SGL.API.Cliente.Mock.Funcionalidades.Cadastros.ExecucaoMonitoramento;
//using SF.SGL.API.Cliente.Mock.SignalR;

//using ServiceProvider serviceProvider = RegisterServices(args);
//IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

//static ServiceProvider RegisterServices(string[] args)
//{
//    IConfiguration configuration = SetupConfiguration(args);

//    var serviceCollection = new ServiceCollection();
//    serviceCollection.AddSingleton(configuration);

//    return serviceCollection.BuildServiceProvider();
//}

//static IConfiguration SetupConfiguration(string[] args)
//{
//    return new ConfigurationBuilder()
//        .SetBasePath(Directory.GetCurrentDirectory())
//        .AddJsonFile("appsettings.json")
//        .AddEnvironmentVariables()
//        .AddCommandLine(args)
//        .Build();
//}


using SF.SGL.API.Cliente.Mock.SignalR;

await MainClient.ExecuteAsync();

Console.ReadLine();

//Thread.Sleep(10000);
//ExecucaoMonitoramentoMock execucaoMonitoramentoMock = new(configuration);
//var x = execucaoMonitoramentoMock.ChamarApiExecucaoMonitoramentoMock();

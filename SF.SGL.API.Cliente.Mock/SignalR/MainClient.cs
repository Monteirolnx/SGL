using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace SF.SGL.API.Cliente.Mock.SignalR
{
  
    public class MainClient
    {
        public static async Task ExecuteAsync()
        {
            //Replace port "7054" with the port running the MainSignalServer project
            var uri = "https://localhost:44351/current-time";

            await using HubConnection? hubConnection = new HubConnectionBuilder().WithUrl(uri).Build();

            LogExecMonitoramento logExecMonitoramento = new();
            hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                logExecMonitoramento = JsonConvert.DeserializeObject<LogExecMonitoramento>(message);
            });

            await hubConnection.StartAsync();

            while (true)
            {
                if (logExecMonitoramento is not null && logExecMonitoramento.Id >0)
                {
                    Console.WriteLine(logExecMonitoramento.Id);
                    Console.WriteLine(logExecMonitoramento.Guid);
                    Console.WriteLine(logExecMonitoramento.SistemaNome);
                    Thread.Sleep(2000);
                }
                
            }
                        

          
        }

        public class LogExecMonitoramento
        {
            public long Id { get; set; }

            public Guid Guid { get; set; }

            public bool Status { get; set; }

            public DateTime Data { get; set; }

            public string Mensagem { get; set; }

            public int MonitoramentoId { get; set; }

            public string MonitoramentoNome { get; set; }

            public string SistemaNome { get; set; }
        }
    }
}

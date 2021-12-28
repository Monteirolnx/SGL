namespace SF.SGL.API.Hub;

using Microsoft.AspNetCore.SignalR;

public class SGLHub : Hub
{
    public async Task EnviarMensagem(string mensagem)
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("ExecucaoMonitoramento", mensagem);
        }
    } 
}

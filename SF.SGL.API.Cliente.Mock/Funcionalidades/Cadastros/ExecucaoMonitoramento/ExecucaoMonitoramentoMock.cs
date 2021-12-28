
//using Microsoft.Extensions.Configuration;

namespace SF.SGL.API.Cliente.Mock.Funcionalidades.Cadastros.ExecucaoMonitoramento;

//public class ExecucaoMonitoramentoMock
//{
//    private readonly IConfiguration _configuration;

//    public ExecucaoMonitoramentoMock(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public async Task<HttpResponseMessage> ChamarApiExecucaoMonitoramentoMock()
//    {
//        try
//        {
//            using HttpClient httpClient = new();
//            string serviceEndpoint = $"api/ExecucaoMonitoramento/Adiciona";
//            UriBuilder uriBuilder = new(string.Concat(_configuration["EnderecoBaseSGLAPI"], serviceEndpoint));
//            return await httpClient.PostAsJsonAsync(uriBuilder.Uri, "d");
//        }
//        catch (Exception)
//        {
//            throw;
//        }
//    }
//}

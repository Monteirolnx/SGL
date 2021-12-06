using SF.SGL.API.Funcionalidades.Cadastros.Sistemas.Excecoes;
using SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.Excecoes;
using SF.SGL.API.Funcionalidades.Consultas.LogOperacao.Excecoes;

namespace SF.SGL.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                FuncionalidadeSistemasException => (int)HttpStatusCode.BadRequest,
                FuncionalidadeLogAuditoriaException => (int)HttpStatusCode.BadRequest,
                FuncionalidadeLogOperacaoException => (int)HttpStatusCode.BadRequest,

                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            string result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}

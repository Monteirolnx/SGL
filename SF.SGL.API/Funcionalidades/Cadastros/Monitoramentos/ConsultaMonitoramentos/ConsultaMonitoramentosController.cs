namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.ConsultaMonitoramentos;

[ApiController,Route("api/[controller]")]
public class ConsultaMonitoramentosController : ControllerBase
{
    [HttpGet, Route("ConsultaTodos")]
    public async Task<IActionResult> ConsultaTodos([FromServices] IMediator mediator)
    {
        ConsultaMonitoramentos.Resultado resultado = await mediator.Send(new ConsultaMonitoramentos.Query());
        return Ok(resultado.Monitoramentos);
    }
}

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.ConsultaSistemas;

[ApiController ,Route("api/[controller]")]
public class ConsultaSistemasController : ControllerBase
{
    [HttpGet,Route("ConsultaTodos")]
    public async Task<IActionResult> ConsultaTodos([FromServices] IMediator mediator)
    {
        ConsultaSistemas.Resultado resultado = await mediator.Send(new ConsultaSistemas.Query());
        return Ok(resultado.Sistemas);
    }
}

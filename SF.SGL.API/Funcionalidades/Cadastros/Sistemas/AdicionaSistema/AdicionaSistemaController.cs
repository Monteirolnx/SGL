namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.AdicionaSistema;

[ApiController, Route("api/[controller]")]
public class AdicionaSistemaController : ControllerBase
{
    [HttpPost, Route("Adiciona"), ModelValidation]
    public async Task<IActionResult> Adiciona([FromServices] IMediator mediator, AdicionaSistema.Command command)
    {
        int iD = await mediator.Send(command);
        return Ok(iD);
    }
}

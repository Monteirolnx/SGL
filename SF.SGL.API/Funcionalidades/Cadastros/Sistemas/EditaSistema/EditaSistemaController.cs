namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.EditaSistema;

[ApiController, Route("api/[controller]")]
public class EditaSistemaController : ControllerBase
{
    [HttpGet,Route("ConsultaSistemaPorId/{id}")]
    public async Task<IActionResult> ConsultaSistemaPorId([FromServices] IMediator mediator,int id)
    {
        AuxConsultaSistemaPorId.Resultado resultado = await mediator.Send(new AuxConsultaSistemaPorId.Query() { Id = id });
        return Ok(resultado.Sistema);
    }

    [HttpPut, Route("Edita/{id}"), ModelValidation]
    public async Task<IActionResult> Edita([FromServices]IMediator mediator,int id, EditaSistema.Command command)
    {
        if (id != command.Id)
            return BadRequest();

        await mediator.Send(command);

        return Ok();
    }
}

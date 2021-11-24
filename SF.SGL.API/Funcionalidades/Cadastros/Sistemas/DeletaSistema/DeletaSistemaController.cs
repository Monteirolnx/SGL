namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.DeletaSistema;

[ApiController, Route("api/[controller]")]
public class DeletaSistemaController : ControllerBase
{
    [HttpDelete,Route("Deleta/{id}")]
    public async Task<IActionResult> Deleta([FromServices] IMediator mediator,int id)
    {
        DeletaSistema.Command deletaSistemaCommand = await mediator.Send(new DeletaSistema.Query { Id = id });
        await mediator.Send(deletaSistemaCommand);

        return Ok();
    }
}

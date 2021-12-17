namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.DeletaMonitoramento;

[ApiController, Route("api/[controller]")]
public class DeletaMonitoramentoController : ControllerBase
{
    [HttpDelete, Route("Deleta/{id}")]
    public async Task<IActionResult> Deleta([FromServices] IMediator mediator, int id)
    {
        DeletaMonitoramento.Command deletaSistemaCommand = await mediator.Send(new DeletaMonitoramento.Query { Id = id });
        await mediator.Send(deletaSistemaCommand);

        return Ok();
    }
}

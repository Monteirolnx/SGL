namespace SF.SGL.API.Funcionalidades.Cadastros.ExecucaoMonitoramento;

[ApiController, Route("api/[controller]")]
public class ExecucaoMonitoramentoController : ControllerBase
{
    [HttpPost, Route("Adiciona"), ModelValidation]
    public async Task<IActionResult> Adiciona([FromServices] IMediator mediator, ExecucaoMonitoramento.Command command)
    {
        long iD = await mediator.Send(command);

        if (command.Status == false)
        {
            // retorna 202
        }

        //return Created 201
        return CreatedAtAction(nameof(Adiciona), new { Guid = command.Guid }, iD);
    }
}

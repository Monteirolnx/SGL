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
            return Accepted();
        }

        return CreatedAtAction(nameof(Adiciona), new { command.Guid }, iD);
    }
}

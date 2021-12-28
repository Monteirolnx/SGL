namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.EditaMonitoramento;

[ApiController,Route("api/[controller]")]
public class EditaMonitoramentoController : ControllerBase
{
    [HttpGet, Route("ConsultaMonitoramentoPorId/{id}")]
    public async Task<IActionResult> ConsultaSistemaPorId([FromServices] IMediator mediator, int id)
    {
        AuxConsultaMonitoramentoPorId.Resultado resultado = await mediator.Send(new AuxConsultaMonitoramentoPorId.Query() { Id = id });
        return Ok(resultado.Monitoramento);
    }

    [HttpPut, Route("Edita/{id}"), ModelValidation]
    public async Task<IActionResult> Edita([FromServices] IMediator mediator, int id, EditaMonitoramento.Command command)
    {
        if (id != command.Id)
            return BadRequest();

        await mediator.Send(command);

        return Ok();
    }
}

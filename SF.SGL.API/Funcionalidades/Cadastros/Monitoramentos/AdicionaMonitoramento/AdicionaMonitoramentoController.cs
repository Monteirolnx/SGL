namespace SF.SGL.API.Funcionalidades.Cadastros.Monitoramentos.AdicionaMonitoramento;

[ApiController, Route("api/[controller]")]
public class AdicionaMonitoramentoController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemasCadMonit")]
    public async Task<IActionResult> AuxConsultaSistemasCadMonit([FromServices] IMediator mediator)
    {
        AuxConsultaSistemas.Resultado resultado = await mediator.Send(new AuxConsultaSistemas.Query());
        return Ok(resultado.Resultados);
    }

    [HttpPost, Route("Adiciona"), ModelValidation]
    public async Task<IActionResult> Adiciona([FromServices] IMediator mediator, AdicionaMonitoramento.Command command)
    {
        int iD = await mediator.Send(command);
        return Ok(iD);
    }
}

namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes;

[ApiController, Route("api/[controller]")]
public class ConsultaLogsOperacoesController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemas")]
    public async Task<IActionResult> AuxConsultaSistemas([FromServices] IMediator mediator)
    {
        AuxConsultaSistemas.Resultado resultado = await mediator.Send(new AuxConsultaSistemas.Query());
        return Ok(resultado.Resultados);
    }

    [HttpPost, Route("Consultar")]
    public async Task<IActionResult> Consultar([FromServices] IMediator mediator, ConsultaLogsOperacoes.Query query)
    {
        ConsultaLogsOperacoes.Resultado resultado = await mediator.Send(query);

        return Ok(resultado);
    }
}

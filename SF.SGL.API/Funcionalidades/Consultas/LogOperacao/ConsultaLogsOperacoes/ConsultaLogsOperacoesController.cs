namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes;

[ApiController, Route("api/[controller]")]
public class ConsultaLogsOperacoesController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemasLogOper")]
    public async Task<IActionResult> AuxConsultaSistemasLogOper([FromServices] IMediator mediator)
    {
        AuxConsultaSistemas.Resultado resultado = await mediator.Send(new AuxConsultaSistemas.Query());
        return Ok(resultado.Resultados);
    }

    [HttpPost, Route("ConsultaLogOper")]
    public async Task<IActionResult> ConsultaLogOper([FromServices] IMediator mediator, ConsultaLogsOperacoes.Query query)
    {
        ConsultaLogsOperacoes.RepostaConsultaLogOperacao repostaConsultaLogOperacao = await mediator.Send(query);

        return Ok(repostaConsultaLogOperacao);
    }
}

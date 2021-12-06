namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes;

[ApiController, Route("api/[controller]")]
public class ConsultaLogsOperacoesController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemasLogOper")]
    public async Task<IActionResult> AuxConsultaSistemasLogOper([FromServices] IMediator mediator)
    {
        AuxConsultaSistemasLogOper.Resultado resultado = await mediator.Send(new AuxConsultaSistemasLogOper.Query());
        return Ok(resultado.Resultados);
    }

    [HttpPost, Route("ConsultarLogOper")]
    public async Task<IActionResult> ConsultarLogOper([FromServices] IMediator mediator, ConsultaLogsOperacoes.Query query)
    {
        ConsultaLogsOperacoes.RepostaConsultaLogOperacao repostaConsultaLogOperacao = await mediator.Send(query);

        return Ok(repostaConsultaLogOperacao);
    }
}

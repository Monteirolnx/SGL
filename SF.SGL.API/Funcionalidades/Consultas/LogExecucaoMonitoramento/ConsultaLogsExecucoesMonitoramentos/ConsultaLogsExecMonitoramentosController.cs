namespace SF.SGL.API.Funcionalidades.Consultas.LogExecucaoMonitoramento.ConsultaLogsExecucoesMonitoramentos;

[ApiController, Route("api/[controller]")]
public class ConsultaLogsExecMonitoramentosController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemasLogExecMonitor")]
    public async Task<IActionResult> AuxConsultaSistemasLogExecMonitor([FromServices] IMediator mediator)
    {
        AuxConsultaSistemas.Resultado resultado = await mediator.Send(new AuxConsultaSistemas.Query());
        return Ok(resultado.Resultados);
    }

    /// <summary>
    /// Consulta Monitoramentos pelo Id do Sistema.
    /// </summary>
    /// <param name="sistemaId">Id do Sistema.</param>
    /// <returns>Lista de Monitoramentos vinculados ao Sistema.</returns>
    [HttpGet, Route("AuxConsultaMonitoramentosLogExecMonitor/{sistemaId}")]
    public async Task<IActionResult> AuxConsultaMonitoramentosLogExecMonitor([FromServices] IMediator mediator, int sistemaId)
    {
        AuxConsultaMonitorSistema.Resultado resultado = await mediator.Send(new AuxConsultaMonitorSistema.Query() { Id = sistemaId });
        return Ok(resultado.Monitoramentos);
    }

    [HttpPost, Route("ConsultaLogsExecMonitoramentos")]
    public async Task<IActionResult> ConsultaLogsExecMonitoramentos([FromServices] IMediator mediator, ConsultaLogsExecMonitoramentos.Query query)
    {
        ConsultaLogsExecMonitoramentos.RespostaConsultaLogExecMonitoramento respostaConsultaLogExecMonitoramento = await mediator.Send(query);

        return Ok(respostaConsultaLogExecMonitoramento);
    }
}

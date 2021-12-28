namespace SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.ConsultaLogsAuditoria;

[ApiController,Route("api/[controller]")]
public class ConsultaLogsAuditoriaController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemasLogAudit")]
    public async Task<IActionResult> AuxConsultaSistemasLogAudit([FromServices] IMediator mediator)
    {
        AuxConsultaSistemas.Resultado resultado = await mediator.Send(new AuxConsultaSistemas.Query());
        return Ok(resultado.Resultados);
    }

    [HttpPost, Route("ConsultaLogAudit")]
    public async Task<IActionResult> ConsultaLogAudit([FromServices] IMediator mediator, ConsultaLogsAuditoria.Query query)
    {
        ConsultaLogsAuditoria.RepostaConsultaLogAuditoria repostaConsultaLogAuditoria = await mediator.Send(query);

        return Ok(repostaConsultaLogAuditoria);
    }
}

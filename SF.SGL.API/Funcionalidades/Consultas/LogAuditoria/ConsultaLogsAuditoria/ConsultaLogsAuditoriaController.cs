namespace SF.SGL.API.Funcionalidades.Consultas.LogAuditoria.ConsultaLogsAuditoria;

[ApiController,Route("api/[controller]")]
public class ConsultaLogsAuditoriaController : ControllerBase
{
    [HttpGet, Route("AuxConsultaSistemasLogAudit")]
    public async Task<IActionResult> AuxConsultaSistemasLogAudit([FromServices] IMediator mediator)
    {
        AuxConsultaSistemasLogAudit.Resultado resultado = await mediator.Send(new AuxConsultaSistemasLogAudit.Query());
        return Ok(resultado.Resultados);
    }

    [HttpPost, Route("ConsultarLogAudit")]
    public async Task<IActionResult> ConsultarLogAudit([FromServices] IMediator mediator, ConsultaLogsAuditoria.Query query)
    {
        ConsultaLogsAuditoria.RepostaConsultaLogAuditoria repostaConsultaLogAuditoria = await mediator.Send(query);

        return Ok(repostaConsultaLogAuditoria);
    }
}

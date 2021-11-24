namespace SF.SGL.API.Funcionalidades.Parametizacoes.ExpurgoLogs.AdicionaParametrizacao;

[Route("api/[controller]")]
[ApiController]
public class AdicionaParametrizacaoController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdicionaParametrizacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("Adiciona")]
    [ModelValidation]
    public async Task<IActionResult> Adiciona(AdicionaParametrizacao.Command command)
    {
        int iD = await _mediator.Send(command);
        return Ok(iD);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SF.SGL.API.Funcionalidades.Consultas.LogOperacao.ConsultaLogsOperacoes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaLogsOperacoesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultaLogsOperacoesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("AuxConsultaSistemas")]
        public async Task<IActionResult> AuxConsultaSistemas()
        {
            AuxConsultaSistemas.Resultado resultado = await _mediator.Send(new AuxConsultaSistemas.Query());
            return Ok(resultado.Resultados);
        }
    }
}

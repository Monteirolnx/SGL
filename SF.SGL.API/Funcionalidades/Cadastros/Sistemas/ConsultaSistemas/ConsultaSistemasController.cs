using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.ConsultaSistemas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaSistemasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultaSistemasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ConsultaTodos")]
        public async Task<IActionResult> ConsultaTodos()
        {
            ConsultaSistemas.Resultado resultado = await _mediator.Send(new ConsultaSistemas.Query());
            return Ok(resultado.Resultados);
        }
    }
}

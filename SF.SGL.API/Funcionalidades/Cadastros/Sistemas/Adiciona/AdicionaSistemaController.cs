using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SF.SGL.API.Filtros;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.Adiciona
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdicionaSistemaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdicionaSistemaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Adiciona")]
        [ModelValidation]
        public async Task<IActionResult> Adiciona(Adiciona.Command command)
        {
            int iD = await _mediator.Send(command);
            return Ok(iD);
        }
    }
}

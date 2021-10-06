using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.DeletaSistema
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeletaSistemaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeletaSistemaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        [Route("Deleta/{id}")]
        public async Task<IActionResult> Deleta(int id)
        {
            DeletaSistema.Command command = await _mediator.Send(new DeletaSistema.Query { Id = id });
            await _mediator.Send(command);

            return Ok();
        }
    }
}

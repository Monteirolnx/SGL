using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.ObtemTodosSistemas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObtemTodosSistemasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ObtemTodosSistemasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ObtemTodos")]
        public async Task<IActionResult> ObtemTodos()
        {
            ObtemTodosSistemas.Resultado resultado = await _mediator.Send(new ObtemTodosSistemas.Query());
            return Ok(resultado.Resultados);
        }
    }
}

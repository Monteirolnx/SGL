using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SF.SGL.API.Filtros;

namespace SF.SGL.API.Funcionalidades.Sistemas
{
    [Route("api/[controller]")]
    [ApiController]
    public class SistemasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SistemasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("AdicionaSistema")]
        [ModelValidation]
        public async Task<IActionResult> AdicionaSistema(AdicionaSistema.Command command)
        {
            int sistemaId = await _mediator.Send(command);
            return RedirectToAction(nameof(ObtemSistemaPorId), new { id = sistemaId });
        }

        [HttpDelete]
        [Route("DeletaSistema/{id}")]
        public async Task<IActionResult> DeletaSistema(int id)
        {
            DeletaSistema.Command command = await _mediator.Send(new DeletaSistema.Query { Id = id });
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPut]
        [Route("EditaSistema/{id}")]
        [ModelValidation]
        public async Task<IActionResult> EditaSistema(int id, EditaSistema.Command command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(new EditaSistema.Query() { Id = id });
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("ObtemTodosSistemas")]
        public async Task<IActionResult> ObtemTodosSistemas()
        {
            ObtemTodosSistemas.Result resultado = await _mediator.Send(new ObtemTodosSistemas.Query());
            return Ok(resultado.Resultados);
        }

        [HttpGet]
        [Route("ObtemTodosSistemasPaginado")]
        public async Task<IActionResult> ObtemTodosSistemasPaginado(string sortOrder,
            string currentFilter, string palavraChave, int? numeroPagina, int tamanhoPagina)
        {
            ObtemTodosSistemasPaginado.Result resultado = 
                await _mediator.Send(new ObtemTodosSistemasPaginado.Query 
                { CurrentFilter = currentFilter, NumeroPagina = numeroPagina, PalavraChave = palavraChave, SortOrder = sortOrder, TamanhoPagina = tamanhoPagina });
            return Ok(resultado);
        }

        [HttpGet]
        [Route("ObtemSistemaPorId/{id}")]
        public async Task<IActionResult> ObtemSistemaPorId(int id)
        {
            ObtemSistemaPorId.Model resultado = await _mediator.Send(new ObtemSistemaPorId.Query() { Id = id });
            return Ok(resultado);
        }
    }
}

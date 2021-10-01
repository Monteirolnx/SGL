using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SF.SGL.API.Filtros;
using SF.SGL.API.Funcionalidades.Sistemas.AdicionaSistema;
using SF.SGL.API.Funcionalidades.Sistemas.DeletaSistema;
using SF.SGL.API.Funcionalidades.Sistemas.EditaSistema;
using SF.SGL.API.Funcionalidades.Sistemas.ObtemSistemaPorId;
using SF.SGL.API.Funcionalidades.Sistemas.ObtemTodosSistemas;

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
        public async Task<IActionResult> AdicionaSistema(AdicionaSistemaMediator.Command command)
        {
            int sistemaId = await _mediator.Send(command);
            return RedirectToAction(nameof(ObtemSistemaPorId), new { id = sistemaId });
        }

        [HttpDelete]
        [Route("DeletaSistema/{id}")]
        public async Task<IActionResult> DeletaSistema(int id)
        {
            DeletaSistemaMediator.Command command = await _mediator.Send(new DeletaSistemaMediator.Query { Id = id });
            await _mediator.Send(command);

            return Ok();
        }

        [HttpPut]
        [Route("EditaSistema/{id}")]
        [ModelValidation]
        public async Task<IActionResult> EditaSistema(int id, EditaSistemaMediator.Command command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(new EditaSistemaMediator.Query() { Id = id });
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("ObtemTodosSistemas")]
        public async Task<IActionResult> ObtemTodosSistemas()
        {
            ObtemTodosSistemasMediator.Result resultado = await _mediator.Send(new ObtemTodosSistemasMediator.Query());
            return Ok(resultado.Resultados);
        }

        [HttpGet]
        [Route("ObtemSistemaPorId/{id}")]
        public async Task<IActionResult> ObtemSistemaPorId(int id)
        {
            ObtemSistemaPorIdMediator.Model resultado = await _mediator.Send(new ObtemSistemaPorIdMediator.Query() { Id = id });
            return Ok(resultado);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema;
using SF_SGL_API.Funcionalidades.Sistema.DeletaSistema;
using SF_SGL_API.Funcionalidades.Sistema.EditaSistema;
using SF_SGL_API.Funcionalidades.Sistema.ObtemSistemaPorId;
using SF_SGL_API.Funcionalidades.Sistema.ObtemTodosSistemas;
using static SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema.AdicionaSistemaMediator;
using static SF_SGL_API.Funcionalidades.Sistema.DeletaSistema.DeletaSistemaMediator;
using static SF_SGL_API.Funcionalidades.Sistema.EditaSistema.EditaSistemaMediator;
using static SF_SGL_API.Funcionalidades.Sistema.ExisteSistema.ExisteSistemaMediator;
using static SF_SGL_API.Funcionalidades.Sistema.ObtemSistemaPorId.ObtemSistemaPorIdMediator;
using static SF_SGL_API.Funcionalidades.Sistema.ObtemTodosSistemas.ObtemTodosSistemasMediator;

namespace SF_SGL_API.Funcionalidades.Sistema
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
        [Route("api/AdicionaSistema")]
        public async Task<ActionResult> AdicionaSistema(AdicionaSistemaInputModel adicionaSistemaInputModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Task<int> tarefaAdicionaSistema = Task.FromResult(await _mediator
                .Send(new AdicionaSistemaCommand(adicionaSistemaInputModel)));

            if (!tarefaAdicionaSistema.IsCompletedSuccessfully)
                return Problem();

            return RedirectToAction(nameof(ObtemSistemaPorId), new { id = tarefaAdicionaSistema.Result });
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/EditaSistema/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditaSistema(int id, EditaSistemaInputModel editaSistemaInputModel)
        {
            try
            {
                if (id != editaSistemaInputModel.Id)
                    return BadRequest();

                Task<EditaSistemaInputModel> tarefaPesquisaId = Task.FromResult(await _mediator.Send(new EditaSistemaQuery(id)));
                if (!tarefaPesquisaId.IsCompletedSuccessfully)
                    return Problem();
                if (tarefaPesquisaId.Result == null)
                    return NotFound();

                Task<Unit> tarefaEditaSistema = Task.FromResult(await _mediator.Send(new EditaSistemaCommand(editaSistemaInputModel)));
                if (!tarefaEditaSistema.IsCompletedSuccessfully)
                    return Problem();

                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SistemaExiste(id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpDelete]
        [Route("api/DeletaSistema/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletaSistema(int id)
        {
            Task<DeletaSistemaViewModel> tarefaPesquisaId = Task.FromResult(await _mediator.Send(new DeletaSistemaQuery(id)));
            if (!tarefaPesquisaId.IsCompletedSuccessfully)
                return Problem();
            if (tarefaPesquisaId.Result == null)
                return NotFound();

            var tarefaDeletaSistema = Task.FromResult(await _mediator.Send(new DeletaSistemaCommand(tarefaPesquisaId.Result)));
            if (!tarefaDeletaSistema.IsCompletedSuccessfully)
                return Problem();

            return Ok();
        }

        [HttpGet]
        [Route("api/ObtemTodosSistemas")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ObtemTodosSistemasViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtemTodosSistemas()
        {
            Task<IEnumerable<ObtemTodosSistemasViewModel>> tarefaObtemTodosSistemas = Task.FromResult(await _mediator.Send(new ObtemTodosSistemasQuery()));
            if (!tarefaObtemTodosSistemas.IsCompletedSuccessfully)
                return Problem();
            if (!tarefaObtemTodosSistemas.Result.Any())
                return NotFound();

            return Ok(tarefaObtemTodosSistemas.Result);
        }

        [HttpGet]
        [Route("api/ObtemSistemaPorId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ObtemSistemaPorIdViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtemSistemaPorId(int id)
        {
            Task<ObtemSistemaPorIdViewModel> tarefaObtemSistemasPorId = Task.FromResult(await _mediator.Send(new ObtemSistemaPorIdQuery(id)));
            if (!tarefaObtemSistemasPorId.IsCompletedSuccessfully)
                return Problem();
            if (tarefaObtemSistemasPorId.Result == null)
                return NotFound();

            return Ok(tarefaObtemSistemasPorId.Result);
        }

        private bool SistemaExiste(int id)
        {
            return _mediator.Send(new ExisteSistemaQuery(id)).Result;
        }
    }
}

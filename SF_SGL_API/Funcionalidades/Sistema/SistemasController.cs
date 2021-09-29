using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SF_SGL_API.Funcionalidades.Sistema.AdicionaSistema;
using SF_SGL_API.Funcionalidades.Sistema.DeletaSistema;
using SF_SGL_API.Funcionalidades.Sistema.EditaSistema;
using SF_SGL_API.Funcionalidades.Sistema.ExisteSistema;
using SF_SGL_API.Funcionalidades.Sistema.ObtemSistemaPorId;
using SF_SGL_API.Funcionalidades.Sistema.ObtemTodosSistemas;

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

        // GET: api/Sistemas
        [HttpGet]
        public async Task<IEnumerable<ObtemTodosSistemasModelo>> GetSistema()
        {
            return await _mediator.Send(new ObtemTodosSistemasQuery());
        }

        // GET: api/Sistemas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObtemSistemaPorIdModelo>> GetSistema(int id)
        {
            ObtemSistemaPorIdModelo sistema = await _mediator.Send(new ObtemSistemaPorIdQuery(id));

            if (sistema == null)
                return NotFound();

            return sistema;
        }

        // PUT: api/Sistemas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSistema(int id, EditaSistemaModelo editaSistemaModelo)
        {
            try
            {
                if (id != editaSistemaModelo.Id)
                    return BadRequest();

                EditaSistemaModelo sistema = await _mediator.Send(new EditaSistemaQuery(id));
                if (sistema == null)
                    return NotFound();

                Task<Unit> tarefa = Task.FromResult(await _mediator.Send(editaSistemaModelo));
                if (!tarefa.IsCompletedSuccessfully)
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

        // POST: api/Sistemas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdicionaSistemaModelo>> PostSistema(AdicionaSistemaModelo adicionaSistemaModelo)
        {
            int idSistema = await _mediator.Send(adicionaSistemaModelo);

            return CreatedAtAction("GetSistema", new { id = idSistema });
        }

        // DELETE: api/Sistemas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSistema(int id)
        {
            DeletaSistemaModelo sistema = await _mediator.Send(new DeletaSistemaQuery(id));
            if (sistema == null)
                return NotFound();

            Task<Unit> tarefa = Task.FromResult(await _mediator.Send(sistema));
            if (!tarefa.IsCompletedSuccessfully)
                return Problem();

            return Ok();
        }

        private bool SistemaExiste(int id)
        {
            return _mediator.Send(new ExisteSistemaQuery(id)).Result;
        }
    }
}

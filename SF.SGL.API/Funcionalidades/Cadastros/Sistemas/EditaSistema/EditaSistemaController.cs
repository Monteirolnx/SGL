﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SF.SGL.API.Filtros;

namespace SF.SGL.API.Funcionalidades.Cadastros.Sistemas.EditaSistema
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditaSistemaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EditaSistemaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ConsultaSistemaPorId/{id}")]
        public async Task<IActionResult> ConsultaSistemaPorId(int id)
        {
            ConsultaSistemaPorId.Command resultado = await _mediator.Send(new ConsultaSistemaPorId.Query() { Id = id });
            return Ok(resultado);
        }

        [HttpPut]
        [Route("Edita/{id}")]
        [ModelValidation]
        public async Task<IActionResult> Edita(int id, EditaSistema.Command command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);

            return Ok();
        }

    }
}

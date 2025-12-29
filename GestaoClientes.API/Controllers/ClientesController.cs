using System;
using System.Threading.Tasks;
using GestaoClientes.Application.Features.Clientes.Commands.CriaCliente;
using GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId;
using GestaoClientes.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestaoClientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriaClienteCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(ObterPorId), new { id = id }, new { id = id });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var cliente = await _mediator.Send(new ObtemClientePorIdQuery(id));

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }
    }
}

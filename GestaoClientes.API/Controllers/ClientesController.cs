using System;
using System.Threading.Tasks;
using GestaoClientes.Application.Features.Clientes.Commands.CriaCliente;
using GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId;
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
            var id = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(ObterPorId),
                new { id },
                new { id }
            );
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClienteDto>> ObterPorId(Guid id)
        {
            var query = new ObtemClientePorIdQuery(id);
            var cliente = await _mediator.Send(query);

            if (cliente is null)
                return NotFound();

            return Ok(cliente);
        }
    }
}

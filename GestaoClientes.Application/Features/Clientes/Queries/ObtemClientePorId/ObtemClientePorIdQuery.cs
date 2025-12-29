using System;
using MediatR;

namespace GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId
{
    public class ObtemClientePorIdQuery : IRequest<ClienteDto?>
    {
        public Guid Id { get; set; }

        public ObtemClientePorIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

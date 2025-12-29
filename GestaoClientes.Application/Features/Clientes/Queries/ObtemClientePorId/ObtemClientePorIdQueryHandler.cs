using System.Threading;
using System.Threading.Tasks;
using GestaoClientes.Domain.Clientes;
using MediatR;

namespace GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId
{
    public class ObtemClientePorIdQueryHandler : IRequestHandler<ObtemClientePorIdQuery, ClienteDto?>
    {
        private readonly IClienteRepository _clienteRepository;

        public ObtemClientePorIdQueryHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteDto?> Handle(ObtemClientePorIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.ObterPorId(request.Id);

            if (cliente == null)
                return null;

            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Cnpj = cliente.Cnpj.Valor,
                Ativo = cliente.Ativo,
                DataCadastro = cliente.DataCadastro
            };
        }
    }
}

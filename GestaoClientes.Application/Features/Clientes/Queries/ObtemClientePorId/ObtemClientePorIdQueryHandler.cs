using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GestaoClientes.Domain.Clientes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId
{
    public class ObtemClientePorIdQueryHandler : IRequestHandler<ObtemClientePorIdQuery, ClienteDto?>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ObtemClientePorIdQueryHandler> _logger;

        public ObtemClientePorIdQueryHandler(IClienteRepository clienteRepository, IMapper mapper, ILogger<ObtemClientePorIdQueryHandler> logger)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ClienteDto?> Handle(ObtemClientePorIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando cliente por ID: {Id}", request.Id);

            var cliente = await _clienteRepository.ObterPorId(request.Id, cancellationToken);

            if (cliente == null)
            {
                _logger.LogWarning("Cliente não encontrado. ID: {Id}", request.Id);
                return null;
            }

            _logger.LogInformation("Cliente encontrado. ID: {Id}", request.Id);
            return _mapper.Map<ClienteDto>(cliente);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Common;
using GestaoClientes.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GestaoClientes.Application.Features.Clientes.Commands.CriaCliente
{
    public class CriaClienteCommandHandler : IRequestHandler<CriaClienteCommand, Guid>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CriaClienteCommandHandler> _logger;

        public CriaClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork, ILogger<CriaClienteCommandHandler> logger)
        {
            _clienteRepository = clienteRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(CriaClienteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando criação de cliente. CNPJ: {Cnpj}", request.Cnpj);

            var cnpj = new Cnpj(request.Cnpj);

            if (await _clienteRepository.ExisteCnpj(cnpj, cancellationToken))
            {
                _logger.LogWarning("Tentativa de criação de cliente com CNPJ já existente: {Cnpj}", request.Cnpj);
                throw new DomainException($"O CNPJ {request.Cnpj} já está cadastrado.");
            }

            var cliente = new Cliente(request.Nome, request.Email, cnpj);

            await _clienteRepository.Adicionar(cliente, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Cliente criado com sucesso. ID: {Id}", cliente.Id);

            return cliente.Id;
        }
    }
}

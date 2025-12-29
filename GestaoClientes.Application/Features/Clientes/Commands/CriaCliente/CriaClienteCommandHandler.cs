using System;
using System.Threading;
using System.Threading.Tasks;
using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Common;
using GestaoClientes.Domain.Exceptions;
using MediatR;

namespace GestaoClientes.Application.Features.Clientes.Commands.CriaCliente
{
    public class CriaClienteCommandHandler : IRequestHandler<CriaClienteCommand, Guid>
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CriaClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
        {
            _clienteRepository = clienteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CriaClienteCommand request, CancellationToken cancellationToken)
        {
            // Validação de VO acontece na construção do objeto
            var cnpj = new Cnpj(request.Cnpj);

            // Validação de regra de negócio (unicidade)
            if (await _clienteRepository.ExisteCnpj(cnpj))
            {
                throw new DomainException($"O CNPJ {request.Cnpj} já está cadastrado.");
            }

            // Criação da entidade
            var cliente = new Cliente(request.Nome, request.Email, request.Cnpj);

            // Persistência
            await _clienteRepository.Adicionar(cliente);
            
            // Commit da transação
            await _unitOfWork.CommitAsync(cancellationToken);

            return cliente.Id;
        }
    }
}

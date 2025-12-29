using FluentAssertions;
using GestaoClientes.Application.Features.Clientes.Commands.CriaCliente;
using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Common;
using GestaoClientes.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GestaoClientes.Tests.Features.Clientes.Commands.CriaCliente
{
    public class CriaClienteCommandHandlerTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CriaClienteCommandHandler>> _loggerMock;
        private readonly CriaClienteCommandHandler _handler;

        public CriaClienteCommandHandlerTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CriaClienteCommandHandler>>();

            _handler = new CriaClienteCommandHandler(
                _clienteRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Criar_Cliente_Com_Sucesso()
        {
            // Arrange
            var command = new CriaClienteCommand
            {
                Nome = "Empresa Y",
                Email = "contato@empresay.com",
                Cnpj = "11.222.333/0001-81"
            };

            _clienteRepositoryMock.Setup(r => r.ExisteCnpj(It.IsAny<Cnpj>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            _clienteRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_DomainException_Quando_Cnpj_Ja_Existe()
        {
            // Arrange
            var command = new CriaClienteCommand
            {
                Nome = "Empresa Z",
                Email = "contato@empresaz.com",
                Cnpj = "11.222.333/0001-81"
            };

            _clienteRepositoryMock.Setup(r => r.ExisteCnpj(It.IsAny<Cnpj>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage($"O CNPJ {command.Cnpj} já está cadastrado.");
            
            _clienteRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Deve_Lancar_DomainException_Quando_Dados_Essenciais_Forem_Invalidos()
        {
            // Arrange
            var command = new CriaClienteCommand
            {
                Nome = "",
                Email = "contato@empresaw.com",
                Cnpj = "11.222.333/0001-81"
            };

            _clienteRepositoryMock.Setup(r => r.ExisteCnpj(It.IsAny<Cnpj>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage("O nome é obrigatório.");

            _clienteRepositoryMock.Verify(r => r.Adicionar(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
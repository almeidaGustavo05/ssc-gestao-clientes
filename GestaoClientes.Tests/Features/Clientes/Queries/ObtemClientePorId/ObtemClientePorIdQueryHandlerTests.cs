using AutoMapper;
using FluentAssertions;
using GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId;
using GestaoClientes.Domain.Clientes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GestaoClientes.Tests.Features.Clientes.Queries.ObtemClientePorId
{
    public class ObtemClientePorIdQueryHandlerTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ObtemClientePorIdQueryHandler>> _loggerMock;
        private readonly ObtemClientePorIdQueryHandler _handler;

        public ObtemClientePorIdQueryHandlerTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ObtemClientePorIdQueryHandler>>();

            _handler = new ObtemClientePorIdQueryHandler(
                _clienteRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_ClienteDto_Quando_Cliente_Existir()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var query = new ObtemClientePorIdQuery(clienteId);
            var cliente = new Cliente("Empresa X", "contato@empresa.com", new Cnpj("11.222.333/0001-81"));
            var clienteDto = new ClienteDto { Id = clienteId, Nome = "Empresa X" };

            _clienteRepositoryMock.Setup(r => r.ObterPorId(clienteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cliente);

            _mapperMock.Setup(m => m.Map<ClienteDto>(cliente))
                .Returns(clienteDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(clienteDto);
            _clienteRepositoryMock.Verify(r => r.ObterPorId(clienteId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Null_Quando_Cliente_Nao_Existir()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var query = new ObtemClientePorIdQuery(clienteId);

            _clienteRepositoryMock.Setup(r => r.ObterPorId(clienteId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cliente?)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _clienteRepositoryMock.Verify(r => r.ObterPorId(clienteId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(m => m.Map<ClienteDto>(It.IsAny<Cliente>()), Times.Never);
        }
    }
}
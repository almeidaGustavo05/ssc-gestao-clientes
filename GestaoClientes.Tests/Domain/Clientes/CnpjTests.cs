using FluentAssertions;
using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Exceptions;
using Xunit;

namespace GestaoClientes.Tests.Domain.Clientes
{
    public class CnpjTests
    {
        [Fact]
        public void Deve_Criar_Cnpj_Valido()
        {
            // Arrange
            var cnpjValido = "11.222.333/0001-81";

            // Act
            var cnpj = new Cnpj(cnpjValido);

            // Assert
            cnpj.Valor.Should().Be("11222333000181");
        }

        [Theory]
        [InlineData("11.222.333/0001-00")]
        [InlineData("00.000.000/0000-00")]
        [InlineData("123")]
        [InlineData("")]
        [InlineData(null)]
        public void Deve_Lancar_DomainException_Para_Cnpj_Invalido(string? cnpjInvalido)
        {
            // Act
            Action act = () => new Cnpj(cnpjInvalido!);

            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("CNPJ inválido");
        }
    }
}
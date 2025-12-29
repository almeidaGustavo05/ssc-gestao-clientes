using System;

namespace GestaoClientes.Application.Features.Clientes.Queries.ObtemClientePorId
{
    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}

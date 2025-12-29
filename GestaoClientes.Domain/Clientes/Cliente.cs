using GestaoClientes.Domain.Common;
using GestaoClientes.Domain.Exceptions;

namespace GestaoClientes.Domain.Clientes
{
    public class Cliente : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public Cnpj Cnpj { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }

        protected Cliente() 
        {
            Nome = null!;
            Email = null!;
            Cnpj = null!;
        }

        public Cliente(string nome, string email, string cnpj)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("O e-mail é obrigatório.");

            Nome = nome;
            Email = email;
            Cnpj = new Cnpj(cnpj);
            Ativo = true;
            DataCadastro = DateTime.UtcNow;
        }

        public void AtualizarDados(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("O e-mail é obrigatório.");

            Nome = nome;
            Email = email;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void Ativar()
        {
            Ativo = true;
        }
    }
}

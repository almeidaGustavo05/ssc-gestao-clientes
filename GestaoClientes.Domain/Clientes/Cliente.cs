using GestaoClientes.Domain.Common;
using GestaoClientes.Domain.Exceptions;

namespace GestaoClientes.Domain.Clientes
{
    public class Cliente : Entity, IAggregateRoot
    {
        public virtual string Nome { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual Cnpj Cnpj { get; protected set; }
        public virtual bool Ativo { get; protected set; }
        public virtual DateTime DataCadastro { get; protected set; }

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

        public virtual void AtualizarDados(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("O e-mail é obrigatório.");

            Nome = nome;
            Email = email;
        }

        public virtual void Desativar()
        {
            Ativo = false;
        }

        public virtual void Ativar()
        {
            Ativo = true;
        }
    }
}

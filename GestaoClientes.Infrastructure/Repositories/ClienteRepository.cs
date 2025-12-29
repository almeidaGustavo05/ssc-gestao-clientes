using System;
using System.Threading;
using System.Threading.Tasks;
using GestaoClientes.Domain.Clientes;
using GestaoClientes.Domain.Common;
using NHibernate;
using NHibernate.Linq;

namespace GestaoClientes.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ISession _session;

        public ClienteRepository(ISession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public async Task Adicionar(Cliente cliente, CancellationToken cancellationToken)
        {
            await _session.SaveAsync(cliente, cancellationToken);
        }

        public async Task<Cliente?> ObterPorId(Guid id, CancellationToken cancellationToken)
        {
            return await _session.GetAsync<Cliente>(id, cancellationToken);
        }

        public async Task<Cliente?> ObterPorCnpj(Cnpj cnpj, CancellationToken cancellationToken)
        {
            return await _session.Query<Cliente>()
                                 .FirstOrDefaultAsync(c => c.Cnpj.Valor == cnpj.Valor, cancellationToken);
        }

        public async Task<bool> ExisteCnpj(Cnpj cnpj, CancellationToken cancellationToken)
        {
             return await _session.Query<Cliente>()
                                  .AnyAsync(c => c.Cnpj.Valor == cnpj.Valor, cancellationToken);
        }
    }
}

using System;
using System.Threading.Tasks;
using GestaoClientes.Domain.Common;

namespace GestaoClientes.Domain.Clientes
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente?> ObterPorId(Guid id, CancellationToken cancellationToken);
        Task<Cliente?> ObterPorCnpj(Cnpj cnpj, CancellationToken cancellationToken);
        Task<bool> ExisteCnpj(Cnpj cnpj, CancellationToken cancellationToken);
    }
}

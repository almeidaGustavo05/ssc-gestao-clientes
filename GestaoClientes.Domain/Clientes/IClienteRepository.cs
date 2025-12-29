using System;
using System.Threading.Tasks;
using GestaoClientes.Domain.Common;

namespace GestaoClientes.Domain.Clientes
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente?> ObterPorId(Guid id);
        Task<Cliente?> ObterPorCnpj(Cnpj cnpj);
        Task<bool> ExisteCnpj(Cnpj cnpj);
    }
}

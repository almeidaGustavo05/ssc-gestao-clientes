using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoClientes.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace GestaoClientes.Domain.Common
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task Adicionar(T entity, CancellationToken cancellationToken);
    }
}

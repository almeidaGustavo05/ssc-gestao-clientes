using GestaoClientes.Domain.Common;
using NHibernate;

namespace GestaoClientes.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;

        public UnitOfWork(ISession session)
        {
            _session = session;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            using (var transaction = _session.BeginTransaction())
            {
                await _session.FlushAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(
            CancellationToken ct = default);

        Task<T> ExecuteInTransactionAsync<T>(
            Func<CancellationToken, Task<T>> operation,
            CancellationToken ct = default);
    }
}

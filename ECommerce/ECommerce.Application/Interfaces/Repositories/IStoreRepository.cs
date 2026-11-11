using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<Store?> GetByManagerIdAsync(
            int managerId,
            CancellationToken ct = default);
    }
}

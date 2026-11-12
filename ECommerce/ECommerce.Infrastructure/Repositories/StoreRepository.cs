using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class StoreRepository
        : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Store?> GetByManagerIdAsync(
            int managerId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(
                    s => s.ManagerId == managerId,
                    ct);
        }
    }
}

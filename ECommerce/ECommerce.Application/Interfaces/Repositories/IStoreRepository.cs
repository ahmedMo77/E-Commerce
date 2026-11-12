using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<Store?> GetByManagerIdAsync(
            int managerId,
            CancellationToken ct = default);
    }
}

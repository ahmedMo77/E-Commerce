using ECommerce.Application.DTOs.Store;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IStoreService
    {
        Task<StoreDto> CreateStoreAsync(
            int managerId,
            CreateStoreDto dto,
            CancellationToken ct = default);

        Task<StoreDto?> GetMyStoreAsync(
            int managerId,
            CancellationToken ct = default);

        Task<StoreDto> UpdateStoreAsync(
            int managerId,
            UpdateStoreDto dto,
            CancellationToken ct = default);
    }
}

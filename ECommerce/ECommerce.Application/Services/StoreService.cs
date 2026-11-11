using AutoMapper;
using ECommerce.Application.DTOs.Store;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StoreService(
            IStoreRepository storeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _storeRepository = storeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StoreDto> CreateStoreAsync(
            int managerId,
            CreateStoreDto dto,
            CancellationToken ct = default)
        {
            var existingStore = await _storeRepository
                .GetByManagerIdAsync(managerId, ct);

            if (existingStore is not null)
                throw new InvalidOperationException(
                    "You already have a store.");

            var store = _mapper.Map<Store>(dto);

            store.ManagerId = managerId;

            await _storeRepository.AddAsync(store, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<StoreDto>(store);
        }

        public async Task<StoreDto?> GetMyStoreAsync(
            int managerId,
            CancellationToken ct = default)
        {
            var store = await _storeRepository
                .GetByManagerIdAsync(managerId, ct);

            return store is null
                ? null
                : _mapper.Map<StoreDto>(store);
        }

        public async Task<StoreDto> UpdateStoreAsync(
            int managerId,
            UpdateStoreDto dto,
            CancellationToken ct = default)
        {
            var store = await _storeRepository
                .GetByManagerIdAsync(managerId, ct);

            if (store is null)
                throw new KeyNotFoundException("Store not found.");

            _mapper.Map(dto, store);

            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<StoreDto>(store);
        }
    }
}

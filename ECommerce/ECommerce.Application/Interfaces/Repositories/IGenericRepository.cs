using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(
            int id, 
            CancellationToken ct = default);

        Task<IEnumerable<T>> GetAllAsync(
            CancellationToken ct = default);

        Task AddAsync(
            T entity, 
            CancellationToken ct = default);

        void Update(T entity);
        void Delete(T entity);
    }
}

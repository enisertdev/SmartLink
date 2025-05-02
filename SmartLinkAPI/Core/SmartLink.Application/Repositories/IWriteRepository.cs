using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLink.Domain.Entities.Common;

namespace SmartLink.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        bool RemoveAsync(T entity);
        bool RemoveRangeAsync(IEnumerable<T> entities);
        bool UpdateAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}
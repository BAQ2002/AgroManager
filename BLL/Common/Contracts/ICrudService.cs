using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Common.Contracts
{
   
    public interface ICrudService<TEntity>
    {
      
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);
       
        Task DeleteAsync(Guid id, CancellationToken ct = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}

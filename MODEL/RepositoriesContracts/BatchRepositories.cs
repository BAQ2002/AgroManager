using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBatchRepository<TBatch>
        where TBatch : BatchEntity
    {
        Task AddAsync(TBatch entity, CancellationToken ct = default);
        Task DeleteAsync(TBatch entity, CancellationToken ct = default);
        Task<TBatch?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
    public interface IBovinePastureBatchRepository : IBatchRepository<BovinePastureBatch>
    {
        Task<BovinePastureBatch?> GetByPastureIdAsync(Guid pastureId, CancellationToken ct = default);
    }

    public interface ISwineBeefBatchRepository : IBatchRepository<SwineBeefBatch>
    {
    }

}

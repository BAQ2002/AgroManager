using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBovinePastureBatchRepository
    {
        Task AddAsync(BovinePastureBatch entity, CancellationToken ct = default);
        Task DeleteAsync(BovinePastureBatch entity, CancellationToken ct = default);
        Task<BovinePastureBatch?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BovinePastureBatch?> GetByPastureIdAsync(Guid pastureId, CancellationToken ct = default);
    }

    public interface ISwineBeefBatchRepository
    {
        Task AddAsync(SwineBeefBatch entity, CancellationToken ct = default);
        Task DeleteAsync(SwineBeefBatch entity, CancellationToken ct = default);
        Task<SwineBeefBatch?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }

}

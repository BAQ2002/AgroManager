using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{

    public interface IMilkRepository
    {
        Task AddAsync(MilkEntity entity, CancellationToken ct = default);
        Task DeleteAsync(MilkEntity entity, CancellationToken ct = default);
        Task<MilkEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<MilkEntity?> GetByBovineIdAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBatchMemberRepository
    {
        Task AddAsync(BatchMemberEntity entity, CancellationToken ct = default);
        Task DeleteAsync(BatchMemberEntity entity, CancellationToken ct = default);
        Task<BatchMemberEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BatchMemberEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<BatchMemberEntity?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }


}



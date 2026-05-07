using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBatchMemberRepository<TBatchMember>
        where TBatchMember : BatchMemberEntity
    {
        Task AddAsync(TBatchMember entity, CancellationToken ct = default);
        Task DeleteAsync(TBatchMember entity, CancellationToken ct = default);
        Task<TBatchMember?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<TBatchMember?> GetCurrentByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<IReadOnlyList<TBatchMember>> GetHistoryByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<IReadOnlyList<TBatchMember>> GetHistoryByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<IReadOnlyList<TBatchMember>> ListActiveByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<bool> ExistsActiveByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<bool> CloseActiveMembershipAsync(Guid animalId, DateTimeOffset batchExitDate, string? exitReason, CancellationToken ct = default);
    }



    public interface IBovinePastureMemberRepository : IBatchMemberRepository<BovinePastureMember>
    {
    }

    public interface ISwineBeefMemberRepository : IBatchMemberRepository<SwineBeefMember>
    {
    }
}

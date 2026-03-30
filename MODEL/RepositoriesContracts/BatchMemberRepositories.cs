using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{   
    public interface IBovinePastureMemberRepository
    {
        Task AddAsync(BovinePastureMember entity, CancellationToken ct = default);
        Task DeleteAsync(BovinePastureMember entity, CancellationToken ct = default);
        Task<BovinePastureMember?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BovinePastureMember?> GetCurrentByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<BovinePastureMember?> GetCurrentByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<IReadOnlyList<BovinePastureMember>> GetHistoryByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<IReadOnlyList<BovinePastureMember>> GetHistoryByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<IReadOnlyList<BovinePastureMember>> ListActiveByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<bool> ExistsActiveByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<bool> CloseActiveMembershipAsync(Guid animalId, DateTimeOffset batchExitDate, string? exitReason, CancellationToken ct = default);


    }

    public interface ISwineBeefMemberRepository
    {
        Task AddAsync(SwineBeefMember entity, CancellationToken ct = default);
        Task DeleteAsync(SwineBeefMember entity, CancellationToken ct = default);
        Task<SwineBeefMember?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<SwineBeefMember?> GetCurrentByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<SwineBeefMember?> GetCurrentByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<IReadOnlyList<SwineBeefMember>> GetHistoryByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<IReadOnlyList<SwineBeefMember>> GetHistoryByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<IReadOnlyList<SwineBeefMember>> ListActiveByBatchIdAsync(Guid batchId, CancellationToken ct = default);
        Task<bool> ExistsActiveByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<bool> CloseActiveMembershipAsync(Guid animalId, DateTimeOffset batchExitDate, string? exitReason, CancellationToken ct = default);
    }
}

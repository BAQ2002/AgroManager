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
        Task<BovinePastureMember?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<BovinePastureMember?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }

    public interface ISwineBeefMemberRepository
    {
        Task AddAsync(SwineBeefMember entity, CancellationToken ct = default);
        Task DeleteAsync(SwineBeefMember entity, CancellationToken ct = default);
        Task<SwineBeefMember?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<SwineBeefMember?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<SwineBeefMember?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }
}

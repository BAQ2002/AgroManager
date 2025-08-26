using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBovineParentageRepository
    {
        Task AddAsync(BovineParentageEntity entity, CancellationToken ct = default);
        Task DeleteAsync(BovineParentageEntity entity, CancellationToken ct = default);
        Task<BovineParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<BovineParentageEntity?> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default);
        Task<BovineParentageEntity?> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default);
        Task<BovineParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default);
    }

    public interface ISwineParentageRepository
    {
        Task AddAsync(SwineParentageEntity entity, CancellationToken ct = default);
        Task DeleteAsync(SwineParentageEntity entity, CancellationToken ct = default);
        Task<SwineParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<SwineParentageEntity?> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default);
        Task<SwineParentageEntity?> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default);
        Task<SwineParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default);
    }
}

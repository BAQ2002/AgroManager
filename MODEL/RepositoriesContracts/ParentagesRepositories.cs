using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{

    public interface IAnimalParentageRepository<TParentageEntity>
    where TParentageEntity : ParentageEntity
    {
 
        Task AddAsync(TParentageEntity entity, CancellationToken ct = default);
        Task DeleteAsync(TParentageEntity entity, CancellationToken ct = default);
        Task<TParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<IReadOnlyList<TParentageEntity>> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default);
        Task<IReadOnlyList<TParentageEntity>> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default);
        Task<TParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default);
    }
    public interface IBovineParentageRepository : IAnimalParentageRepository<BovineParentageEntity> { }

    public interface ISwineParentageRepository : IAnimalParentageRepository<SwineParentageEntity> { }
}

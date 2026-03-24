using MODEL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IAnimalParentageService<TAnimal, TParentage>
        where TAnimal : AnimalEntity
        where TParentage : ParentageEntity
    {
        Task<TParentage?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<TParentage> SetParentageAsync(Guid animalId, TParentage parentage, CancellationToken ct = default);
        Task RemoveParentageAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBovineParentageService : IAnimalParentageService<BovineEntity, BovineParentageEntity> { }

    public interface ISwineParentageService : IAnimalParentageService<SwineEntity, SwineParentageEntity> { }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IBovineRepository
    {
        Task AddAsync(BovineEntity entity, CancellationToken ct = default);
        Task DeleteAsync(BovineEntity entity, CancellationToken ct = default);
        Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default);
        Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }
    public interface ISwineRepository
    {
        Task AddAsync(SwineEntity entity, CancellationToken ct = default);
        Task DeleteAsync(SwineEntity entity, CancellationToken ct = default);
        Task<SwineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<SwineEntity?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<SwineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default);
        Task<SwineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }

    public interface IMilkRepository
    {
        Task AddAsync(MilkEntity entity, CancellationToken ct = default);
        Task DeleteAsync(MilkEntity entity, CancellationToken ct = default);
        Task<MilkEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<MilkEntity?> GetByBovineIdAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBatchAnimalRepository
    {
        Task AddAsync(BatchAnimal_AssocEntity entity, CancellationToken ct = default);
        Task DeleteAsync(BatchAnimal_AssocEntity entity, CancellationToken ct = default);
        Task<BatchAnimal_AssocEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BatchAnimal_AssocEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<BatchAnimal_AssocEntity?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }
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



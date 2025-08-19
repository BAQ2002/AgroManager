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
        Task<MilkEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBatchAnimalRepository
    {
        Task AddAsync(BatchAnimalEntity entity, CancellationToken ct = default);
        Task DeleteAsync(BatchAnimalEntity entity, CancellationToken ct = default);
        Task<BatchAnimalEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<BatchAnimalEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<BatchAnimalEntity?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }
    public interface IParentageRepository
    {
        Task AddAsync(ParentageEntity entity, CancellationToken ct = default);
        Task DeleteAsync(ParentageEntity entity, CancellationToken ct = default);
        Task<ParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
        Task<ParentageEntity?> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default);
        Task<ParentageEntity?> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default);
        Task<ParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default);
    }
}

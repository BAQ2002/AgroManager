using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public interface IWeightRepository<TWeight>
        where TWeight : WeightEntity
    {
        Task AddAsync(TWeight entity, CancellationToken ct = default);
        Task DeleteAsync(TWeight entity, CancellationToken ct = default);
        Task<TWeight?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<TWeight>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBovineWeightRepository : IWeightRepository<BovineWeight>
    {
    }

    public interface ISwineWeightRepository : IWeightRepository<SwineWeight>
    {
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    

    public interface IWeightTimelineReader
    {
        Task<IReadOnlyList<WeightRecord>> GetHistoryAsync(Guid animalId, CancellationToken ct = default);
        Task<WeightRecord?> GetLatestAsync(Guid animalId, CancellationToken ct = default);
    }

    public interface IBovineWeightTimelineReader : IWeightTimelineReader
    {
    }

    public interface ISwineWeightTimelineReader : IWeightTimelineReader
    {
    }

    /// <summary>
    /// Logical tracker for querying an animal weight history and latest point.
    /// </summary>
    public interface IWeightTracker
    {
        Task<IReadOnlyList<WeightRecord>> GetHistoryAsync(CancellationToken ct = default);
        Task<WeightRecord?> GetLatestAsync(CancellationToken ct = default);
    }

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


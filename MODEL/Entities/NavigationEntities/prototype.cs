using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    /// <summary>
    /// Immutable weight measurement point (date + value) used by monitoring timelines.
    /// </summary>
    public sealed record WeightRecord(DateOnly Date, float Value);



    public abstract class WeightRecording
    {
        public Dictionary<WeightRecord> WeightHistory {  get; set; }

        public WeightRecord LatestWeight { get; set; }

        public abstract Task<IReadOnlyList<WeightRecord>> GetHistoryAsync(Guid animalId, CancellationToken ct = default);
        public abstract Task<WeightRecord?> GetLatestAsync(Guid animalId, CancellationToken ct = default);

        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
    }
}

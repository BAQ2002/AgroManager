using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    /// <summary>
    /// Immutable representation of one milking at an unambiguous instant.
    /// </summary>
    public sealed record MilkPoint(DateTimeOffset OccurredAt, float Liters);

    /// <summary>
    /// Tracks milk measurements for one animal without exposing persistence entities.
    /// History contains every individual milking. Operational dates must be calculated
    /// by callers with the farm's explicitly configured time zone.
    /// </summary>
    public interface IMilkTracker
    {
        Task<IReadOnlyList<MilkPoint>> GetHistoryAsync(CancellationToken ct = default);
        Task<MilkPoint?> GetLatestAsync(CancellationToken ct = default);
    }

    public interface IBovineMilkTracker : IMilkTracker
    {
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    /// <summary>
    /// Immutable milk measurement for a calendar day.
    /// </summary>
    public sealed record MilkPoint(DateOnly Date, float Liters);

    /// <summary>
    /// Tracks milk measurements for one animal without exposing persistence entities.
    /// Dates are calendar dates (without a time zone), future dates are rejected.
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
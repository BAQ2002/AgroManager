using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Persistence-backed milk tracker bound only to an opaque animal identifier.
    /// </summary>
    public abstract class MilkTrackerBase<TMilk> : IMilkTracker
        where TMilk : MilkEntity
    {
        private readonly Guid _animalId;

        protected MilkTrackerBase(Guid animalId)
        {
            if (animalId == Guid.Empty)
                throw new ArgumentException("An animal identifier is required.", nameof(animalId));

            _animalId = animalId;
        }


        protected abstract Task<IReadOnlyList<TMilk>> ReadEntriesAsync(Guid animalId, CancellationToken ct = default);


        public async Task<IReadOnlyList<MilkPoint>> GetHistoryAsync(CancellationToken ct = default)
        {
            IReadOnlyList<TMilk> entries = await ReadEntriesAsync(_animalId, ct).ConfigureAwait(false);

            return entries
                .OrderBy(entry => entry.OccurredAt)
                .ThenBy(entry => entry.Id)
                .Select(entry => new MilkPoint(entry.OccurredAt, entry.Liters))
                .ToList();
        }

        public async Task<MilkPoint?> GetLatestAsync(CancellationToken ct = default)
        {
            IReadOnlyList<TMilk> entries = await ReadEntriesAsync(_animalId, ct).ConfigureAwait(false);
            TMilk? latest = entries
                .OrderByDescending(entry => entry.OccurredAt)
                .ThenByDescending(entry => entry.Id)
                .FirstOrDefault();

            return latest is null
                ? null : new MilkPoint(latest.OccurredAt, latest.Liters);
        }
    }
}

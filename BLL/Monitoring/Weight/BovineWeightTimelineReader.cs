using MODEL;
namespace BLL.Monitoring.Weight
{
    /// <summary>
    /// Bovine-specific timeline reader backed by <see cref="IBovineWeightRepository"/>.
    /// </summary>
    public sealed class BovineWeightTimelineReader : IBovineWeightTimelineReader
    {
        private readonly IBovineWeightRepository _repository;

        public BovineWeightTimelineReader(IBovineWeightRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<WeightRecord>> GetHistoryAsync(Guid animalId, CancellationToken ct = default)
        {
            IReadOnlyList<BovineWeight> entries = await _repository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);
            return entries
                .OrderBy(w => w.OccurrenceDate)
                .Select(w => new WeightRecord(w.OccurrenceDate, w.Weight))
                .ToList();
        }

        public async Task<WeightRecord?> GetLatestAsync(Guid animalId, CancellationToken ct = default)
        {
            IReadOnlyList<BovineWeight> entries = await _repository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);
            BovineWeight? latest = entries
                .OrderByDescending(w => w.OccurrenceDate)
                .FirstOrDefault();

            return latest is null ? null : new WeightRecord(latest.OccurrenceDate, latest.Weight);
        }
    }
}

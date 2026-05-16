using MODEL;

namespace BLL
{
    /// <summary>
    /// Bovine-specific tracker that delegates persistence reads to <see cref="IBovineWeightRepository"/>
    /// while reusing transversal timeline rules from <see cref="WeightTrackerBase{TWeight}"/>.
    /// </summary>
    public class BovineWeightTracker : WeightTrackerBase<BovineWeight>, IBovineWeightTracker
    {
        private readonly IBovineWeightRepository _repository;

        /// <summary>
        /// Creates a tracker bound to one bovine id.
        /// </summary>
        /// <param name="animalId">Bovine identifier used for all read operations.</param>
        /// <param name="repository">Repository adapter responsible for bovine weight persistence.</param>
        public BovineWeightTracker(Guid animalId, IBovineWeightRepository repository) : base(animalId)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <inheritdoc />
        protected override Task<IReadOnlyList<BovineWeight>> ReadEntriesAsync(Guid animalId, CancellationToken ct = default)
            => _repository.GetByAnimalIdAsync(animalId, ct);
    }
}

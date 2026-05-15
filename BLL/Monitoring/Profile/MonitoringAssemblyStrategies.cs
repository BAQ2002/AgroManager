using MODEL;


namespace BLL.Monitoring.Profile
{
    /// <summary>
    /// Builds monitoring capabilities for <see cref="BovineEntity"/> animals.
    /// </summary>
    public sealed class BovineMonitoringAssemblyStrategy : IMonitoringAssemblyStrategy
    {
        private readonly IBovineWeightTimelineReader _weightReader;

        public BovineMonitoringAssemblyStrategy(IBovineWeightTimelineReader weightReader)
        {
            _weightReader = weightReader ?? throw new ArgumentNullException(nameof(weightReader));
        }

        public Type AnimalType => typeof(BovineEntity);

        public bool CanHandle(AnimalEntity animal) => animal is BovineEntity;

        public IEnumerable<IMonitoringCapability> BuildCapabilities(AnimalEntity animal)
        {
            if (animal is not BovineEntity bovine)
                throw new ArgumentException("Invalid animal type for bovine strategy.", nameof(animal));

            yield return new WeightMonitoringCapability(new WeightTracker(bovine.Id, _weightReader));
        }
    }

    /// <summary>
    /// Builds monitoring capabilities for <see cref="SwineEntity"/> animals.
    /// </summary>
    public sealed class SwineMonitoringAssemblyStrategy : IMonitoringAssemblyStrategy
    {
        private readonly ISwineWeightTimelineReader _weightReader;

        public SwineMonitoringAssemblyStrategy(ISwineWeightTimelineReader weightReader)
        {
            _weightReader = weightReader ?? throw new ArgumentNullException(nameof(weightReader));
        }

        public Type AnimalType => typeof(SwineEntity);

        public bool CanHandle(AnimalEntity animal) => animal is SwineEntity;

        public IEnumerable<IMonitoringCapability> BuildCapabilities(AnimalEntity animal)
        {
            if (animal is not SwineEntity swine)
                throw new ArgumentException("Invalid animal type for swine strategy.", nameof(animal));

            yield return new WeightMonitoringCapability(new WeightTracker(swine.Id, _weightReader));
        }
    }
}

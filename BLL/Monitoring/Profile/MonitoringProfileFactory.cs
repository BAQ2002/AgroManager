using MODEL;


namespace BLL.Monitoring.Profile
{
    /// <summary>
    /// Composes monitoring profiles by selecting the best assembly strategy for the provided animal type.
    /// </summary>
    public sealed class MonitoringProfileFactory : IMonitoringProfileFactory
    {
        private readonly IReadOnlyList<IMonitoringAssemblyStrategy> _strategies;

        public MonitoringProfileFactory(IEnumerable<IMonitoringAssemblyStrategy> strategies)
        {
            _strategies = (strategies ?? throw new ArgumentNullException(nameof(strategies))).ToList();
        }

        public IMonitoringProfile CreateForAnimal(AnimalEntity animal)
        {
            ArgumentNullException.ThrowIfNull(animal);

            IMonitoringAssemblyStrategy? strategy = _strategies.FirstOrDefault(s => s.AnimalType == animal.GetType())
                ?? _strategies.FirstOrDefault(s => s.CanHandle(animal));

            if (strategy is null)
                throw new InvalidOperationException($"No monitoring assembly strategy configured for animal type '{animal.GetType().FullName}'.");

            IEnumerable<IMonitoringCapability> capabilities = strategy.BuildCapabilities(animal);
            return new MonitoringProfile(animal.Id, animal.GetType(), capabilities);
        }

        public IMonitoringProfile CreateForBovine(BovineEntity bovine) => CreateForAnimal(bovine);
        public IMonitoringProfile CreateForSwine(SwineEntity swine) => CreateForAnimal(swine);
    }
}

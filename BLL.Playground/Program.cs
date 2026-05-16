using BLL.Monitoring.Profile;
using BLL.Monitoring.Weight;
using BLL.Services;
using INFRA;
using INFRA.Repositories.Weight;
using Microsoft.EntityFrameworkCore;
using MODEL;

Console.WriteLine("=== MonitoringProfile CLI Demo (Database-backed) ===");

string connectionString = Environment.GetEnvironmentVariable("AGROMANAGER_CONNECTION")
    ?? "Host=localhost;Port=5432;Database=agromanager_db;Username=admin;Password=admin123";

var options = new DbContextOptionsBuilder<AgroManagerDbContext>()
    .UseNpgsql(connectionString)
    .Options;

var contextFactory = new SimpleDbContextFactory(options);

IAnimalRepository<BovineEntity> bovineRepo = new BovineRepositoryEF(contextFactory);
IAnimalRepository<SwineEntity> swineRepo = new SwineRepositoryEF(contextFactory);
IBovineWeightRepository bovineWeightRepo = new BovineWeightRepositoryEF(contextFactory);
ISwineWeightRepository swineWeightRepo = new SwineWeightRepositoryEF(contextFactory);

var bovine = new BovineEntity { Name = $"Bovino CLI {Guid.NewGuid():N}" };
var swine = new SwineEntity { Name = $"Suino CLI {Guid.NewGuid():N}" };

var addedBovineWeights = new List<BovineWeight>();
var addedSwineWeights = new List<SwineWeight>();

try
{
    await bovineRepo.AddAsync(bovine);
    await swineRepo.AddAsync(swine);

    addedBovineWeights = await SeedBovineAsync(bovineWeightRepo, bovine.Id);
    addedSwineWeights = await SeedSwineAsync(swineWeightRepo, swine.Id);

    var bovineReader = new BovineWeightTimelineReader(bovineWeightRepo);
    var swineReader = new SwineWeightTimelineReader(swineWeightRepo);

    IMonitoringAssemblyStrategy[] strategies =
    [
        new BovineMonitoringAssemblyStrategy(bovineReader),
        new SwineMonitoringAssemblyStrategy(swineReader)
    ];

    IMonitoringProfileFactory factory = new MonitoringProfileFactory(strategies);

    IMonitoringProfile bovineProfile = factory.CreateForBovine(bovine);
    IMonitoringProfile swineProfile = factory.CreateForSwine(swine);

    Console.WriteLine("\n--- BOVINE ---");
    await PrintWeightAsync(bovineProfile);

    Console.WriteLine("\n--- SWINE ---");
    await PrintWeightAsync(swineProfile);
}
finally
{
    foreach (BovineWeight item in addedBovineWeights)
        await bovineWeightRepo.DeleteAsync(item);

    foreach (SwineWeight item in addedSwineWeights)
        await swineWeightRepo.DeleteAsync(item);

    await bovineRepo.DeleteAsync(bovine);
    await swineRepo.DeleteAsync(swine);

    Console.WriteLine("\nRegistros e animais de teste removidos com sucesso.");
}

static async Task PrintWeightAsync(IMonitoringProfile profile)
{
    if (!profile.TryGetCapability<WeightMonitoringCapability>(out WeightMonitoringCapability? weightCap) || weightCap is null)
    {
        Console.WriteLine($"Animal {profile.AnimalId} não possui capability de peso.");
        return;
    }

    IReadOnlyList<WeightPoint> history = await weightCap.Tracker.GetHistoryAsync();
    WeightPoint? latest = await weightCap.Tracker.GetLatestAsync();

    Console.WriteLine($"AnimalType: {profile.AnimalType.Name}");
    Console.WriteLine($"AnimalId:   {profile.AnimalId}");
    Console.WriteLine("Histórico ordenado:");

    foreach (WeightPoint point in history)
        Console.WriteLine($" - {point.Date:yyyy-MM-dd}: {point.Value} kg");

    Console.WriteLine(latest is null
        ? "Último peso: (sem registros)"
        : $"Último peso: {latest.Date:yyyy-MM-dd} => {latest.Value} kg");
}

static async Task<List<BovineWeight>> SeedBovineAsync(IBovineWeightRepository repo, Guid bovineId)
{
    List<BovineWeight> data =
    [
        new() { BovineId = bovineId, OccurrenceDate = new DateOnly(2026, 5, 10), Weight = 420f },
        new() { BovineId = bovineId, OccurrenceDate = new DateOnly(2026, 5, 1), Weight = 401f },
        new() { BovineId = bovineId, OccurrenceDate = new DateOnly(2026, 5, 15), Weight = 432f }
    ];

    foreach (BovineWeight item in data)
        await repo.AddAsync(item);

    return data;
}

static async Task<List<SwineWeight>> SeedSwineAsync(ISwineWeightRepository repo, Guid swineId)
{
    List<SwineWeight> data =
    [
        new() { SwineId = swineId, OccurrenceDate = new DateOnly(2026, 5, 11), Weight = 95f },
        new() { SwineId = swineId, OccurrenceDate = new DateOnly(2026, 5, 5), Weight = 88f },
        new() { SwineId = swineId, OccurrenceDate = new DateOnly(2026, 5, 14), Weight = 101f }
    ];

    foreach (SwineWeight item in data)
        await repo.AddAsync(item);

    return data;
}

file sealed class SimpleDbContextFactory : IDbContextFactory<AgroManagerDbContext>
{
    private readonly DbContextOptions<AgroManagerDbContext> _options;

    public SimpleDbContextFactory(DbContextOptions<AgroManagerDbContext> options)
    {
        _options = options;
    }

    public AgroManagerDbContext CreateDbContext() => new(_options);

    public Task<AgroManagerDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(new AgroManagerDbContext(_options));
}
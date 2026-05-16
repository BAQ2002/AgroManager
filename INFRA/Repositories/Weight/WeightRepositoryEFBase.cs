using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA.Repositories.Weight;

public abstract class WeightRepositoryEFBase<TWeight> : IWeightRepository<TWeight>
    where TWeight : WeightEntity
{
    private readonly IDbContextFactory<AgroManagerDbContext> _factory;

    protected WeightRepositoryEFBase(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

    protected abstract DbSet<TWeight> GetSet(AgroManagerDbContext db);
    protected abstract IQueryable<TWeight> FilterByAnimalId(IQueryable<TWeight> query, Guid animalId);

    public async Task AddAsync(TWeight entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetSet(db).Add(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TWeight entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetSet(db).Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<TWeight?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetSet(db).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<TWeight>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await FilterByAnimalId(GetSet(db).AsNoTracking(), animalId)
            .OrderByDescending(x => x.OccurrenceDate)
            .ToListAsync(ct);
    }
}
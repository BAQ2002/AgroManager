using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA;

public abstract class AnimalWeightRepositoryEFBase<TWeight> : IWeightRepository<TWeight>
    where TWeight : WeightEntity
{
    private readonly IDbContextFactory<AgroManagerDbContext> _factory;

    protected AnimalWeightRepositoryEFBase(IDbContextFactory<AgroManagerDbContext> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }
    /// <summary>
    /// Obtém o <see cref="DbSet{TEntity}"/> correspondente ao tipo concreto de animal.
    /// </summary>
    protected abstract DbSet<TWeight> GetDbSet(AgroManagerDbContext db);
    protected abstract IQueryable<TWeight> GetByAnimalId(IQueryable<TWeight> query, Guid animalId);

    public async Task AddAsync(TWeight entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetDbSet(db).Add(entity);
        await db.SaveChangesAsync(ct);
    }
    public virtual async Task UpdateAsync(TWeight entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetDbSet(db).Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TWeight entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetDbSet(db).Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<TWeight?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetDbSet(db).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
    }
    
    public async Task<IReadOnlyList<TWeight>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetByAnimalId(GetDbSet(db).AsNoTracking(), animalId)
            .OrderByDescending(x => x.OccurrenceDate)
            .ToListAsync(ct);
    }
}
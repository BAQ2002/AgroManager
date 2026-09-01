using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA;

/// <summary>
/// Classe que implementa os métodos de interação com o banco de dados
/// </summary>
public abstract class MilkRepositoryEFBase<TMilk> : IMilkRepository<TMilk>
    where TMilk : MilkEntity
{
    private readonly IDbContextFactory<AgroManagerDbContext> _factory;
    protected MilkRepositoryEFBase(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

    protected abstract DbSet<TMilk> GetSet(AgroManagerDbContext db);
    protected abstract IQueryable<TMilk> FilterByAnimalId(IQueryable<TMilk> query, Guid animalId);


    public async Task AddAsync(TMilk entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetSet(db).Add(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TMilk entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetSet(db).Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<TMilk?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetSet(db).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<IReadOnlyList<TMilk?>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await FilterByAnimalId(GetSet(db).AsNoTracking(), animalId)
            .OrderByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Id)
            .ToListAsync(ct);
    }
}

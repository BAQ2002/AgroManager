using Microsoft.EntityFrameworkCore;
using MODEL;

namespace INFRA;

/// <summary>
/// Implementação base EF Core para repositórios de entidades derivadas de <see cref="AnimalEntity"/>.
/// Centraliza operações CRUD e consultas comuns, mantendo os repositórios concretos enxutos.
/// </summary>
/// <typeparam name="TAnimal">Tipo concreto da entidade animal.</typeparam>
public abstract class AnimalRepositoryEFBase<TAnimal> : IAnimalRepository<TAnimal>
    where TAnimal : AnimalEntity
{
    private readonly IDbContextFactory<AgroManagerDbContext> _factory;

    protected AnimalRepositoryEFBase(IDbContextFactory<AgroManagerDbContext> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Obtém o <see cref="DbSet{TEntity}"/> correspondente ao tipo concreto de animal.
    /// </summary>
    protected abstract DbSet<TAnimal> GetDbSet(AgroManagerDbContext db);

    public virtual async Task AddAsync(TAnimal entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetDbSet(db).Add(entity);
        await db.SaveChangesAsync(ct);
    }

    public virtual async Task UpdateAsync(TAnimal entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetDbSet(db).Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteAsync(TAnimal entity, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        GetDbSet(db).Remove(entity);
        await db.SaveChangesAsync(ct);
    }

    public virtual async Task<IReadOnlyList<TAnimal>> ListAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetDbSet(db).AsNoTracking().ToListAsync(ct);
    }

    public virtual async Task<IReadOnlyList<TAnimal>> ListAsync(AnimalFiltersModel filters, CancellationToken ct = default)
    {
        if (filters is null) throw new ArgumentNullException(nameof(filters));

        await using var db = await _factory.CreateDbContextAsync(ct);

        IQueryable<TAnimal> query = GetDbSet(db).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            string normalizedName = filters.Name.Trim();
            query = query.Where(x => x.Name != null && x.Name.Contains(normalizedName));
        }

        if (filters.Origin.HasValue)
        {
            query = query.Where(x => x.Origin == filters.Origin.Value);
        }

        if (filters.Gender.HasValue)
        {
            query = query.Where(x => x.Gender == filters.Gender.Value);
        }

        if (filters.BirthDateFrom.HasValue)
        {
            query = query.Where(x => x.BirthDate.HasValue && x.BirthDate.Value >= filters.BirthDateFrom.Value);
        }

        if (filters.BirthDateTo.HasValue)
        {
            query = query.Where(x => x.BirthDate.HasValue && x.BirthDate.Value <= filters.BirthDateTo.Value);
        }

        if (filters.Skip.HasValue && filters.Skip.Value > 0)
        {
            query = query.Skip(filters.Skip.Value);
        }

        if (filters.Take.HasValue && filters.Take.Value > 0)
        {
            query = query.Take(filters.Take.Value);
        }

        return await query.ToListAsync(ct);
    }

    public virtual async Task<TAnimal?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetDbSet(db).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
    }

    public virtual async Task<TAnimal?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetDbSet(db).AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
    }

    public virtual async Task<TAnimal?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetDbSet(db).AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
    }

    public virtual async Task<TAnimal?> GetByGenderAsync(int gender, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await GetDbSet(db).AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
    }
}

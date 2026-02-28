using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA
{
    /// <summary>
    /// Classe que implementa os métodos de interação com o banco de dados
    /// </summary>
    public sealed class SwineRepositoryEF : IAnimalRepository<SwineEntity>
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public SwineRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(SwineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Swines.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(SwineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);

            db.Swines.Update(entity);       // marca como Modified
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(SwineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Swines.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<SwineEntity>> ListAsync(CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().ToListAsync(ct);
        }

        public async Task<SwineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<SwineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<SwineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
        }

        public async Task<SwineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Swines.AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
        }
    }
}

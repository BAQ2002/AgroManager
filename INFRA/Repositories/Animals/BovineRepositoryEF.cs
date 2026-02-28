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
    public sealed class BovineRepositoryEF : IAnimalRepository<BovineEntity>
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public BovineRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Bovines.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);

            db.Bovines.Update(entity);       // marca como Modified
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Bovines.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<BovineEntity>> ListAsync(CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().ToListAsync(ct);
        }

        public async Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
        }

        public async Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
        }
    }


}

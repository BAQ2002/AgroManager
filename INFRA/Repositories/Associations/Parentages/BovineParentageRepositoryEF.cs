using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA.Repositories
{
    public sealed class BovineParentageRepositoryEF : IBovineParentageRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public BovineParentageRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;
        public async Task AddAsync(BovineParentageEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BovineParentages.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BovineParentageEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BovineParentages.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
        public async Task<BovineParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == animalId, ct);
        }
        public async Task<BovineParentageEntity?> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.FatherId == fatherId, ct);
        }
        public async Task<BovineParentageEntity?> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.MotherId == motherId, ct);
        }
        public async Task<BovineParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BovineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.SurrogateMotherId == surrogateMotherId, ct);
        }
    }
}

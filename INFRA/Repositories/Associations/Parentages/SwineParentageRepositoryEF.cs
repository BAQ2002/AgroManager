using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INFRA
{
    public sealed class SwineParentageRepositoryEF : ISwineParentageRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public SwineParentageRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;
        public async Task AddAsync(SwineParentageEntity entity, CancellationToken ct = default) 
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.SwineParentages.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(SwineParentageEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.SwineParentages.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
        public async Task<SwineParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == animalId, ct);
        }
        public async Task<SwineParentageEntity?> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.FatherId == fatherId, ct);
        }
        public async Task<SwineParentageEntity?> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.MotherId == motherId, ct);
        }
        public async Task<SwineParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.SwineParentages.AsNoTracking().SingleOrDefaultAsync(x => x.SurrogateMotherId == surrogateMotherId, ct);
        }
    }
}

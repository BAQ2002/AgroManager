using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA
{
    public class ParentageRepositoryEF : IParentageRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public ParentageRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;
        public async Task AddAsync(ParentageEntity entity, CancellationToken ct = default) 
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Parentages.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(ParentageEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.Parentages.Remove(entity);
            await db.SaveChangesAsync(ct);
        }
        public async Task<ParentageEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Parentages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == animalId, ct);
        }
        public async Task<ParentageEntity?> GetByFatherIdAsync(Guid fatherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Parentages.AsNoTracking().SingleOrDefaultAsync(x => x.FatherId == fatherId, ct);
        }
        public async Task<ParentageEntity?> GetByMotherIdAsync(Guid motherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Parentages.AsNoTracking().SingleOrDefaultAsync(x => x.MotherId == motherId, ct);
        }
        public async Task<ParentageEntity?> GetBySurrogateMotherIdAsync(Guid surrogateMotherId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.Parentages.AsNoTracking().SingleOrDefaultAsync(x => x.SurrogateMotherId == surrogateMotherId, ct);
        }
    }
}

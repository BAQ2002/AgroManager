using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRA
{
    internal class BatchAnimalRepositoryEF : IBatchAnimalRepository
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _factory;
        public BatchAnimalRepositoryEF(IDbContextFactory<AgroManagerDbContext> factory) => _factory = factory;

        public async Task AddAsync(BatchAnimal_AssocEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BatchAnimals.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BatchAnimal_AssocEntity entity, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            db.BatchAnimals.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<BatchAnimal_AssocEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BatchAnimals.AsNoTracking().SingleOrDefaultAsync(bA => bA.Id == id, ct);
        }

        public async Task<BatchAnimal_AssocEntity?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BatchAnimals.AsNoTracking().SingleOrDefaultAsync(bA => bA.AnimalId == animalId, ct);
        }
        public async Task<BatchAnimal_AssocEntity?> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            await using var db = await _factory.CreateDbContextAsync(ct);
            return await db.BatchAnimals.AsNoTracking().SingleOrDefaultAsync(bA => bA.BatchId == batchId, ct);
        }
    }
}


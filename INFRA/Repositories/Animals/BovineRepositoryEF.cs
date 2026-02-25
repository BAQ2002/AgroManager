using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Threading;

namespace INFRA
{
    /// <summary>
    /// Classe que implementa os métodos de interação com o banco de dados
    /// </summary>
    public sealed class BovineRepositoryEF : IAnimalRepository<BovineEntity>
    {
        private readonly AgroManagerDbContext _db;

        public BovineRepositoryEF(AgroManagerDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(BovineEntity entity, CancellationToken ct = default)
        {
            _db.Bovines.Add(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(BovineEntity entity, CancellationToken ct = default)
        {
            _db.Bovines.Update(entity); // marca como Modified
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BovineEntity entity, CancellationToken ct = default)
        {
            _db.Bovines.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            return await _db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
        }

        public async Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            return await _db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
        }
    }
}

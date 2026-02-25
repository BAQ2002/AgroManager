using Microsoft.EntityFrameworkCore;
using MODEL;
using System;
using System.Threading;

namespace INFRA
{
    /// <summary>
    /// Repositório EF Core para operações de persistência de <see cref="BovineEntity"/>.
    /// </summary>
    /// <remarks>
    /// A instância de <see cref="AgroManagerDbContext"/> não é mantida como estado do repositório.
    /// Em cada operação, um novo contexto é criado via <see cref="IDbContextFactory{TContext}"/>,
    /// reduzindo acoplamento ao ciclo de vida de request HTTP e permitindo uso seguro em cenários
    /// com múltiplas unidades de trabalho.
    /// </remarks>
    public sealed class BovineRepositoryEF : IAnimalRepository<BovineEntity>
    {
        private readonly IDbContextFactory<AgroManagerDbContext> _dbContextFactory;

        /// <summary>
        /// Inicializa o repositório com a fábrica de contextos do EF Core.
        /// </summary>
        /// <param name="dbContextFactory">Fábrica usada para criar instâncias de <see cref="AgroManagerDbContext"/> sob demanda.</param>
        public BovineRepositoryEF(IDbContextFactory<AgroManagerDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            db.Bovines.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            db.Bovines.Update(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(BovineEntity entity, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            db.Bovines.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => x.Gender == gender, ct);
        }

        public async Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync(ct);
            return await db.Bovines.AsNoTracking().SingleOrDefaultAsync(x => (int)x.Gender == gender, ct);
        }
    }
}

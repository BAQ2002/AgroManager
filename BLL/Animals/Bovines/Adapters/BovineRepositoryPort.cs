using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Animals.Ports;
using MODEL;

namespace BLL.Animals.Bovines.Adapters
{
    /// <summary>
    /// Adapter que converte o contrato genérico da BLL (IAnimalRepositoryPort)
    /// para o contrato específico de bovinos já existente no MODEL (IBovineRepository).
    /// </summary>
    public sealed class BovineRepositoryPort : IAnimalRepositoryPort<BovineEntity>
    {
        private readonly IBovineRepository _repository;

        public BovineRepositoryPort(IBovineRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task AddAsync(BovineEntity entity, CancellationToken ct = default)
        {
            return _repository.AddAsync(entity, ct);
        }

        public Task UpdateAsync(BovineEntity entity, CancellationToken ct = default)
        {
            return _repository.UpdateAsync(entity, ct);
        }

        public Task DeleteAsync(BovineEntity entity, CancellationToken ct = default)
        {
            return _repository.DeleteAsync(entity, ct);
        }

        public Task<BovineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return _repository.GetByIdAsync(id, ct);
        }

        public Task<BovineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return _repository.GetByNameAsync(name, ct);
        }

        public Task<BovineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            return _repository.GetByGenderAsync(gender, ct);
        }

        public Task<BovineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            return _repository.GetByGenderAsync(gender, ct);
        }
    }
}

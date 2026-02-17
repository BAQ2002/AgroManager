using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Animals.Ports;
using MODEL;

namespace BLL.Animals.Swines.Adapters
{
    /// <summary>
    /// Adapter que converte o contrato genérico da BLL (IAnimalRepositoryPort)
    /// para o contrato específico de suínos já existente no MODEL (ISwineRepository).
    /// </summary>
    public sealed class SwineRepositoryPort : IAnimalRepositoryPort<SwineEntity>
    {
        private readonly ISwineRepository _repository;

        public SwineRepositoryPort(ISwineRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task AddAsync(SwineEntity entity, CancellationToken ct = default)
        {
            return _repository.AddAsync(entity, ct);
        }

        public Task DeleteAsync(SwineEntity entity, CancellationToken ct = default)
        {
            return _repository.DeleteAsync(entity, ct);
        }

        public Task<SwineEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return _repository.GetByIdAsync(id, ct);
        }

        public Task<SwineEntity?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return _repository.GetByNameAsync(name, ct);
        }

        public Task<SwineEntity?> GetByGenderAsync(Gender gender, CancellationToken ct = default)
        {
            return _repository.GetByGenderAsync(gender, ct);
        }

        public Task<SwineEntity?> GetByGenderAsync(int gender, CancellationToken ct = default)
        {
            return _repository.GetByGenderAsync(gender, ct);
        }
    }
}

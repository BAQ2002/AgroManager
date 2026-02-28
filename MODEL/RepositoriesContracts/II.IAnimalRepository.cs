using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL

{   /// <summary>
    /// Define um contrato genérico (port) para acesso a dados de animais na BLL
    /// </summary>
    /// <typeparam name="TAnimal">
    /// Tipo da entidade animal. Deve herdar de AnimalEntity.
    /// </typeparam>
    public interface IAnimalRepository<TAnimal> where TAnimal : AnimalEntity
    {   
        Task AddAsync(TAnimal entity, CancellationToken ct = default);

        Task UpdateAsync(TAnimal entity, CancellationToken ct = default); // <-- NOVO

        Task DeleteAsync(TAnimal entity, CancellationToken ct = default);

        Task<TAnimal?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<TAnimal?> GetByNameAsync(string name, CancellationToken ct = default);

        Task<IReadOnlyList<TAnimal>> ListAsync(CancellationToken ct = default);

        Task<TAnimal?> GetByGenderAsync(Gender gender, CancellationToken ct = default);

        Task<TAnimal?> GetByGenderAsync(int gender, CancellationToken ct = default);
    }
}

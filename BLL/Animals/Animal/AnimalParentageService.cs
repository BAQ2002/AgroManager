using BLL.Common.Exceptions;
using MODEL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services
{
    public abstract class AnimalParentageService<TAnimal, TParentage> : IAnimalParentageService<TAnimal, TParentage>
        where TAnimal : AnimalEntity
        where TParentage : ParentageEntity
    {
        private readonly IAnimalRepository<TAnimal> _animalRepository;
        private readonly IAnimalParentageRepository<TParentage> _parentageRepository;

        protected AnimalParentageService(
            IAnimalRepository<TAnimal> animalRepository,
            IAnimalParentageRepository<TParentage> parentageRepository)
        {
            _animalRepository = animalRepository ?? throw new ArgumentNullException(nameof(animalRepository));
            _parentageRepository = parentageRepository ?? throw new ArgumentNullException(nameof(parentageRepository));
        }

        public async Task<TParentage?> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
        {
            if (animalId == Guid.Empty)
                throw new BusinessRuleException("O identificador informado é inválido.");

            return await _parentageRepository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);
        }

        public async Task<TParentage> SetParentageAsync(Guid animalId, TParentage parentage, CancellationToken ct = default)
        {
            if (animalId == Guid.Empty)
                throw new BusinessRuleException("O identificador informado é inválido.");

            ArgumentNullException.ThrowIfNull(parentage);

            TAnimal? animal = await _animalRepository.GetByIdAsync(animalId, ct).ConfigureAwait(false) ?? throw new NotFoundException("Animal não encontrado.");
            
            parentage.Id = animalId;

            await ValidateParentageRulesAsync(animalId, parentage, ct).ConfigureAwait(false);

            TParentage? existing = await _parentageRepository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);

            if (existing is null)
            {
                await _parentageRepository.AddAsync(parentage, ct).ConfigureAwait(false);
            }
            else
            {
                await _parentageRepository.UpdateAsync(parentage, ct).ConfigureAwait(false);
            }

            return parentage;
        }

        public async Task RemoveParentageAsync(Guid animalId, CancellationToken ct = default)
        {
            if (animalId == Guid.Empty)
                throw new BusinessRuleException("O identificador informado é inválido.");

            TParentage? existing = await _parentageRepository.GetByAnimalIdAsync(animalId, ct).ConfigureAwait(false);

            if (existing is null)
                return;

            await _parentageRepository.DeleteAsync(existing, ct).ConfigureAwait(false);
        }

        protected virtual async Task ValidateParentageRulesAsync(Guid animalId, TParentage parentage, CancellationToken ct)
        {
            bool hasFatherId = parentage.FatherId.HasValue && parentage.FatherId.Value != Guid.Empty;
            bool hasMotherId = parentage.MotherId != Guid.Empty;
            bool hasSurrogateMotherId = parentage.SurrogateMotherId.HasValue && parentage.SurrogateMotherId.Value != Guid.Empty;

            if (hasFatherId && parentage.FatherId!.Value == animalId)
                throw new BusinessRuleException("Um animal não pode ser pai de si mesmo.");

            if (hasMotherId && parentage.MotherId == animalId)
                throw new BusinessRuleException("Um animal não pode ser mãe de si mesmo.");

            if (hasSurrogateMotherId && parentage.SurrogateMotherId!.Value == animalId)
                throw new BusinessRuleException("Um animal não pode ser mãe de aluguel de si mesmo.");

            if (hasFatherId && hasMotherId && parentage.FatherId!.Value == parentage.MotherId)
                throw new BusinessRuleException("Pai e mãe não podem ser o mesmo animal.");

            if (parentage.FatherFlag == ParentFlag.Internal)
            {
                if (!hasFatherId)
                    throw new BusinessRuleException("Quando o pai for interno, o FatherId deve ser informado.");

                await ValidateInternalParentAsync(parentage.FatherId!.Value, Gender.Male, "pai", ct).ConfigureAwait(false);
            }

            if (parentage.MotherFlag == ParentFlag.Internal)
            {
                if (!hasMotherId)
                    throw new BusinessRuleException("Quando a mãe for interna, o MotherId deve ser informado.");

                await ValidateInternalParentAsync(parentage.MotherId, Gender.Female, "mãe", ct).ConfigureAwait(false);
            }

            if (parentage.SurrogateMotherFlag == ParentFlag.Internal)
            {
                if (!hasSurrogateMotherId)
                    throw new BusinessRuleException("Quando a mãe de aluguel for interna, o SurrogateMotherId deve ser informado.");

                await ValidateInternalParentAsync(parentage.SurrogateMotherId!.Value, Gender.Female, "mãe de aluguel", ct).ConfigureAwait(false);
            }
        }

        private async Task ValidateInternalParentAsync(Guid parentId, Gender expectedGender, string label, CancellationToken ct)
        {
            TAnimal? parent = await _animalRepository.GetByIdAsync(parentId, ct).ConfigureAwait(false);

            if (parent is null)
                throw new BusinessRuleException($"O {label} interno informado não existe.");

            if (parent.Gender != expectedGender)
                throw new BusinessRuleException($"O {label} interno informado deve ter gênero {expectedGender}.");
        }
    }
}
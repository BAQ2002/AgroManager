using BLL.Common.Exceptions;
using MODEL;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Contrato público da BLL para operações relacionadas a bovinos.
    /// Herda o fluxo padrão de CRUD definido em <see cref="IAnimalService{TAnimal}"/>
    /// e permite extensão com operações específicas de bovinos quando necessário.
    /// </summary>
    public interface IBovineService : IAnimalService<BovineEntity>
    { }

    /// <summary>
    /// Implementa regras de negócio específicas para bovinos sobre o pipeline comum de
    /// <see cref="AnimalService{TAnimal}"/>.
    /// O fluxo de criação/atualização/exclusão é executado na classe base, que chama os ganchos
    /// <see cref="ValidateSpecificRules(BovineEntity)"/> e <see cref="ValidateDeleteSpecificRulesAsync(BovineEntity, CancellationToken)"/>.
    /// </summary>
    public sealed class BovineService : AnimalService<BovineEntity>, IBovineService
    {
        /// <summary>
        /// Inicializa o serviço de bovinos delegando a persistência para um repositório que implementa
        /// <see cref="IAnimalRepository{TAnimal}"/> para <see cref="BovineEntity"/>.
        /// </summary>
        /// <param name="repository">Port adaptado para acesso e escrita de dados de bovinos.</param>
        public BovineService(IAnimalRepository<BovineEntity> repository, IPhotoStorage photoStorage) : base(repository, photoStorage)
        {
        }

        /// <summary>
        /// Executa validações de domínio específicas de bovinos.
        /// Este método é chamado pela classe base antes de persistir em
        /// <see cref="AnimalService{TAnimal}.CreateAsync(TAnimal, CancellationToken)"/> e
        /// <see cref="AnimalService{TAnimal}.UpdateAsync(TAnimal, CancellationToken)"/>.
        /// </summary>
        /// <param name="entity">Entidade bovina que será validada.</param>
        /// <exception cref="BusinessRuleException">
        /// Lançada quando <c>CattleType</c> não é informado, quando o gênero está como <see cref="Gender.Unknown"/>
        /// ou quando uma fêmea não possui <c>MaritalStatus</c> definido.
        /// </exception>
        protected override void ValidateSpecificRules(BovineEntity entity)
        {
            if (entity.CattleType is null) throw new BusinessRuleException("O tipo do bovino deve ser informado.");

            if (entity.Gender == Gender.Unknown) throw new BusinessRuleException("O gênero do bovino deve ser informado.");

            if (entity.Gender == Gender.Female && entity.MaritalStatus is null) throw new BusinessRuleException("O estado marital do bovino deve ser informado para fêmeas.");
        }

        /// <summary>
        /// Executa validações adicionais antes da exclusão de bovinos.
        /// Este gancho é chamado pela base em <see cref="AnimalService{TAnimal}.DeleteAsync(Guid, CancellationToken)"/>
        /// após as regras comuns e antes da chamada ao repositório para remoção.
        /// </summary>
        /// <param name="entity">Entidade bovina que está no fluxo de exclusão.</param>
        /// <param name="ct">Token de cancelamento do fluxo assíncrono.</param>
        /// <returns>
        /// <see cref="Task.CompletedTask"/>, pois atualmente não há validações adicionais com dependências externas.
        /// </returns>
        protected override Task ValidateDeleteSpecificRulesAsync(BovineEntity entity, CancellationToken ct)
        {
            return Task.CompletedTask;
        }
    }
}


using BLL.Common.Exceptions;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Contrato público da BLL para operações relacionadas a bovinos.
    /// // métodos exclusivos de bovino podem ser adicionados aqui futuramente
    /// </summary>
    public interface IBovineService : IAnimalService<BovineEntity>
    { }

    /// <summary>
    /// Implementa regras de negócio específicas para bovinos.
    /// </summary>
    public sealed class BovineService : AnimalServiceBase<BovineEntity>, IBovineService
    {
        /// <summary>
        /// Inicializa o service de bovinos utilizando o port genérico de repositório.
        /// </summary>
        /// <param name="repository">Port adaptado para acesso a dados de bovinos.</param>
        public BovineService(IAnimalRepository<BovineEntity> repository): base(repository)
        {
        }

        /// <summary>
        /// Aplica validações específicas de bovinos.
        /// </summary>
        /// <param name="entity">Entidade a ser validada.</param>
        protected override void ValidateSpecificRules(BovineEntity entity)
        {
            // -------------------- CattleType (obrigatório) --------------------
            if (entity.CattleType is null) throw new BusinessRuleException("O tipo do bovino (CattleType) deve ser informado.");

            // -------------------- Gender (regra de negócio) --------------------
            // Seu MODEL permite Unknown, mas aqui decidimos uma regra real:
            // cadastro deve definir o gênero para o domínio fazer sentido.
            if (entity.Gender == Gender.Unknown) throw new BusinessRuleException("O gênero do bovino deve ser informado.");

            // -------------------- MaritalStatus (regra por gênero) --------------------
            // Exemplo de regra real: status marital faz sentido para fêmea.
            if (entity.Gender == Gender.Female && entity.MaritalStatus is null) throw new BusinessRuleException("O estado marital do bovino deve ser informado para fêmeas.");
        }

        /// <summary>
        /// Aplica validações específicas antes de excluir um bovino.
        /// </summary>
        /// <param name="entity">Entidade que será excluída.</param>
        /// <param name="ct">Token de cancelamento.</param>
        protected override Task ValidateDeleteSpecificRulesAsync(BovineEntity entity, CancellationToken ct)
        {
            // Aqui futuramente entram regras como:
            // - não excluir se existir MilkEntity vinculado
            // - não excluir se estiver em Batch / Parentage
            // Por enquanto, não há dependências extras conectadas neste service.

            return Task.CompletedTask;
        }
    }
}

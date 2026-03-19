using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Threading.Tasks;
using BLL.Common.Exceptions;
using MODEL;

namespace BLL.Services
{
    internal interface ISwineService : IAnimalService<SwineEntity>
    {}
    /// <summary>
    /// Implementa regras de negócio específicas para suínos.
    /// </summary>
    public sealed class SwineService : AnimalServiceBase<SwineEntity>, ISwineService
    {
        /// <summary>
        /// Inicializa o service de suínos utilizando o port genérico de repositório.
        /// </summary>
        /// <param name="repository">Port adaptado para acesso a dados de suínos.</param>
        public SwineService(IAnimalRepository<SwineEntity> repository, IPhotoStorage photoStorage) : base(repository, photoStorage)
        {
        }

        /// <summary>
        /// Aplica validações específicas de suínos.
        /// </summary>
        /// <param name="entity">Entidade a ser validada.</param>
        protected override void ValidateSpecificRules(SwineEntity entity)
        {
            // -------------------- PorcType (obrigatório) --------------------
            if (entity.PorcType is null) throw new BusinessRuleException("O tipo do suíno (PorcType) deve ser informado.");

            // -------------------- Gender (regra de negócio) --------------------
            if (entity.Gender == Gender.Unknown) throw new BusinessRuleException("O gênero do suíno deve ser informado.");
        }

        /// <summary>
        /// Aplica validações específicas antes de excluir um suíno.
        /// </summary>
        /// <param name="entity">Entidade que será excluída.</param>
        /// <param name="ct">Token de cancelamento.</param>
        protected override Task ValidateDeleteSpecificRulesAsync(SwineEntity entity, CancellationToken ct)
        {
            // Futuro:
            // - não excluir se estiver em Batch / Parentage / etc.

            return Task.CompletedTask;
        }
    }
}

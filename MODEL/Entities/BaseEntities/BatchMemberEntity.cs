using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{

    /// <summary>
    /// Classe associativa entre <see cref="AnimalEntity"/> e <see cref="BatchEntity"/>
    /// <para><see cref="AnimalId"/>       -> <see cref="AnimalEntity"/>.Id.    </para>
    /// <para><see cref="BatchId"/>        -> <see cref="BatchEntity"/>.Id.     </para>
    /// <para><see cref="BatchEntryDate"/> -> Data de entrada do animal.        </para>
    /// <para><see cref="BatchExitDate"/>? -> Data de saída do animal.          </para>
    /// <para><see cref="ExitReason"/>?    -> Motivo da saída do animal.        </para>
    /// </summary>
    public abstract class BatchMemberEntity : BaseEntity
    {
        private Guid _animalId;
        private Guid _batchId;
        private DateTimeOffset _batchEntryDate;
        private DateTimeOffset? _batchExitDate;
        private string? _exitReason;


        /// <summary>Referencia -> <see cref="AnimalEntity"/>.Id. </summary>
        public Guid AnimalId 
        {
            get => _animalId;
            set { _animalId = value; } 
        }

        /// <summary>Referencia -> <see cref="BatchEntity"/>.Id. </summary>
        public Guid BatchId 
        { 
            get => _batchId;
            set { _batchId = value; } 
        }


        public DateTimeOffset BatchEntryDate 
        { 
            get => _batchEntryDate;
            set { _batchEntryDate = value; } 
        }
        public DateTimeOffset? BatchExitDate 
        { 
            get => _batchExitDate;
            set { _batchExitDate = value; }
        }
        public string? ExitReason 
        { 
            get => _exitReason;
            set { _exitReason = value; } 
        }

        public BatchMemberEntity(Guid animalId, Guid batchId)
        {
            AnimalId = animalId;
            BatchId = batchId;
        }
    }

}

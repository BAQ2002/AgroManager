using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class BatchAnimalEntity : BaseEntity
    {
        private Guid _animalId;
        private Guid _batchId;
        private DateTimeOffset _batchEntryDate;
        private DateTimeOffset? _batchExitDate;
        private string? _exitReason;

        public Guid AnimalId 
        {
            get => _animalId;
            set { _animalId = value; } 
        }
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
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public struct BatchAnimalStruct //Classe associativa entre batch e Animal
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        
        public Guid AnimalId { get; set; }
        public Guid BatchId { get; set; }
        public DateTimeOffset BatchEntryDate { get; set; }
        public DateTimeOffset? BatchExitDate { get; set; }
        public string? ExitReason { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DataBaseStructs
{
    public struct FiliationStruct
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt {get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public Guid AnimalId { get; set; }
        public Guid FatherId { get; set; }
        public Guid MotherId { get; set; }
        public Guid SurrogateMotherId { get; set; }
        public int FatherFlag { get; set; }
        public int MotherFlag { get; set; }
        public int SurrogateMotherFlag { get; set; }
    }
}

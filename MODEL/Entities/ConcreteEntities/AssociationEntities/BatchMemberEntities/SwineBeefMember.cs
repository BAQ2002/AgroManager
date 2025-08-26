using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class SwineBeefMember : BatchMemberEntity
    {
        public SwineBeefMember(Guid animalId, Guid batchId) : base(animalId, batchId)
        {
        }
    }
}

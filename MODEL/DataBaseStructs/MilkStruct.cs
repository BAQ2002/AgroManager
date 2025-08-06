using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public struct MilkStruct
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public Guid cowId { get; set; }
        
        public DateOnly? OcurranceDate { get; set; } //Data que ocorreu a ordenha
        public float Liters {  get; set; }
        //public string? propriedade1 { get; set; }
        //public string? propriedade2 { get; set; }
        //public string? propriedadeETC { get; set; }
    }
}

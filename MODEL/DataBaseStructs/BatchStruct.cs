using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{    
    /// <summary>
    /// Struct utilizada para traduzir BatchEntity para o DataBase
    /// </summary>
    public struct BatchStruct
    {
        public Guid Id {  get; set; }            
        public DateTimeOffset CreatedAt {get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MODEL
{
    public struct AnimalStruct
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public string? Name { get; set; }
        public int Origin { get; set; }
        public int Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public DateOnly? PurchaseDate { get; set; }
        public DateOnly? DeathDate { get; set; }

    }
}

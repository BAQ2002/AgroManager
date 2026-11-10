using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    /// <summary>
    /// Milk measurement associated with a bovine.
    /// </summary>
    public sealed class BovineMilk : MilkEntity
    {
        private Guid _bovineId;

        public Guid BovineId
        {
            get => _bovineId;
            set {
                if (value == Guid.Empty)
                    throw new ArgumentException("A bovine identifier is required.", nameof(value));
                
                _bovineId = value; 
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public sealed class BovineWeight : WeightEntity
    {
        private Guid _bovineId;

        public Guid BovineId
        {
            get => _bovineId;
            set { _bovineId = value; }
        }
    }
}

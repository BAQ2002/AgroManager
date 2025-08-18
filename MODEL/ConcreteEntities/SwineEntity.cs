using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class SwineEntity : AnimalEntity
    {
        private PorcType? _porcType = null;
        public PorcType? PorcType
        {
            get => _porcType;
            set { _porcType = value; }
        }

    }
}

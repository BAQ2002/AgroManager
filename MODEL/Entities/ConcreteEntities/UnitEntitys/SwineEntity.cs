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
        private SwineBreed? _breed = null;

        public PorcType? PorcType
        {
            get => _porcType;
            set { _porcType = value; }
        }
        public SwineBreed? Breed
        {
            get => _breed;
            set { _breed = value; }
        }

    }
}

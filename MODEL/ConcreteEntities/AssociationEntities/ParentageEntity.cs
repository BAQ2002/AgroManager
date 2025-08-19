using System;

namespace MODEL
{
       
    /// Informações de parentesco do animal.
    public class ParentageEntity : BaseEntity
    {
        private BreedingType _type = BreedingType.Unknown;
        public BreedingType BreedingType
        {
            get => _type;
            set { _type = value; }
        }
        // ------------------ Pai -------------------------------------------
        private Guid _fatherId;    // id do pai
        private ParentFlag _fatherFlag = ParentFlag.Unknown; //Descreve se o pai é interno ou externo

        public Guid FatherId
        {
            get => _fatherId;
            set { _fatherId = value; }
        }

        public ParentFlag FatherFlag
        {
            get => _fatherFlag;
            set { _fatherFlag = value; }
        }

        // ------------------ Mães ------------------------------------------
        private Guid _motherId; //Id da mãe genética
        private Guid? _surrogateMotherId; //Id da mãe de aluguel (se tiver)

        private ParentFlag _motherFlag = ParentFlag.Unknown; //Descreve se a mãe é interno ou externo                                             
        private ParentFlag? _surrogateMotherFlag = ParentFlag.Unknown; //Descreve se a mãe de aluguel é interno ou externo (se tiver)    

        public Guid MotherId
        {
            get => _motherId;
            set { _motherId = value; }
        }

        public Guid? SurrogateMotherId
        {
            get => _surrogateMotherId;
            set { _surrogateMotherId = value; }
        }

        public ParentFlag MotherFlag
        {
            get => _motherFlag;
            set { _motherFlag = value; }
        }

        public ParentFlag? SurrogateMotherFlag
        {
            get => _surrogateMotherFlag;
            set { _surrogateMotherFlag = value; }
        }

    }
}

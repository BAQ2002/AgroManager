using System;

namespace MODEL
{
    /// <summary>
    /// Informações de parentesco do animal.
    /// </summary>
    public abstract class ParentageEntity : BaseEntity
    {
        #region ------------------ Tipo de Cruza -------------------------------------------

        private BreedingType _type = BreedingType.Unknown;
        public BreedingType BreedingType
        {
            get => _type;
            set { _type = value; }
        }
        #endregion

        #region ------------------ Pai -------------------------------------------

        private Guid? _fatherId;    // id do pai
        private ParentFlag _fatherFlag = ParentFlag.Unknown; //Descreve se o pai é interno ou externo

        public Guid? FatherId
        {
            get => _fatherId;
            set { _fatherId = value; }
        }

        public ParentFlag FatherFlag
        {
            get => _fatherFlag;
            set { _fatherFlag = value; }
        }
        #endregion

        #region ------------------ Mãe ------------------------------------------

        private Guid _motherId; //Id da mãe genética    
        private ParentFlag _motherFlag = ParentFlag.Unknown; //Descreve se a mãe é interno ou externo
                                                             
        public Guid MotherId
        {
            get => _motherId;
            set { _motherId = value; }
        }
        public ParentFlag MotherFlag
        {
            get => _motherFlag;
            set { _motherFlag = value; }
        }
        #endregion

        #region ------------------ Mãe de Aluguel ------------------------------------------

        private Guid? _surrogateMotherId; //Id da mãe de aluguel (se tiver)
        private ParentFlag _surrogateMotherFlag = ParentFlag.Unknown; //Descreve se a mãe de aluguel é interno ou externo (se tiver)

        public Guid? SurrogateMotherId
        {
            get => _surrogateMotherId;
            set { _surrogateMotherId = value; }
        }

        public ParentFlag SurrogateMotherFlag
        {
            get => _surrogateMotherFlag;
            set { _surrogateMotherFlag = value; }
        }
        #endregion
    }
}

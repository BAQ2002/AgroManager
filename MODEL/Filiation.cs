using System;

namespace MODEL
{
    #region ---------FILIATION-------------
    // Informações de filiação (Parentesco entre animais)
    public enum BreedingType     // Tipo de cruza
    {
        Unknown = 0,             // desconhecido   
        Natural,                 // monta natural
        ArtificialInsemination   // sêmen utilizado Inseminação Artificial
    }

    public enum ParentFlag
    {
        Unknown = 0,
        Internal,
        External
    }
    #endregion
    /// Informações de parentesco do animal.
    public class Filiation
    {
        private BreedingType _type = BreedingType.Unknown;

        // ------------------ Pai -------------------------------------------
        private int _fatherId;    // id do pai
        private ParentFlag _fatherFlag = ParentFlag.Unknown; //Descreve se o pai é interno ou externo

        public int FatherId
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
        private int _motherId; //Id da mãe genética
        private int? _surrogateMotherId; //Id da mãe de aluguel (se tiver)

        private ParentFlag _motherFlag = ParentFlag.Unknown; //Descreve se a mãe é interno ou externo                                             
        private ParentFlag? _surrogateMotherFlag = ParentFlag.Unknown; //Descreve se a mãe de aluguel é interno ou externo (se tiver)    

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

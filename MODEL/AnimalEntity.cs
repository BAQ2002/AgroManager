using System;

namespace MODEL
{
    #region CLASSE ANIMAL --------------------------------------

    public abstract class AnimalEntity : BaseEntity
    {
        #region Private Propeties --------------------------------------
        private string? _name = null; //Nome ou apelido do animal (opcional)
        private Origin _origin = Origin.Unknown; 
        private Gender _gender = Gender.None;
        private DateOnly? _birthDate = null;
        private DateOnly? _purchaseDate = null;
        private DateOnly? _deathDate = null;
        private Filiation? _filiation; // Informações sobre pai/mãe.

        #endregion

        #region Public Propeties --------------------------------------
        public string? Name 
        { 
            get => _name; 
            protected set { _name = value; } 
        } 

        public Origin Origin 
        {
            get => _origin;
            protected set { _origin = value; }
        }

        public Gender Gender 
        { 
            get => _gender; 
            protected set { _gender = value; }
        } 

        public DateOnly? BirthDate 
        { 
            get => _birthDate; 
            protected set { _birthDate = value; } 
        }

        public DateOnly? PurchaseDate 
        {
            get => _purchaseDate; 
            protected set { _purchaseDate = value; }
        }

        public DateOnly? DeathDate 
        {
            get => _deathDate; 
            protected set { _deathDate = value; }
        }

        public Filiation? Filiation {
            get => _filiation; 
            protected set { _filiation = value; }
        }

        #endregion
        /// Refatorar Age para entregar a idade em valor mais granular (Anos e Meses)
        public int? Age =>
            BirthDate is null
                ? null
                : (int)Math.Floor(
                    (DateTime.Today - BirthDate.Value.ToDateTime(TimeOnly.MinValue))
                    .TotalDays / 365.25);

        public AnimalEntity(string name,Gender gender = Gender.None,
                      DateOnly? birthDate = null, DateOnly? purchaseDate = null, Filiation? filiation = null)         
        {   
            Name = name;
            Gender = gender;
            BirthDate = birthDate;
            PurchaseDate = purchaseDate;
            Filiation = filiation ?? new Filiation();
        }    
    }
#endregion
}

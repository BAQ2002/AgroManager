using Fazendinha.Model;
using System;

namespace MODEL
{
    #region CLASSE ANIMAL --------------------------------------
    public interface IHealthTracker
    {

    }
    public interface IFoodTracker
    {

    }
    public abstract class AnimalEntity : BaseEntity
    {
        #region Private Propeties --------------------------------------
        private string? _name = null; //Nome ou apelido do animal (opcional)
        private AcquisitionOrigin _origin = AcquisitionOrigin.Unknown; 
        private Gender _gender = Gender.Unknown;
        private DateOnly? _birthDate = null;
        private DateOnly? _purchaseDate = null;
        private DateOnly? _deathDate = null;
        private IHealthTracker HealthTracker;
        private IWeightTracker weightTracker;
        private IFoodTracker foodTracker;
        #endregion

        #region Public Propeties --------------------------------------
        public string? Name 
        { 
            get => _name; 
            set { _name = value; } 
        } 

        public virtual AcquisitionOrigin Origin 
        {
            get => _origin;
            set { _origin = value; }
        }

        public virtual Gender Gender 
        { 
            get => _gender; 
            set { _gender = value; }
        } 

        public DateOnly? BirthDate 
        { 
            get => _birthDate; 
            set { _birthDate = value; } 
        }

        public DateOnly? PurchaseDate 
        {
            get => _purchaseDate; 
            set { _purchaseDate = value; }
        }

        public DateOnly? DeathDate 
        {
            get => _deathDate; 
            set { _deathDate = value; }
        }


        #endregion
        /// Refatorar Age para entregar a idade em valor mais granular (Anos e Meses)
        public int? Age =>
            BirthDate is null ? null
                : (int)Math.Floor(
                    (DateTime.Today - BirthDate.Value.ToDateTime(TimeOnly.MinValue)).TotalDays / 365.25);

        public AnimalEntity() { }

        public AnimalEntity(string name,Gender gender = Gender.Unknown,
                      DateOnly? birthDate = null, DateOnly? purchaseDate = null, ParentageEntity? filiation = null)         
        {   
            Name = name;
            Gender = gender;
            BirthDate = birthDate;
            PurchaseDate = purchaseDate;
        }    
    }
#endregion
}

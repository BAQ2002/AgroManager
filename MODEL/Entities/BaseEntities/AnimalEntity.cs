using Fazendinha.Model;
using System;

namespace MODEL
{
    public abstract class AnimalEntity : BaseEntity
    {
        #region Private Propeties --------------------------------------
        private string? _name = null; //Nome ou apelido do animal (opcional)
        private AcquisitionOrigin _origin = AcquisitionOrigin.Unknown;
        private Gender _gender = Gender.Unknown;
        private DateOnly? _birthDate = null;
        private DateOnly? _purchaseDate = null;
        private DateOnly? _deathDate = null;
        private string? _photoKey = null; // Chave lógica do objeto no storage (bucket/key)

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

        public string? PhotoKey
        {
            get => _photoKey;
            set { _photoKey = string.IsNullOrWhiteSpace(value) ? null : value.Trim(); }
        }

        #endregion

        /// <summary>
        /// Idade em anos completos para manter compatibilidade com os consumidores atuais.
        /// </summary>
        public int? Age => GetAge(AgeUnit.Years);

        /// <summary>
        /// Retorna a idade em dias, meses ou anos completos com base na data de nascimento.
        /// </summary>
        public int? GetAge(AgeUnit unit, DateOnly? referenceDate = null)
        {
            if (BirthDate is null)
                return null;

            DateOnly targetDate = referenceDate ?? DateOnly.FromDateTime(DateTime.Today);
            if (targetDate < BirthDate.Value)
                return 0;

            return unit switch
            {
                AgeUnit.Days => targetDate.DayNumber - BirthDate.Value.DayNumber,
                AgeUnit.Months => GetTotalMonths(BirthDate.Value, targetDate),
                _ => GetTotalYears(BirthDate.Value, targetDate)
            };
        }

        /// <summary>
        /// Retorna todas as unidades de idade em uma única chamada.
        /// </summary>
        public AnimalAgeInfo? GetAgeInfo(DateOnly? referenceDate = null)
        {
            if (BirthDate is null)
                return null;

            DateOnly targetDate = referenceDate ?? DateOnly.FromDateTime(DateTime.Today);
            if (targetDate < BirthDate.Value)
                return new AnimalAgeInfo(0, 0, 0);

            return new AnimalAgeInfo(
                Days: targetDate.DayNumber - BirthDate.Value.DayNumber,
                Months: GetTotalMonths(BirthDate.Value, targetDate),
                Years: GetTotalYears(BirthDate.Value, targetDate));
        }

        private static int GetTotalYears(DateOnly birthDate, DateOnly referenceDate)
        {
            int years = referenceDate.Year - birthDate.Year;

            if (referenceDate < birthDate.AddYears(years))
                years--;

            return years < 0 ? 0 : years;
        }

        private static int GetTotalMonths(DateOnly birthDate, DateOnly referenceDate)
        {
            int months = (referenceDate.Year - birthDate.Year) * 12 + (referenceDate.Month - birthDate.Month);

            if (referenceDate.Day < birthDate.Day)
                months--;

            return months < 0 ? 0 : months;
        }

        public AnimalEntity() { }

        public AnimalEntity(string name, Gender gender = Gender.Unknown,
                      DateOnly? birthDate = null, DateOnly? purchaseDate = null, ParentageEntity? filiation = null)
        {
            Name = name;
            Gender = gender;
            BirthDate = birthDate;
            PurchaseDate = purchaseDate;
        }
    }
}

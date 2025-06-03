using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public int? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is not PersonResponse other) return false;

            return PersonID == other.PersonID &&
                   PersonName == other.PersonName &&
                   Email == other.Email &&
                   DateOfBirth == other.DateOfBirth &&
                   Gender == other.Gender &&
                   CountryID == other.CountryID &&
                   CountryName == other.CountryName &&
                   Address == other.Address &&
                   ReceiveNewsLetters == other.ReceiveNewsLetters &&
                   Age == other.Age;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"PersonResponse: {PersonID}, {PersonName}, {Email}, {DateOfBirth?.ToString("dd MMM yyyy")}, {Gender}, {CountryID}, {CountryName}, {Address}, {ReceiveNewsLetters}, {Age}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters,
            };
        }
    }

    public static class PersonResponseExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = Enum.TryParse(person.Gender, out GenderOptions gender) ? gender : GenderOptions.PreferNotToSay,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = person.DateOfBirth.HasValue ? (int?)((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }
}

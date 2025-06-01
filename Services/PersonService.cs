using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons = new List<Person>();
        private readonly ICountryService _countryService = new CountryService();

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countryService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            // Check if personAddRequest is not null.
            // Validate all properties of personAddRequest.
            // Convert personAddRequest to Person.
            // Generate a new PersonID
            // Add it to List<Person>
            // Return PersonResponse object with generated PersonID.

            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            #region Property Validations

            ValidationHelper.ModelValidation(personAddRequest);

            if (_persons.Any(p =>
                !string.IsNullOrEmpty(p.Email) &&
                p.Email == personAddRequest.Email))
            {
                throw new ArgumentException(nameof(personAddRequest.Email), "Email already exists.");
            }

            if (personAddRequest.DateOfBirth.HasValue && (personAddRequest.DateOfBirth.Value > DateTime.Now || personAddRequest.DateOfBirth.Value >= DateTime.Now.AddYears(-17)))
            {
                throw new ArgumentException(nameof(personAddRequest.DateOfBirth), "Date of Birth must be at least 18 years old and cannot be in the future.");
            }

            #endregion

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();

            _persons.Add(person);

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            throw new NotImplementedException();
        }
    }
}

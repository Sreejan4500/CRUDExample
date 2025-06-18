using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons = new List<Person>();
        private readonly ICountryService _countryService = new CountryService();

        public PersonService(bool initializePersons = true)
        {
            if(!initializePersons) return;

            // Initialize with some dummy data for testing purposes.
            _persons = new List<Person>        
            {
                new Person
                {
                    PersonID = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PersonName = "John Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    Gender = GenderOptions.Male.ToString(),
                    CountryID = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Address = "123 Main St, Cityville",
                    ReceiveNewsLetters = true
                },
                new Person
                {
                    PersonID = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    PersonName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = new DateTime(1985, 8, 23),
                    Gender = GenderOptions.Female.ToString(),
                    CountryID = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Address = "456 Oak Ave, Townsville",
                    ReceiveNewsLetters = false
                },
                new Person
                {
                    PersonID = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    PersonName = "Alex Johnson",
                    Email = "alex.johnson@example.com",
                    DateOfBirth = new DateTime(1992, 12, 2),
                    Gender = GenderOptions.Other.ToString(),
                    CountryID = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Address = "789 Pine Rd, Villagetown",
                    ReceiveNewsLetters = true
                },
                new Person
                {
                    PersonID = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    PersonName = "Sam Lee",
                    Email = "sam.lee@example.com",
                    DateOfBirth = new DateTime(1978, 3, 30),
                    Gender = GenderOptions.Male.ToString(),
                    CountryID = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Address = "321 Maple St, Hamlet",
                    ReceiveNewsLetters = false
                },
                new Person
                {
                    PersonID = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    PersonName = "Morgan Brown",
                    Email = "morgan.brown@example.com",
                    DateOfBirth = new DateTime(1988, 7, 19),
                    Gender = GenderOptions.PreferNotToSay.ToString(),
                    CountryID = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    Address = "654 Cedar Ave, Borough",
                    ReceiveNewsLetters = true
                }
            };
        }

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
            // Convert each Person in List<Person> to PersonResponse and return the list.
            return _persons.Select(ConvertPersonToPersonResponse).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            // Check if personID is not null.
            // Find the person with the given personID in List<Person>.
            // If found, convert it to PersonResponse and return it.
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID), "Person ID cannot be null.");
            }

            Person? person = _persons.FirstOrDefault(p => p.PersonID == personID.Value);

            if (person == null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            // Check if searchBy is not null or empty.
            // Get all persons from List<Person> and filter them based on searchBy and searchString.
            // If searchBy is "PersonName", filter by PersonName.
            // Convert each filtered Person to PersonResponse and return the list.
            
            List<PersonResponse> getAllPersons = GetAllPersons();
            List<PersonResponse> filteredPersons = getAllPersons;

            if (string.IsNullOrWhiteSpace(searchBy) && string.IsNullOrEmpty(searchString))
                return filteredPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    filteredPersons = getAllPersons
#pragma warning disable CS8604 // Possible null reference argument.
                        .Where(p => !string.IsNullOrEmpty(p.PersonName) && p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;

                case nameof(PersonResponse.Email):
                    filteredPersons = getAllPersons
                        .Where(p => !string.IsNullOrEmpty(p.Email) && p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):                                        
                    if (DateTime.TryParse(searchString, out DateTime dateOfBirth))
                    {
                        filteredPersons = getAllPersons
                            .Where(p => p.DateOfBirth.HasValue && p.DateOfBirth.Value.Date == dateOfBirth.Date)
                            .ToList();
                    }
                    break;

                case nameof(PersonResponse.CountryID):
                    if (Guid.TryParse(searchString, out Guid countryID))
                    {
                        filteredPersons = getAllPersons
                            .Where(p => p.CountryID.HasValue && p.CountryID.Value == countryID)
                            .ToList();
                    }
                    break;

                case nameof(PersonResponse.Gender):
                    filteredPersons = getAllPersons
                        .Where(p => !string.IsNullOrEmpty(p.Gender.ToString()) && p.Gender.ToString().Equals(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;

                case nameof(PersonResponse.Address):
                    filteredPersons = getAllPersons
                        .Where(p => !string.IsNullOrEmpty(p.Address) && p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;

                case nameof(PersonResponse.ReceiveNewsLetters):
                    if (bool.TryParse(searchString, out bool receiveNewsLetters))
                    {
                        filteredPersons = getAllPersons
                            .Where(p => p.ReceiveNewsLetters == receiveNewsLetters)
                            .ToList();
                    }
                    break;
                default:
                    filteredPersons = getAllPersons;
                    break;
            }

            return filteredPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if(sortBy == null) 
                return allPersons;

            List<PersonResponse> sortedPersons = new List<PersonResponse>();

            switch (sortBy)
            {
                case nameof(PersonResponse.PersonName):
                    if (sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
                    break;

                case nameof(PersonResponse.CountryName):
                    if (sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.CountryName, StringComparer.OrdinalIgnoreCase).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    if (sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList();
                    break;

                case nameof(PersonResponse.ReceiveNewsLetters):
                    if (sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    if(sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    if (sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.DateOfBirth).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.DateOfBirth).ToList();
                    break;

                case nameof(PersonResponse.Age):
                    if (sortOrder == SortOrderOptions.ASC)
                        sortedPersons = allPersons.OrderBy(p => p.Age).ToList();
                    else
                        sortedPersons = allPersons.OrderByDescending(p => p.Age).ToList();
                    break;

                default:
                    sortedPersons = allPersons;
                    break;
            }
            return sortedPersons;
        }

        public PersonResponse? UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            // Check if personUpdateRequest is not null.
            // Validate all properties of personUpdateRequest.
            // Find the person with the given PersonID in List<Person>.
            // If not found, return null.
            // If found, update the properties of the person with the values from personUpdateRequest.
            // Convert the updated person to PersonResponse and return it.

            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest), "Person update request cannot be null.");
            }

            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonID == personUpdateRequest.PersonID);

            if (matchingPerson == null)
            {
                return null;
            }

            matchingPerson = personUpdateRequest.ToPerson();

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            // Check if personID is not null.
            // Get the matching Person from List<Person> based on the given personID.
            // If not found, return false.
            // If found, remove it from List<Person> and return true.

            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID), "Person ID cannot be null.");
            }
            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonID == personID.Value);
            if (matchingPerson == null) 
                return false;

            return _persons.Remove(matchingPerson); // Returns true if the person was successfully removed, false otherwise.
        }
    }
}

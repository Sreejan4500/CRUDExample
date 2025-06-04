using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService = new PersonService();
        private readonly ICountryService _countryService = new CountryService();
        private readonly ITestOutputHelper _outputHelper;

        public PersonServiceTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        #region AddPerson Tests

        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_NullPersonName()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest { PersonName = null };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "John Doe",
                Email = "johndoe12@gmail.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = GenderOptions.Male,
                CountryID = Guid.NewGuid(),
                Address = "123 Main St, Springfield, USA",
                ReceiveNewsLetters = true,
            };
            // Act
            var response = _personService.AddPerson(personAddRequest);
            List<PersonResponse> personResponses = new List<PersonResponse> { response };
            // Assert
            Assert.NotNull(response);
            Assert.True(response.PersonID != Guid.Empty);
            Assert.Contains(response, personResponses);
        }

        [Fact]
        public void AddPerson_ProperEmailFormat()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest { Email = "janedoe" };
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_DuplicateEmail()
        {
            // Arrange
            PersonAddRequest personAddRequest1 = new PersonAddRequest { PersonName = "Jane", CountryID = Guid.NewGuid(), Email = "jane@gmail.com" };
            PersonAddRequest personAddRequest2 = new PersonAddRequest { PersonName = "Jane", CountryID = Guid.NewGuid(), Email = "jane@gmail.com" };

            _personService.AddPerson(personAddRequest1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest2));
        }

        [Fact]
        public void AddPerson_MinimumAgeRequirement()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest { PersonName = "Jane", DateOfBirth = DateTime.Now.AddYears(-17), }; // Less than 18 years old
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
        }

        #endregion

        #region GetAllPersons Tests

        [Fact]
        public void GetAllPersons_EmptyListBeforeAddingPersons()
        {
            // Act
            List<PersonResponse> persons = _personService.GetAllPersons();

            // Assert
            Assert.Empty(persons);
        }

        [Fact]
        public void GetAllPersons_ListOfPersonsAfterAddingPersons()
        {
            // Arrange            
            List<PersonResponse> personResponses_from_add = PersonServiceTestHelper.AddPersonsToService(_countryService, _personService);


            // Act
            List<PersonResponse> personResponses_from_get = _personService.GetAllPersons();

            #region Print the output for debugging

            _outputHelper.WriteLine($"Expected:\nTotal Persons: {personResponses_from_add.Count}");
            foreach (PersonResponse personResponse_from_add in personResponses_from_add)
            {
                _outputHelper.WriteLine($"PersonID: {personResponse_from_add.PersonID}, Name: {personResponse_from_add.PersonName}, Email: {personResponse_from_add.Email}");
            }

            _outputHelper.WriteLine($"Actual:\nTotal Persons: {personResponses_from_get.Count}");
            foreach (PersonResponse personResponse_from_get in personResponses_from_get)
            {
                _outputHelper.WriteLine($"PersonID: {personResponse_from_get.PersonID}, Name: {personResponse_from_get.PersonName}, Email: {personResponse_from_get.Email}");
            }

            #endregion

            // Assert
            foreach (PersonResponse personResponse in personResponses_from_add)
            {
                Assert.Contains(personResponse, personResponses_from_get);
            }
        }

        #endregion

        #region GetPersonByID Tests

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            // Arrange
            Guid? personID = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.GetPersonByPersonID(personID));
        }

        [Fact]
        public void GetPersonByPersonID_ValidPersonID()
        {
            // Arrange
            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "John Doe",
                Email = "john@gmail.com",
                CountryID = Guid.NewGuid(),
                DateOfBirth = new DateTime(2000, 5, 4)
            };

            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            // Act
            PersonResponse? personResponse_from_get = _personService.GetPersonByPersonID(personResponse_from_add.PersonID);

            // Asert
            Assert.Equal(personResponse_from_add, personResponse_from_get);

        }

        [Fact]
        public void GetPersonByPersonID_InvalidPersonID()
        {
            // Arrange
            Guid personID = Guid.NewGuid(); // Assuming this ID does not exist
            // Act
            PersonResponse? personResponse = _personService.GetPersonByPersonID(personID);
            // Assert
            Assert.Null(personResponse);
        }
        #endregion


        #region GetFilteredPersons Tests

        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            // Arrange
            List<PersonResponse> personResponses_from_add = PersonServiceTestHelper.AddPersonsToService(_countryService, _personService);

            // Act
            List<PersonResponse> personResponses_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            #region Print the output for debugging

            _outputHelper.WriteLine($"Expected:\nTotal Persons: {personResponses_from_add.Count}");
            foreach (PersonResponse personResponse_from_add in personResponses_from_add)
            {
                _outputHelper.WriteLine($"PersonID: {personResponse_from_add.PersonID}, Name: {personResponse_from_add.PersonName}, Email: {personResponse_from_add.Email}");
            }

            _outputHelper.WriteLine($"Actual:\nTotal Persons: {personResponses_from_search.Count}");
            foreach (PersonResponse personResponse_from_search in personResponses_from_search)
            {
                _outputHelper.WriteLine($"PersonID: {personResponse_from_search.PersonID}, Name: {personResponse_from_search.PersonName}, Email: {personResponse_from_search.Email}");
            }

            #endregion

            // Assert
            foreach (PersonResponse personResponse in personResponses_from_add)
            {
                Assert.Contains(personResponse, personResponses_from_search);
            }
        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            // Arrange            
            List<PersonResponse> personResponses_from_add = PersonServiceTestHelper.AddPersonsToService(_countryService, _personService);

            // Act
            List<PersonResponse> personResponses_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            #region Print the output for debugging

            _outputHelper.WriteLine($"Expected:\nTotal Persons: {personResponses_from_add.Count}");
            foreach (PersonResponse personResponse_from_add in personResponses_from_add)
            {
                if (personResponse_from_add.PersonName != null && personResponse_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    _outputHelper.WriteLine($"PersonID: {personResponse_from_add.PersonID}, Name: {personResponse_from_add.PersonName}, Email: {personResponse_from_add.Email}");
            }

            _outputHelper.WriteLine($"Actual:\nTotal Persons: {personResponses_from_search.Count}");
            foreach (PersonResponse personResponse_from_search in personResponses_from_search)
            {
                _outputHelper.WriteLine($"PersonID: {personResponse_from_search.PersonID}, Name: {personResponse_from_search.PersonName}, Email: {personResponse_from_search.Email}");
            }

            #endregion

            // Assert
            foreach (PersonResponse personResponse_from_add in personResponses_from_add)
            {
                if (personResponse_from_add.PersonName != null && personResponse_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.Contains(personResponse_from_add, personResponses_from_search);
                }
            }
        }

        #endregion

        #region SortPersons Tests

        [Fact]
        public void GetSortedPersons_DescOrder()

        {
            // Arrange            
            List<PersonResponse> personResponses_from_add = PersonServiceTestHelper.AddPersonsToService(_countryService, _personService);
            personResponses_from_add = personResponses_from_add.OrderByDescending(p => p.PersonName).ToList(); // Sort by PersonName in descending order

            // Act
            List<PersonResponse> getAllPersons = _personService.GetAllPersons();
            List<PersonResponse> personResponses_from_sort = _personService.GetSortedPersons(getAllPersons, nameof(Person.PersonName), SortOrderOptions.DESC);


            // Assert
            for (int i = 0; i < personResponses_from_add.Count; i++)
            {
                Assert.Equal(personResponses_from_add[i].PersonName, personResponses_from_sort[i].PersonName);
            }
        }

        #endregion


        #region UpdatePerson Tests

        [Fact]
        public void UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequest? personUpdateRequest = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            // Arrange
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = Guid.NewGuid(), // Assuming this ID does not exist
                PersonName = "Sayan",
                Email = "sayan@gmail.com",
            };

            // Act & Assert
            Assert.Null(_personService.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "India" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "Satan",
                Email = "satan@gmail.com",
                CountryID = countryResponse.CountryID
            };

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = personResponse.PersonID,
                PersonName = null,
                Email = "satan@smail.com"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.UpdatePerson(personUpdateRequest));
        }

        [Fact]
        public void UpdatePerson_ProperPersonDetails()
        {
            // Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "India" };
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "Satan",
                Email = "satan@gmail.com",
                CountryID = countryResponse.CountryID
            };

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = personResponse.PersonID,
                PersonName = "Sayan",
                Email = "sayan@gmail.com"
            };

            // Act
            PersonResponse? updatedPersonResponse = _personService.UpdatePerson(personUpdateRequest);

            // Assert
            Assert.NotNull(updatedPersonResponse);
        }

        #endregion

        #region DeletePerson Tests

        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            // Arrange
            CountryAddRequest country = new CountryAddRequest { CountryName = "India" };
            CountryResponse _countryResponse = _countryService.AddCountry(country);

            PersonAddRequest person = new PersonAddRequest
            {
                PersonName = "Virat",
                Email = "viratkohli18@gmail.com",
                CountryID = _countryResponse.CountryID
            };

            PersonResponse personResponse = _personService.AddPerson(person);

            // Act & Assert
            Assert.True(_personService.DeletePerson(personResponse.PersonID));
        }

        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            // Arrange
            Guid personID = Guid.NewGuid(); // A new Guid, considering it doesn't exist.

            // Act & Assert
            Assert.False(_personService.DeletePerson(personID));
        }

        #endregion

    }

    public class PersonServiceTestHelper
    {
        public static List<PersonResponse> AddPersonsToService(ICountryService _countryService, IPersonService _personService)
        {
            CountryAddRequest country1 = new CountryAddRequest { CountryName = "India" };
            CountryAddRequest country2 = new CountryAddRequest { CountryName = "USA" };

            CountryResponse countryResponse1 = _countryService.AddCountry(country1);
            CountryResponse countryResponse2 = _countryService.AddCountry(country2);

            PersonAddRequest person1 = new PersonAddRequest
            {
                PersonName = "Mary",
                Email = "mary@yahoo.com",
                CountryID = countryResponse1.CountryID
            };
            PersonAddRequest person2 = new PersonAddRequest
            {
                PersonName = "Juleus",
                Email = "juleus@yahoo.com",
                CountryID = countryResponse1.CountryID
            };
            PersonAddRequest person3 = new PersonAddRequest
            {
                PersonName = "August",
                Email = "august@yahoo.com",
                CountryID = countryResponse1.CountryID
            };

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>()
            {
                person1,
                person2,
                person3
            };

            List<PersonResponse> personResponses_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personService.AddPerson(personAddRequest);
                personResponses_from_add.Add(personResponse);
            }

            return personResponses_from_add;
        }
    }
}

using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService = new PersonService();

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

    }
}

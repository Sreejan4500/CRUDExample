using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService = new CountriesService();

        #region AddCountry Tests
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_NullCountryName()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request);
            });
        }

        // When CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            // Arrange
            CountryAddRequest? request1 = new()
            {
                CountryName = "India"
            };

            CountryAddRequest? request2 = new()
            {
                CountryName = "India"
            };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        // When you supply proper CountryName, it should insert into the list of countries.
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? request = new()
            {
                CountryName = "Japan"
            };

            // Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countryResponses = _countriesService.GetAllCountries();

            // Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countryResponses);
        }
        #endregion

        #region GetAllCountries Tests

        // When you call GetAllCountries, it should return an empty list of countries (before adding any countries).
        [Fact]
        public void GetAllCountries_EmptyListBeforeAddingCountries()
        {
            // Act
            List<CountryResponse> response = _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(response);
        }

        // When you call GetAllCountries, it should return a list of countries (after adding some countries).
        [Fact]
        public void GetAllCountries_ListOfCountriesAfterAddingCountries()
        {
            // Arrange
            CountryAddRequest? request1 = new()
            {
                CountryName = "India"
            };
            CountryAddRequest? request2 = new()
            {
                CountryName = "USA"
            };

            // Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
            List<CountryResponse> response = _countriesService.GetAllCountries();

            // Assert
            Assert.Equal(2, response.Count);
        }

        #endregion

        #region GetCountryByCountryID Tests

        /// <summary>
        /// Validates that the <see cref="_countriesService.GetCountryByCountryID(Guid?)"/> method  throws an <see
        /// cref="ArgumentNullException"/> when the <paramref name="countryID"/> parameter is null.
        /// </summary>
        /// <remarks>This test ensures that the method correctly handles null input for the <paramref
        /// name="countryID"/> parameter  by throwing the expected exception, adhering to the method's precondition
        /// requirements.</remarks>
        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            // Arrange
            Guid? countryID = null;
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _countriesService.GetCountryByCountryID(countryID);
            });
        }

        /// <summary>
        /// Tests the <see cref="_countriesService.GetCountryByCountryID"/> method to ensure it returns the correct 
        /// <see cref="CountryResponse"/> object when provided with a valid country ID.When you call GetCountryByCountryID with a valid country ID, it should return the corresponding country object.
        /// </summary>
        /// <remarks>This test verifies that the method retrieves the expected country details when a
        /// valid country ID is supplied. It ensures that the returned <see cref="CountryResponse"/> object is not null
        /// and that its properties match  the values of the country added earlier.</remarks>
        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            // Arrange
            CountryAddRequest? request = new()
            {
                CountryName = "Germany"
            };
            CountryResponse response = _countriesService.AddCountry(request);
            Guid? countryID = response.CountryID;
            // Act
            CountryResponse? countryResponse = _countriesService.GetCountryByCountryID(countryID);
            // Assert
            Assert.NotNull(countryResponse);
            Assert.Equal(response.CountryName, countryResponse.CountryName);
        }

        /// <summary>
        /// Tests the behavior of the <see cref="_countriesService.GetCountryByCountryID(Guid?)"/> method  when an invalid or
        /// non-existent country ID is provided.
        /// </summary>
        /// <remarks>This test verifies that the method returns <see langword="null"/> when the specified  <paramref
        /// name="countryID"/> does not correspond to any existing country.</remarks>
        [Fact]
        public void GetCountryByCountryID_InvalidCountryID()
        {
            // Arrange
            Guid? countryID = Guid.NewGuid(); // A random GUID that does not exist in the list
            // Act
            CountryResponse? countryResponse = _countriesService.GetCountryByCountryID(countryID);
            // Assert
            Assert.Null(countryResponse); // Should return null since no country with this ID exists
        }
        #endregion
    }
}

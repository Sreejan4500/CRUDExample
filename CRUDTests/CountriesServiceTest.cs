using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService = new CountriesService();

        #region AddCountry Tests

        /// <summary>
        /// Tests the <see cref="ICountriesService.AddCountry"/> method to ensure it throws an  <see
        /// cref="ArgumentNullException"/> when the <paramref name="request"/> parameter is null.
        /// </summary>
        /// <remarks>This test verifies that the <see cref="ICountriesService.AddCountry"/> method
        /// enforces  proper argument validation by throwing an exception when a null request is provided.</remarks>
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

        /// <summary>
        /// Tests that the <see cref="_countriesService.AddCountry"/> method throws an <see cref="ArgumentException"/> 
        /// when the <see cref="CountryAddRequest.CountryName"/> property is <see langword="null"/>.
        /// </summary>
        /// <remarks>This test ensures that the <see cref="_countriesService.AddCountry"/> method
        /// correctly validates  the <see cref="CountryAddRequest.CountryName"/> property and throws an appropriate
        /// exception  when the property is not set.</remarks>
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

        /// <summary>
        /// Tests the behavior of the <see cref="_countriesService.AddCountry"/> method when attempting to add  a
        /// country with a duplicate name.
        /// </summary>
        /// <remarks>This test verifies that the <see cref="_countriesService.AddCountry"/> method throws
        /// an  <see cref="ArgumentException"/> when a country with the same name is added more than once.</remarks>
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

        /// <summary>
        /// Tests the addition of a country with valid details and verifies that the country is successfully added.
        /// </summary>
        /// <remarks>This test ensures that the <see cref="_countriesService.AddCountry"/> method
        /// correctly adds a country when provided with valid input and that the added country is retrievable using <see
        /// cref="_countriesService.GetAllCountries"/>.</remarks>
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

        /// <summary>
        /// Tests that the <see cref="_countriesService.GetAllCountries"/> method returns an empty list when no
        /// countries have been added.
        /// </summary>
        /// <remarks>This test verifies the initial state of the <see cref="_countriesService"/> service,
        /// ensuring that calling <see cref="_countriesService.GetAllCountries"/> before adding any countries results in
        /// an empty list.</remarks>
        [Fact]
        public void GetAllCountries_EmptyListBeforeAddingCountries()
        {
            // Act
            List<CountryResponse> response = _countriesService.GetAllCountries();

            // Assert
            Assert.Empty(response);
        }

        /// <summary>
        /// Tests that the <see cref="_countriesService.GetAllCountries"/> method returns a list of countries  after
        /// adding multiple countries using the <see cref="_countriesService.AddCountry"/> method.
        /// </summary>
        /// <remarks>This test verifies that the <see cref="_countriesService.GetAllCountries"/> method
        /// correctly retrieves  all countries that were previously added via the <see
        /// cref="_countriesService.AddCountry"/> method.  It ensures that the count of returned countries matches the
        /// number of countries added.</remarks>
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

using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CountryAddRequest? request1 = new ()
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
    }
}

#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries = new List<Country>();

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            // Validate the input request: countryAddRequest should not be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest), "CountryAddRequest cannot be null.");
            }

            // Validate the country name: it should not be null or empty
            if (string.IsNullOrWhiteSpace(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }

            // Check for duplicate country names in the list
            if (_countries.Any(c => c.CountryName.Equals(countryAddRequest.CountryName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Country with name '{countryAddRequest.CountryName}' already exists.", nameof(countryAddRequest.CountryName));
            }

            Country? country = countryAddRequest?.ToCountry();
            country.CountryID = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse(); 
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            throw new NotImplementedException();
        }
    }
}

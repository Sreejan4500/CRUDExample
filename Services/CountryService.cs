#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly List<Country> _countries = new List<Country>();


        /// <summary>
        /// Adds a new country to the collection.
        /// </summary>
        /// <remarks>This method generates a unique identifier for the new country and ensures that
        /// duplicate country names are not allowed.</remarks>
        /// <param name="countryAddRequest">The request object containing the details of the country to be added.  The <see
        /// cref="CountryAddRequest.CountryName"/> property must not be null or empty.</param>
        /// <returns>A <see cref="CountryResponse"/> object representing the added country, including its generated unique
        /// identifier.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="countryAddRequest"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <see cref="CountryAddRequest.CountryName"/> is null, empty, or consists only of whitespace. Thrown
        /// if a country with the same name already exists in the collection.</exception>
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

        /// <summary>
        /// Retrieves a list of all countries.
        /// </summary>
        /// <remarks>This method converts internal country data into <see cref="CountryResponse"/> objects
        /// and returns them as a collection. The returned list is a snapshot of the current state and does not reflect
        /// subsequent changes to the underlying data.</remarks>
        /// <returns>A list of <see cref="CountryResponse"/> objects representing all countries. The list will be empty if no
        /// countries are available.</returns>
        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }


        /// <summary>
        /// Retrieves a country by its unique identifier.
        /// </summary>
        /// <remarks>This method searches for a country in the internal collection based on the provided
        /// <paramref name="countryID"/>. If a match is found, the country is converted to a <see
        /// cref="CountryResponse"/> object and returned.</remarks>
        /// <param name="countryID">The unique identifier of the country to retrieve. Must be a non-null and non-empty <see cref="Guid"/>.</param>
        /// <returns>A <see cref="CountryResponse"/> object representing the country with the specified identifier,  or <see
        /// langword="null"/> if no matching country is found or if <paramref name="countryID"/> is null or empty.</returns>
        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            // Checks if "countryID" is null or empty
            // If so, throws an ArgumentNullException with a message indicating that the countryID cannot be null.
            // Otherwise, searches for the country with the specified ID.
            // Get matching country from List<Country> based on the provided countryID
            // If no match is found, returns null.
            // Converts the matching "Country" object to a "CountryResponse" object and returns it.
            if (countryID == null || countryID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(countryID), "CountryID cannot be null.");
            }
            Country? country = _countries.FirstOrDefault(c => c.CountryID == countryID.Value);
            if (country == null)
            {
                return null;
            }
            return country.ToCountryResponse();
        }
    }
}

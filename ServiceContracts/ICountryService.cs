using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity.
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// Adds a country object to the list of countries.
        /// </summary>
        /// <param name="countryAddRequest">Country object to add.</param>
        /// <returns>Returns the country object after adding it (including the newly generated country id).</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// It returns the list of all countries.
        /// </summary>
        /// <returns>It returns the countries as a list.</returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns a country object by its country ID.
        /// </summary>
        /// <param name="countryID">Country ID to search for</param>
        /// <returns>Matching country as CountryResponse</returns>
        CountryResponse? GetCountryByCountryID(Guid? countryID);
    }
}

using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class used as return type for most CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified object is a <c>CountryResponse</c> and has the same 
        /// <c>CountryID</c> and <c>CountryName</c> values as the current instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if(obj is null)
            {
                return false;
            }
            if(obj is not CountryResponse)
            {
                return false;
            }

            CountryResponse countryResponse = (CountryResponse)obj;
            return CountryID == countryResponse.CountryID &&
                   CountryName == countryResponse.CountryName;
        }

        /// <summary>
        /// Returns the hash code for the current instance.
        /// </summary>
        /// <remarks>This method overrides the default implementation of <see cref="object.GetHashCode"/>.
        /// The returned hash code is suitable for use in hashing algorithms and data structures such as hash
        /// tables.</remarks>
        /// <returns>An integer representing the hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName,
            };
        }
    }
}

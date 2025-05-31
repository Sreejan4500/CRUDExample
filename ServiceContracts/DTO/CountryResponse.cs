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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

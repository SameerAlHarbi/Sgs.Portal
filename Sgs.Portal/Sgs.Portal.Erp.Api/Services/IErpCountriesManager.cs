using Sgs.Portal.Erp.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sgs.Portal.Erp.Api.Services
{
    public interface IErpCountriesManager
    {
        Task<IEnumerable<Country>> GetCountriesCollectionAsync(string[] codes = null
            , string name = null
            , string nationalityName = ""
            , bool fillRegions = false
            , bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN");

        Task<IEnumerable<Region>> GetRegionsCollectionAsync(string[] codes = null
            , string name = null
            , string[] countriesCodes = null
            , bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN");

        Task<IEnumerable<City>> GetCitiesCollectionAsync(string[] codes = null
            , string name = null
            , string[] countriesCodes = null
            , string[] regionsCodes = null
            , bool fillAirports = false
            , string language = "AR,EN");

        Task<IEnumerable<Airport>> GetAirportCollectionAsync(string[] codes = null
            , string name = null
            , string[] countriesCodes = null
            , string[] citiesCodes = null
            , string language = "AR,EN");
    }
}

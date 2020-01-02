
using Sgs.Portal.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgs.Portal.Shared.Services
{
    public interface ICountriesManager
    {
        Task<IEnumerable<Country>> GetCountriesCollection(string[] codes = null
            , string name = null
            , string nationalityName = ""
            , bool fillRegions = false
            , bool fillCities = true
            , bool fillAirports = false);

        async Task<IEnumerable<Region>> GetRegionsCollection(string[] codes = null,
            string name = null, string[] countriesCodes = null,
            bool fillCities = true, bool fillAirports = false)
        {
            var countriesCollection = await GetCountriesCollection(countriesCodes, fillRegions: true,
                fillCities: fillCities, fillAirports: fillAirports);
            var regionsCollection = countriesCollection.SelectMany(c => c.Regions)
                .Where(r => (codes == null || codes.Contains(r.Code))
                    && (string.IsNullOrWhiteSpace(name)
                            || r.Name_Ar.Trim().ToUpper().Contains(name.Trim().ToUpper())
                            || r.Name_En.Trim().ToUpper().Contains(name.Trim().ToUpper())
                        )
                       );

            return regionsCollection;
        }

        async Task<IEnumerable<City>> GetCitiesCollection(string[] codes = null,
            string name = null, string[] countriesCodes = null, string[] regionsCodes = null, bool fillAirports = false)
        {
            var regionsCollection = await GetRegionsCollection(regionsCodes, countriesCodes: countriesCodes, fillAirports: fillAirports);
            var citiesCollection = regionsCollection.SelectMany(r => r.Cities)
                .Where(c => (codes == null && codes.Contains(c.Code))
                    && (string.IsNullOrWhiteSpace(name)
                            || c.Name_Ar.Trim().ToUpper().Contains(name.Trim().ToUpper())
                            || c.Name_En.Trim().ToUpper().Contains(name.Trim().ToUpper())
                        )
                       );

            return citiesCollection;
        }

        async Task<IEnumerable<Airport>> GetAirportCollection(string[] codes = null,
           string name = null, string[] countriesCodes = null, string[] citiesCodes = null)
        {
            var citiesCollection = await GetCitiesCollection(citiesCodes, countriesCodes: countriesCodes, fillAirports: true);
            var airportsCollection = citiesCollection.SelectMany(c => c.Airports)
                .Where(a => (codes == null && codes.Contains(a.Code))
                    && (string.IsNullOrWhiteSpace(name)
                            || a.Name_Ar.Trim().ToUpper().Contains(name.Trim().ToUpper())
                            || a.Name_En.Trim().ToUpper().Contains(name.Trim().ToUpper())
                        )
                       );

            return airportsCollection;
        }
    }
}

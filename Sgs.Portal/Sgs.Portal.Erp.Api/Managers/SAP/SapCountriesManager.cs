using Sgs.Portal.Erp.Api.Models;
using Sgs.Portal.Erp.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapCountriesManager : IErpCountriesManager
    {
        private readonly SapConfiguration _sapConfiguration;

        public SapCountriesManager(SapConfiguration sapConfiguration)
        {
            _sapConfiguration = sapConfiguration;
        }

        public Task<IEnumerable<Country>> GetCountriesCollection(string[] codes = null, string name = null, string nationalityName = "", bool fillRegions = false, bool fillCities = true, bool fillAirports = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Airport>> GetAirportCollection(string[] codes = null, string name = null, string[] countriesCodes = null, string[] citiesCodes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<City>> GetCitiesCollection(string[] codes = null, string name = null, string[] countriesCodes = null, string[] regionsCodes = null, bool fillAirports = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Region>> GetRegionsCollection(string[] codes = null, string name = null, string[] countriesCodes = null, bool fillCities = true, bool fillAirports = false)
        {
            throw new NotImplementedException();
        }
    }
}
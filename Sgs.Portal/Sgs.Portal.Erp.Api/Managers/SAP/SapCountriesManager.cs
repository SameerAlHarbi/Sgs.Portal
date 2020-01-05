using SAP.Middleware.Connector;
using Sgs.Portal.Erp.Api.Models;
using Sgs.Portal.Erp.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapCountriesManager : SapManager, IErpCountriesManager
    {
        public SapCountriesManager(IDestinationConfiguration destinationConfiguration,ISapConfiguration sapConfiguration) 
            : base(destinationConfiguration,sapConfiguration)
        {
        }

        public async Task<IEnumerable<Country>> GetCountriesCollection(string[] codes = null,
            string name = null, string nationalityName = "", 
            bool fillRegions = false, bool fillCities = true, 
            bool fillAirports = false, string language = "AR,EN")
        {
            try
            {
                IRfcFunction func = this.getIRfcFunction("Z_COUNTRY_NATIONALITY");

                //Passing Parameters
                func["IM_LANGU"].SetValue('E');
                func.Invoke(rfcDestination);

                var results = func["T_CONT_NATI"].GetTable()
                    .Select(c => new Country
                    {
                        Code = c.GetValue("LAND1").ToString(),
                        Name_Ar = c.GetValue("LANDX50").ToString(),
                        NationalityName_Ar = c.GetValue("NATIO50").ToString()              
                    });
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<Airport>> GetAirportCollection(string[] codes = null,
            string name = null,
            string[] countriesCodes = null,
            string[] citiesCodes = null,
            string language = "AR,EN")
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<City>> GetCitiesCollection(string[] codes = null,
            string name = null, string[] countriesCodes = null,
            string[] regionsCodes = null, bool fillAirports = false,
            string language = "AR,EN")
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Region>> GetRegionsCollection(string[] codes = null, string name = null,
            string[] countriesCodes = null, bool fillCities = true, 
            bool fillAirports = false, string language = "AR,EN")
        {
            throw new NotImplementedException();
        }
    }
}
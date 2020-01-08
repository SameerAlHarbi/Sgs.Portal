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

        private async Task<List<Country>> getCountriesList(string[] codes = null
            , string name = null
            , string nationalityName = ""
            , bool arabic = true
            , bool english = true)
        {
            var arabicResults = new List<Country>();
            var englishResults = new List<Country>();
            var results = new List<Country>();

            IRfcFunction func = this.getIRfcFunction("Z_COUNTRY_NATIONALITY");

            //Fill arabic list
            if (arabic)
            {
                func["IM_LANGU"].SetValue('A');
                func.Invoke(rfcDestination);

                arabicResults = func["T_CONT_NATI"].GetTable()
                    .Select(c => new Country
                    {
                        Code = c.GetValue("LAND1").ToString(),
                        Name_Ar = c.GetValue("LANDX50").ToString(),
                        NationalityName_Ar = c.GetValue("NATIO50").ToString()
                    }).ToList();
            }

            //Fill english list
            if (english)
            {
                func["IM_LANGU"].SetValue('E');
                func.Invoke(rfcDestination);

                englishResults = func["T_CONT_NATI"].GetTable()
                    .Select(c => new Country
                    {
                        Code = c.GetValue("LAND1").ToString(),
                        Name_En = c.GetValue("LANDX50").ToString(),
                        NationalityName_En = c.GetValue("NATIO50").ToString()
                    }).ToList();
            }

            //Merge arabic and english
            if (arabicResults.Count() > 0)
            {
                foreach (var arabicResult in arabicResults)
                {
                    var englishResult = englishResults.FirstOrDefault(r => r.Code == arabicResult.Code);
                    arabicResult.Name_En = englishResult?.Name_En ?? null;
                    arabicResult.NationalityName_En = englishResult?.NationalityName_En ?? null;
                }

                results = arabicResults;
            }
            else
            {
                results = englishResults;
            }

            //Apply Filters
            if (results.Count > 0)
            {
                results = results.Where(r => (codes == null
                        || codes.Any(c => c.Trim().ToUpper() == r.Code.Trim().ToUpper()))
                    && (string.IsNullOrWhiteSpace(name)
                        || r.Name_Ar.Trim().ToUpper().Contains(name.Trim().ToUpper())
                        || r.Name_En.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                    && (string.IsNullOrWhiteSpace(nationalityName)
                        || r.NationalityName_Ar.Trim().ToUpper().Contains(nationalityName.Trim().ToUpper())
                        || r.NationalityName_En.Trim().ToUpper().Contains(nationalityName.Trim().ToUpper()))
                    ).ToList();
            }

            return await Task.FromResult(results);
        }

        public async Task<IEnumerable<Country>> GetCountriesCollection(string[] codes = null
            , string name = null
            , string nationalityName = ""
            , bool fillRegions = false, bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                //Setup languages choice
                language = !string.IsNullOrWhiteSpace(language) ? language : "AR,EN";
                var languages = language.Trim().ToUpper().Split(',');
                bool english = languages.Contains("EN");
                bool arabic = languages.Contains("AR") || !english;

                var results = await getCountriesList(codes, name, nationalityName, arabic, english);         
              
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
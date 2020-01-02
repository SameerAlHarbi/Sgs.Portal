using Microsoft.EntityFrameworkCore;
using Sameer.Shared.Data;
using Sgs.Portal.Shared.Models;
using Sgs.Portal.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgs.Portal.Db.Logic
{
    public class CountriesManager : GeneralManager<Country>, ICountriesManager
    {
        public CountriesManager(IRepository repo) : base(repo)
        {
        }

        public async Task<IEnumerable<Country>> GetCountriesCollection(string[] codes = null,
            string name = null, string nationalityName = "", bool fillRegions = false, 
            bool fillCities = true, bool fillAirports = false)
        {
            try
            {
                var allCountries = this.GetAll(c => 
                        (codes == null || codes.Contains(c.Code))
                    &&  (string.IsNullOrWhiteSpace(name) ||
                               c.Name_Ar.Trim().ToUpper().Contains(name.Trim().ToUpper())
                            || c.Name_En.Trim().ToUpper().Contains(name.Trim().ToUpper()))
                    && (string.IsNullOrWhiteSpace(nationalityName) ||
                               c.NationalityName_Ar.Trim().ToUpper().Contains(nationalityName.Trim().ToUpper())
                            || c.NationalityName_En.Trim().ToUpper().Contains(nationalityName.Trim().ToUpper())));
                if(fillRegions)
                {
                    if(fillCities)
                    {
                        if (fillAirports)
                        {
                            this.AddInclude(allCountries, c => c.Regions.SelectMany(r => r.Cities.SelectMany(ct => ct.Airports)));
                        }
                        else
                        {
                            this.AddInclude(allCountries, c => c.Regions.SelectMany(r => r.Cities));
                        }
                    }
                    else
                    {
                        this.AddInclude(allCountries, c => c.Regions);
                    }
                }

                return await allCountries.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

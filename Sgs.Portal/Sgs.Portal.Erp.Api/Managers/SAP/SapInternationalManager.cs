using SAP.Middleware.Connector;
using Sgs.Portal.Erp.Api.Models.International;
using Sgs.Portal.Erp.Api.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgs.Portal.Erp.Api.Managers.SAP
{
    public class SapInternationalManager : SapManager, IErpInternationalManager
    {
        public SapInternationalManager(IDestinationConfiguration destinationConfiguration, ISapConfiguration sapConfiguration)
            : base(destinationConfiguration, sapConfiguration)
        {
        }

        private Task<List<Country>> getCountriesListAsync(string[] codes = null
            , string name = null
            , string nationalityName = ""
            , bool arabic = true
            , bool english = true)
        {
            return Task.Run(async () =>
            {
                //Check language choice.
                if (!arabic && !english)
                {
                    arabic = true;
                }

                //Get SAP function by name and set parameters.
                IRfcFunction func = this.getIRfcFunction("Z_COUNTRY_NATIONALITY");

                //Get arabic results.
                var arabicResults = arabic ?
                    await Task.Run(() =>
                    {
                        func["IM_LANGU"].SetValue('A');
                        func.Invoke(rfcDestination);

                        var taskResults = func["T_CONT_NATI"].GetTable()?
                            .Select(c => new Country
                            {
                                Code = c.GetValue("LAND1")?.ToString() ?? string.Empty,
                                Name_Ar = c.GetValue("LANDX50")?.ToString() ?? string.Empty,
                                NationalityName_Ar = c.GetValue("NATIO50")?.ToString() ?? string.Empty
                            }).ToList();

                        return taskResults ?? new List<Country>();
                    })
                : new List<Country>();

                //Get english results.
                var englishResults = english ?
                await Task.Run(() =>
                {
                    func["IM_LANGU"].SetValue('E');
                    func.Invoke(rfcDestination);

                    var taskResults = func["T_CONT_NATI"].GetTable()?
                        .Select(c => new Country
                        {
                            Code = c.GetValue("LAND1")?.ToString() ?? string.Empty,
                            Name_En = c.GetValue("LANDX50")?.ToString() ?? string.Empty,
                            NationalityName_En = c.GetValue("NATIO50")?.ToString() ?? string.Empty
                        }).ToList();

                    return taskResults ?? new List<Country>();
                })
                : new List<Country>();

                //Merge arabic and english results.
                var allResults = arabicResults.Union(englishResults).Where(c => !string.IsNullOrWhiteSpace(c.Code));
                var MergedResults = new ConcurrentBag<Country>();
                Parallel.ForEach(allResults.GroupBy(c => c.Code), resultsGroup =>
                {
                    MergedResults.Add(new Country
                    {
                        Code = resultsGroup.Key,
                        Name_Ar = resultsGroup.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.Name_Ar))?.Name_Ar ?? string.Empty,
                        Name_En = resultsGroup.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.Name_En))?.Name_En ?? string.Empty,
                        NationalityName_Ar = resultsGroup
                            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.NationalityName_Ar))?.NationalityName_Ar ?? string.Empty,
                        NationalityName_En = resultsGroup
                            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.NationalityName_En))?.NationalityName_En ?? string.Empty,
                        Regions = new List<Region>()
                    });
                });

                var finalResults = MergedResults.ToList();

                //Apply filters to the final results.
                if (finalResults.Count > 0)
                {
                    finalResults = finalResults.Where(r => (codes == null
                             || codes.Any(c => c.Trim().ToLower() == r.Code.Trim().ToLower()))
                         && (string.IsNullOrWhiteSpace(name)
                             || r.Name_Ar.Trim().ToLower().Contains(name.Trim().ToLower())
                             || r.Name_En.Trim().ToLower().Contains(name.Trim().ToLower()))
                         && (string.IsNullOrWhiteSpace(nationalityName)
                             || r.NationalityName_Ar.Trim().ToLower().Contains(nationalityName.Trim().ToLower())
                             || r.NationalityName_En.Trim().ToLower().Contains(nationalityName.Trim().ToLower()))
                         ).ToList();
                }

                //Return final results.
                return finalResults;
            });
        }

        public async Task<List<Region>> getRegionsListAsync(string[] codes = null
            , string name = null
            , string[] countriesCodes = null
            , bool arabic = true
            , bool english = true)
        {
            try
            {
                var cities = await getCitiesListAsync(null, null, countriesCodes, codes, arabic, english);
                return cities?.GroupBy(c => new { c.CountryCode, c.RegionCode })
                            .Select(cg => new Region
                            {
                                Code = cg.Key.RegionCode,
                                CountryCode = cg.Key.CountryCode,
                                Name_Ar = string.Empty,
                                Name_En = string.Empty
                            }).Distinct().ToList() ?? new List<Region>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task<List<City>> getCitiesListAsync(string[] codes = null
           , string name = null
           , string[] countriesCodes = null
           , string[] regionsCodes = null
           , bool arabic = true
           , bool english = true)
        {
            return Task.Run(async () =>
            {
                //Check language choice.
                if (!arabic && !english)
                {
                    arabic = true;
                }

                //Get SAP function by name and set parameters.
                IRfcFunction func = this.getIRfcFunction("Z_CITY_NAMES");

                if (countriesCodes != null && countriesCodes.Count() == 1)
                {
                    func["IM_LAND"].SetValue(countriesCodes.First());
                }

                //Get arabic results.
                var arabicResults = arabic ?
                     await Task.Run(() =>
                     {
                         func["IM_LANGU"].SetValue('A');
                         func.Invoke(rfcDestination);

                         var taskResults = func["T_CITY"].GetTable()?
                             .Select(c => new City
                             {
                                 Code = c.GetValue("CITYC")?.ToString() ?? string.Empty,
                                 Name_Ar = c.GetValue("BEZEI")?.ToString() ?? string.Empty,
                                 CountryCode = c.GetValue("LAND1")?.ToString() ?? string.Empty,
                                 RegionCode = c.GetValue("REGIO")?.ToString() ?? string.Empty
                             }).ToList();

                         return taskResults ?? new List<City>();
                     })
                 : new List<City>();

                //Get english results.
                var englishResults = english ?
                     await Task.Run(() =>
                     {
                         func["IM_LANGU"].SetValue('E');
                         func.Invoke(rfcDestination);

                         var taskResults = func["T_CITY"].GetTable()?
                             .Select(c => new City
                             {
                                 Code = c.GetValue("CITYC")?.ToString() ?? string.Empty,
                                 Name_En = c.GetValue("BEZEI")?.ToString() ?? string.Empty,
                                 CountryCode = c.GetValue("LAND1")?.ToString() ?? string.Empty,
                                 RegionCode = c.GetValue("REGIO")?.ToString() ?? string.Empty
                             }).ToList();

                         return taskResults ?? new List<City>();
                     })
                : new List<City>();

                //Merge arabic and english results.
                var allResults = arabicResults.Union(englishResults).Where( c => !string.IsNullOrWhiteSpace(c.Code)
                    && !string.IsNullOrWhiteSpace(c.CountryCode) 
                    && !string.IsNullOrWhiteSpace(c.RegionCode));
                var mergedResults = new ConcurrentBag<City>();
                Parallel.ForEach(allResults.GroupBy(c => new { c.Code, c.CountryCode }), resultsGroup =>
                {
                    mergedResults.Add(new City
                    {
                        Code = resultsGroup.Key.Code,
                        Name_Ar = resultsGroup.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.Name_Ar))?.Name_Ar ?? string.Empty,
                        Name_En = resultsGroup.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.Name_En))?.Name_En ?? string.Empty,
                        RegionCode = resultsGroup.FirstOrDefault().RegionCode,
                        CountryCode = resultsGroup.Key.CountryCode,
                        Airports = new List<Airport>()
                    });
                });

                var finalResults = mergedResults.ToList();

                //Apply Filters.
                if (mergedResults.Count > 0)
                {
                    finalResults = finalResults.Where(r => (codes == null
                            || codes.Any(c => c.Trim().ToLower() == r.Code.Trim().ToLower()))
                        && (string.IsNullOrWhiteSpace(name)
                            || r.Name_Ar.Trim().ToLower().Contains(name.Trim().ToLower())
                            || r.Name_En.Trim().ToLower().Contains(name.Trim().ToLower()))
                        && (countriesCodes == null
                            || countriesCodes.Any(c => c.Trim().ToLower() == r.CountryCode.Trim().ToLower()))
                        && (regionsCodes == null
                            || regionsCodes.Any(c => c.Trim().ToLower() == r.RegionCode.Trim().ToLower()))
                        ).ToList();
                }

                //Return final results.
                return finalResults;
            });
        }

        //TODO: Fill Airports
        public async Task<IEnumerable<Country>> GetCountriesCollectionAsync(string[] codes = null
            , string name = null
            , string nationalityName = ""
            , bool fillRegions = false
            , bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                //Setup language choices
                language = !string.IsNullOrWhiteSpace(language) ? language : "AR,EN";
                var languages = language.Trim().ToUpper().Split(',');
                bool english = languages.Contains("EN");
                bool arabic = languages.Contains("AR") || !english;

                //Get countries results.
                var countriesResults = await getCountriesListAsync(codes
                    , name
                    , nationalityName
                    , arabic
                    , english);

                //Fill countries results with regions and cites.
                if (countriesResults.Count() > 0 && fillRegions)
                {
                    var countriesCodes = countriesResults.Where(c => !string.IsNullOrWhiteSpace(c.Code)).Select(c => c.Code).ToArray();

                    var citiesResults = await getCitiesListAsync(null
                        , null
                        , countriesCodes
                        , null
                        , arabic
                        , english);

                    var regionsResults = citiesResults?
                        .GroupBy(c => new { c.CountryCode, c.RegionCode })
                        .Select(cg => new Region
                        {
                            Code = cg.Key.RegionCode,
                            CountryCode = cg.Key.CountryCode,
                            Name_Ar = string.Empty,
                            Name_En = string.Empty,
                            Cities = fillCities ? cg.ToList() : new List<City>()
                        });

                    foreach (var country in countriesResults)
                    {
                        country.Regions = regionsResults
                            .Where(r => !string.IsNullOrWhiteSpace(r.CountryCode) && r.CountryCode == country.Code)
                            .OrderBy(r => r.Code).ToList();
                    }
                }

                return countriesResults;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //TODO: Enhance sap regions info
        public async Task<IEnumerable<Region>> GetRegionsCollectionAsync(string[] codes = null
            , string name = null
            , string[] countriesCodes = null
            , bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                //Setup language choices
                language = !string.IsNullOrWhiteSpace(language) ? language : "AR,EN";
                var languages = language.Trim().ToUpper().Split(',');
                bool english = languages.Contains("EN");
                bool arabic = languages.Contains("AR") || !english;

                var citiesResults = await getCitiesListAsync(null
                    , null
                    , countriesCodes
                    , codes
                    , arabic
                    , english);

                var regionsResults = citiesResults?
                        .GroupBy(c => new { c.CountryCode, c.RegionCode })
                        .Select(cg => new Region
                        {
                            Code = cg.Key.RegionCode,
                            CountryCode = cg.Key.CountryCode,
                            Name_Ar = string.Empty,
                            Name_En = string.Empty,
                            Cities = fillCities ? cg.ToList() : new List<City>()
                        }).ToList() ?? new List<Region>();

                regionsResults = regionsResults
                    .Where(r => 
                            string.IsNullOrWhiteSpace(name) 
                         || r.Name_Ar.Trim().ToLower().Contains(name.Trim().ToLower())
                         || r.Name_En.Trim().ToLower().Contains(name.Trim().ToLower()))
                    .ToList();

                return regionsResults;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<City>> GetCitiesCollectionAsync(string[] codes = null,
           string name = null, string[] countriesCodes = null,
           string[] regionsCodes = null, bool fillAirports = false,
           string language = "AR,EN")
        {
            try
            {
                //Setup language choices
                language = !string.IsNullOrWhiteSpace(language) ? language : "AR,EN";
                var languages = language.Trim().ToUpper().Split(',');
                bool english = languages.Contains("EN");
                bool arabic = languages.Contains("AR") || !english;

                var results = await getCitiesListAsync(codes
                    , name
                    , countriesCodes
                    , regionsCodes
                    , arabic
                    , english);

                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //TODO: Fill Airports
        public async Task<IEnumerable<Airport>> GetAirportCollectionAsync(string[] codes = null,
            string name = null,
            string[] countriesCodes = null,
            string[] citiesCodes = null,
            string language = "AR,EN")
        {
            return await Task.FromResult(new List<Airport>());
        }


    }
}
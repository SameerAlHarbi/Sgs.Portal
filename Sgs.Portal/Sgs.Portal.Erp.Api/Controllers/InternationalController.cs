using AutoMapper;
using Sgs.Portal.Erp.Api.Models.International;
using Sgs.Portal.Erp.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sgs.Portal.Erp.Api.Controllers
{
    [RoutePrefix("international")]
    public class InternationalController : BaseApiController
    {
        private readonly IErpInternationalManager _internationalManager;

        public InternationalController(IErpInternationalManager internationalManager
            , IMapper mapper)
            : base(mapper)
        {
            _internationalManager = internationalManager;
        }

        /// <summary>
        /// Get all countries.
        /// </summary>
        /// <param name="code">Filter countries by codes separated by comma.</param>
        /// <param name="name">Filter countries by name.</param>
        /// <param name="nationality">Filter countries by nationality.</param>
        /// <param name="fillRegions">Fill all regions of the returned countries.</param>
        /// <param name="fillCities">Fill all cities for each region.</param>
        /// <param name="fillAirports">Fill all airports for each city.</param>
        /// <param name="language">Languages separated by comma.</param>
        /// <returns>Collections of countries in JSON.</returns>
        [HttpGet()]
        [Route("countries")]
        [ResponseType(typeof(IEnumerable<Country>))]
        public async Task<IHttpActionResult> GetCountries(string code = null
            , string name = null
            , string nationality = null
            , bool fillRegions = false
            , bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                var results = await _internationalManager
                    .GetCountriesCollectionAsync(code?.Split(',')
                        , name
                        , nationality
                        , fillRegions
                        , fillCities
                        , fillAirports
                        , language: language);

                return Ok(results ?? new List<Country>());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get single country by code.
        /// </summary>
        /// <param name="code">Code of specified country</param>
        /// <param name="fillRegions">Fill all regions in returned country.</param>
        /// <param name="fillCities">Fill all cities in returned country regions</param>
        /// <param name="fillAirports">Fill all airports in each city of the returned country regions</param>
        /// <param name="language">Languages separated by comma.</param>
        /// <returns>Country item</returns>
        [HttpGet()]
        [Route("countries/{code}")]
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> GetCountry(string code
            , bool fillRegions = false
            , bool fillCities = true
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) 
                    || code.Split(',').Length > 1)
                {
                    return BadRequest("Code not valid !");
                }

                var results = await _internationalManager
                    .GetCountriesCollectionAsync(new string[] { code }
                    , fillRegions: fillRegions
                    , fillCities: fillCities
                    , fillAirports: fillAirports
                    , language: language);

                var result = results?.FirstOrDefault();

                if (result == null)
                {
                    return BadRequest("NotFound");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Get all regions in specified country.
        /// </summary>
        /// <param name="code">Code of specified country</param>
        /// <param name="regionCode">Filter regions by codes separated by comma.</param>
        /// <param name="name">Name of region for search</param>
        /// <param name="fillCities">Fill all cities in returned regions.</param>
        /// <param name="fillAirports">Fill all airports in each city of the returned regions.</param>
        /// <param name="language">Languages separated by comma.</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("countries/{code}/regions")]
        [ResponseType(typeof(IEnumerable<Region>))]
        public async Task<IHttpActionResult> GetCountryRegions(string code
            , string regionCode = null
            , string name = null
            , bool fillCities = false
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || code.Split(',').Length > 1)
                {
                    return BadRequest("Country code not valid !");
                }

                var results = await _internationalManager
                    .GetCountriesCollectionAsync(new string[] { code }
                    , fillRegions: true
                    , fillCities: fillCities
                    , fillAirports: fillAirports
                    , language: language);

                var result = results?.FirstOrDefault();

                if (result == null)
                {
                    return BadRequest("NotFound");
                }

                result.Regions = result.Regions?.Where(r =>
                       (string.IsNullOrWhiteSpace(regionCode)
                        || r.Code.Trim().ToLower().Contains(regionCode.Trim().ToLower()))
                    && (string.IsNullOrWhiteSpace(name)
                        || r.Name_Ar.Trim().ToLower().Contains(name.Trim().ToLower())
                        || r.Name_En.Trim().ToLower().Contains(name.Trim().ToLower())))
                    .ToList() ?? new List<Region>();

                return Ok(result.Regions);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("countries/{code}/regions/{regionCode}")]
        [ResponseType(typeof(Region))]
        public async Task<IHttpActionResult> GetRegion(
              string code
            , string regionCode 
            , bool fillCities = false
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || code.Split(',').Length > 1)
                {
                    return BadRequest("Country code not valid !");
                }

                if (string.IsNullOrWhiteSpace(regionCode) || regionCode.Split(',').Length > 1)
                {
                    return BadRequest("Region code not valid !");
                }

                var results = await _internationalManager
                   .GetCountriesCollectionAsync(new string[] { code }
                   , fillRegions: true
                   , fillCities: fillCities
                   , fillAirports: fillAirports
                   , language: language);

                var result = results?.FirstOrDefault()?.Regions?
                    .FirstOrDefault(r => r.Code.Trim().ToLower() == regionCode.Trim().ToLower());

                if (result == null)
                {
                    return BadRequest("NotFound");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("countries/{code}/regions/{regionCode}/Cities")]
        [ResponseType(typeof(IEnumerable<City>))]
        public async Task<IHttpActionResult> GetRegionCities(
              string code
            , string regionCode
            , string cityCode = null
            , string cityName = null
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || code.Split(',').Length > 1)
                {
                    return BadRequest("Country code not valid !");
                }

                if (string.IsNullOrWhiteSpace(regionCode) || regionCode.Split(',').Length > 1)
                {
                    return BadRequest("Region code not valid !");
                }

                var results = await _internationalManager
                   .GetCountriesCollectionAsync(new string[] { code }
                   , fillRegions: true
                   , fillCities: true
                   , fillAirports: fillAirports
                   , language: language);

                var result = results?.FirstOrDefault()?.Regions?
                    .FirstOrDefault(r => r.Code.Trim().ToLower() == regionCode.Trim().ToLower());

                if (result == null)
                {
                    return BadRequest("NotFound");
                }

                result.Cities = result.Cities?.Where(c =>
                       (string.IsNullOrWhiteSpace(cityCode)
                        || c.Code.Trim().ToLower().Contains(cityCode.Trim().ToLower()))
                    && (string.IsNullOrWhiteSpace(cityName)
                        || c.Name_Ar.Trim().ToLower().Contains(cityName.Trim().ToLower())
                        || c.Name_En.Trim().ToLower().Contains(cityName.Trim().ToLower())))
                    .ToList() ?? new List<City>();

                return Ok(result.Cities);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("countries/{code}/regions/{regionCode}/Cities/{cityCode}")]
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> GetCity(
              string code
            , string regionCode
            , string cityCode
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || code.Split(',').Length > 1)
                {
                    return BadRequest("Country code not valid !");
                }

                if (string.IsNullOrWhiteSpace(regionCode) || regionCode.Split(',').Length > 1)
                {
                    return BadRequest("Region code not valid !");
                }

                if (string.IsNullOrWhiteSpace(cityCode) || cityCode.Split(',').Length > 1)
                {
                    return BadRequest("City code not valid !");
                }

                var results = await _internationalManager
                   .GetCountriesCollectionAsync(new string[] { code }
                   , fillRegions: true
                   , fillCities: true
                   , fillAirports: fillAirports
                   , language: language);

                var result = results?.FirstOrDefault()?.Regions?
                    .FirstOrDefault(r => r.Code.Trim().ToLower() == regionCode.Trim().ToLower())?
                    .Cities?.FirstOrDefault(c => c.Code.Trim().ToLower() == cityCode.Trim().ToLower());

                if (result == null)
                {
                    return BadRequest("NotFound");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("countries/{code}/regions/{regionCode}/Cities/{cityCode}/airports")]
        [ResponseType(typeof(IEnumerable<Airport>))]
        public async Task<IHttpActionResult> GetCityAirports(string code
            , string regionCode
            , string cityCode
            , string airportCode = null
            , string language = "AR,EN")
        {
            try
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(code) || code.Split(',').Length > 1)
                    {
                        return BadRequest("Country code not valid !");
                    }

                    if (string.IsNullOrWhiteSpace(regionCode) || regionCode.Split(',').Length > 1)
                    {
                        return BadRequest("Region code not valid !");
                    }

                    if (string.IsNullOrWhiteSpace(cityCode) || cityCode.Split(',').Length > 1)
                    {
                        return BadRequest("City code not valid !");
                    }

                    var results = await _internationalManager
                       .GetCountriesCollectionAsync(new string[] { code }
                       , fillRegions: true
                       , fillCities: true
                       , fillAirports: true
                       , language: language);

                    var result = results?.FirstOrDefault()?.Regions?
                        .FirstOrDefault(r => r.Code.Trim().ToLower() == regionCode.Trim().ToLower())?
                        .Cities?.FirstOrDefault(c => c.Code.Trim().ToLower() == cityCode.Trim().ToLower());

                    if (result == null)
                    {
                        return BadRequest("NotFound");
                    }

                    result.Airports = result.Airports?
                        .Where(a => string.IsNullOrWhiteSpace(airportCode)
                         || a.Code.Trim().ToLower().Contains(airportCode.Trim().ToLower()))
                        .ToList() ?? new List<Airport>();                

                    return Ok(result.Airports);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("countries/{code}/regions/{regionCode}/Cities/{cityCode}/airports/{airportCode}")]
        [ResponseType(typeof(IEnumerable<Airport>))]
        public async Task<IHttpActionResult> GetAirport(string code
            , string regionCode
            , string cityCode
            , string airportCode
            , string language = "AR,EN")
        {
            try
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(code) || code.Split(',').Length > 1)
                    {
                        return BadRequest("Country code not valid !");
                    }

                    if (string.IsNullOrWhiteSpace(regionCode) || regionCode.Split(',').Length > 1)
                    {
                        return BadRequest("Region code not valid !");
                    }

                    if (string.IsNullOrWhiteSpace(cityCode) || cityCode.Split(',').Length > 1)
                    {
                        return BadRequest("City code not valid !");
                    }

                    if (string.IsNullOrWhiteSpace(cityCode) || cityCode.Split(',').Length > 1)
                    {
                        return BadRequest("Airport code not valid !");
                    }

                    var results = await _internationalManager
                       .GetCountriesCollectionAsync(new string[] { code }
                       , fillRegions: true
                       , fillCities: true
                       , fillAirports: true
                       , language: language);

                    var result = results?.FirstOrDefault()?.Regions?
                        .FirstOrDefault(r => r.Code.Trim().ToLower() == regionCode.Trim().ToLower())?
                        .Cities?.FirstOrDefault(c => c.Code.Trim().ToLower() == cityCode.Trim().ToLower())?
                        .Airports?.FirstOrDefault(a => a.Code.Trim().ToLower() == airportCode.Trim().ToLower());
                        
                    if (result == null)
                    {
                        return BadRequest("NotFound");
                    }

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("regions")]
        [ResponseType(typeof(IEnumerable<Region>))]
        public async Task<IHttpActionResult> GetRegions(string code = null
            , string name = null
            , string countries = null
            , bool fillCities = false
            , bool fillAirports = false
            , string language = "AR,EN")
        {
            try
            {
                var results = await _internationalManager.GetRegionsCollectionAsync(code?.Split(',')
                    , name
                    , countries?.Split(',')
                    , fillCities
                    , fillAirports
                    , language: language);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("cities")]
        [ResponseType(typeof(IEnumerable<Country>))]
        public async Task<IHttpActionResult> GetCities(string code = null
            , string name = null
            , string countries = null
            , string regions = null
            , bool fillAirports = false
            , string airportCode = null
            , string language = "AR,EN")
        {
            try
            {
                if (!string.IsNullOrEmpty(airportCode))
                {
                    fillAirports = true;
                }

                var results = await _internationalManager.GetCitiesCollectionAsync(code?.Split(',')
                    , name
                    , countries?.Split(',')
                    , regions?.Split(',')
                    , fillAirports: fillAirports
                    , language: language);
                return Ok(results.Where(c => string.IsNullOrEmpty(airportCode) 
                    || (c.Airports != null && c.Airports.Any(a => a.Code.Trim().ToLower().Contains(airportCode)))));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet()]
        [Route("airports")]
        [ResponseType(typeof(IEnumerable<Airport>))]
        public async Task<IHttpActionResult> GetAirports(string code = null
            , string name = null
            , string countries = null
            , string regions = null
            , string cities = null
            , string language = "AR,EN")
        {
            try
            {
                var results = await _internationalManager.GetCitiesCollectionAsync(cities?.Split(',')
                    , null
                    , countries?.Split(',')
                    , regions?.Split(',')
                    , fillAirports: true
                    , language: language);

               var codes = !string.IsNullOrWhiteSpace(code) ? code.Trim().ToLower().Split(',') : null;

                return Ok(results.Where(c => (string.IsNullOrEmpty(code) && string.IsNullOrWhiteSpace(name))
                    || c.Airports != null && 
                        ((!string.IsNullOrWhiteSpace(code) && c.Airports.Any(a => codes.Contains(a.Code.Trim().ToLower())))
                    || (!string.IsNullOrWhiteSpace(name) && c.Airports
                            .Any(a => (!string.IsNullOrWhiteSpace(a.Name_Ar) && a.Name_Ar.Trim().ToLower().Contains(name.Trim().ToLower()))
                                || (!string.IsNullOrWhiteSpace(a.Name_En) && a.Name_En.Trim().ToLower().Contains(name.Trim().ToLower())))
                     ))));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
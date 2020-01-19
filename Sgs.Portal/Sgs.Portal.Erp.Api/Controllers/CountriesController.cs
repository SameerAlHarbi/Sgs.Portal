using AutoMapper;
using Sgs.Portal.Erp.Api.Models;
using Sgs.Portal.Erp.Api.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sgs.Portal.Erp.Api.Controllers
{
    //[RoutePrefix("countries")]
    public class CountriesController : BaseApiController
    {
        private readonly IErpCountriesManager _countriesManager;

        public CountriesController(IErpCountriesManager countriesManager
            ,IMapper mapper)
            : base(mapper)
        {
            _countriesManager = countriesManager;
        }

        [Route()]
        [ResponseType(typeof(IEnumerable<Country>))]
        [HttpGet()]
        [Route("countries")]
        public async Task<IHttpActionResult> GetCountries(string codes = null
            , string name = null
            , string nationalityName = null
            , bool fillRegions = false
            , bool fillCities = true
            , string language = "AR,EN")
        {
            try
            {
                var results = await _countriesManager
                    .GetCountriesCollectionAsync(codes?.Split(',')
                        , name
                        , nationalityName
                        , fillRegions
                        , fillCities
                        , language: language);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("countries/{code}")]
        public async Task<IHttpActionResult> GetCountry(string code)
        {
            try
            {
                var results = await _countriesManager.GetCountriesCollectionAsync(new string[] { code });
                var result = results.FirstOrDefault();

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route()]
        [ResponseType(typeof(IEnumerable<Country>))]
        [HttpGet()]
        [Route("cities")]
        public async Task<IHttpActionResult> GetCities(string codes = null, string name = null, string countries = null,string regions = null, string language = "AR,EN")
        {
            try
            {
                var results = await _countriesManager.GetCitiesCollectionAsync(codes?.Split(',')
                    , name
                    , countries?.Split(',')
                    , regions?.Split(',')
                    , language: language
                    );
                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
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
    [RoutePrefix("countries")]
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
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var results = await _countriesManager.GetCountriesCollection();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{code}")]
        public async Task<IHttpActionResult> Get(string code)
        {
            try
            {
                var results = await _countriesManager.GetCountriesCollection(new string[] { code });
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

        [Route("config/test")]
        [HttpGet()]
        public async Task<IHttpActionResult> GetConfigRead()
        {
            return Ok(ConfigurationManager.AppSettings["test"]);
        }

    }
}
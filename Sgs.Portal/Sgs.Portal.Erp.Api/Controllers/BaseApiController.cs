using AutoMapper;
using System.Web.Http;

namespace Sgs.Portal.Erp.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        private readonly IMapper _mapper;

        public BaseApiController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
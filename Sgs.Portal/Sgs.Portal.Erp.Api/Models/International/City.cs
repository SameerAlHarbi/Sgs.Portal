using System.Collections.Generic;

namespace Sgs.Portal.Erp.Api.Models.International
{
    public class City
    {
        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string CountryCode { get; set; }

        public string RegionCode { get; set; }

        public List<Airport> Airports { get; set; }
    }
}

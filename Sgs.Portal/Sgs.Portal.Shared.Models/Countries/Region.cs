using Sameer.Shared;
using System.Collections.Generic;

namespace Sgs.Portal.Shared.Models
{
    public class Region : ISameerObject
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string CountryCode { get; set; }

        public Country Country { get; set; }

        public List<City> Cities { get; set; }
    }
}

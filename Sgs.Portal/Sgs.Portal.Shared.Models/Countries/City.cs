using Sameer.Shared;
using System.Collections.Generic;

namespace Sgs.Portal.Shared.Models
{
    public class City : ISameerObject
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string RegionCode { get; set; }

        public Region Region { get; set; }

        public List<Airport> Airports { get; set; }
    }
}

using Sameer.Shared;

namespace Sgs.Portal.Shared.Models
{
    public class Airport : ISameerObject
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public bool Domestic { get; set; }

        public bool International { get; set; }

        public bool Approved { get; set; }

        public string CityCode { get; set; }

        public City City { get; set; }
    }
}

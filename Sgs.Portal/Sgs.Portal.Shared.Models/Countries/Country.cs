using Sameer.Shared;
using System.Collections.Generic;

namespace Sgs.Portal.Shared.Models
{
    public class Country : ISameerObject
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string NationalityName_Ar { get; set; }

        public string NationalityName_En { get; set; }

        public List<Region> Regions { get; set; }
    }
}

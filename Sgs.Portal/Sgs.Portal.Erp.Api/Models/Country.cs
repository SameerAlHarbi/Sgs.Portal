using System.Collections.Generic;

namespace Sgs.Portal.Erp.Api.Models
{
    public class Country
    {
        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public string NationalityName_Ar { get; set; }

        public string NationalityName_En { get; set; }

        public List<Region> Regions { get; set; }
    }
}
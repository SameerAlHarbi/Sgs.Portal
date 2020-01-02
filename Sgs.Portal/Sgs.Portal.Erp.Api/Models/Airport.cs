namespace Sgs.Portal.Erp.Api.Models
{
    public class Airport
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name_Ar { get; set; }

        public string Name_En { get; set; }

        public bool Domestic { get; set; }

        public bool International { get; set; }

        public bool Approved { get; set; }

        public string CityCode { get; set; }
    }
}

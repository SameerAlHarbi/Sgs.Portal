using System;
using Sameer.Shared;

namespace Sgs.Portal.Shared.Helpers
{
    public class VacationRequestViewModel : IApiViewModel
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName_Ar { get; set; }

        public string EmployeeName_En { get; set; }

        public string DepartmentCode { get; set; }

        public string DepartmentName_Ar { get; set; }

        public string DepartmentName_En { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ByEmployeeId { get; set; }

        public string ByEmployeeName_Ar { get; set; }

        public string ByEmployeeName_En { get; set; }

        public string VacationTypeCode { get; set; }

        public string VacationTypeName_Ar { get; set; }

        public string VacationTypeName_En { get; set; }

        public decimal? VacationTypeBalance { get; set; }

        public int? CommissionerId { get; set; }

        public string CommissionerName_Ar { get; set; }

        public string CommissionerName_En { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public string Note { get; set; }

    }
}

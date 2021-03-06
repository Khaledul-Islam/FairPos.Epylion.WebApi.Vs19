using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.Models.Setups
{
    public class Employee
    {
        public decimal EmpID { get; set; }

        public string RFCardNo { get; set; }

        public string StaffType { get; set; }

        public string vDesignation { get; set; }

        public string vDesignationBangla { get; set; }

        public string Department { get; set; }

        public string DepartmentBangla { get; set; }

        public string Name { get; set; }

        public string NameBangla { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Unit { get; set; }

        public decimal? FPSEnrollment { get; set; }

        public int? FamilyMembers { get; set; }

        public bool? DiscAllowed { get; set; }

        public string IsTransfer { get; set; }

        public bool? IsCreditAllowed { get; set; }

        public decimal? CreditLimit { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? UpdateDate { get; set; }

        public decimal? SpouseId { get; set; }

        [FIK.DAL.FIK_NoCUD]
        public int RecordFilter { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public int RecordCount { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public decimal AvailableLimit { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public object EmpImage { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public decimal Balance { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public decimal AvailableBalance { get; set; }
        [FIK.DAL.FIK_NoCUD]
        public List<EmployeeProduct> LstEmployeeProduct { get; set; }

    }
}

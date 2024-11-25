using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Services
{
    public class APIRequestAndResponseClass
    {
    }

    public class UserDetail
    {
        public string applicationcode { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class BranchList
    {
        public string BranchName { get; set; }
        public string BranchID { get; set; }
        public string Email { get; set; }
    }

    public class ResponseResult
    {
        public bool success { get; set; }
        public string token { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string StaffId { get; set; }
        public string Username { get; set; }
        public string BranchName { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
        public string currentFiscalYear { get; set; }

    }

    public class StaffList
    {
        public string fullName { get; set; }
        public string JobID { get; set; }
        public string email { get; set; }
        public string Username { get; set; }
        public string StaffId { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string BranchName { get; set; }
        public string FunctionalTitle { get; set; }
        public string BranchID { get; set; }
    }

    public class BranchListWithProvience
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string LocationName { get; set; }
        public string BranchFRO { get; set; }
        public string BranchFROEmail { get; set; }
        public string BranchFROStaffID { get; set; }
        public string BranchSRO { get; set; }
        public string BranchSROEmail { get; set; }
        public string BranchSROStaffID { get; set; }
    }

    public class DepartmentList
    {
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameNepali { get; set; }
        public string UnderDepartment { get; set; }
        public string DepartmentCode { get; set; }
        public string HeadEmpCode { get; set; }
        public string IsActive { get; set; }
        public string FunctionalCategoryID { get; set; }
        public string DepartmentForBranchTypes { get; set; }
        public string DepartmentEmail { get; set; }
    }
}

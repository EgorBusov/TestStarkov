using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStarkof.Data.Entities;

namespace TestStarkof.Models.Export
{
    public class AllInformationExportModel
    {
        public int IdDepartment { get; set; }
        public string DepartmentName { get; set; }
        public EmployeeExportModel LeadEmployee { get; set; }
        public List<EmployeeExportModel> Employees { get; set; }
        public List<AllInformationExportModel> ChildDepartments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStarkof.Data.Entities;
using TestStarkof.Models.Import;

namespace TestStarkof.Models.Export
{
    public class EmployeeExportModel
    {
        public int Id { get; set; }
        public string Department { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public JobTitleExportModel JobTitle { get; set; }
    }
}

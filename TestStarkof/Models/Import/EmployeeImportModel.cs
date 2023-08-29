using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.Models.Import
{
    /// <summary>
    /// Модель загрузки сотрудника
    /// </summary>
    public class EmployeeImportModel
    {
        public string Department { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string JobTitle { get; set; }
    }
}

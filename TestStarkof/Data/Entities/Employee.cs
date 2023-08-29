using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual JobTitle JobTitle { get; set; }
    }
}

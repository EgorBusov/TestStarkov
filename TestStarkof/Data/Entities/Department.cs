using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.Data.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public virtual Department Parent { get; set; }
        public int? ManagerId { get; set; }
        public virtual Employee Manager { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Department> ChildDepartments { get; set; } = new List<Department>();
    }
}

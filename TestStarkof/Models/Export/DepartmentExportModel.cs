using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.Models.Export
{
    public class DepartmentExportModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Manager { get; set; }
        public string Phone { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStarkof.Data.Entities;

namespace TestStarkof.Models.Import
{
    /// <summary>
    /// Модель для загрузки департаментов
    /// </summary>
    public class DepartmentImportModel
    {
        public string Name { get; set; }
        public string? Parent { get; set; }
        public string Manager { get; set; }
        public string Phone { get; set; }
    }
}

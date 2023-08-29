using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.Services.Interfaces
{
    public interface IImportDataService
    {
        int ImportDepartments(string path);
        int ImportEmployees(string path);
        int ImportJobTitles(string path);
    }
}

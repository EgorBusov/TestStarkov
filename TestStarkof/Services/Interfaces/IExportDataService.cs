using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStarkof.Models.Export;

namespace TestStarkof.Services.Interfaces
{
    public interface IExportDataService
    {
        List<DepartmentExportModel> GetDepartments();
        List<EmployeeExportModel> GetEmployees();
        List<JobTitleExportModel> GetJobTitles();
        List<AllInformationExportModel> GetAllInformationModels();
        AllInformationExportModel GetAllInformationModelById(int id);
    }
}

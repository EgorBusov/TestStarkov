using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStarkof.Data.Interfaces;
using TestStarkof.Models.Export;
using TestStarkof.Services.Interfaces;

namespace TestStarkof.Services
{
    /// <summary>
    /// получение моделей для экспорта
    /// </summary>
    public class ExportDataService : IExportDataService
    {
        private readonly IDataContext _dataContext;

        public ExportDataService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// Получение моделей департаментов для вывода
        /// </summary>
        /// <returns></returns>
        public List<DepartmentExportModel> GetDepartments()
        {
            return _dataContext.Departments
                .Select(d => new DepartmentExportModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Manager = d.Manager.FullName,
                    Parent = d.Parent.Name,
                    Phone = d.Phone
                })
                .ToList() ?? new List<DepartmentExportModel>();
        }

        /// <summary>
        /// Получение моделей сотрудников для вывода
        /// </summary>
        /// <returns></returns>
        public List<EmployeeExportModel> GetEmployees()
        {
            return _dataContext.Employees
                .Select(d => new EmployeeExportModel()
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    Login = d.Login,
                    Password = d.Password,
                    JobTitle = new JobTitleExportModel() { Id = d.JobTitle.Id, Name = d.JobTitle.Name },
                    Department = d.Department.Name
                })
                .ToList() ?? new List<EmployeeExportModel>();
        }

        /// <summary>
        /// получение моделей должностей для вывода
        /// </summary>
        /// <returns></returns>
        public List<JobTitleExportModel> GetJobTitles()
        {
            return _dataContext.JobTitles
                .Select(d => new JobTitleExportModel()
                {
                    Id = d.Id,
                    Name= d.Name
                })
                .ToList() ?? new List<JobTitleExportModel>();
        }

        /// <summary>
        /// получение моделей для вывода в консоль
        /// </summary>
        /// <returns></returns>
        public List<AllInformationExportModel> GetAllInformationModels()
        {
            List<AllInformationExportModel> models = new List<AllInformationExportModel>();

            List<int> idDepartmentsWithoutParentDepartment = _dataContext.Departments
                .Where(x => x.Parent == null)
                .Select(x => x.Id)
                .ToList();

            foreach(var id in idDepartmentsWithoutParentDepartment)
            {
                AllInformationExportModel model = GetAllInformationModelById(id);
                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Получение моделей по Id департамента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<AllInformationExportModel> GetAllInformationModelsByIdParentDepartment(int id)
        {
            List<AllInformationExportModel> models = new List<AllInformationExportModel>();

            List<int> idChildDepartments = _dataContext.Departments
                .Where(x => x.Parent.Id == id)
                .Select(x=>x.Id)
                .ToList();

            if(idChildDepartments.Count == 0)
                return models;

            foreach(var childDepartment in idChildDepartments)
            {
                AllInformationExportModel model = new AllInformationExportModel()
                {
                    IdDepartment = childDepartment,

                    LeadEmployee = _dataContext.Departments
                    .Where(x => x.Manager.Id == childDepartment)
                    .Select (x => new EmployeeExportModel
                    {
                        Id = x.Id,
                        JobTitle = new JobTitleExportModel() { Id = x.Manager.JobTitle.Id, Name = x.Manager.JobTitle.Name }
                    }).FirstOrDefault() ?? new EmployeeExportModel(),

                    Employees = _dataContext.Employees
                    .Where(x => x.Department.Id == childDepartment)
                    .Select(x => new EmployeeExportModel()
                    {
                        Id = x.Id,
                        JobTitle = new JobTitleExportModel() { Id = x.JobTitle.Id, Name = x.JobTitle.Name }
                    }).ToList(),

                    ChildDepartments = GetAllInformationModelsByIdParentDepartment(childDepartment)
                };

                models.Add(model);
            }

            return models;
        }

        /// <summary>
        /// Получение модели об одном департаменте
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AllInformationExportModel GetAllInformationModelById(int id)
        {
            AllInformationExportModel model = new AllInformationExportModel()
            {
                IdDepartment = id,
                DepartmentName = _dataContext.Departments
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefault() ?? String.Empty,

                LeadEmployee = _dataContext.Departments
                    .Where(x => x.Manager.Id == id)
                    .Select(x => new EmployeeExportModel
                    {
                        Id = x.Id,
                        JobTitle = new JobTitleExportModel() { Id = x.Manager.JobTitle.Id, Name = x.Manager.JobTitle.Name }
                    }).FirstOrDefault() ?? new EmployeeExportModel(),

                Employees = _dataContext.Employees
                    .Where(x => x.Department.Id == id)
                    .Select(x => new EmployeeExportModel()
                    {
                        Id = x.Id,
                        JobTitle = new JobTitleExportModel() { Id = x.JobTitle.Id, Name = x.JobTitle.Name },
                    }).ToList(),

                ChildDepartments = GetAllInformationModelsByIdParentDepartment(id)
            };

            return model;
        }
    }
}

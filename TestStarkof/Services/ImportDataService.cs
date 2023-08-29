using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestStarkof.Data.Entities;
using TestStarkof.Data.Interfaces;
using TestStarkof.Models.Import;
using TestStarkof.Services.Interfaces;

namespace TestStarkof.Services
{
    public class ImportDataService : IImportDataService
    {
        private readonly IDataContext _dataContext;

        public ImportDataService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Import

        /// <summary>
        /// Импорт департаментов в бд. Возврат: количество именений/добавлений
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int ImportDepartments(string path)
        {
            if (!ValidatorStaticService.ValidatePathFile(path))
                return 0;

            List<DepartmentImportModel> departments = ReadDepartamentsFromFile(path);

            foreach (DepartmentImportModel model in departments)
            {
                var department =_dataContext.Departments
                    .FirstOrDefault(x => x.Name == model.Name && x.Parent.Name == model.Parent);

                if (department != null)
                {
                    if (department.Manager.FullName != model.Manager)
                    {
                        var manager = _dataContext.Employees.FirstOrDefault(x => x.FullName == model.Manager);
                        if (manager != null)
                            department.Manager = manager;
                    }
                }
                else
                {
                    Department addDepartment = new Department()
                    {
                        Name = model.Name,
                        Phone = model.Phone,
                        Manager = _dataContext.Employees.FirstOrDefault(x => x.FullName == model.Name),
                        Parent = _dataContext.Departments.FirstOrDefault(x => x.Name == model.Name),
                    };

                    if (addDepartment.Parent == null)
                        continue;

                    _dataContext.Departments.Add(addDepartment);
                }
            }

            return _dataContext.SaveChanges();
        }

        /// <summary>
        /// Импорт сотрудников в бд. Возврат: количество именений/добавлений
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int ImportEmployees(string path)
        {
            if (!ValidatorStaticService.ValidatePathFile(path))
                return 0;

            List<EmployeeImportModel> employees = ReadEmployeesFromFile(path);

            foreach(EmployeeImportModel model in employees)
            {
                var employee = _dataContext.Employees.FirstOrDefault(x => x.FullName == model.FullName);

                if (employee != null)
                {
                    if (employee.Department.Name != model.Department)
                    {
                        var department = _dataContext.Departments.FirstOrDefault(x => x.Name == model.Department);
                        if (department != null)
                            employee.Department = department;
                    }

                    if(employee.JobTitle.Name != model.JobTitle)
                    {
                        var jobTitle = _dataContext.JobTitles.FirstOrDefault(x => x.Name == model.JobTitle);
                        if(jobTitle != null)
                            employee.JobTitle = jobTitle;
                    }

                    employee.FullName = model.FullName;
                    employee.Login = model.Login;
                    employee.Password = model.Password;
                }
                else
                {
                    Employee addEmployee = new Employee()
                    {
                        FullName = model.FullName,
                        Login = model.Login,
                        Password = model.Password,
                        Department = _dataContext.Departments.FirstOrDefault(x => x.Name == model.Department),
                        JobTitle = _dataContext.JobTitles.FirstOrDefault(x => x.Name == model.JobTitle)
                    };

                    if (addEmployee.Department == null || addEmployee.JobTitle == null)
                        continue;

                    _dataContext.Employees.AddAsync(addEmployee);
                }
            }

            return _dataContext.SaveChanges();
        }

        /// <summary>
        /// Импорт должностей в бд. Возврат: количество именений/добавлений
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int ImportJobTitles(string path)
        {
            if (!ValidatorStaticService.ValidatePathFile(path))
                return 0;

            List<JobTitleImportModel> jobTitles = ReadJobTitlesFromFile(path);

            foreach (JobTitleImportModel model in jobTitles)
            {
                
                JobTitle jobTitle = new JobTitle()
                {
                    Name = model.Name
                };

                _dataContext.JobTitles.Add(jobTitle);
            }

            return _dataContext.SaveChanges();
        }

        #endregion

        #region Read

        /// <summary>
        /// Чтение из файла департаментов
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<DepartmentImportModel> ReadDepartamentsFromFile(string path)
        {
            List<DepartmentImportModel> departments = new List<DepartmentImportModel>();

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.ReadLine();
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("\t");

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] values = parser.ReadFields();

                        DepartmentImportModel department = new DepartmentImportModel
                        {
                            Name = values[0],
                            Parent = values[1],
                            Manager = values[2],
                            Phone = values[3],
                        };

                        FormatedDepartment(department);

                        if (!department.Validate())
                            throw new Exception();

                        departments.Add(department);
                    }
                    catch
                    {
                        Console.Error.WriteLine("Ошибка: некорректная запись в файле.");
                        continue;
                    }
                }
            }

            return departments;
        }

        /// <summary>
        /// Чтение из файла сотрудников
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<EmployeeImportModel> ReadEmployeesFromFile(string path)
        {
            List<EmployeeImportModel> employees = new List<EmployeeImportModel>();

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] values = parser.ReadFields();

                        EmployeeImportModel employee = new EmployeeImportModel
                        {
                            Department = values[0],
                            FullName = values[1],
                            Login = values[2],
                            Password = values[3],
                            JobTitle = values[4],
                        };

                        FormatedEmployee(employee);

                        if (!employee.Validate())
                            throw new Exception();

                        employees.Add(employee);
                    }
                    catch
                    {
                        Console.Error.WriteLine("Ошибка: некорректная запись в файле.");
                        continue;
                    }
                }
            }

            return employees;
        }

        /// <summary>
        /// Чтение из файла должностей
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<JobTitleImportModel> ReadJobTitlesFromFile(string path)
        {
            List<JobTitleImportModel> jobTitles = new List<JobTitleImportModel>();

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    try
                    {
                        string[] values = parser.ReadFields();

                        JobTitleImportModel jobTitle = new JobTitleImportModel
                        {
                            Name = values[0]
                        };

                        FormatedJobTitle(jobTitle);

                        if (!jobTitle.Validate())
                            throw new Exception();

                        jobTitles.Add(jobTitle);
                    }
                    catch
                    {
                        Console.Error.WriteLine("Ошибка: некорректная запись в файле.");
                        continue;
                    }
                }
            }

            return jobTitles;
        }

        #endregion

        #region Formatted

        /// <summary>
        /// приводит поля департамента к стандартному виду
        /// </summary>
        /// <param name="model"></param>
        private void FormatedDepartment(DepartmentImportModel model)
        {

            if (!string.IsNullOrEmpty(model.Name))
            {
                model.Name = model.Name.Trim().ToLower();
                model.Name = char.ToUpper(model.Name[0]) + model.Name.Substring(1);
            }

            if (!string.IsNullOrEmpty(model.Parent))
            {
                model.Parent = model.Parent.Trim().ToLower();
                model.Parent = char.ToUpper(model.Parent[0]) + model.Parent.Substring(1);
            }

            if (!string.IsNullOrEmpty(model.Manager))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                model.Manager = model.Manager.Trim().ToLower();
                model.Manager = textInfo.ToTitleCase(model.Manager);
            }

            if (!string.IsNullOrEmpty(model.Phone))
            {
                model.Phone = Regex.Replace(model.Phone, "[^0-9]", "");
                if (model.Phone[0] == '8' && model.Phone.Count() == 11)
                {
                    model.Phone = '7' + model.Phone.Substring(1);
                }
            }
        }

        /// <summary>
        /// приводит поля сотрудника к стандартному виду
        /// </summary>
        /// <param name="model"></param>
        private void FormatedEmployee(EmployeeImportModel model)
        {
            if (string.IsNullOrEmpty(model.Department))
            {
                model.Department = model.Department.Trim().ToLower();
                model.Department = char.ToUpper(model.Department[0]) + model.Department.Substring(1);
            }

            if(string.IsNullOrEmpty(model.FullName))
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                model.FullName = model.FullName.Trim().ToLower();
                model.FullName = textInfo.ToTitleCase(model.FullName);
            }

            if(string.IsNullOrEmpty(model.JobTitle))
            {
                model.JobTitle = model.JobTitle.Trim().ToLower();
                model.JobTitle = char.ToUpper(model.JobTitle[0]) + model.JobTitle.Substring(1);
            }
        }

        /// <summary>
        /// Приводит поля должности к стандартному виду
        /// </summary>
        /// <param name="model"></param>
        private void FormatedJobTitle(JobTitleImportModel model)
        {
            if(string.IsNullOrEmpty(model.Name))
            {
                model.Name = model.Name.Trim().ToLower();
                model.Name = char.ToUpper(model.Name[0]) + model.Name.Substring(1);
            }
        }

        #endregion
    }
}

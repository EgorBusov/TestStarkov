using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestStarkof.Data.Entities;
using TestStarkof.Models.Export;
using TestStarkof.Services.Interfaces;

namespace TestStarkof.Services
{
    public class PresentationService : IPresentationService
    {
        private readonly IExportDataService _exportDataService;

        public PresentationService(IExportDataService exportDataService)
        {
            _exportDataService = exportDataService;
        }

        /// <summary>
        /// Вывод в консоль департаментов
        /// </summary>
        public void PrintDepartments()
        {
            int count = 0;
            var departmentModels = _exportDataService.GetDepartments();

            Console.WriteLine($"{"Id",-10} | " +
                $"{"Департамент",-30} | " +
                $"{"Родительский департамент",-30} | " +
                $"{"Руководитель",-40} | " +
                $"{"телефон",-12}");

            foreach ( var model in departmentModels)
            {
                Console.WriteLine($"{model.Id, -10} | " +
                    $"{model.Name,-30} | " +
                    $"{model.Parent,-30} | " +
                    $"{model.Manager,-40} | " +
                    $"{model.Phone,-12}");

                count++;
            }

            Console.WriteLine($"Количество: {count}");
        }

        /// <summary>
        /// Вывод работников в консоль
        /// </summary>
        public void PrintEmployees()
        {
            int count = 0;
            var employeeModels = _exportDataService.GetEmployees();

            Console.WriteLine($"{"Id",-10} | " +
                $"{"Департамент",-30} | " +
                $"{"ФИО",-40} | " +
                $"{"Логин",-20} | " +
                $"{"Пароль",-20} | " +
                $"{"Должность",-20}");

            foreach( var model in employeeModels)
            {
                Console.WriteLine($"{model.Id,-10}| " +
                    $"{model.Department,-30} | " +
                    $"{model.FullName,-40} | " +
                    $"{model.Login,-20} | " +
                    $"{model.Password,-20} | " +
                    $"{model.JobTitle.Name,-12}");

                count++;
            }

            Console.WriteLine($"Количество: {count}");
        }

        /// <summary>
        /// Вывод должностей в консоль
        /// </summary>
        public void PrintJobTitles()
        {
            int count = 0;
            var jobTitleModels = _exportDataService.GetJobTitles();

            Console.WriteLine($"{"Id",-10} | " +
                $"{"Название",-20}");

            foreach(var model in jobTitleModels)
            {
                Console.WriteLine($"{model.Id,-10}| " +
                    $"{model.Name,-30}");

                count++;
            }

            Console.WriteLine($"Количество: {count}");
        }

        /// <summary>
        /// Вывод в консоль инфомрации о департаментах, сотрудниках департаментов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void PrintAllInformationModels(int id = 0)
        {
            List<AllInformationExportModel> models = new List<AllInformationExportModel>();

            if(id != 0)
            {
                models.Add(_exportDataService.GetAllInformationModelById(id));
            }
            else
            {
                models = _exportDataService.GetAllInformationModels();
            }

            foreach(var model in models)
            {
                int levelPrefix = 0;
                PrintAllInformationModel(model, ref levelPrefix);
            }
        }

        /// <summary>
        /// Вывод в консоль информации о департаменте и его сотрудниках
        /// </summary>
        /// <param name="model"></param>
        /// <param name="levelPrefix"></param>
        private void PrintAllInformationModel(AllInformationExportModel model,ref int levelPrefix)
        {
            levelPrefix++;

            string prefixDepartment = new string('=', levelPrefix) + " ";
            string prefixLeadEmployee = new string('*', levelPrefix) + " ";
            string prefixEmployee = new string(' ', levelPrefix) + "- ";

            Console.WriteLine($"{prefixDepartment}{model.DepartmentName} ID={model.IdDepartment}");
            Console.WriteLine($"{prefixLeadEmployee}{model.LeadEmployee.FullName} " +
                $"ID={model.LeadEmployee.Id} ({model.LeadEmployee.JobTitle.Name} ID={model.LeadEmployee.JobTitle.Id}");

            foreach (var employee in model.Employees)
            {
                Console.WriteLine($"{prefixEmployee} ID={employee.Id} ({employee.JobTitle.Name})");
            }
            
            if(model.ChildDepartments.Count > 0)
            {
                foreach(var department in  model.ChildDepartments)
                {
                    PrintAllInformationModel(department, ref levelPrefix);
                }
            }
        }
    }
}

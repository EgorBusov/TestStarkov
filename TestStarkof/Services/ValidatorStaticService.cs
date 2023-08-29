using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestStarkof.Data.Entities;
using TestStarkof.Models.Import;
using TestStarkof.Services.Interfaces;

namespace TestStarkof.Services
{
    /// <summary>
    /// Проверка на валидность
    /// </summary>
    public static class ValidatorStaticService
    {
        private static string _extentionFile = ".tsv";
        private static string _patternPhone = @"^(\+7|7|8)\d{10}$";

        /// <summary>
        /// Валидация пути к файлу и расширения
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ValidatePathFile(string path)
        {
            bool a = string.IsNullOrEmpty(path);
            bool b = !File.Exists(path);
            bool c = Path.GetExtension(path) != _extentionFile;

            if (a ||
                b ||
                c)
                return false;
                  
            return true;
        }

        /// <summary>
        /// Валидация модели добавления департамента
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Validate(this DepartmentImportModel model)
        {
            if (model == null ||
                string.IsNullOrEmpty(model.Name) ||
                string.IsNullOrEmpty(model.Phone)) 
                return false;

            if(!Regex.IsMatch(model.Phone, _patternPhone))
                return false;

            return true;
        }

        /// <summary>
        /// Валидация модели добавиления сотрудника
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Validate(this EmployeeImportModel model)
        {
            if(model == null ||
                string.IsNullOrEmpty(model.Department) ||
                string.IsNullOrEmpty(model.FullName) ||
                string.IsNullOrEmpty(model.Login) ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.JobTitle))
                return false;

            return true;
        }

        /// <summary>
        /// Валидация модели должности
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Validate(this JobTitleImportModel model)
        {
            if(model == null ||
                string.IsNullOrEmpty(model.Name))
                return false;

            return true;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.Services.Interfaces
{
    public interface IPresentationService
    {
        void PrintDepartments();
        void PrintEmployees();
        void PrintJobTitles();
        void PrintAllInformationModels(int id = 0);

    }
}

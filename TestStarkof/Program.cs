using Npgsql;
using TestStarkof.Data;
using TestStarkof.Data.Interfaces;
using TestStarkof.Services;
using TestStarkof.Services.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {      
        IDataContext _dataContext = new PostgreContext();
        IImportDataService _importDataService = new ImportDataService(_dataContext);
        IExportDataService _exportDataService = new ExportDataService(_dataContext);
        IPresentationService _presentationService = new PresentationService(_exportDataService);

        bool whileTracker = true;

        while(whileTracker)
        {
            Console.WriteLine("1 - импорт департаментов\n" +
            "2 - импорт сотрудников\n" +
            "3 - импорт должностей\n" +
            "4 - вывод всей информации\n" +
            "5 - вывод информации по id\n" +
            "6 - закрыть программу");

            var check = Console.ReadLine();

            int result;
            string path;

            switch (check)
            {
                case "1":
                    Console.WriteLine("Введите путь к файлу");
                    path = Console.ReadLine();

                    result = _importDataService.ImportDepartments(path);
                    Console.WriteLine($"Изменений/добавлений: {result}");

                    _presentationService.PrintDepartments();
                    break;
                case "2":
                    Console.WriteLine("Введите путь к файлу");

                    path = Console.ReadLine();

                    result = _importDataService.ImportEmployees(path);
                    Console.WriteLine($"Изменений/добавлений: {result}");

                    _presentationService.PrintEmployees();
                    break;
                case "3":
                    Console.WriteLine("Введите путь к файлу");

                    path = Console.ReadLine();

                    result = _importDataService.ImportJobTitles(path);
                    Console.WriteLine($"Изменений/добавлений: {result}");

                    _presentationService.PrintJobTitles();
                    break;
                case "4":
                    _presentationService.PrintAllInformationModels();
                    break;
                case "5":
                    Console.WriteLine("Введите id департамента");

                    string res = Console.ReadLine();
                    int id;
                    bool checkParse = int.TryParse(res, out id);

                    if (!checkParse)
                    {
                        Console.WriteLine("Это не число");
                        continue;
                    }

                    _presentationService.PrintAllInformationModels(id);
                    break;
                case "6":
                    whileTracker = false;
                    break;
                default: 
                    Console.WriteLine("Я такого не умею.");
                    break;
            }
        }
    }
}
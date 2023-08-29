using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStarkof.Data.Entities;

namespace TestStarkof.Data.Interfaces
{
    public interface IDataContext
    {
        DbSet<Department> Departments { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<JobTitle> JobTitles { get; set; }

        int SaveChanges();
        Task AddRangeAsync(params object[] entities);
    }
}

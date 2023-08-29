using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarkof.AppResourses
{
    public static class AppResources
    {
        private static string _connectionStringPostgre = "";

        public static string GetConnectionStringPostgre()
        {
            return _connectionStringPostgre;
        }
    }
}

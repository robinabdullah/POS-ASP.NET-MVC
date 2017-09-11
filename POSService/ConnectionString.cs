using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSService
{
    class ConnectionString
    {
        //public static string connectionStringLinq = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+ Directory.GetCurrentDirectory() + @"\Files\POS.mdf;Integrated Security=True";
        //public static string connectionStringLinq = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\Using soft\programming\Programmings\Github Repos\Point Of Sale\Point Of Sale\Files\POSNew12.mdf;Integrated Security=True";

        public static string connectionStringLinq = @"Data Source=.\SQLEXPRESS;Initial Catalog=POSNew12;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
          
    }
}

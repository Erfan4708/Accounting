using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;

namespace DataLayer_Test
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                int TypeID = 2;
                var result = db.AccountingRepository.Get(n => n.TypeID == TypeID);
                var result_list = result.ToList();
                foreach (var row in result_list)
                {
                    var FullName = row.Customers.FullName;
                    Console.WriteLine(FullName);
                }
            }
            Console.ReadLine();
        }
    }
}
  
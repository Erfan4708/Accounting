using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer;
using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;

namespace DataLayer_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICustomerRepository customer = new CustomerRepository();

            Customers AddCustomer = new Customers()
            {
                FullName = "مریم تنابنده",
                Mobile = "09128456985",
                Email = "maryam@maryam.com",
                Address = "تهران",
                CustomerImage = "NoPhoto"
            };
            customer.InsertCustomer(AddCustomer);
            customer.Save();

            var list = customer.GetAllCustomers();
            
        }
    }
}

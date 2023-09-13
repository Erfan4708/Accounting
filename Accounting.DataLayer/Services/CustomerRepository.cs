using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Repositories;
using Accounting.ViewModels.Customers;
using System.Data.Entity;
namespace Accounting.DataLayer.Services

{
    public class CustomerRepository : ICustomerRepository
    {
        Accounting_DBEntities2 db;

        public CustomerRepository(Accounting_DBEntities2 context)
        {
            db = context;
        }

        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();
        }

        public Customers GetCustomerbyId(int customerid)
        {
            return db.Customers.Find(customerid);
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerid)
        {
            try
            {
                var customer = GetCustomerbyId(customerid);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }



        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<Customers> GetCustomersByFilter(string parameter)
        {
            return db.Customers.Where(n=>
            n.FullName.Contains(parameter) || n.Email.Contains(parameter) || n.Mobile.Contains(parameter)).ToList();
        }


        public List<ListCustomerViewModel> GetnameCustomers(string filter = "")
        {
            if (filter == null)
            {
                return db.Customers.Select(c => new ListCustomerViewModel()
                {
                    FullName = c.FullName
                }).ToList();
            }
            else
            {
                return db.Customers.Where(c => c.FullName.Contains(filter)).Select(c => new ListCustomerViewModel()
                {
                    FullName = c.FullName
                }).ToList();
            }
        }

        public int GetCustomerIdByName(string name)
        {
            return db.Customers.First(c=> c.FullName == name).CustomerID;
        }
    }
}

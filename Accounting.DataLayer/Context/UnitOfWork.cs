using Accounting.DataLayer.Repositories;
using Accounting.DataLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Context
{
    public class UnitOfWork: IDisposable
    {
        Accounting_DBEntities2 db = new Accounting_DBEntities2();

        private ICustomerRepository _customerRepository;
        public ICustomerRepository CustomerRepository { get
            {
                if(_customerRepository == null)
                {
                    _customerRepository = new CustomerRepository(db);
                }
                return _customerRepository;
            } 
        }

        private GenericRepository<Accounting> _AccountingRepository;
        public GenericRepository<Accounting> AccountingRepository { get
           {
                if(_AccountingRepository == null)
                {
                    _AccountingRepository = new GenericRepository<Accounting>(db);
                }
                return _AccountingRepository;
            } 
           }

        private GenericRepository<Login> _LoginRepository;
        public GenericRepository<Login> LoginRepository 
        { 
            get
            {
                if(_LoginRepository == null)
                {
                    _LoginRepository = new GenericRepository<Login>(db);
                    return _LoginRepository;
                }        
                return _LoginRepository;           
            } 
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}

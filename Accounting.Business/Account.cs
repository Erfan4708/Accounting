using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Context;
using Accounting.ViewModels.Accounting;


namespace Accounting.Business
{
    public class Account
    {
        public static ReportViewModel ReportFormMain()
        {
           ReportViewModel reportViewModel = new ReportViewModel();
            
            using(UnitOfWork db = new UnitOfWork())
            {
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);
                var receive = db.AccountingRepository.Get(n => n.DateTime >= startDate && n.DateTime <= endDate && n.TypeID == 1).Select(n => n.Amount).ToList();
                var pay = db.AccountingRepository.Get(n => n.DateTime >= startDate && n.DateTime <= endDate && n.TypeID == 2).Select(n => n.Amount).ToList();

                reportViewModel.Receive = receive.Sum();
                reportViewModel.Pay = pay.Sum();
                reportViewModel.Balance = (receive.Sum() - pay.Sum());

            }
            return reportViewModel;
        }
    }
}

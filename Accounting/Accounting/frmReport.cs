using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.Utility;
using Accounting.Forms;
using System.Windows.Forms.VisualStyles;
using Accounting.DataLayer;
using System.Xml.Linq;
using Accounting.ViewModels.Customers;

namespace Accounting
{
    public partial class frmReport : Form
    {
        public int TypeID = 0;
        public frmReport()
        {
            InitializeComponent();
        }


        private void frmReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel() 
                {
                    CustomerID = 0,
                    FullName = "انتخاب کنید" 
                });
                list.AddRange(db.CustomerRepository.GetnameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }
            if (TypeID == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            else
            {
                this.Text = "گزارش پرداختی ها";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");

            foreach (DataGridViewRow item in dgReport.Rows)
            {
                dtPrint.Rows.Add(
                item.Cells[0].Value.ToString(),
                item.Cells[1].Value.ToString(),
                item.Cells[2].Value.ToString(),
                item.Cells[3].Value.ToString()
                );
            }
            stiPrint.Load(Application.StartupPath + "/Report.mrt");
            stiPrint.RegData("DT", dtPrint);
            stiPrint.Print();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            dgReport.AutoGenerateColumns = false;
            Filter();
        }

        private void dgReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        void Filter()
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                DateTime? startDate;
                DateTime? endDate;
                 

                int CustomerID = int.Parse(cbCustomer.SelectedValue.ToString());
                if (CustomerID != 0)
                {
                    result.AddRange(db.AccountingRepository.Get(n =>n.TypeID == TypeID && n.CustomerID == CustomerID));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(n => n.TypeID == TypeID));
                }
                if (txtFromDate.Text != "    /  /")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    startDate = DateConvertor.ToMiladi(startDate.Value);
                    result = result.Where(r => r.DateTime >= startDate.Value).ToList();
                }
                if (txtToDate.Text != "    /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    endDate = DateConvertor.ToMiladi(endDate.Value);
                    result = result.Where(r => r.DateTime <= endDate.Value).ToList();

                }
                dgReport.Rows.Clear();
                foreach (var row in result)
                {
                    var FullName = row.Customers.FullName;
                    dgReport.Rows.Add(row.ID, FullName, row.Amount, row.DateTime.ToShamsi(), row.Description);
                    
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                using(UnitOfWork db = new UnitOfWork())
                {
                    db.AccountingRepository.Delete(id);
                    db.Save();
                    Filter();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null) { 
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewAccounting frm = new frmNewAccounting();
                frm.AccountingID = id;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
        }

        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

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
using ValidationComponents;

namespace Accounting.Forms
{
    public partial class frmNewAccounting : Form
    {
        public frmNewAccounting()
        {
            InitializeComponent();
        }

        private void frmNewAccounting_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = db.CustomerRepository.GetnameCustomers();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = db.CustomerRepository.GetnameCustomers(txtFilter.Text);
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if (!rbRecive.Checked || rbPay.Checked)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        DataLayer.Accounting accounting = new DataLayer.Accounting()
                        {
                            Amount = Convert.ToInt32(txtAmount.Value.ToString()),
                            CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text), 
                            TypeID = (rbRecive.Checked)?1:2,
                            DateTime = DateTime.Now,
                            Description = txtDescription.Text,
                    };
                        db.AccountingRepository.insert(accounting);
                        db.Save();
                        DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    MessageBox.Show("لطفا نوع تراکنش را وارد کنید.");
                }
            }
        }
    }
}

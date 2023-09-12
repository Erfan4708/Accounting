using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.Forms
{
    public partial class frmAddOrEditCustomer : Form
    {
        public int CustomerId = 0;
        public frmAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog() == DialogResult.OK) 
            {
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(BaseValidator.IsFormValid(this.components))
            {

                String imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                string path = Application.StartupPath + "/Images/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path + imageName);

                using(UnitOfWork db = new UnitOfWork())
                {
                    Customers customer = new Customers()
                    {
                        FullName = txtName.Text,
                        Mobile = txtMobile.Text,
                        Email = txtEmail.Text,
                        Address = txtAddress.Text,
                        CustomerImage = imageName
                    };

                    if (CustomerId == 0)
                    {
                        db.CustomerRepository.InsertCustomer(customer);
                    }
                    else
                    {
                        customer.CustomerID = CustomerId;
                        db.CustomerRepository.UpdateCustomer(customer);
                    }
                    
                    db.Save();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void frmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (CustomerId != 0)
            {
                using(UnitOfWork db = new UnitOfWork())
                {
                    this.Text = "ویرایش شخص";
                    var Customer = db.CustomerRepository.GetCustomerbyId(CustomerId);
                    txtName.Text = Customer.FullName;
                    txtMobile.Text = Customer.Mobile;
                    txtEmail.Text = Customer.Email;
                    txtAddress.Text = Customer.Address;
                    pcCustomer.ImageLocation = Application.StartupPath + "/Images/" + Customer.CustomerImage;
                    
                }
            }
        }
    }
}

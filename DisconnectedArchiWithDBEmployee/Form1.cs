using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace DisconnectedArchiWithDBEmployee
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlDataAdapter da;
        SqlCommandBuilder scb;
        DataSet ds;
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["defaultConn"].ConnectionString);
        }

        private DataSet GetEmployee()
        {
            da = new SqlDataAdapter("select * from employee", conn);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            scb = new SqlCommandBuilder(da); // track DataSet & generate qry & assign to da object

            ds = new DataSet();
            da.Fill(ds, "emp");// emp is a table name given to DataSet table
            return ds;
        }
         
        public void ClearField()
        {
            txtId.Clear();
            txtName.Clear();
            txtEmail.Clear();
            txtSal.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ds=GetEmployee();
                DataRow row = ds.Tables["emp"].NewRow();
                row["ename"]=txtName.Text;
                row["email"]=txtEmail.Text;
                row["esal"]=txtSal.Text;

                ds.Tables["emp"].Rows.Add(row);

                int result = da.Update(ds.Tables["emp"]);
                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearField();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployee();
                DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    txtName.Text = row["ename"].ToString();
                    txtEmail.Text = row["email"].ToString();
                    txtSal.Text = row["esal"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found !");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCklear_Click(object sender, EventArgs e)
        {
            ClearField();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployee();
                DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    row["ename"] = txtName.Text;
                    row["email"] = txtEmail.Text;
                    row["esal"] = txtSal.Text;
                    int result = da.Update(ds.Tables["emp"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Record updated");
                        ClearField();
                    }


                }
                else
                {
                    MessageBox.Show("Record not found for Id");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetEmployee();
                DataRow row = ds.Tables["emp"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    row.Delete();
                    int result = da.Update(ds.Tables["emp"]);
                    if (result >= 1)
                    {
                        MessageBox.Show("Employee Deleted");
                        ClearField();
                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ds=GetEmployee();
            dataGridView1.DataSource = ds.Tables["emp"];
        }
    }
}

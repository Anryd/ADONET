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

namespace PluralsightADONET
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlDataAdapter sAdapt;
        SqlCommandBuilder cmb;
        DataSet ds;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string cs = @"Data Source=ELEV\;Initial Catalog=ADONET1;Integrated Security=True";
            con = new SqlConnection(cs);
            sAdapt = new SqlDataAdapter("Select * from Employees", con);
            cmb = new SqlCommandBuilder(sAdapt);
            ds = new DataSet();
            sAdapt.Fill(ds, "Employees");
            ds.Tables[0].Constraints.Add("Empno_PK", ds.Tables[0].Columns[0], true);
            if (ds.Tables["Employees"].Rows.Contains(txtEmpno.Text) == true)
                MessageBox.Show("Employee already exists", this.Text);
            else
            {
                DataRow row;
                row = ds.Tables[0].NewRow();
                row["Empno"] = txtEmpno.Text;
                row["Ename"] = txtEname.Text;
                row["Salary"] = txtSalary.Text;
                row["HireDate"] = dtpHireDate.Value;

                ds.Tables[0].Rows.Add(row);
                sAdapt.Update(ds.Tables[0]);
                MessageBox.Show("Added Employee Record", this.Text);
                gViewEmployeers.DataSource = ds.Tables[0]; 
                gViewEmployeers.Refresh();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            string cs = @"Data Source=ELEV\;Initial Catalog=ADONET1;Integrated Security=True";
            con = new SqlConnection(cs);
            sAdapt = new SqlDataAdapter("Select * from Employees", con);
            cmb = new SqlCommandBuilder(sAdapt);
            ds = new DataSet();
            sAdapt.Fill(ds, "Employees");
            ds.Tables["Employees"].Constraints.Add("Empno_PK", ds.Tables["Employees"].Columns["Empno"], true);
            gViewEmployeers.DataSource = ds.Tables[0];


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int empno = int.Parse(txtEmpno.Text);
            if (ds.Tables["Employees"].Rows.Contains(empno) == true)
            {
                DataRow row = ds.Tables["Employees"].Rows.Find(empno);
                txtEname.Text = row["Ename"].ToString();
                txtSalary.Text = row["Salary"].ToString();
                dtpHireDate.Text = row["HireDate"].ToString();
            }
            else
            {
                MessageBox.Show("Empno doesn't exists");
                txtEname.Clear();
                txtSalary.Clear();
                //dtpHireDate.Clear();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int eno = int.Parse(txtEmpno.Text);
            DataRow row;
            row = ds.Tables["Employees"].Rows.Find(eno);
            row.BeginEdit();
            row["Ename"] = txtEname.Text;
            row["Salary"] = txtSalary.Text;
            row.EndEdit();
            sAdapt.Update(ds.Tables["Employees"]);
            MessageBox.Show("Anställd uppdaterad", "Update");

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Är du säker på att du vill ta bort anställningsnr?", "Delete", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int eno = int.Parse(txtEmpno.Text);
                ds.Tables["Employees"].Rows.Find(eno).Delete();
                sAdapt.Update(ds.Tables["Employees"]);
                MessageBox.Show("Anställningsnr borttaget", "Delete");
                txtEname.Clear();
                txtSalary.Clear();
                dtpHireDate.Text = "";

            }
        }
    }
}

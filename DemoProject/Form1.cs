using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoProject
{
    public partial class Form1 : Form
    {
        StudentTable stu = new StudentTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtName.Text = txtGender.Text = txtAge.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            stu.StudentID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            stu.Name = txtName.Text.Trim();
            stu.Gender = txtGender.Text.Trim();
            stu.Age = txtAge.Text.Trim();
            using (DBEntities db = new DBEntities())
            {
                if (stu.StudentID == 0)//INSERT 
                    db.StudentTables.Add(stu);
                else //UPDATE
                    db.Entry(stu).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted Successully");
        }


        void PopulateDataGridView()
        {
            dgvStudent.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvStudent.DataSource = db.StudentTables.ToList<StudentTable>();
            }
        }

        private void dgvStudent_DoubleClick(object sender, EventArgs e)
        {
            if(dgvStudent.CurrentRow.Index != -1)
            {
                stu.StudentID = Convert.ToInt32(dgvStudent.CurrentRow.Cells["StudentID"].Value);

                using(DBEntities db =new DBEntities())
                {
                    stu = db.StudentTables.Where(x => x.StudentID == stu.StudentID).FirstOrDefault();
                    txtName.Text = stu.Name;
                    txtGender.Text = stu.Gender;
                    txtAge.Text = stu.Age;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }
    }
}

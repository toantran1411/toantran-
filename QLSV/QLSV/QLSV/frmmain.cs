using QLSV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class frmmain : Form
    {
        public frmmain()
        {
            InitializeComponent();
        }

        private void frmmain_Load(object sender, EventArgs e)
        {
            Model1 context = new Model1();
            var list = context.Faculties.ToList();
            var liststudent = context.Students.ToList();
            fillFacultyCBB(list);
            FillDGV(liststudent);
        }
        private void fillFacultyCBB(List<Faculty> list)
        {
            this.cbbFaculty.DataSource = list;
            this.cbbFaculty.DisplayMember = "FacultyName";
            this.cbbFaculty.ValueMember = "FacultyID";
        }
        private void FillDGV(List<Student> list)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in list)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells["MSSV"].Value = item.StudentID;
                dgvStudent.Rows[index].Cells["FullName"].Value = item.FullName;
                dgvStudent.Rows[index].Cells["Faculty"].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells["AverageScore"].Value = item.AverageScore;

            }
        }
        private void ClearInputFields()
        {
            txtMSSV.Text = "";
            txtName.Text = "";
            cbbFaculty.SelectedValue = 1;
            txtAVS.Text = "";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var ID = txtMSSV.Text;
            var Fullname = txtName.Text;
            var Faculty = cbbFaculty.SelectedValue;
            var Score = txtAVS.Text;
            if (txtMSSV.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống MSSV");
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống Họ tên");
                return;
            }
            if (txtAVS.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống Điểm trung bình");
                return;
            }
            if (float.Parse(txtAVS.Text) < 0 || float.Parse(txtAVS.Text) > 10)
            {
                MessageBox.Show("Vui lòng nhập điểm từ 0-10");
                return;
            }
            try
            {
                Student student = new Student();
                student.StudentID = ID;
                student.FullName = Fullname;
                student.FacultyID = (int)Faculty;
                student.AverageScore = float.Parse(Score);
                Model1 db = new Model1();
                db.Students.Add(student);
                db.SaveChanges();
                var liststudent = db.Students.ToList();
                FillDGV(liststudent);
                ClearInputFields();
                MessageBox.Show("Đã thêm thành công!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm đó là: " + ex.InnerException.Message);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex;
            txtMSSV.Text = dgvStudent.Rows[row].Cells["MSSV"].Value.ToString();
            txtName.Text = dgvStudent.Rows[row].Cells["FullName"].Value.ToString();
            cbbFaculty.Text = dgvStudent.Rows[row].Cells["Faculty"].Value.ToString();
            txtAVS.Text = dgvStudent.Rows[row].Cells["AverageScore"].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            var ID = txtMSSV.Text;
            var student = db.Students.SingleOrDefault(x => x.StudentID == ID);
            if(student == null)
            {
                MessageBox.Show("Không tìm thấy sinh viên có mã số" + ID);
                return;
            }
            else
            {
                student.FullName = txtName.Text;
                student.FacultyID = (int)cbbFaculty.SelectedValue;
                student.AverageScore = float.Parse(txtAVS.Text);
                db.SaveChanges();
                var liststudent = db.Students.ToList();
                FillDGV(liststudent);
                ClearInputFields();
                MessageBox.Show("Đã sửa thành công!!");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            var ID = txtMSSV.Text;
            var student = db.Students.SingleOrDefault(x => x.StudentID == ID);
            if(student == null)
            {
                MessageBox.Show("Không tìm thấy sinh viên có mã số" + ID);
                return;
            }
            else
            {
                db.Students.Remove(student);
                db.SaveChanges();
                var liststudent = db.Students.ToList();
                FillDGV(liststudent);
                ClearInputFields();
                MessageBox.Show("Đã xóa thành công!!");
            }
        }
    }
}

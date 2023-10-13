using Microsoft.Reporting.WinForms;
using QLSV.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            var ListStudentReportDto = db.Students.Select(c => new StudentReport
            {
                StudentID = c.StudentID,
                FullName = c.FullName,
                AverageScore = c.AverageScore ?? 0,
                FacultyName = c.Faculty.FacultyName
            }).ToList();
            this.reportViewer1.LocalReport.ReportPath = "rptStudentReport.rdlc";
            var reportDataSource = new ReportDataSource("StudentDataSet", ListStudentReportDto);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}

using COMP2007_S2016_Lesson5C___LAB_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace COMP2007_S2016_Lesson5C___LAB_3
{
    public partial class Departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDepartments();
        }

        protected void GetDepartments()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                //string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the Students Table using EF and LINQ
                var Departments = (from allDepartments in db.Departments
                                select allDepartments);

                // bind the result to the GridView
                //DepartmentsGridView.DataSource = Departments.AsQueryable().OrderBy(SortString).ToList();
                DepartmentsGridView.DataSource = Departments.AsQueryable().ToList();
                DepartmentsGridView.DataBind();
            }
        }
    }
}
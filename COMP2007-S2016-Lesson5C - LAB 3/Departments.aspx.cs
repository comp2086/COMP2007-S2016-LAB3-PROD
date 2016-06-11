using COMP2007_S2016_Lesson5C___LAB_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace COMP2007_S2016_Lesson5C___LAB_3
{
    public partial class Departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time, populate the student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID"; // default sort column
                Session["SortDirection"] = "ASC";

                // Get the department data
                GetDepartments();
            }
        }

        /**
         * <summary>
         * This method gets the department data from the DB
         * </summary>
         * 
         * @method GetDepartments
         * @returns {void}
         */
        protected void GetDepartments()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the Students Table using EF and LINQ
                var Departments = (from allDepartments in db.Departments
                                   select allDepartments);

                // bind the result to the GridView
                DepartmentsGridView.DataSource = Departments.AsQueryable().OrderBy(SortString).ToList();
                DepartmentsGridView.DataBind();
            }
        }

        /**
         * <summary>
         * This event handler allows sorting to occur for the Departments page
         * </summary>
         * 
         * @method DepartmentsGridView_PageIndexChanging
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @returns {void}
         */
        protected void DepartmentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            var nextSortColumn = e.SortExpression.ToString();
            var prevSortColumn = Session["SortColumn"].ToString();
            var prevSortDirection = Session["SortDirection"].ToString();

            if (nextSortColumn.Equals(prevSortColumn))
            {
                Session["SortDirection"] = prevSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                Session["SortColumn"] = nextSortColumn;
                Session["SortDirection"] = "ASC";
            }

            // Redirect to the first page
            DepartmentsGridView.PageIndex = 0;

            GetDepartments();
        }

        /**
         * <summary>
         * This event handler allows pagination to occur for the Departments page
         * </summary>
         * 
         * @method DepartmentsGridView_PageIndexChanging
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @returns {void}
         */
        protected void DepartmentsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the new page number
            DepartmentsGridView.PageIndex = e.NewPageIndex;

            // refresh the grid
            GetDepartments();
        }

        /**
         * <summary>
         * This event handler allows to change the pagesize of the Departments gridview
         * </summary>
         * 
         * @method PageSizeDropDownList_SelectedIndexChanged
         * @param {object} sender
         * @param {EventArgs} e
         * @returns {void}
         */
        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set the new Page size
            DepartmentsGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            // refresh the grid
            this.GetDepartments();

        }
    }
}
 
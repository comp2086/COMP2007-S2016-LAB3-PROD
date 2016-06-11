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

        /**
         * <summary>
         * This event handler deletes a department from the db using EF
         * </summary>
         * 
         * @method DepartmentsGridView_RowDeleting
         * @param {object} sender
         * @param {GridViewDeleteEventArgs} e
         * @returns {void}
         */
        protected void DepartmentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // store which row was clicked
            int selectedRow = e.RowIndex;

            // get the selected StudentID using the Grid's DataKey collection
            int DepartmentID = Convert.ToInt32(DepartmentsGridView.DataKeys[selectedRow].Values["DepartmentID"]);

            // use EF to find the selected student in the DB and remove it
            using (DefaultConnection db = new DefaultConnection())
            {
                // create object of the Student class and store the query string inside of it
                Department deletedDepartment = (from departmentRecords in db.Departments
                                          where departmentRecords.DepartmentID == DepartmentID
                                          select departmentRecords).FirstOrDefault();

                // remove the selected student from the db
                db.Departments.Remove(deletedDepartment);

                // save my changes back to the database
                db.SaveChanges();

                // refresh the grid
                this.GetDepartments();
            }
        }

        protected void DepartmentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var prevSortColumn = Session["SortColumn"].ToString();
            var prevSortDirection = Session["SortDirection"].ToString();

            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) // if header row has been clicked
                {
                    LinkButton linkbutton = new LinkButton();

                    for (int index = 0; index < DepartmentsGridView.Columns.Count - 1; index++)
                    {
                        if (DepartmentsGridView.Columns[index].SortExpression == prevSortColumn)
                        {
                            if (prevSortDirection == "ASC")
                            {
                                linkbutton.Text = " <i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkbutton.Text = " <i class='fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkbutton);
                        }
                    }
                }
            }
        }
    }
}
 
<%@ Page Title="Departments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="COMP2007_S2016_Lesson5C___LAB_3.Departments" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="DepartmentsGridView" runat="server"
        CssClass="table table-bordered table-striped table-hover"
        AutoGenerateColumns="false" DataKeyNames="DepartmentID">
        <Columns>
            <asp:BoundField DataField="DepartmentID" HeaderText="Department ID" Visible="true" SortExpression="DepartmentID" />
            <asp:BoundField DataField="Name" HeaderText="Name" Visible="true" SortExpression="Name" />
            <asp:BoundField DataField="Budget" HeaderText="Budget" Visible="true" SortExpression="Budget" DataFormatString="{0:c}" />
            <asp:HyperLinkField HeaderText="Edit" Text="<i class='fa fa-pencil-square-o fa-lg'></i> Edit"
                NavigateUrl="~/DepartmentDetails.aspx.cs" ControlStyle-CssClass="btn btn-primary btn-sm" runat="server"
                DataNavigateUrlFields="DepartmentID" DataNavigateUrlFormatString="DepartmentDetails.aspx?DepartmentID={0}" />
            <asp:CommandField HeaderText="Delete" DeleteText="<i class='fa fa-trash-o fa-lg'></i> Delete"
                ShowDeleteButton="true" ButtonType="Link" ControlStyle-CssClass="btn btn-danger btn-sm" />
        </Columns>
    </asp:GridView>
</asp:Content>

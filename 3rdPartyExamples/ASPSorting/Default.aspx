<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GridViewOnSorting._Default" %>

<form runat="server">
    <asp:GridView runat="server" id="gridPersons" AutoGenerateColumns="false" AllowSorting="true" OnSorting="gridPersons_Sorting">
        <Columns>
            <asp:BoundField HeaderText="First name" DataField="FirstName" SortExpression="FirstName" />
            <asp:BoundField HeaderText="Last name"  DataField="LastName" SortExpression="LastName" />
            <asp:BoundField HeaderText="Father's first name"  DataField="Father.FirstName" SortExpression="Father.FirstName" />
        </Columns>
    </asp:GridView>
</form>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CerberusMain.aspx.cs" Inherits="Cerberus.Web.CerberusMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <title></title>
    <style type="text/css">
        .panel-header {}
    </style>
</head>
<body>
    <form id="frmMain" runat="server">
    <div class="panel-primary">
        <asp:Panel ID="panHeader" runat="server" Height="63px">
            <asp:Button ID="cmdRefresh" runat="server" Text="Button" Height="51px" Width="76px" CssClass="btn-primary" />
            <asp:Button ID="cmd2" runat="server" Text="Button" Height="51px" Width="76px" CssClass="btn-primary" />
        </asp:Panel>
    </div>
    <div class="panel-info">
        <asp:Panel ID="panGrid" runat="server" CssClass="panel-info">
            <asp:GridView ID="gvEFT" runat="server" OnRowDataBound="gvEFT_RowDataBound" CssClass="table-bordered" AllowSorting="True" BackColor="#E1FFFF" Caption="Eft Terminals" OnSorting="gvEFT_Sorting">
            </asp:GridView>
        </asp:Panel>
    </div>
    <div>
        <asp:Panel ID="panFooter" runat="server" CssClass="panel-footer" Height="80px">
        </asp:Panel>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CerberusMain.aspx.cs" Inherits="Cerberus.Web.CerberusMain" %>

<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Linear" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Circular" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.State" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxGauges.v14.2, Version=14.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGauges.Gauges.Digital" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <title></title>
    <style type="text/css">
        .panel-header {}
    </style>
    <link href="CSS/CerberusMain.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmMain" runat="server">
        <div class="container">
            <div class="panel-header">
                <asp:Panel ID="panHeader" runat="server" Height="63px">
                    <asp:Button ID="cmdRefresh" runat="server" Text="Refresh" Height="51px" Width="76px" CssClass="btn-primary" OnClick="cmdRefresh_Click" />
                    <asp:Button ID="cmd2" runat="server" Text="Stuff" Height="51px" Width="76px" CssClass="btn-primary" />
                    <asp:Label ID="lblLastUpdated" runat="server" Text="Last Updated: "></asp:Label>
                </asp:Panel>
            </div>
            <div class="grid">
                <asp:Panel ID="panGrid" runat="server" CssClass="panel-info">
                    <asp:GridView ID="gvEFT" runat="server" OnRowDataBound="gvEFT_RowDataBound" CssClass="table-bordered" AllowSorting="True" BackColor="#E1FFFF" Caption="Eft Terminals" OnSorting="gvEFT_Sorting">
                    </asp:GridView>
                </asp:Panel>
            </div>
            <div class="filters">
                <asp:Panel ID="panFilters" runat="server" CssClass="panel-info">
                    <asp:Button ID="cmdByOffice" runat="server" Text="By Office" OnClick="cmdByOffice_Click" />
                    <asp:Button ID="cmdByDate" runat="server" OnClick="cmdByDate_Click" Text="By Date" />
                    <asp:Button ID="cmdByDateRange" runat="server" OnClick="cmdByDateRange_Click" Text="By Date Range" />
                    <asp:Button ID="cmdAllTerminals" runat="server" OnClick="cmdAllTerminals_Click" Text="All Terminals" />
                </asp:Panel>
            </div>
             <div class="metrics">
                <asp:Panel ID="panMetrics" runat="server" CssClass="panel-info">
                    <dx:ASPxGaugeControl ID="gauNewTerminals" runat="server" BackColor="White" Height="260px" LayoutInterval="6" Value="80" Width="260px">
                        <Gauges>
                            <dx:CircularGauge Bounds="6, 6, 248, 248" Name="circularGauge4">
                                <scales>
                                    <dx:ArcScaleComponent AcceptOrder="0" AppearanceMajorTickmark-BorderBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceMajorTickmark-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceMinorTickmark-BorderBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceMinorTickmark-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceTickmarkText-Font="Tahoma, 9pt" AppearanceTickmarkText-TextBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#9BE2F7&quot;/&gt;" Center="125, 125" EndAngle="60" MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeType="Circular_Style19_1" MajorTickmark-TextOffset="-15" MajorTickmark-TextOrientation="LeftToRight" MaxValue="100" MinorTickCount="4" MinorTickmark-ShapeType="Circular_Style19_2" Name="scale1" RadiusX="85" RadiusY="85" StartAngle="-240" Value="80">
                                        <ranges>
                                            <dx:ArcScaleRangeWeb AppearanceRange-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#059272&quot;/&gt;" EndThickness="14" EndValue="33" Name="Range0" ShapeOffset="14" StartThickness="14" />
                                            <dx:ArcScaleRangeWeb AppearanceRange-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#CDB864&quot;/&gt;" EndThickness="14" EndValue="66" Name="Range1" ShapeOffset="14" StartThickness="14" StartValue="33" />
                                            <dx:ArcScaleRangeWeb AppearanceRange-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:#933F6F&quot;/&gt;" EndThickness="14" EndValue="100" Name="Range2" ShapeOffset="14" StartThickness="14" StartValue="66" />
                                        </ranges>
                                    </dx:ArcScaleComponent>
                                </scales>
                                <backgroundlayers>
                                    <dx:ArcScaleBackgroundLayerComponent AcceptOrder="-1000" ArcScale="" Name="bg" ScaleID="scale1" ShapeType="CircularFull_Style19" ZOrder="1000" />
                                </backgroundlayers>
                                <needles>
                                    <dx:ArcScaleNeedleComponent AcceptOrder="50" ArcScale="" EndOffset="2" Name="needle" ScaleID="scale1" ShapeType="CircularFull_Style19" StartOffset="-25" ZOrder="-50" />
                                </needles>
                                <spindlecaps>
                                    <dx:ArcScaleSpindleCapComponent AcceptOrder="100" ArcScale="" Name="circularGauge4_SpindleCap1" ScaleID="scale1" ShapeType="CircularFull_Style19" Size="30, 30" ZOrder="-100" />
                                </spindlecaps>
                            </dx:CircularGauge>
                        </Gauges>
                        <LayoutPadding All="6" Bottom="6" Left="6" Right="6" Top="6" />
                    </dx:ASPxGaugeControl>
                    <dx:ASPxGaugeControl ID="gauKnownTerminals" runat="server" BackColor="White" Height="260px" LayoutInterval="6" Value="80" Width="260px">
                        <Gauges>
                            <dx:CircularGauge Bounds="6, 6, 248, 248" Name="circularGauge4">
                                <scales>
                                    <dx:ArcScaleComponent AcceptOrder="0" AppearanceMajorTickmark-BorderBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceMajorTickmark-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceMinorTickmark-BorderBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceMinorTickmark-ContentBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:White&quot;/&gt;" AppearanceTickmarkText-Font="Tahoma, 14pt" AppearanceTickmarkText-TextBrush="&lt;BrushObject Type=&quot;Solid&quot; Data=&quot;Color:LightGrey&quot;/&gt;" Center="125, 125" EndAngle="60" MajorTickmark-FormatString="{0:F0}" MajorTickmark-ShapeOffset="-11" MajorTickmark-ShapeScale="0.5, 1" MajorTickmark-ShapeType="Circular_Style5_1" MajorTickmark-TextOffset="-28" MajorTickmark-TextOrientation="LeftToRight" MaxValue="100" MinorTickCount="4" MinorTickmark-ShapeOffset="-5" MinorTickmark-ShapeScale="0.5, 1" MinorTickmark-ShapeType="Circular_Style5_2" Name="scale1" RadiusX="122" RadiusY="122" StartAngle="-240" Value="80">
                                        <ranges>
                                            <dx:ArcScaleRangeWeb EndThickness="14" EndValue="33" Name="Range0" ShapeOffset="14" StartThickness="14" />
                                            <dx:ArcScaleRangeWeb EndThickness="14" EndValue="66" Name="Range1" ShapeOffset="14" StartThickness="14" StartValue="33" />
                                            <dx:ArcScaleRangeWeb EndThickness="14" EndValue="100" Name="Range2" ShapeOffset="14" StartThickness="14" StartValue="66" />
                                        </ranges>
                                    </dx:ArcScaleComponent>
                                </scales>
                                <backgroundlayers>
                                    <dx:ArcScaleBackgroundLayerComponent AcceptOrder="-1000" ArcScale="" Name="bg" ScaleID="scale1" ShapeType="CircularFull_Style5" ZOrder="1000" />
                                </backgroundlayers>
                                <needles>
                                    <dx:ArcScaleNeedleComponent AcceptOrder="50" ArcScale="" Name="needle" ScaleID="scale1" ShapeType="CircularFull_Style5" StartOffset="-23.5" ZOrder="-50" />
                                </needles>
                                <spindlecaps>
                                    <dx:ArcScaleSpindleCapComponent AcceptOrder="100" ArcScale="" Name="circularGauge4_SpindleCap1" ScaleID="scale1" ShapeType="Empty" Size="30, 30" ZOrder="-100" />
                                </spindlecaps>
                            </dx:CircularGauge>
                        </Gauges>
                        <LayoutPadding All="6" Bottom="6" Left="6" Right="6" Top="6" />
                    </dx:ASPxGaugeControl>
                </asp:Panel>
            </div>
        </div>

    <div class="panel-footer">
        <asp:Panel ID="panFooter" runat="server" CssClass="panel-footer" Height="80px">
        </asp:Panel>
    </div>
    </form>
</body>
</html>

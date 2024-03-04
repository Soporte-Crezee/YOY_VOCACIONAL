<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CargaEscuelasReport.aspx.cs"
    Inherits="POV.Web.PortalOperaciones.Reportes.CargaEscuelasReport" %>

<%@ Register Assembly="DevExpress.XtraReports.v12.1.Web, Version=12.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YOY - Reporte de importar excel de escuelas</title>
    <link rel="shortcut icon"  href="~/Images/Yoy_Favicon20px.png"/>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #toolBar {
            width: 512px;
            margin: 0 auto;
        }
        #viewer {
            width: 744px;
            margin: 0 auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!-- inicio page-->
    <div class="page">
        <!--inicio header-->
        <div class="header">
            <div class="header_topmenu">
               <a href="#" class="first">
                    <asp:Label ID="LblNombreUsuario" runat="server"></asp:Label>
                </a>
                 &ensp; 
                <asp:HyperLink ID="HyperLink7" NavigateUrl="~/Auth/Logout.aspx" runat="server">Salir</asp:HyperLink> &emsp;

            </div>
            <div class="title_logo_app">PORTAL OPERACIONES</div>
            
        </div>
        <!--fin header-->
        <!--Contenido principal-->
        <div class="main">
            <h3 class="ui-widget-header ui-widget-header-label" style="margin-top: -13px;">
            <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>/Reporte importar excel de escuelas</h3>
            <div>
                <div id="toolBar">
                    <dx:ReportToolbar ID="rptToolBar" runat="server" ShowDefaultButtons="False" ReportViewerID="rptVEscuelas">
                        <Items>
                            <dx:ReportToolbarButton ItemKind="Search" ToolTip="Mostar la pantalla de Busqueda" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind="PrintReport" ToolTip="Imprimir el Reporte" />
                            <dx:ReportToolbarButton ItemKind="PrintPage" ToolTip="Imprimir la página actual " />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" ToolTip="Primera página" />
                            <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" ToolTip="Página Anterior" />
                            <dx:ReportToolbarLabel ItemKind="PageLabel" />
                            <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                            </dx:ReportToolbarComboBox>
                            <dx:ReportToolbarLabel ItemKind="OfLabel" Text="de" />
                            <dx:ReportToolbarTextBox ItemKind="PageCount" />
                            <dx:ReportToolbarButton ItemKind="NextPage" ToolTip="Página siguiente" />
                            <dx:ReportToolbarButton ItemKind="LastPage" ToolTip="Última página" />
                            <dx:ReportToolbarSeparator />
                            <dx:ReportToolbarButton ItemKind="SaveToDisk" ToolTip="Exportar y Guardar en disco" />
                            <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                                <Elements>
                                    <dx:ListElement Value="pdf" />
                                </Elements>
                            </dx:ReportToolbarComboBox>
                        </Items>
                        <Styles>
                            <LabelStyle>
                                <Margins MarginLeft='3px' MarginRight='3px' />
                            </LabelStyle>
                        </Styles>
                    </dx:ReportToolbar>
                </div>
                <div id="viewer">
                    <dx:ReportViewer ID="rptVEscuelas" runat="server">
                    </dx:ReportViewer>
                </div>
            </div>
        </div>
        <!--End contenido principal-->
        <div class="clear">
        </div>
    </div>
    <!-- fin page -->
    <!--begin footer-->
    <div class="footer">
    </div>
    <!--End footer-->
    </form>
</body>
</html>

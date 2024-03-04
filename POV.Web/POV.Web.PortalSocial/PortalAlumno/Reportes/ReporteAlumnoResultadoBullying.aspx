<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteAlumnoResultadoBullying.aspx.cs" Inherits="POV.Web.PortalSocial.PortalAlumno.Reportes.ReporteAlumnoResultadoBullying" %>

<%@ Register Assembly="DevExpress.XtraReports.v12.1.Web, Version=12.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>YOY - ESTUDIANTE</title>
    <link rel="icon" href="../../Images/Yoy_Favicon20px.png" />
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximun-scale=1, user-scalable=no" />
    <meta name="viewport" content="width=device-width" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Talentos.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/headerandfooter.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/reporte.css" rel="stylesheet" type="text/css" />
    <style>
        #form1 {
            min-height: 768px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Inicio page -->
        <!-- Inicio header -->
        <div class="navbar navbar-default navbar-static-top">
            <div id="header" class="header-container col-md-12 col-sm-12 col-xs-12 text-center">
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 text-center">
                    <asp:Image ID="image1" runat="server" ImageUrl="~/Images/SOV_VOCAREER1.png" />
                </div>
                <div class="col-xs-3 col-sm-2 col-md-1 col-lg-1">
                    <asp:Image ID="image2" runat="server" ImageUrl="~/Images/SOV_ELEMENTOS_portalestudiante.png" />
                </div>
                <div class="col-xs-8 col-sm-6 col-md-5 col-lg-7">
                    <h2 class="pull-left titulo-portal" id="tituloPortal" runat="server">PORTAL ESTUDIANTE</h2>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12" id="underheader"></div>
            </div>
        </div>
        <!-- fin header -->

        <!-- Contenido principal -->
        <div class="container">
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
                </li>
                <li>Importar reporte
                </li>
            </ol>
            <div id="toolBar">
                <dx:ReportToolbar ID="rptToolbar" runat="server" ShowDefaultButtons='False' ReportViewerID="rptVAlumnos">
                    <Items>
                        <dx:ReportToolbarButton ItemKind='Search' ToolTip="Mostrar la pantalla de Busqueda" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind='PrintReport' ToolTip="Imprimir el Expediente" />
                        <dx:ReportToolbarButton ItemKind='PrintPage' ToolTip="Imprimir la página actual" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' ToolTip="Primera página" />
                        <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' ToolTip="Página anterior" />
                        <dx:ReportToolbarLabel ItemKind='PageLabel' />
                        <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'>
                        </dx:ReportToolbarComboBox>
                        <dx:ReportToolbarLabel ItemKind='OfLabel' Text="de" />
                        <dx:ReportToolbarTextBox ItemKind='PageCount' />
                        <dx:ReportToolbarButton ItemKind='NextPage' ToolTip="Página siguiente" />
                        <dx:ReportToolbarButton ItemKind='LastPage' ToolTip="Última página" />
                        <dx:ReportToolbarSeparator />
                        <dx:ReportToolbarButton ItemKind='SaveToDisk' ToolTip="Exportar y guardar en disco" />
                        <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                            <%--<dx:ReportToolbarButton ItemKind='SaveToDisk' ToolTip="Exportar y guardar en disco" />
                        <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width='70px'>--%>
                            <Elements>
                                <dx:ListElement Value='pdf' />
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
            <div id="viewer" class="text-center table-responsive">
                <dx:ReportViewer ID="rptVAlumnos" runat="server">
                </dx:ReportViewer>
            </div>
        </div>
        <div class="clear"></div>
        <!-- fin contenido principal -->
    </form>
    <!-- Inicio footer -->
    <div class="footer">
        <div class="footerpersonalizado"></div>
    </div>
    <!-- fin header -->
</body>
</html>

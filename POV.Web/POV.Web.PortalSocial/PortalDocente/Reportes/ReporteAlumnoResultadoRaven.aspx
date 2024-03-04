<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteAlumnoResultadoRaven.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Reportes.ReporteAlumnoResultadoRaven" %>

<%@ Register Assembly="DevExpress.XtraReports.v12.1.Web, Version=12.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>YOY - ORIENTADOR</title>
    <link rel="icon" href="~/Images/Yoy_Favicon20px.png" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="viewport" content="width=device-width" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Talentos.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/headerandfooter.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/reporte.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <!-- inicio  page -->
        <!-- inicio header -->
        <div class="navbar navbar-default navbar-static-top">
            <div id="header" class="header-container col-md-12 col-sm-12 col-xs-12 text-center">
                <div class="col-xs-12 col-sm-3 col-md-3 col-lg-3 text-center">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/SOV_VOCAREER1.png" />
                </div>
                <div class="col-xs-3 col-sm-2 col-md-1 col-lg-1">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/SOV_ELEMENTOS_portalorientador.png"/>
                </div>
                <div class="col-xs-8 col-sm-6 col-md-5 col-lg-7">
                    <h2 class="pull-left titulo-portal" id="tituloPortal" runat="server">PORTAL ORIENTADOR</h2>
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
                <li>
                    Importar reporte
                </li>
            </ol>
            <div id="toolBar">
                <dx:ReportToolbar ID="rptToolbar" runat='server' ShowDefaultButtons='False' ReportViewerID="rptVAlumnos">
                    <Items>
                        <dx:ReportToolbarButton ItemKind='SaveToDisk' ToolTip="Exportar y guardar en disco" />
                        <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
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
        <div class="clear">
        </div>
        <!-- End contenido principal -->

        <!-- Inicio footer -->
        <div class="footer">
            <div class="footerpersonalizado">
            </div>
        </div>
        <!-- End footer -->
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteAlumnoResultadoCleaver.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.Reportes.ReporteAlumnoResultadoCleaver" %>

<%@ Register Assembly="DevExpress.XtraReports.v12.1.Web, Version=12.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>YOY - PADRES</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta name="viewport" content="width=device-width" />
    <link rel="icon" href="~/Images/Yoy_Favicon20px.png" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/reporte.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- begin header -->
        <div class="header-container">
            <div class="container-fluid">
                <%-- Logo --%>
                <div class="col-xs-12 col-sm-12 col-md-3 col-lg-3 text-center">
                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/Default.aspx" runat="server">
                        <img src="~/images/SOV_VOCAREER1.png" runat="server" id="imgLogo_Header" alt="logo" class=""  style="padding-top:5px; margin-top:10px;margin-bottom:10px;"/>
                    </asp:HyperLink>
                </div>
                <%-- Menu-Top --%>
                <div class="">
                    <div class="menu_container">
                        <%-- Menu-Top-Tutor --%>
                        <div class="text-center">
                            <div class="col-xs-3 col-sm-2 col-md-1 col-lg-1">
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/SOV_tutores.png" class="" style="margin: 20px 0px 0px 10px; padding: 0px" width="50px"/>
                            </div>
                            <div class="col-xs-9 col-sm-10 col-md-8 col-lg-8" >
                                <h1 class="pull-left titulo-portal" style="padding-top:10px;" id="tituloPortalResponsivo">PORTAL PADRES</h1>
                            </div>
                        </div>
                    </div>
                </div>
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
            <div id="viewer" class="table-responsive">
                <dx:ReportViewer ID="rptVAlumnos" runat="server">
                </dx:ReportViewer>
            </div>
        </div>
        <!-- End contenido principal -->

        <!--begin footer-->
        <div class="footer">
            <div class="footerpersonalizado"></div>
        </div>
        <!--End footer-->
    </form>
</body>
</html>

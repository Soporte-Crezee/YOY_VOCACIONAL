<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CargaOrientadoresReport.aspx.cs" Inherits="POV.Web.PortalOperaciones.Reportes.CargaOrientadoresReport" %>
<%@ Register Assembly="DevExpress.XtraReports.v12.1.Web, Version=12.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Reporte de carga orientadores</title>
    <link rel="icon" href="~/Images/Yoy_Favicon20px.png" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Menu.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap-theme.css" rel="stylesheet" type="text/css" />
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
    <div class="page">
            <div class="page">
                <!-- Header -->
                <!--inicio header-->
            <div class="col-xs-13">
                <div class="col-xs-13">
                    <div id="header" style="margin: 0px; padding: 15px;">
                        <div class="row vertical-divider" style="margin-top: 0px">
                            <div class="col-xs-12 col-sm-4 col-md-3">
                                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Default.aspx" runat="server">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/SOV_ELEMENTOS logo VOCAREER1.png" Style="margin: 14px 15px 0px; padding: 0px" Width="280px" class="img-responsive" />
                                </asp:HyperLink>
                            </div>
                            <div class="col-xs-3 col-sm-8 col-md-9">
                                <div class="col-xs-3 col-sm-2">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/SOV_ELEMENTOS_portaloperaciones.png" Style="margin: 12px 0px 0px 20px; padding: 0px" Width="50px" class="img-responsive" />
                                </div>
                                <div class="col-xs-9 col-sm-10">
                                    <h2 class="pull-left title_logo_app" id="tituloPortal" runat="server">PORTAL OPERACIONES</h2>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
                <!-- Main -->
                <div class="ui-widget-content">
                    <h3 class="ui-widget-header ui-widget-header-label" style="margin-top: 0px;">
                        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>/Reportes
                    </h3>
                    <div>
                        <div id="toolBar">
                            <dx:ReportToolbar ID="rptToolBar" runat='server' ShowDefaultButtons='False' ReportViewerID="rptVAlumnos">
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
                            <dx:ReportViewer ID="rptVAlumnos" runat="server">
                            </dx:ReportViewer>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
                <!-- Footer -->
                <div class="footer">
                </div>
            </div>
        </div>
    </form>
</body>
</html>

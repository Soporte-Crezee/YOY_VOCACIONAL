<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoExportacionAlumno.aspx.cs" Inherits="POV.Web.PortalOperaciones.Reportes.ResultadoExportacionAlumno" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Log exportación estudiante</title>
    <link rel="icon" href="~/Images/Yoy_Favicon20px.png" />
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/Menu.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/ui-lightness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/bootstrap-theme.css" rel="stylesheet" type="text/css" />
    
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery-1.12.1.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/bootstrap.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery-1.7.2.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery-ui-1.8.16.custom.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.menu.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/api/api.dialogs.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.blockUI.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.ui.datepicker-es.js")%>" type="text/javascript"></script>
    <style type="text/css">
        #toolBar {
            width: 512px;
            margin: 0 auto;
        }

        #viewer {
            width: 1330px;
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
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/SOV_ELEMENTOS_portal operaciones.png" Style="margin: 12px 0px 0px 20px; padding: 0px" Width="50px" class="img-responsive" />
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
                        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>/Log exportaci&oacute;n estudiantes
                    </h3>
                    <div class="col-xs-12 form-group table-responsive">
                        <div id="viewer" class="col-xs-12 form-group " >
                            <asp:TextBox ID="txtFichero" runat="server" Rows="20" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 " >
                        <div class="col-md-offset-5">
                            <asp:Button ID="btnBorrarLog" runat="server" OnClick="btnBorrarLog_Click" Text="Borrar log" CssClass="btn-green" />
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

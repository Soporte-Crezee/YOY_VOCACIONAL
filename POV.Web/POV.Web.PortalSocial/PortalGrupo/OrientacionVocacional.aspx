<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="OrientacionVocacional.aspx.cs" Inherits="POV.Web.PortalSocial.PortalGrupo.OrientacionVocacional" %>

<%@ Register TagPrefix="config" TagName="Solicitud" Src="~/PortalGrupo/SolicitudOrientacionVocacional.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title>Solicitar</title>
    <link href="<% =Page.ResolveClientUrl("~/Styles/fullcalendar/fullcalendar.css")%>" rel="stylesheet" />
    <!-- <link href="<% =Page.ResolveClientUrl("~/Styles/fullcalendar/fullcalendar.print.css")%>" rel="stylesheet" /> -->

    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/moment.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/fullcalendar.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/fullcalendar/es.js")%>" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.orientacionvocacional.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.orientacionvocacional.js" type="text/javascript"></script>
</head>
<body>
    <form id="fomr1" runat="server">

        <div id="panel-container" class="panel_edicion_perfil">
            <div class="col-xs-12">
                <div id="docente-config">
                    <!-- configuracion de calendario del docente -->
                    <div id="Configuracion" runat="server">
                        <config:Solicitud ID="Solicitud" runat="server" OnSolicitarClicked="Solicitud_SolicitarClicked"></config:Solicitud>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(loadControls);

            function loadControls() {
                //$('#< %=txtFechaNacimiento.ClientID %>').datepicker({ changeYear: true });
            }
        </script>
    </form>
</body>
</html>
<%--</asp:Content>--%>

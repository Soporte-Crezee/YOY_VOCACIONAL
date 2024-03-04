<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/PortalGrupo/PortalGrupo.master"
    AutoEventWireup="true" CodeBehind="Alumnos.aspx.cs" Inherits="POV.Web.PortalSocial.PortalGrupo.Alumnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.contactos.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.contactos.js" type="text/javascript"></script>

    <script type="text/javascript">
        function viewDetall(element) {
            window.location = "../Social/ViewMuro.aspx?u=" + element;
        }
    </script>
    <style type="text/css">
        div.accordion.ui-accordion.ui-widget.ui-helper-reset h3 {
            background-color: #ff9e19 !important;
            border-color: #FF9E19 !important;
            color: #FFF !important;
            font-family: Roboto-Bold !important;
        }

        h3.ui-accordion-header.ui-corner-top.ui-state-default.ui-accordion-icons.ui-accordion-header-active.ui-state-active {
            background-color: #ff9e19 !important;
            border-color: #FF9E19 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="page_title_select">
    <label>Aspirantes comunes</label>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="content_left_panel">
    <div id="user_img" style="margin-top: 10px;">
        <asp:Image runat="server" ID="ImgUser" CssClass="profile_img_comunes" ImageUrl="../images/VOCAREER_aspirantesComunes.png" />
    </div>
    <div class="clear"></div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div id="info_perfil">
        <h1 class="tBienvenida">Contactos            
        </h1>
    </div>
    <div>
        <div class="titulo_marco_general">
            Lista de estudiantes
        </div>
    </div>
    <div class="listado_Aspirantes">
        <div id="ContactosStream">
        </div>
    </div>
    <div id="more">
    </div>
    <br />
    <script type="text/javascript">
        var pubs_current = 1;
    </script>
</asp:Content>

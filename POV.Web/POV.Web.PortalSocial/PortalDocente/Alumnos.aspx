<%@ Page Title="YOY - ORIENTADOR" Language="C#" MasterPageFile="~/PortalDocente/PortalDocente.master"
    AutoEventWireup="true" CodeBehind="Alumnos.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Alumnos" %>

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
        .more {
            border: 1px solid #DDD;
            font-size: 14pt;
            font-weight: 700;
            text-align: center;
            margin-top: 5px;
            background-color: #f2f2f2;
            padding: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div class="bodyadaptable">
        <div id="info_perfil">
            <div class="titulo_marco_general">
                <asp:Label ID="LblNombreGrupo" runat="server" Text="" Visible="false"></asp:Label>
                Estudiantes
            
            </div>
            <div class="subline_title"></div>
        </div>
        <div>
            <div class="listado_Aspirantes">
                <ul id="ContactosStream" style="padding: 0px">
                </ul>
            </div>
        </div>
        <div id="more">
        </div>
        <br />
    </div>
    <script type="text/javascript">
        var pubs_current = 1;
    </script>
</asp:Content>

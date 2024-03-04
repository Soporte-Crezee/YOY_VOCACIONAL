<%@ Page Title="YOY - ORIENTADOR" Language="C#" MasterPageFile="~/PortalDocente/PortalDocente.master" AutoEventWireup="true" CodeBehind="ReporteResultadoPruebasAlumno.aspx.cs" Inherits="POV.Web.PortalSocial.PortalDocente.Reportes.ReporteResultadoPruebasAlumno" %>

<%@ Register Assembly="DevExpress.XtraReports.v12.1.Web, Version=12.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.orientacionvocacional.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.orientacionvocacional.js" type="text/javascript"></script>
    <link href="~/Styles/headerandfooter.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #toolBar {
            width: 744px;
            margin: 4px auto;
        }

        #viewer {
            width: 744px;
            margin: 0 auto;
        }

        .divContenido {
            background-color: #F3F3F5;
            width: 97%;
            margin: 0 auto;
        }
        .ui-widget-header {
            background:#05AEDF !important;
            border-color: #05AEDF !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div class="divPrincipal col-xs-12 col-md-12">
        <!-- Cabecera -->
        <div id="info_alumno">
            <asp:Label ID="LblNombreUsuario" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="LblNombreGrupo" runat="server" Text="" Visible="false"></asp:Label>
        </div>

        <div class="col-xs-12 titulo_marco_general">
            <label class="titulo_label_general">B&uacute;squeda por estudiante</label>
        </div>
        <div class="col-xs-12 form-group">
            <div class="col-xs-12 form-group">
                <div class="col-xs-12 col-md-3"></div>
                <div class="col-xs-12 col-md-6">
                    <div class="col-xs-12 form-group">
                        <br />
                        <label class="col-sm-8 control-label">Nombre del estudiante</label>
                        <div class="col-sm-12">
                            <asp:DropDownList ID="ddlAlumno" runat="server" class="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-md-3"></div>
            </div>

            <div class="col-xs-12 form-group">
                <div class="col-xs-12 col-md-3"></div>
                <div class="col-xs-12 col-md-6">
                    <div class="col-xs-12 form-group">
                        <div class="col-xs-3"></div>
                        <div class="col-xs-3">
                            <asp:Button ID="btnBuscarAlumno" runat="server" Text="Buscar" OnClick="btnBuscarAlumno_Click" CssClass="button_clip_39215E" />
                        </div>
                        <div class="col-xs-3"></div>
                    </div>
                </div>
                <div class="col-xs-12 col-md-3"></div>

            </div>

        </div>
    </div>
    <asp:HiddenField ID="hdnSocialHubID" runat="server" />
    <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
    <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnTipoPublicacionTexto" runat="server" />
    <asp:HiddenField ID="hdnTipoPublicacionSuscripcionReactivo" runat="server" />
    <asp:HiddenField ID="hdnFuente" runat="server" Value="D" />
</asp:Content>


﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarEstado.aspx.cs" Inherits="POV.Web.PortalOperaciones.Estados.RegistrarEstado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            
            $(".boton").button();
            $("#frmMain").validate({  rules:{<%=txtNombre.UniqueID %>:{required: true,maxlength: 30}}
            ,submitHandler: function(form) {
                $(form).block();
                form.submit();
            }});
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
            /Registrar estado</h3>
        <div class="finder ui-widget-content">
            <table class="finder">
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblPais" runat="server" Text="País" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPais" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="textoEnunciado"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblCodigo" runat="server" Text="Código" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCodigo" MaxLength="10" runat="server" CssClass="textoEnunciado"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div class="line"></div>
            <table>
                <tr>
                    <td class="td-label"></td>
                    <td class="label">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-green" OnClick="btnGuardar_Click" />
                    </td>
                    <td class="label">
                        <asp:HyperLink ID="btnCancelar" CssClass="btn-cancel" NavigateUrl="~/Estados/BuscarEstado.aspx" runat="server">Cancelar</asp:HyperLink>
                    </td>
                    <td class="td-label"></td>
                </tr>
            </table>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
        </div>
    </div>
</asp:Content>
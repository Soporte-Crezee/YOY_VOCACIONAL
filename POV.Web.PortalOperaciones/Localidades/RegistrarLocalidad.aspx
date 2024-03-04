<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarLocalidad.aspx.cs" Inherits="POV.Web.PortalOperaciones.Localidades.RegistrarLocalidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            
            $(".boton").button();
                $("#frmMain").validate({  
                    rules:{<%=txtNombre.UniqueID %>:{required: true,maxlength: 30},
                    <%=ddlPais.UniqueID %>:{required: true},
                    <%=ddlEstado.UniqueID %>:{required: true},
                    <%=ddlCiudad.UniqueID %>:{required: true}}
                ,submitHandler: function(form) {
                $(form).block();
                 form.submit();
        }});
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>/Registrar nueva
        localidad</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblPais" runat="server" Text="País"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblCiudad" runat="server" Text="Ciudad"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList ID="ddlCiudad" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtNombre" MaxLength="30"  runat="server" CssClass="textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblCodigo" runat="server" Text="Código"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtCodigo" MaxLength="10" runat="server" CssClass="required textoEnunciado"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="line"></div>
        <table>
            <tr>
                <td>
                    <div>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                        <asp:HyperLink ID="btnCancelar" CssClass="boton" NavigateUrl="~/Localidades/BuscarLocalidades.aspx" runat="server">Cancelar</asp:HyperLink>
                        </div>
                </td>
            </tr>
        </table>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

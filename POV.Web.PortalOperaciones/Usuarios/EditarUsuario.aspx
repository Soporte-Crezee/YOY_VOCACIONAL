<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="EditarUsuario.aspx.cs" Inherits="POV.Web.PortalOperaciones.Usuarios.EditarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {

            $(".boton").button();
            $("#frmMain").validate({
                rules: {
                    '<%=txtNombre.UniqueID %>': {
                        required: true,
                        minlength: 6,
                        maxlength: 50
                    },
                    '<%=txtContrasena.UniqueID %>': {
                        required: false,
                        minlength: 6,
                        maxlength: 15
                    },
                    '<%=txtRepeatContrasena.UniqueID %>': {
                        required: false,
                        minlength: 6,
                        maxlength: 15,
                        equalTo: "#<%=txtContrasena.ClientID%>"
                    },
                    '<%=txtEmail.UniqueID %>': {

                        maxlength: 50,
                        email: true
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
                    form.submit();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Editar usuario</h3>
    <div class="ui-widget-content" style="padding: 5px">
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre de usuario" CssClass="whitespace textoClave"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEmail" runat="server" Text="Correo electrónico"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblActivo" runat="server" Text="Activo"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkActivo" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Cambiar contrase&ntilde;a</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblContrasena" runat="server" Text="Nueva contraseña"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtContrasena" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblRepeatContrasena" runat="server" Text="Repetir nueva contraseña"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRepeatContrasena" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="line">
        </div>
        <div>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
            <asp:HyperLink ID="btnCancelar" CssClass="boton" NavigateUrl="~/Usuarios/BuscarUsuarios.aspx"
                runat="server">Cancelar</asp:HyperLink>
        </div>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

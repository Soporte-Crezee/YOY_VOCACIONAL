<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarPerfil.aspx.cs" Inherits="POV.Web.PortalOperaciones.Perfiles.EditarPerfil" %>

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
                        maxlength: 40
                    },
                    '<%=txtDescripcion.UniqueID %>': {
                        required: false,
                        maxlength: 100
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
                    form.submit();
                }
            });
        }
    </script>
    <style type="text/css">
        .campoDesc
        {
            white-space: pre-line;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Editar perfil</h3>
    <div class="ui-widget-content" style="padding: 5px">
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre de perfil"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="40" CssClass="whitespace textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr class="td-label-texarea">
                <td>
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine"
                        MaxLength="100" CssClass="textoEnunciado" Width="500" Columns="80" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEstatus" runat="server" Text="Estatus"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbEstatus" runat="server">
                        <asp:ListItem Value="true" Text="Activos"></asp:ListItem>
                        <asp:ListItem Value="false" Text="Inactivos"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblOperacion" runat="server" Text="Operaci&oacute;n"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkOperacion" runat="server" />
                </td>
            </tr>
        </table>
        <div class="line">
        </div>
        <div>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
            <asp:HyperLink ID="btnCancelar" CssClass="boton" NavigateUrl="~/Perfiles/BuscarPerfiles.aspx"
                runat="server">Cancelar</asp:HyperLink>
        </div>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarPerfil.aspx.cs" Inherits="POV.Web.PortalOperaciones.Perfiles.RegistrarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
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
                        required: true,
                        maxlength: 100
                    },
                }
                , submitHandler: function (form) {
                    $(form).block();
                    form.submit();
                } 
            });
        }

        jQuery.validator.addMethod("whitespace", function (value, element) {

            var isValid = true;
            var largo = value.length;
            if (largo > 0) {

                var temp = value.indexOf(" ", 0);
                if (temp != -1)
                    isValid = false;
            }
            else
                isValid = false;


            return this.optional(element) || isValid;
        }, "No puede tener espacios en blanco");

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Registrar perfil</h3>
    <div class=" ui-widget-content" style="padding: 5px">
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre del perfil"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="40" CssClass="whitespace textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label-texarea">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine"
                        MaxLength="100" CssClass="textoEnunciado" Width="500" Columns="80" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblOperaciones" runat="server" Text="Operaci&oacute;n"></asp:Label>
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

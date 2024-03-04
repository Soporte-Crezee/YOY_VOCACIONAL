<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarDirector.aspx.cs" Inherits="POV.Web.PortalOperaciones.Directores.RegistrarDirector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {

            $(".boton").button();

            //var ddate = new Date();
            //var minddate = new Date(ddate.get, 0, 1);

            $('#<%=txtFechaNacimiento.ClientID %>').datepicker({ yearRange: '-100:+0', changeYear: true, changeMonth: true, dateFormat: "dd/mm/yy" });

            $("#frmMain").validate({
                rules: {
                    '<%=txtCurp.UniqueID %>': {
                        required: true,
                        maxlength: 18
                    },
                    '<%=txtNombre.UniqueID %>': {
                        required: true,
                        maxlength: 200
                    },
                    '<%=txtPApellido.UniqueID %>': {
                        required: true,
                        maxlength: 100
                    },
                    '<%=txtCorreo.UniqueID %>': {
                        required: true,
                        maxlength: 30
                    },
                    '<%=txtFechaNacimiento.UniqueID %>': {
                        required: true
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
        /Registrar director</h3>
    <div class="ui-widget-content" style="padding: 5px">
        <table >
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblCurp" runat="server" Text="CURP"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCurp" runat="server" MaxLength="30" CssClass="whitespace textoNombrePersona"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="200" CssClass="textoNombrePersona"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblPApellido" runat="server" Text="Primer apellido"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPApellido" runat="server" MaxLength="100" CssClass="textoNombrePersona"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblSApellido" runat="server" Text="Segundo apellido"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSApellido" runat="server" MaxLength="100" CssClass="textoNombrePersona"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblFechaNacimiento" runat="server" Text="Fecha de nacimiento"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server" MaxLength="30" CssClass="whitespace"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblSexo" runat="server" Text="Sexo"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="CbSexo" ToolTip="sexo del director">
                        <asp:ListItem Text="Hombre" Value="true" />
                        <asp:ListItem Text="Mujer" Value="false" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNivelEscolar" runat="server" Text="Nivel escolar"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNivelEscolar" runat="server" MaxLength="50" CssClass="whitespace textoNombrePersona"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblCorreo" runat="server" Text="Correo"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCorreo" runat="server" MaxLength="30" CssClass="whitespace"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblTelefono" runat="server" Text="T&eacute;lefono"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20" CssClass="whitespace digits"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div class="line">  </div>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
            <asp:HyperLink ID="btnCancelar" CssClass="boton" NavigateUrl="~/Directores/BuscarDirectores.aspx"
                runat="server">Cancelar</asp:HyperLink>
      
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls() {

            var ddate = new Date();
            var maxddate = new Date(ddate.getFullYear(), -1, 1);

            $('<%=txtFechaNacimiento.ClientID %>').datepicker({ maxDate: maxddate, changeYear: true });
        }
    </script>
</asp:Content>

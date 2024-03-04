<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarAsistencia.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias.RegistrarAsistencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();


            $("#frmMain").validate(
				{
				    rules: {
				        '<%=txtNombre.UniqueID %>': { required: true, maxlength: 100 },
				        '<%=txtDescripcion.UniqueID %>': { required: false, maxlength: 500 },
				        '<%=ddlTemaAsistencia.UniqueID %>': { required: true, min: 1 }
				    },
				    submitHandler: function (form) {
				        $('.main').block();
				        form.submit();
				    }
				});
        }
        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="BuscarAsistencias.aspx">Volver</asp:HyperLink>/Registrar
        asistencia
    </h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <label class="label">
                        <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" MaxLength="100"
                        TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label class="label">
                        <asp:Label runat="server" ID="lblTema" Text="Tema"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlTemaAsistencia" ToolTip="Este campo es obligatorio"
                        TabIndex="2" />
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <label class="label">
                        <asp:Label runat="server" ID="lblDescripcion" Text="Descripción"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" ReadOnly="False"
                        Enabled="True" MaxLength="500" TabIndex="4" Height="50px" Width="500px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div class="line">
        </div>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" CssClass="boton" OnClick="BtnGuardar_Click" />
        <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" CssClass="boton" OnClick="BtnCancelar_Click"
            OnClientClick="CancelValidate()" />
        <asp:UpdatePanel runat="server" ID="updRegistrar">
            <ContentTemplate>
                <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                    style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

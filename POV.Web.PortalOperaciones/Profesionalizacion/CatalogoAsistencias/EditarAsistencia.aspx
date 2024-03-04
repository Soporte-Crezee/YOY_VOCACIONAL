<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarAsistencia.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias.EditarAsistencia" %>

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
				        '<%=ddlEstatus.UniqueID %>': { required: true, min: 1 },
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
        <asp:LinkButton runat="server" ID="lnkBack1" OnClientClick="CancelValidate()" OnClick="lnkBack_Click">Volver</asp:LinkButton>/Editar
        asistencia
    </h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" CssClass="required "
                        MaxLength="100" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblTema" Text="Tema"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="ddlTemaAsistencia" TabIndex="2" ToolTip="Este campo es obligatorio"
                        CssClass="required">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblEstatus" Text="Estatus"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="required" ToolTip="Este campo es obligatorio"
                        TabIndex="3">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <asp:Label runat="server" ID="lblDescripcion" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" ReadOnly="False"
                        Enabled="True" MaxLength="500" TabIndex="4" Height="70px" Width="500px">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div class="line">
        </div>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" CssClass="boton" OnClick="BtnGuardar_Click" />
        <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" CssClass="boton" OnClick="BtnCancelar_Click"
            OnClientClick="CancelValidate()" />
        <asp:UpdatePanel runat="server" ID="updEditar">
            <ContentTemplate>
                <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                    style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

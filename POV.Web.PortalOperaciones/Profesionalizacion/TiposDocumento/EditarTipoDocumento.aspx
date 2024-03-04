<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarTipoDocumento.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.TiposDocumento.EditarTipoDocumento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {

            $('.boton').button();
            $("#frmMain").validate(
                {
                    rules: {
                        '<%=txtNombre.UniqueID %>': { required: true, maxlength: 50 },
                        '<%=txtExtension.UniqueID %>': { maxlength: 30 },
                        '<%=txtMime.UniqueID %>': { maxlength: 100 },
                        '<%=txtFuente.UniqueID %>': { maxlength: 100 }

                    },
                    submitHandler: function (form) {
                        $(form).block();
                        form.submit();
                    }
                });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="BuscarTipoDocumento.aspx">Volver</asp:HyperLink>
        /Editar tipo de documento</h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblID" Text="ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombre"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" Text="Tipo de documento" ID="lblTipoDocumento"></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="RadBtnListTipoDocumento" runat="server" RepeatDirection="Horizontal"
                        CellSpacing="12" AutoPostBack="true" OnSelectedIndexChanged="RadBtnListTipoDocumento_SelectedIndexChanged">
                        <asp:ListItem Text="Archivo" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Fuente" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <asp:UpdatePanel runat="server" ID="updExtension">
                    <ContentTemplate>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblExtension" Text="Extensión"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtExtension"></asp:TextBox>
                        </td>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </tr>
            <tr>
                <asp:UpdatePanel runat="server" ID="updMIME">
                    <ContentTemplate>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblMime" Text="MIME"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMime"></asp:TextBox>
                        </td>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </tr>
            <tr>
                <asp:UpdatePanel runat="server" ID="updFuente">
                    <ContentTemplate>
                        <td class="td-label">
                            <asp:Label runat="server" ID="lblFuente" Text="Fuente"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFuente"></asp:TextBox>
                        </td>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblEsEditable" Text="Es editable"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlEsEditable">
                        <asp:ListItem Text="Si" Value="true"></asp:ListItem>
                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="line">
        </div>
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="BuscarTipoDocumento.aspx"
            CssClass="boton"></asp:HyperLink>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

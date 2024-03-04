<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarAsignaturaPlanEducativo.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoAsignaturas.EditarAsignaturaPlanEducativo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <link href="../Styles/Reactivos.css" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button({ icons: {
                primary: "ui-icon-search"
            }
            });

            $("#frmMain").validate({
                rules: {
                    '<%=TxtClaveAsignatura.UniqueID %>': {
                        required: true,
                        maxlength: 50
                    },
                    '<%=TxtTituloAsignatura.UniqueID %>': {
                        required: true,
                        maxlength: 50
                    },
                    '<%=cbGradoAsignatura.UniqueID %>': {
                        required: true
                    },
                    '<%=cbAreaConocimiento.UniqueID %>': {
                        required: true
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
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
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Editar Asignatura</h3>
    <div class="main_div ui-widget-content">
        <h2>
            Informaci&oacute;n de la asignatura</h2>
            <hr />
            <br />
        <table>
            <tr>
                <td class="td-label">
                    <label>
                        ID</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtIDAsignatura" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Clave</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtClaveAsignatura" runat="server" Width="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        T&iacute;tulo</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtTituloAsignatura" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Grado</label>
                </td>
                <td>
                    <asp:DropDownList ID="cbGradoAsignatura" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Área de conocimiento</label>
                </td>
                <td>
                    <asp:DropDownList ID="cbAreaConocimiento" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="line">
        </div>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" CssClass="boton" OnClick="BtnGuardar_OnClick" />
        <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
            CssClass="boton" OnClientClick="CancelValidate()" />
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

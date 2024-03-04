<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarCurso.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos.RegistrarCurso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();

            $(".fileup").each(function (index) {
                $(this).change(function () {

                    var val = $(this).val();

                    switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
                        case 'pdf':

                            break;
                        default:
                            $(this).val('');
                            $("#txtRedirect").val("");
                            $(hdnmessageinputid).val("El archivo seleccionado debe ser un archivo PDF.");
                            $(hdnmessagetypeinputid).val('1');
                            mostrarError();
                            break;
                    }
                });

            });
        }

        function ValidateFields() {
            $("#frmMain").validate();
        }

        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Agregar
        curso</h3>
    <div class="main_div ui-widget-content" style="padding: 15px;">
        <%-- BEGIN formulario  principal--%>
        <table cellspacing="8">
            <tr>
                <td class="td-label">
                    <label>
                        Nombre</label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="required" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Tema</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLTema"/>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Modalidad</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLPresencial"/>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estado</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLEstatusProfesionalizacion" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Documento de información adicional</label>
                </td>
                <td>
                    <asp:FileUpload runat="server" ID="FileInformacion" CssClass="fileup" />
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                    </label>
                </td>
                <td>
                    <asp:Label ID="LblExtensiones" runat="server"></asp:Label>
                </td>
            </tr>
            <%-- END formulario  principal--%>
        </table>
        <br />
        <div class="line">
        </div>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" OnClick="BtnGuardar_OnClick"
            CssClass="boton" OnClientClick="ValidateFields()" />
        <span style="">
            <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
                CssClass="boton" OnClientClick="CancelValidate()" />
        </span>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

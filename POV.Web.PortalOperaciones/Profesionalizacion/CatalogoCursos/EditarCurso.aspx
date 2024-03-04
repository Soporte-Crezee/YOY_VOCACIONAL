<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarCurso.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos.EditarCurso" %>

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
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Editar
        curso</h3>
    <div class="main_div ui-widget-content" style="padding: 15px;">
        <%-- BEGIN formulario  principal--%>
        <table cellspacing="7px">
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtID" ReadOnly="true" Enabled="true" runat="server"
                        CssClass="number"></asp:TextBox>
                </td>
            </tr>
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
                    <asp:DropDownList runat="server" ID="DDLTema" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Modalidad</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLPresencial" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estado</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLEstatusProfesionalizacion" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Tiene docto. de informaci&oacute;n</label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdArchivoPrevio" runat="server">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkTieneDocto" runat="server" Enabled="false" />
                            <asp:Button ID="BtnEliminarDoctoInformacion" runat="server" Text="Eliminar archivo"
                                CssClass="boton" 
                                OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" 
                                onclick="BtnEliminarDoctoInformacion_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblInformacion" Text="PDF de informaci&oacute;n adicional"></asp:Label>
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
        <div class="line"></div>
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

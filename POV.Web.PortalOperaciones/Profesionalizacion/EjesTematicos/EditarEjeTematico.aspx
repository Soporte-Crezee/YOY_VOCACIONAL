<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarEjeTematico.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.EditarEjeTematico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();

            $("#frmMain").validate(
				{
				    rules: {
				        '<%=txtNombre.UniqueID %>': { required: true, maxlength: 200 },
				        '<%=txtDescripcion.UniqueID %>': { required: false, maxlength: 1000 },
				        '<%=cbNivelEducativo.UniqueID %>': { required: true },
				        '<%=cbGradoAsignatura.UniqueID %>': { required: true },
				        '<%=ddlAreasProfesionalizacion.UniqueID %>': { required: true },
				        '<%=ddlMaterias.UniqueID %>': { required: true }
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
        <asp:LinkButton ID="lnkBack" runat="server" OnClientClick="CancelValidate()" OnClick="btnCancelar_Click">Volver</asp:LinkButton>/Editar
        eje o ámbito</h3>
    <div class="form-register  ui-widget-content" style="width: 100%">
        <h2>
            Datos del eje o ámbito</h2>
        <div class="line">
        </div>
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombre" EnableViewState="true" CssClass="required" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td>
                    <asp:TextBox TextMode="MultiLine" EnableViewState="true" ID="txtDescripcion" runat="server"
                       Columns="80" Width="500" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlEstado" runat="server" EnableViewState="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Panel ID="Panel1" runat="server" GroupingText="Datos de la asignatura">
            <table>
                <tr>
                    <td class="td-label">
                        <label>
                            Nivel educativo</label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updNivelEducativo" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="cbNivelEducativo" runat="server" AutoPostBack="true" CssClass="required"
                                    OnSelectedIndexChanged="cbNivelEducativo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            Grado</label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdGrado" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="cbGradoAsignatura" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbGradoAsignatura_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblAreaProfesionalizacion" runat="server" Text="Asignatura"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlAreasProfesionalizacion" EnableViewState="true" CssClass="required"
                                    runat="server" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAreasProfesionalizacion_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblMateriasProfesionalizacion" runat="server" Text="Bloque"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlMaterias" runat="server" EnableViewState="true" AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>   
            </table>
        </asp:Panel>
        <br />
        <div class="line">
        </div>
        <br />
        <div class="DDFloatLeft">
            <asp:Button ID="btnGuardar" Visible="false" runat="server" Text="Guardar" CssClass="boton"
                Width="69px" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CancelValidate();"
                CssClass="boton" OnClick="btnCancelar_Click" />
        </div>
    </div>
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
        style="display: none;" />
</asp:Content>

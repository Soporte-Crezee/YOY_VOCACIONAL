<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarEjeTematico.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.RegistrarEjeTematico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $(".boton_search").button({ icons: {
                primary: "ui-icon-search"
            }
            });
        }
        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }

        function ValidateFields() {
            $("#frmMain").validate(
             {
                 rules: {
                     '<%=txtNombre.UniqueID %>': { required: true, maxlength: 200 },
                     '<%=txtDescripcion.UniqueID %>': { maxlength: 1000 },
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="btnCancelar_Click" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Registrar
        eje o ámbito</h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
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
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="required"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripción"></asp:Label>
                </td>
                <td>
                    <asp:TextBox TextMode="MultiLine" ID="txtDescripcion" EnableViewState="true" runat="server"
                       Columns="80" Width="500" Rows="5"></asp:TextBox>
                </td>
            </tr>
        </table>
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
                        <asp:Label ID="lblAreaProfesionalizacion" runat="server" Text="Asignatura">
                          
                        </asp:Label>
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
                                <asp:DropDownList ID="ddlMaterias" runat="server" EnableViewState="true">
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
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" Visible="false"
            OnClick="btnGuardar_Click" OnClientClick="ValidateFields()" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="CancelValidate();"
            CssClass="boton" OnClick="btnCancelar_Click" />
        <br />
        <br />
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

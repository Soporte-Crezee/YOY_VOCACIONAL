<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurarContenidosAgrupador.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.ConfigurarContenidosAgrupador" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $("#frmMain").validate();
        }

        function CancelValidate() {
        }
    </script>
    <style type="text/css">
        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
            white-space: pre-line;
        }
        .wrap
        {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick">Volver</asp:LinkButton>/Configurar
        recursos didácticos a contenido</h3>
    <div class="main_div ui-widget-content" style="padding: 15px; min-height: 500px;">
        <table cellspacing="5px">
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblNivelEducativo" Text="Nivel educativo"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNivelEducativo" ReadOnly="True" Enabled="False"
                        TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblGrado" Text="Grado"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtGrado" ReadOnly="True" Enabled="False"
                        TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblAsignatura" Text="Asignatura"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAsignatura" ReadOnly="True" Enabled="False"
                        TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblBloque" Text="Bloque"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBloque" ReadOnly="True" Enabled="False"
                        TabIndex="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    Eje o &aacute;mbito
                </td>
                <td>
                    <asp:TextBox ID="txtNombreEjeTematico" runat="server" Enabled="false" ReadOnly="true"
                        TextMode="MultiLine" Rows="2" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    Tema
                </td>
                <td>
                    <asp:TextBox ID="txtNombreSituacion" runat="server" Enabled="false" ReadOnly="true"
                        TextMode="MultiLine" Rows="2" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    Contenido
                </td>
                <td>
                    <asp:TextBox ID="txtNombreClasificador" runat="server" Enabled="false" ReadOnly="true"
                        TextMode="MultiLine" Rows="2" Width="400px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="height: 1%; overflow: hidden;">
            <div style="float: left; width: 49%;">
                <asp:Panel ID="Panel1" runat="server" GroupingText="Recursos didácticos disponibles">
                    <%-- begin formulario buscador contenidos --%>
                    <div class="finder ui-widget-content">
                        <table>
                            <tr>
                                <td class="td-label">
                                    <asp:Label ID="lblClave" runat="server" Text="Clave"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClave" MaxLength="30" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombre" MaxLength="200" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label ID="lblInstitucion" runat="server" Text="Institución de origen"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInsitucion" MaxLength="200" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label ID="lblEtiquetas" runat="server" Text="Etiquetas"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEtiquetas" MaxLength="200" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <label>
                                        Tipo de documento</label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DDLTipoDocumento" />
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                </td>
                                <td>
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <%-- end formulario buscar contenidos --%>
                    <%-- begin resultado consulta contenidos --%>
                    <asp:UpdatePanel ID="updPruebas" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdContenidosBusqueda" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                                HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                                Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grdContenidosBusqueda_RowCommand"
                                OnSorting="grdContenidosBusqueda_Sorting" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:BoundField DataField="InstitucionOrigen" HeaderText="Institucion origen" SortExpression="InstitucionOrigen" />
                                    <asp:BoundField DataField="NombreTipoDocumento" HeaderText="Tipo de documento" SortExpression="NombreTipoDocumento" />
                                    <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                                    <asp:TemplateField HeaderText="Agregar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnAgregar" runat="server" CommandName="agregar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContenidoDigitalID")%>'
                                                ImageUrl="~/images/plus-button.png" ToolTip="Agregar recurso" />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="wrap" />
                                        <ItemStyle CssClass="wrap" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="ui-state-highlight ui-corner-all">
                                        <p>
                                            <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                            resultados</p>
                                    </div>
                                </EmptyDataTemplate>
                                <PagerTemplate>
                                    <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsContenidosBusqueda"
                                        DataSourceType="DataSet" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-- end resultado consulta contenidos --%>
                </asp:Panel>
            </div>
            <div style="float: right; width: 48%;">
                <asp:Panel ID="Panel2" runat="server" GroupingText="Recursos didácticos asignados">
                    <%-- begin contenidos --%>
                    <asp:UpdatePanel ID="updContenidoAgrupador" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="grdContenidosAgrupador" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                                HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                                Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grdContenidosAgrupador_RowCommand"
                                OnSorting="grdContenidosAgrupador_Sorting" AllowSorting="true">
                                <Columns>
                                    <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:BoundField DataField="InstitucionOrigen" HeaderText="Institucion origen" SortExpression="InstitucionOrigen" />
                                    <asp:BoundField DataField="NombreTipoDocumento" HeaderText="Tipo de documento" SortExpression="NombreTipoDocumento" />
                                    <asp:TemplateField HeaderText="Quitar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="quitar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContenidoDigitalID")%>'
                                                ImageUrl="~/images/minus-button.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                                ToolTip="Quitar recurso didáctico" />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="wrap" />
                                        <ItemStyle CssClass="wrap" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="ui-state-highlight ui-corner-all">
                                        <p>
                                            <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                            resultados</p>
                                    </div>
                                </EmptyDataTemplate>
                                <PagerTemplate>
                                    <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsContenidosAgrupador"
                                        DataSourceType="DataTable" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%-- end contenidos --%>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>

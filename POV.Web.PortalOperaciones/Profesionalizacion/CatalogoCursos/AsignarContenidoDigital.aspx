<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AsignarContenidoDigital.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos.AsignarContenidoDigital" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
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
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Asignar contenidos digitales a cursos</h3>
    <div class="main_div ui-widget-content" style="padding: 20px;">
        <h2>
            Información del curso</h2>
        <div class="line">
        </div>
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblID" Text="Identificador"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" MaxLength="10" ID="txtID" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombre" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblTema" Text="Tema"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtTema" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div style="float: left; width: 49%">
            <fieldset style="min-height: 500px">
                <legend>Contenidos digitales disponibles</legend>
                <fieldset class="ui-widget-content">
                    <asp:UpdatePanel runat="server" ID="updBuscar">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblidentificador" Text="Identificador"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtidentificador" MaxLength="10" CssClass="number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblClave" Text="Clave"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtClave" MaxLength="30"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblNom" Text="Nombre"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtNom" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblTipoDocumento" Text="Tipo documento"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="DDLTipoDocumento">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td-label">
                                        <asp:Label runat="server" ID="lblInstitucion" Text="Institución de origen"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtInstitucion" MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td-label">
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
                <div>
                    <asp:UpdatePanel runat="server" ID="updContenidosDisponibles">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="grdConenidosDisponibles" CssClass="DDGridView" RowStyle-CssClass="td"
                                HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="5" AllowPaging="true"
                                Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" OnRowCommand="grdContenidos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="ContenidoDigitalID" HeaderText="ID" />
                                    <asp:BoundField DataField="Clave" HeaderText="Clave" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="InstitucionOrigen" HeaderText="Institución" />
                                    <asp:BoundField DataField="NombreTipoDocumento" HeaderText="Tipo documento" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Acción">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnAsig" runat="server" CommandName="AsigContenidos" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContenidoDigitalID")%>'
                                                ImageUrl="../../Images/plus-button.png" ToolTip="Asignar" />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="wrap" />
                                        <ItemStyle CssClass="wrap" />
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
                                    <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsContenidoDigital"
                                        DataSourceType="DataSet" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                    style="display: none;" />
            </fieldset>
        </div>
        <div style="float: right; width: 49%">
            <fieldset style="min-height: 500px">
                <legend>Contenidos digitales asignados</legend>
                <asp:UpdatePanel ID="updContenidosAsignados" runat="server">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="grdConenidosAsignados" CssClass="DDGridView" RowStyle-CssClass="td"
                            HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="5" AllowPaging="true"
                            Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" OnRowCommand="grdContenidos_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ContenidoDigitalID" HeaderText="ID" />
                                <asp:BoundField DataField="Clave" HeaderText="Clave" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="InstitucionOrigen" HeaderText="Institución" />
                                <asp:BoundField DataField="TipoDocumentoNombre" HeaderText="Tipo documento" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Acción">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDesAsig" runat="server" CommandName="DesAsigContenidos" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContenidoDigitalID")%>'
                                            ImageUrl="../../images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" />
                                    </ItemTemplate>
                                    <ControlStyle CssClass="wrap" />
                                    <ItemStyle CssClass="wrap" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="ui-state-highlight ui-corner-all">
                                    <p>
                                        <span class="ui-icon ui-icon-info" style="float: left"></span>El curso no tiene
                                        contenidos digitales asignados</p>
                                </div>
                            </EmptyDataTemplate>
                            <PagerTemplate>
                                <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsContenidoDigitalAsignados"
                                    DataSourceType="DataTable" />
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
        <div style="clear: both">
        </div>
    </div>
</asp:Content>

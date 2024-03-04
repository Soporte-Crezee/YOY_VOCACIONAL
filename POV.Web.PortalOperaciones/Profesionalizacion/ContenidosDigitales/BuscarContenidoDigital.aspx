<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarContenidoDigital.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.ContenidosDigitales.BuscarContenidoDigital" %>

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
    </script>
    <style type="text/css">
        .break_words
        {
            white-space: pre-line;
            word-break: break-all;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de recursos didácticos</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtID" MaxLength="10" runat="server" CssClass="number"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label ID="lblInstitucion" runat="server" Text="Institución de origen"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtInsitucion" MaxLength="200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblClave" runat="server" Text="Clave"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtClave" MaxLength="30" runat="server"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label ID="lblEtiquetas" runat="server" Text="Etiquetas"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtEtiquetas" MaxLength="200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtNombre" MaxLength="200" runat="server"></asp:TextBox>
                </td>
                <td class="td-label">
                    <label>
                        Tipo de documento</label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="DDLTipoDocumento" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estado</label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="DDLEstatusContenido" />
                </td>
                <td class="td-label">
                </td>
                <td>
                    <div>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="RegistrarContenidoDigital.aspx" id="lnkRegistrar" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblAgregarNuevo" runat="server" Text="Agregar nuevo recurso didáctico"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updPruebas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdContenidos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grd_RowCommand"
                    OnSorting="grd_Sorting" OnRowDataBound="grd_DataBound" AllowSorting="true" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="ContenidoDigitalID" HeaderText="ID" SortExpression="ContenidoDigitalID" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" ItemStyle-CssClass="break_words" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ItemStyle-CssClass="break_words" />
                        <asp:BoundField DataField="InstitucionOrigen" HeaderText="Instituci&oacute;n origen"
                            SortExpression="InstitucionOrigen" ItemStyle-CssClass="break_words" />
                        <asp:BoundField DataField="Tags" HeaderText="Etiquetas" SortExpression="Tags" ItemStyle-CssClass="break_words" />
                        <asp:BoundField DataField="NombreTipoDocumento" HeaderText="Tipo de documento" SortExpression="NombreTipoDocumento"
                            ItemStyle-CssClass="break_words" />
                        <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContenidoDigitalID")%>'
                                    ImageUrl="~/images/edit-button.png" Visible="false" ToolTip="Editar recurso didáctico" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContenidoDigitalID")%>'
                                    ImageUrl="~/images/minus-button.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                    Visible="false" ToolTip="Eliminar recurso didáctico" />
                            </ItemTemplate>
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsContenidosCatalogo"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<%@ Page Title="" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="BuscarProductos.aspx.cs" Inherits="POV.Web.Administracion.Productos.BuscarProductos" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>

<asp:Content ID="Contetn1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="Stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $('.boton').button();
            DoFormBlockUI();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">Cat&aacute;logo de productos
    </h3>

    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre" ToolTip="Nombre del producto"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="250"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblTipoProducto" Text="Tipo producto" ToolTip="Tipo de producto"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="DDLTipoProducto" runat="server" TabIndex="2">
                        <asp:ListItem Text="SELECCIONE..." Value=""></asp:ListItem>
                        <asp:ListItem Text="Expediente" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Prueba" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Horas orientador" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="label">
                    <asp:Button runat="server" ID="btnBuscar" CssClass="btn-green" Text="Buscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
            <tr>
                
            </tr>
            <tr>
                <td colspan="3"></td>
                
            </tr>
        </table>
    </div>

    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="NuevoProducto.aspx" id="lnkNuevoProducto" class="btn-green">
                <span class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <label>
                    Agregar nuevo producto
                </label>
            </a>
        </div>

        <asp:UpdatePanel runat="server" ID="UpdProductos">
            <ContentTemplate>
                <asp:GridView runat="server" ID="grdProductos" CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    EnableSortingAndPagingCallbacks="true" AllowSorting="false" OnRowCommand="grdProductos_RowCommand" Visible="false" OnRowDataBound="grdProductos_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Producto.Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                        <asp:BoundField DataField="Producto.Descripcion" HeaderText="Descripci&oacute;n" SortExpression="Descripcion" />
                        <asp:BoundField DataField="Producto.TipoProducto" HeaderText="Tipo producto" SortExpression="TipoModelo" >
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Precio unitario" SortExpression="Precio">
                            <HeaderTemplate>
                                <asp:Literal ID="thPrecioUnitario" runat="server" Text="Precio unitario" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Literal ID="trPrecioUnitario" runat="server" Text='<%# "$"+Eval("Precio","{0:N2}") %>'></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEdit" CommandName="editar" ImageUrl="~/images/VOCAREER_editar.png" ToolTip="Editar"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CostoProductoId") %>' Visible="false" />
                                <asp:ImageButton runat="server" ID="btnDel" CommandName="eliminar" ImageUrl="~/images/VOCAREER_suprimir.png" ToolTip="Eliminar"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CostoProductoId") %>'
                                    OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>
                                La b&uacute;squeda no produjo resultados 
                            </p>
                        </div>
                    </EmptyDataTemplate>
                    <%--<PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" DataSourceType="DataSet" SessionName="dsProductos"></asp:GridViewPager>
                    </PagerTemplate>--%>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server" style="display: none;" />
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls(sender, args) {
            $('.boton').button();
        }
    </script>
</asp:Content>

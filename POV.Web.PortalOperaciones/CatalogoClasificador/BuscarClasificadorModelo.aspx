<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarClasificadorModelo.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoClasificador.BuscarClasificadorModelo" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
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
    <div class="bodyadaptable col-xs-13 col-md-13">
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:LinkButton ID="lnkBack" runat="server" OnClientClick="CancelValidate()" PostBackUrl="~/CatalogoModeloPrueba/BuscarModeloPrueba.aspx">Volver</asp:LinkButton>/
        Cat&aacute;logo de clasificadores de modelo de prueba</h3>
        <div class="finder ui-widget-content">
            <table class="finder">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblID" runat="server" Text="ID" CssClass="label"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtID" MaxLength="10" runat="server" CssClass="number"></asp:TextBox>
                    </td>
                    <td class="label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="label"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtNombre" MaxLength="100" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="label">
                        <div>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-green" OnClick="btnBuscar_Click" OnClientClick="ValidateFields()" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="true">
                <a href="RegistrarClasificadorModelo.aspx" id="lnkNuevoClasificadorModelo" class="btn-green">
                    <span class=" ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label runat="server" Text="Agregar nuevo clasificador de modelo" ID="lblNuevaModelo"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updClasificadores" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdClasificadores" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grd_RowCommand"
                        OnRowDataBound="grd_DataBound" AllowSorting="true" Visible="false">
                        <Columns>
                            <asp:BoundField DataField="ClasificadorID" HeaderText="ID" SortExpression="ClasificadorID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ClasificadorID")%>'
                                        ImageUrl="../images/VOCAREER_editar.png" ToolTip="Editar" Visible="True" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ClasificadorID")%>'
                                        ImageUrl="../images/VOCAREER_suprimir.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Visible="True" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                resultados
                                </p>
                            </div>
                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="clasificadores" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarModeloPrueba.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoModeloPrueba.BuscarModeloPrueba" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <div class="ui-widget-header">
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">Cat&aacute;logo de modelo de pruebas</h3>
        </div>
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
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-green" OnClick="btnBuscar_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="true">
                <a href="RegistrarModeloPrueba.aspx" id="lnkNuevoModeloPrueba" class="btn-green">
                    <span class=" ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label runat="server" Text="Agregar nuevo modelo de prueba" ID="lblNuevaModelo"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updModelosPrueba" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdModelosPrueba" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grd_RowCommand"
                        OnRowDataBound="grd_DataBound" AllowSorting="true" Visible="false">
                        <Columns>
                            <asp:BoundField DataField="ModeloID" HeaderText="ID" SortExpression="ModeloID" />
                            <asp:TemplateField HeaderText="Nombre modelo" SortExpression="NombreModelo">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblNombreModelo" Text='<%#DataBinder.Eval(Container.DataItem,"NombreModelo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Clasificador" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkClasificador" runat="server" CommandName="clasificador" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ModeloID")%>' Text="Clasificador" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NombreEsEditable" HeaderText="Es editable" SortExpression="NombreEsEditable">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="TipoModelo" HeaderText="Tipo modelo" SortExpression="TipoModelo">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ModeloID")%>'
                                        ImageUrl="../images/VOCAREER_editar.png" ToolTip="Editar" Visible="True" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ModeloID")%>'
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
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="modelos" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

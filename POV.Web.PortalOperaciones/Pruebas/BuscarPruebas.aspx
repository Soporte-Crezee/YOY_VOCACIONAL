<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarPruebas.aspx.cs" Inherits="POV.Web.PortalOperaciones.Pruebas.BuscarPruebas" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <div class="ui-widget-header">
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">Cat&aacute;logo de pruebas</h3>
        </div>
        <div class="finder ui-widget-content">
            <table class="finder">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtID" MaxLength="10" runat="server" CssClass="digits"></asp:TextBox>
                    </td>
                    <td class="label">
                        <asp:Label ID="lblClave" runat="server" Text="Clave"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtClave" MaxLength="30" runat="server" CssClass="textoClave"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtNombre" MaxLength="100" runat="server" CssClass="textoEnunciado"></asp:TextBox>
                    </td>
                    <td class="label">
                        <asp:Label ID="lblModelo" runat="server" Text="Modelo"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:DropDownList ID="ddlModelo" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblEstadoLiberacion" runat="server" Text="Estatus" CssClass="label"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:DropDownList ID="ddlEstadoLiberacion" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                    <td>
                        <div>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-green" OnClick="btnBuscar_Click" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="RegistrarPrueba.aspx" id="lnkRegistrar" class="btn-green"><span class=" ui-icon ui-icon-circle-plus"
                    style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label ID="lblAgregarNuevo" runat="server" Text="Agregar nueva prueba"></asp:Label>
                </a>
            </div>
            <div class="table-responsive">
                <asp:UpdatePanel ID="updPruebas" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="grdPruebas" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                            HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grd_RowCommand"
                            OnRowDataBound="grd_DataBound" AllowSorting="false" Visible="false">
                            <Columns>
                                <asp:BoundField DataField="PruebaID" HeaderText="ID" SortExpression="PruebaID" ItemStyle-CssClass="break_words" />
                                <asp:BoundField DataField="ModeloID" Visible="false" />
                                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                <asp:TemplateField HeaderText="Nombre">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblNombre" CssClass="break_text" Text='<%#DataBinder.Eval(Container.DataItem, "Nombre") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NombreModelo" HeaderText="Modelo" SortExpression="NombreModelo"
                                    ItemStyle-CssClass="break_text" />
                                <asp:BoundField DataField="TipoPrueba" HeaderText="Tipo de prueba" SortExpression="TipoPrueba"
                                    ItemStyle-CssClass="break_text" />
                                <asp:BoundField DataField="NombreEstadoLiberacion" HeaderText="Estado" SortExpression="NombreEstadoLiberacion" />
                                <asp:TemplateField HeaderText="Liberar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnLiberar" runat="server" CommandName="liberar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PruebaID") %>'
                                            ImageUrl="../Images/tick.png" Visible="false" ToolTip="Liberar prueba" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Banco reactivos" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="90" HeaderStyle-CssClass="break_text">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkReactivos" runat="server" CommandName="reactivos" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PruebaID")%>'
                                            Text="Configurar" Visible="false" ToolTip="Configurar banco de reactivos"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Método calificación" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100" HeaderStyle-CssClass="break_text">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkConfig" runat="server" CommandName="config" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PruebaID")%>'
                                            Text="Configurar" Visible="false" ToolTip="Configurar método calificación"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PruebaID")%>'
                                            ImageUrl="../images/VOCAREER_editar.png" Visible="false" ToolTip="Editar prueba" />
                                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PruebaID")%>'
                                            ImageUrl="../images/VOCAREER_suprimir.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                            Visible="false" ToolTip="Eliminar prueba" />
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
                                <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="pruebas" DataSourceType="DataSet" />
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

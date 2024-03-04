<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarPaises.aspx.cs" Inherits="POV.Web.PortalOperaciones.Paises.BuscarPaises" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton_new").button();
            $(".boton_search").button({
                icons: {
                    primary: "ui-icon-search"
                }
            });
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <div class="ui-widget-header">
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">Cat&aacute;logo de paises</h3>
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
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>

                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="textoEnunciado"></asp:TextBox>
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
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="RegistrarPais.aspx" id="lnkNuevoPais" class="btn-green">
                    <span class=" ui-icon ui-icon-circle-plus"
                        style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label ID="lblNuevoPais" runat="server" Text="Agregar nuevo país"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updPaises" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdPaises" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" OnRowCommand="grdPaises_RowCommand" EnableSortingAndPagingCallbacks="True"
                        AllowSorting="true" OnSorting="grdPaises_Sorting" OnRowDataBound="grdPaises_DataBound" Visible="false">
                        <Columns>
                            <asp:BoundField DataField="PaisID" HeaderText="ID" SortExpression="PaisID" HeaderStyle-Width="20%" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" HeaderStyle-Width="20%" />
                            <asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" HeaderStyle-Width="20%" />
                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha de registro" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" SortExpression="FechaRegistro" HeaderStyle-Width="20%" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PaisID")%>'
                                        ImageUrl="../images/VOCAREER_editar.png" ToolTip="Editar" Visible="false" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PaisID")%>'
                                        ImageUrl="../images/VOCAREER_suprimir.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Visible="false" />
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
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="paises" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

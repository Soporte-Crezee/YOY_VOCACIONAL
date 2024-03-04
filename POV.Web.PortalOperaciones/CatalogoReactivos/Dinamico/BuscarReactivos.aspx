<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarReactivos.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoReactivos.Dinamico.BuscarReactivos" %>

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
        .break_words {
            white-space: pre-line;
            word-break: break-all;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="bodyadaptable col-xs-13 col-md-13">
        <div class="ui-widget-header">
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">Cat&aacute;logo de reactivos</h3>
        </div>

        <div class="finder ui-widget-content">
            <table class="finder">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblClave" runat="server" Text="Clave"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtClave" MaxLength="36" runat="server"></asp:TextBox>
                    </td>
                    <td class="label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtNombre" MaxLength="200" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label>Modelo</label>
                    </td>
                    <td class="input">
                        <asp:DropDownList runat="server" ID="ddlModelo" />
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
                <a href="RegistrarReactivoOpcion.aspx" id="lnkRegistrar" class="btn-green"><span class=" ui-icon ui-icon-circle-plus"
                    style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label ID="lblAgregarNuevo" runat="server" Text="Agregar reactivo"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdReactivos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" EnableSortingAndPagingCallbacks="True" OnRowCommand="grd_RowCommand"
                        OnSorting="grd_Sorting" OnRowDataBound="grd_DataBound" AllowSorting="true" Visible="false">
                        <Columns>

                            <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" ItemStyle-CssClass="break_words" />
                            <asp:BoundField DataField="NombreReactivo" HeaderText="Nombre" SortExpression="NombreReactivo" ItemStyle-CssClass="break_words" />
                            <asp:BoundField DataField="NombreModelo" HeaderText="Modelo" SortExpression="NombreModelo" ItemStyle-CssClass="break_words" />
                            <asp:BoundField DataField="MetodoCalificacion" HeaderText="Método de calificación" SortExpression="MetodoCalificacion" ItemStyle-CssClass="break_words" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ReactivoID")%>'
                                        ImageUrl="~/images/VOCAREER_editar.png" Visible="false" ToolTip="Editar reactivo" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ReactivoID")%>'
                                        ImageUrl="~/images/VOCAREER_eliminar.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Visible="false" ToolTip="Eliminar reactivo" />
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
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsReactivosDinamico" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

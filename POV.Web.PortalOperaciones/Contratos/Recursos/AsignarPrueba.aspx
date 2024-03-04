<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AsignarPrueba.aspx.cs" Inherits="POV.Web.PortalOperaciones.Contratos.Recursos.AsignarPrueba" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
        }
    </script>
    <style type="text/css">
        table.conPadding td
        {
            padding: 0 5px;
        }
        table.alignDerecha
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/Contratos/Recursos/ConfigurarRecursosCiclo.aspx">Volver</asp:HyperLink>/Asignar
        prueba</h3>
    <div class="ui-widget-content" style="padding: 5px;">
        <h2>
            Informaci&oacute;n del ciclo de orientaci&oacute;n vocacional</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblNombreCiclo" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombreCiclo" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblFechaInicioCiclo" runat="server" Text="Fecha inicio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaInicioCiclo" runat="server" Enabled="False"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label ID="lblFechaFinCiclo" runat="server" Text="Fecha fin"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFechaFinCiclo" runat="server" Enabled="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlPruebasAsignar" runat="server" GroupingText="Pruebas disponibles">
        <div class="ui-widget-content" style="padding: 15px">
            <table >
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblClave" runat="server" Text="Clave"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClave" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblModelo" runat="server" Text="Modelo"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlModelo" runat="server" Style="vertical-align: middle;">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblTipoPrueba" runat="server" Text="Tipo de prueba"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoPrueba" runat="server" Style="vertical-align: middle;">
                            <asp:ListItem Selected="True">Todas</asp:ListItem>
                            <asp:ListItem Value="true">DIAGNOSTICA</asp:ListItem>
                            <asp:ListItem Value="false">FINAL</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label ID="lblEstadoPrueba" runat="server" Text="Estado"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEstadoPrueba" runat="server" Style="vertical-align: middle;">
                            <asp:ListItem Selected="True">Todos</asp:ListItem>
                            <asp:ListItem Value="0">Inactiva</asp:ListItem>
                            <asp:ListItem Value="1">Activa</asp:ListItem>
                            <asp:ListItem Value="2">Liberada</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="72px" OnClick="btnBuscar_Click"
                            CssClass="btn-green" />
                    </td>
                </tr>
            </table>
            </div>
            <br />
            <br />
            <asp:UpdatePanel ID="updPruebas" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdPruebas" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" OnRowCommand="grdPruebas_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="NombreModelo" HeaderText="Modelo" SortExpression="NombreModelo" />
                            <asp:BoundField DataField="TipoPrueba" HeaderText="Tipo de prueba" SortExpression="TipoPrueba" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                            <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnAsignar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PruebaID")%>'
                                        CommandName="AsigPrueba" runat="server"><span>Asignar</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>No se encuentran coincidencias,
                                    por favor verifique su consulta</p>
                            </div>
                        </EmptyDataTemplate>
                        <HeaderStyle CssClass="th" />
                        <RowStyle CssClass="td" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

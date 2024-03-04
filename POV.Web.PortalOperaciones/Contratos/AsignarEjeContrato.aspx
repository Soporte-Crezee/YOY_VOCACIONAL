<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AsignarEjeContrato.aspx.cs" Inherits="POV.Web.PortalOperaciones.Contratos.AsignarEjeContrato" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/gridview.css")%>" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
        }
    </script>
    <style type="text/css">
        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
            white-space: pre-line;
            margin-bottom: 10px;
        }
        .wrap
        {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="BuscarContrato.aspx">Volver</asp:HyperLink>/Asignar
        ejes o ámbitos al contrato
    </h3>
    <div class=" ui-widget-content" style="padding: 5px">
        <div>
            <h2>
                Informaci&oacute;n del contrato</h2>
            <hr />
            <br />
            <table>
                <tr>
                    <td class="td-label">
                        <label>
                            Clave
                        </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtClave" MaxLength="100" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            Fecha inicio
                        </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaInicio" MaxLength="100" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <label>
                            Fecha fin
                        </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaFin" MaxLength="100" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <div style="float: left; width: 49%">
                <asp:Panel ID="pnlEjesDisponibles" runat="server" GroupingText="Ejes o ámbitos disponibles">
                    <asp:UpdatePanel ID="updEjesDisponibles" runat="server">
                        <ContentTemplate>
                            <div class="ui-widget-content" style="padding: 15px">
                                <table cellspacing="10">
                                    <tr>
                                        <td style="width: 100px; text-align: right">
                                            <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNombreDisponible" Width="200px" MaxLength="100" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="boton" OnClick="BtnBuscarDisponibles_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAgregar" runat="server" Text="Asignar todos" CssClass="boton"
                                            OnClick="BtnAsignarTodos_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView HorizontalAlign="Center" ID="grdEjesTematicosDisponibles" runat="server"
                                CssClass="DDGridView" RowStyle-CssClass="td" HeaderStyle-CssClass="th" AutoGenerateColumns="false"
                                PageSize="5" AllowPaging="true" Width="100%" Visible="False" OnRowDataBound="grdEjesTematicos1_RowDataBound"
                                OnRowCommand="grdEjesTematicos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="EjeTematicoID" HeaderText="ID" SortExpression="EjeTematicoID" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:BoundField DataField="Nivel" HeaderText="Nivel" SortExpression="Nivel"/>
                                    <asp:BoundField DataField="Grado" HeaderText="Grado" SortExpression="Grado"/>
                                    <asp:BoundField DataField="Materia" HeaderText="Asignatura" SortExpression="Materia" />
                                    <asp:BoundField DataField="Bloque" HeaderText="Bloque" SortExpression="Bloque"/>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Acción">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnCrear" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EjeTematicoID")%>'
                                                CommandName="asignar" runat="server" ToolTip="Asignar" Visible="false">
                                            <span>Asignar</span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle CssClass="wrap" />
                                        <ItemStyle CssClass="wrap" />
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
                                    <asp:GridViewPager ID="grdViewPager1" runat="server" SessionName="DsEjeTematicoDisponible"
                                        DataSourceType="DataSet" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <div style="float: right; width: 49%">
                <asp:Panel ID="pnlEjesAsignados" runat="server" GroupingText="Ejes o ámbitos asignados">
                    <asp:UpdatePanel ID="updEjesAsignados" runat="server">
                        <ContentTemplate>
                            <div class="ui-widget-content" style="padding: 15px">
                                <table cellspacing="10">
                                    <tr>
                                        <td style="width: 100px; text-align: right">
                                            <asp:Label ID="Label1" runat="server" Text="Nombre"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNombreAsignado" MaxLength="100" Width="200px" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button2" runat="server" Text="Buscar" CssClass="boton" OnClick="BtnBuscarAsignados_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDelete" runat="server" Text="Quitar todos" CssClass="boton" OnClick="BtnEliminarTodos_Click"
                                            OnClientClick="return confirm(' ¿Está seguro que desea quitar todos los ejes o ámbitos del contrato?  ');" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="grdEjesTematicosAsignados" runat="server" CssClass="DDGridViewWrap"
                                RowStyle-CssClass="td" HeaderStyle-CssClass="th" AutoGenerateColumns="false"
                                PageSize="5" AllowPaging="true" Width="100%" EnableSortingAndPagingCallbacks="True"
                                AllowSorting="true" Visible="false" OnRowDataBound="grdEjesTematicos2_RowDataBound"
                                OnRowCommand="grdEjesTematicos_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="EjeTematicoID" HeaderText="ID" SortExpression="EjeTematicoID" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                    <asp:BoundField DataField="Nivel" HeaderText="Nivel" SortExpression="Nivel"/>
                                    <asp:BoundField DataField="Grado" HeaderText="Grado" SortExpression="Grado"/>
                                    <asp:BoundField DataField="Materia" HeaderText="Asignatura" SortExpression="Materia" />
                                    <asp:BoundField DataField="Bloque" HeaderText="Bloque" SortExpression="Bloque"/>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Acción">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EjeTematicoID")%>'
                                                CommandName="eliminar" runat="server" ToolTip="Eliminar" Visible="false" OnClientClick="return confirm('¿Está seguro que desea quitar este elemento?');"><span>Quitar</span>
                                            </asp:LinkButton>
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
                                    <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsEjeTematicoAsignado"
                                        DataSourceType="DataSet" />
                                </PagerTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>

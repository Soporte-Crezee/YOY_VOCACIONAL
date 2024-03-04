<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarCurso.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoCursos.BuscarCurso" %>

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
<%--        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
            white-space: pre-line;
        }
        .wrap
        {
            white-space: nowrap;
        }--%>
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de cursos</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtID" MaxLength="10" runat="server" CssClass="number"></asp:TextBox>
                </td>
                <td class="td-label">
                    <label>
                        Modalidad</label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="DDLPresencial" />
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
                        Tema</label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="DDLTema" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estado</label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="DDLEstatusProfesionalizacion" />
                </td>
                <td class="td-label"></td>
                <td>
                     <div>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
        </div>
                </td>
            </tr>
        </table>
       
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="True">
            <a href="RegistrarCurso.aspx" id="lnkNuevoCurso" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label runat="server" Text="Agregar nuevo curso" ID="lblNuevaCurso"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updCursos" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdCursos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" Visible="True"
                    OnRowCommand="grdCursos_RowCommand" OnRowDataBound="grdCursos_RowDataBound" OnSorting="grdCursos_Sorting">
                    <Columns>
                        <asp:BoundField DataField="AgrupadorContenidoDigitalID" HeaderText="ID" SortExpression="AgrupadorContenidoDigitalID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ItemStyle-CssClass="break_text"/>
                        <asp:BoundField DataField="NombreTemaCurso" HeaderText="Tema curso" SortExpression="NombreTemaCurso" ItemStyle-CssClass="break_text"/>
                        <asp:BoundField DataField="NombreModalidad" HeaderText="Modalidad" SortExpression="NombreModalidad" />
                        <asp:BoundField DataField="NombreEstatus" HeaderText="Estado" SortExpression="NombreEstatus" />
                        <asp:TemplateField HeaderText="Asignar contenidos" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkbtnAsignarContenido" Text="Asignar contenidos digitales"
                                    CommandName="asignarcontenido" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"AgrupadorContenidoDigitalID") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AgrupadorContenidoDigitalID")%>'
                                    ImageUrl="~/images/edit-button.png" ToolTip="Editar" Visible="False" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AgrupadorContenidoDigitalID")%>'
                                    ImageUrl="~/images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                    Visible="False" />
                            </ItemTemplate>
                            <ControlStyle CssClass="wrap" />
                            <ItemStyle CssClass="wrap" HorizontalAlign="Center" />
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsCursosCatalogo"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

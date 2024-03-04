<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarEjeTematico.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.BuscarEjeTematico" %>

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
            $(".boton_new").button();
            $(".boton_search").button({ icons: {
                primary: "ui-icon-search"
            }
            });
        }
    </script>
    <style type="text/css">
        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
            white-space: pre-line;
        }
        .wrap
        {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de ejes o ámbitos</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblID" runat="server" Text="ID"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtID" EnableViewState="true" CssClass="number" runat="server" MaxLength="10"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label ID="lblGrado" runat="server" Text="Grado"></asp:Label>
                </td>
                <td class="input">
                    <asp:UpdatePanel ID="UpdGrado" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="cbGradoAsignatura" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbGradoAsignatura_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtNombre" EnableViewState="true" runat="server"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label ID="lblAreaProfesionalizacion" runat="server" Text="Asignatura"></asp:Label>
                </td>
                <td class="input">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlAreasProfesionalizacion" EnableViewState="true"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAreasProfesionalizacion_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estado
                    </label>
                </td>
                <td class="input">
                    <asp:DropDownList ID="ddlEstatusProfesionalizacion" EnableViewState="true" runat="server" />
                </td>
                <td class="td-label">
                    <asp:Label ID="lblMateriaProfesionalizacion" runat="server" Text="Bloque"></asp:Label>
                </td>
                <td class="input">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMateriasProfesionalizacion" EnableViewState="true" runat="server">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Nivel Educativo
                    </label>
                </td>
                <td class="input">
                    <asp:UpdatePanel ID="updNivelEducativo" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="cbNivelEducativo" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="cbNivelEducativo_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="td-label">
                </td>
                <td>
                    <div>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton_search" OnClick="btnBuscar_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="pnlCreate" runat="server" class="nuevo" visible="false">
            <a href="RegistrarEjeTematico.aspx" id="lnkNuevoEjeTematico" class="boton_new"><span
                class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                margin-top: -5px;"></span>
                <asp:Label ID="lblNuevoEjeTematico" CssClass=".boton_new" runat="server" Text="Agregar nuevo eje o ámbito"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updEjesTematicos" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdEjesTematicos" runat="server" AutoGenerateColumns="False" CssClass="DDGridView "
                    RowStyle-CssClass="td" Visible="False" HeaderStyle-CssClass="th" AllowPaging="True"
                    Width="100%" OnRowCommand="grdEjesTematicos_RowCommand" OnRowDataBound="grdEjesTematicos_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="ID" DataField="EjeTematicoID">
                            <HeaderStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                        <asp:BoundField HeaderText="Asignatura" DataField="Area" />
                        <asp:BoundField HeaderText="Bloque" DataField="Materias" />
                        <asp:TemplateField HeaderText="Temas" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnConfigurar" ImageUrl="~/images/page_gear.png"
                                    ToolTip="Configurar temas" Visible="false" CommandName="ConfigurarSituaciones"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EjeTematicoID")%>' />
                            </ItemTemplate>
                            <ControlStyle CssClass="wrap" />
                            <ItemStyle CssClass="wrap" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="60px" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btnEdit" ImageUrl="~/Images/edit-button.png"
                                    ToolTip="Editar" Visible="false" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EjeTematicoID")%>' />
                                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/Images/minus-button.png"
                                    ToolTip="Eliminar" Visible="false" CommandName="eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EjeTematicoID")%>' />
                            </ItemTemplate>
                            <ControlStyle CssClass="wrap" />
                            <ItemStyle CssClass="wrap" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>No se encontraron
                                coincidencias, por favor verifique su consulta.
                            </p>
                        </div>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="th" />
                    <PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsEjesTematicos"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                    <RowStyle CssClass="td" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarCiudades.aspx.cs" Inherits="POV.Web.PortalOperaciones.Ciudades.BuscarCiudades" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
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
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">Cat&aacute;logo de ciudades</h3>
        </div>
        <div class="finder ui-widget-content">
            <table class="finder">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPais" runat="server" Text="Pa&iacute;s"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:DropDownList ID="ddlPais" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="label">
                        <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="textoEnunciado"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-green" OnClick="btnBuscar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="RegistrarCiudad.aspx" id="lnkNuevaCiudad" class="btn-green">
                    <span class=" ui-icon ui-icon-circle-plus"
                        style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label ID="lblNuevaCiudad" runat="server" Text="Agregar nueva ciudad"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updCiudades" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdCiudades" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" OnRowCommand="grdCiudades_RowCommand"
                        OnSorting="grdCiudades_Sorting" Visible="false" OnRowDataBound="grdCiudades_DataBound">
                        <Columns>
                            <asp:BoundField DataField="CiudadID" HeaderText="ID" SortExpression="CiudadID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                            <asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" />
                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha de registro" SortExpression="FechaRegistro" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CiudadID")%>'
                                        ImageUrl="../images/VOCAREER_editar.png" Visible="false" ToolTip="Editar" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CiudadID")%>'
                                        ImageUrl="../images/VOCAREER_suprimir.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Visible="false" ToolTip="Eliminar" />
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
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="Ciudades" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

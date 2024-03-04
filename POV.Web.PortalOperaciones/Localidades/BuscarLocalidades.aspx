<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarLocalidades.aspx.cs" Inherits="POV.Web.PortalOperaciones.Localidades.BuscarLocalidades" %>
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
<h3 class="ui-widget-header">
        Cat&aacute;logo de localidades</h3>
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
                    <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblCiudad" runat="server" Text="Ciudad"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList ID="ddlCiudad" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td class="label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox ID="txtNombre" MaxLength="30"  runat="server"  CssClass=" textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td>
                  <div>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
                  </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="RegistrarLocalidad.aspx" id="lnkNuevaLocalidad" class="boton">
                <span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblNuevaLocalidad" runat="server" Text="Agregar nueva localidad"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updLocalidades" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdLocalidades" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" OnRowCommand="grdLocalidades_RowCommand"
                    OnSorting="grdLocalidades_Sorting" Visible="false" 
                        OnRowDataBound="grdLocalidades_DataBound">
                    <Columns>
                        <asp:BoundField DataField="LocalidadID" HeaderText="ID" SortExpression="LocalidadID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                        <asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Estado" />
                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha de registro" SortExpression="FechaRegistro" DataFormatString="{0:d}"  ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "LocalidadID")%>'
                                    ImageUrl="../images/edit-button.png" Visible="false" ToolTip="Editar"/>
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "LocalidadID")%>'
                                    ImageUrl="../images/minus-button.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" 
                                    Visible="false" ToolTip="Eliminar"/>
                            </ItemTemplate>
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="Localidades" DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

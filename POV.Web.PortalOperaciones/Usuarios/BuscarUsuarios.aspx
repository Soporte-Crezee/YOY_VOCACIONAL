<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarUsuarios.aspx.cs" Inherits="POV.Web.PortalOperaciones.Usuarios.BuscarUsuarios" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../Scripts/messages_es.js" type="text/javascript"></script>
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
        Cat&aacute;logo de usuarios</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre de usuario"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtNombreUsuario" MaxLength="50" runat="server" CssClass="textoClave"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label ID="lblEmail" runat="server" Text="Correo electrónico"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtEmail" MaxLength="50" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblActivo" runat="server" Text="Estatus"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="CbActivo">
                        <asp:ListItem Value="">&nbsp;</asp:ListItem>
                        <asp:ListItem Value="True">ACTIVO</asp:ListItem>
                        <asp:ListItem Value="False">INACTIVO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="label"></td>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="BtnBuscar_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="RegistrarUsuario.aspx" id="lnkNuevoReactivo" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblNuevoReactivo" runat="server" Text="Agregar nuevo usuario"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updReactivo" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdUsuarios" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" OnRowCommand="GrdUsuarios_RowCommand" EnableSortingAndPagingCallbacks="True"
                    AllowSorting="true" OnSorting="GrdUsuarios_Sorting" OnRowDataBound="grdUsuarios_DataBound"
                    Visible="false">
                    <Columns>
                        <asp:BoundField DataField="UsuarioID" HeaderText="ID" SortExpression="UsuarioID" />
                        <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre" SortExpression="NombreUsuario" />
                        <asp:BoundField DataField="Email" HeaderText="Correo electrónico" SortExpression="Email" />
                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <%# Boolean.Parse(Eval("EsActivo").ToString())? "S&iacute;": "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Privilegios" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnPrivilegios" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UsuarioID")%>'
                                    CommandName="privilegios" Text="Editar" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UsuarioID")%>'
                                    ImageUrl="../images/edit-button.png" ToolTip="Editar" Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UsuarioID")%>'
                                    ImageUrl="../images/minus-button.png" ToolTip="Desactivar" OnClientClick="return confirm('¿Está seguro que desea desactivar este elemento?');"
                                    Visible="false" />
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsUsuarios" DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

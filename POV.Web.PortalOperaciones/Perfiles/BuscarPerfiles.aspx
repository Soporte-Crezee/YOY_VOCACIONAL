<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarPerfiles.aspx.cs" Inherits="POV.Web.PortalOperaciones.Perfiles.BuscarPerfiles" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../Scripts/messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            ValidarFormBusqueda();
        }
        function ValidarFormBusqueda() {
            $("#frmMain").validate({
                rules: {
                    '<%=txtPerfilID.UniqueID %>': {
                        maxlength: 2
                    },
                    '<%=txtNombrePerfil.UniqueID %>': {
                        maxlength: 40
                    },
                    '<%=txtDescripcion.UniqueID %>': {
                        maxlength: 100
                    }
                }
                , submitHandler: function (form) {
                    $(form).block();
                    form.submit();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de perfiles</h3>
    <div class="finder ui-widget-content">
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblPerfilID" runat="server" Text="ID"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPerfilID" MaxLength="2" runat="server" CssClass="digits"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre del perfil"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombrePerfil" MaxLength="40" runat="server" CssClass="textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" MaxLength="100" runat="server" CssClass="textoEnunciado"></asp:TextBox>
                </td>
                <td class="td-label">
                    <asp:Label ID="lblEstatus" runat="server" Text="Estatus"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cbEstatus" runat="server">
                        <asp:ListItem Value="" Text="Todos" Selected="true"></asp:ListItem>
                        <asp:ListItem Value="true" Text="Activos"></asp:ListItem>
                        <asp:ListItem Value="false" Text="Inactivos"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
            <td></td>
            <td></td>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="BtnBuscar_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="RegistrarPerfil.aspx" id="lnkNuevoPerfil" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblNuevoPerfil" runat="server" Text="Agregar nuevo perfil"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updReactivo" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdPerfiles" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" OnRowCommand="GrdPerfiles_RowCommand" EnableSortingAndPagingCallbacks="True"
                    AllowSorting="true" OnRowDataBound="grdPerfiles_DataBound" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="PerfilID" HeaderText="ID" SortExpression="PerfilID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="NombrePerfil" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripción"
                            ItemStyle-CssClass="break_text" />
                        <asp:TemplateField HeaderText="Estatus">
                            <ItemTemplate>
                                <%# Boolean.Parse(Eval("Estatus").ToString())? "Activo": "Inactivo" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Permisos" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnPermisos" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PerfilID")%>'
                                    CommandName="permisos" Text="Editar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PerfilID")%>'
                                    ImageUrl="../images/edit-button.png" ToolTip="Editar" Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PerfilID")%>'
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsPerfiles" DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

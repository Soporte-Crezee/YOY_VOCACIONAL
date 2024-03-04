<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarDirectores.aspx.cs" Inherits="POV.Web.PortalOperaciones.Directores.BuscarDirectores" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../Scripts/messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton_new").button();
            $(".boton").button();
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de directores</h3>
    <div class="finder ui-widget-content">
        <asp:UpdatePanel ID="updFiltroDirector" runat="server">
            <ContentTemplate>
                <table class="finder">
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblCURP" runat="server" Text="CURP"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurp" runat="server" MaxLength="30" CssClass="textoClave"></asp:TextBox>
                        </td>
                        <td class="label">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="200" CssClass="textoNombrePersona"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblPrimerApellido" runat="server" Text="Primer apellido"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrimerApellido" runat="server" MaxLength="100" CssClass="textoNombrePersona"></asp:TextBox>
                        </td>
                        <td class="label">
                            <asp:Label ID="lblSegundoApellido" runat="server" Text="Segundo apellido"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSegundoApellido" runat="server" MaxLength="100" CssClass="textoNombrePersona"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label ID="lblSexo" runat="server" Text="Sexo"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cbSexo" runat="server">
                                <asp:ListItem Value="" Text="Todos" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="true" Text="Hombre"></asp:ListItem>
                                <asp:ListItem Value="false" Text="Mujer"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="label">
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="../Directores/RegistrarDirector.aspx" id="lnkNuevoDirector" class="boton_new">
                <span class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                    margin-top: -5px;"></span>
                <asp:Label ID="lblNuevoDirector" runat="server" Text="Agregar nuevo director"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updDirectores" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdDirectores" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" OnRowCommand="grdDirectores_RowCommand" Visible="false" OnRowDataBound="grdDirectores_DataBound">
                    <Columns>
                        <asp:BoundField DataField="Curp" HeaderText="CURP" SortExpression="Curp" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" SortExpression="NombreCompleto" />
                        <asp:BoundField DataField="NivelEscolar" HeaderText="Escolaridad" SortExpression="NivelEscolar" />
                        <asp:BoundField DataField="Correo" HeaderText="Correo" SortExpression="Correo" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" SortExpression="Telefono" />
                        <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha de registro" SortExpression="FechaRegistro" DataFormatString="{0:d}"/>
                        <asp:BoundField DataField="Estado" HeaderText="Estatus" SortExpression="Estado" />
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "DirectorID") %>'
                                    ImageUrl="../images/edit-button.png" ToolTip="Editar" Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "DirectorID")%>'
                                    ImageUrl="../images/minus-button.png" ToolTip="Desactivar" OnClientClick="return confirm('¿Está seguro que desactivar al director?');"
                                    Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left;"></span>La b&uacute;squeda
                                no produjo resultados
                            </p>
                        </div>
                    </EmptyDataTemplate>
                    <PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsDirectores" DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

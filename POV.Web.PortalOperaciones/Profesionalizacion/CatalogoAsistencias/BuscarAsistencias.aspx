<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarAsistencias.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAsistencias.BuscarAsistencias" %>

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

        function ValidateFields() {
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo asistencias
    </h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblIdentificador" Text="ID" ToolTip="Identificador"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtIdentificador" TabIndex="1" CssClass="number positive" MaxLength="10"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblTemaAsistencia" Text="Tema asistencia" ToolTip="Tema asistencia"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="ddlTemaAsistencia" TabIndex="3" />
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre" ToolTip="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtNombre" TabIndex="2" MaxLength="100" ToolTip="Nombre"></asp:TextBox>
                </td>
                 <td class="label">
                    <asp:Label runat="server" ID="lbl" Text="Estatus" ToolTip="Estado"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="ddlEstatus" TabIndex="4" />
                </td>
            </tr>
            <tr>
                <td class="label"></td>
                <td class="input"></td>
                <td class="label"></td>
                <td>
                    <div>
            <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="BtnBuscar_Click"
                OnClientClick="ValidateFields();" />
        </div>
                </td>
            </tr>
        </table>
        
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="False">
            <a href="RegistrarAsistencia.aspx" id="lnkNuevaAsistencia" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label runat="server" Text="Agregar nueva asistencia" ID="lblNuevaAsistencia"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updAsistencias" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdAsistencias" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    EnableSortingAndPagingCallbacks="True" Width="100%" AllowSorting="true" Visible="True"
                    OnRowDataBound="grdAsistencias_DataBound" OnRowCommand="grdAsistencias_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="AgrupadorContenidoDigitalID" HeaderText="ID" SortExpression="AgrupadorContenidoDigitalID">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="30px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ItemStyle-CssClass="break_text">
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NombreTemaAsistencia" HeaderText="Tema asistencia" SortExpression="NombreTemaAsistencia" ItemStyle-CssClass="break_text">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NombreEstatus" HeaderText="Estatus" SortExpression="NombreEstatus">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="110px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Contenido digital" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkbtnAsignarContenido" Visible="False" Text="Asignar contenidos digitales"
                                    CommandName="asignarcontenido" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"AgrupadorContenidoDigitalID") %>'>
                                    
                                </asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="180px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AgrupadorContenidoDigitalID")%>'
                                                ImageUrl="~/images/edit-button.png" ToolTip="Editar" Visible="False" />
                                        
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AgrupadorContenidoDigitalID")%>'
                                                ImageUrl="~/images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                                Visible="False" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsAsistencias" DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

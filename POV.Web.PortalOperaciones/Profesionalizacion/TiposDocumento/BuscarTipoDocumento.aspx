<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarTipoDocumento.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.TiposDocumento.BuscarTipoDocumento" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            $("#frmMain").validate();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        Cat&aacute;logo de tipos de documento
    </h3>
    <div class="finder ui-widget-content">
        <asp:UpdatePanel runat="server" ID="UpdFiltroTipoDocumento">
            <ContentTemplate>
                <table class="finder">
                    <tr>
                        <td class="label">
                            <asp:Label runat="server" ID="lblID" Text="ID"></asp:Label>
                        </td>
                        <td class="input">
                            <asp:TextBox runat="server" ID="txtID" CssClass="number" MaxLength="10"></asp:TextBox>
                        </td>
                         <td class="label">
                            <asp:Label runat="server" ID="lblExtension" Text="Extensi&oacute;n"></asp:Label>
                        </td>
                        <td class="input">
                            <asp:TextBox runat="server" ID="txtExtension" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                            <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                        </td>
                        <td class="input">
                            <asp:TextBox runat="server" ID="txtNombre" MaxLength="50"></asp:TextBox>
                        </td>
                        <td class="td-label"></td>
                        <td>
                            <div>
                    <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
                </div>
                        </td>
                    </tr>
                    <tr>
                       
                    </tr>
                </table>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="results">
        <div id="pnlCreate" runat="server" class="nuevo" visible="false">
            <a href="RegistrarTipoDocumento.aspx" id="lnkNuevo" class="boton"><span class="ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label runat="server" ID="lblNuevoTipoDocumento" Text="Agregar nuevo tipo de documento"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel runat="server" ID="updTipoDocumento">
            <ContentTemplate>
                <asp:GridView runat="server" ID="grdTiposDocumento" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="100%" OnRowCommand="grdTipoDocumento_RowComand" OnRowDataBound="grdTipoDocumento_DataBound"
                    Visible="false">
                    <Columns>
                        <asp:BoundField DataField="TipoDocumentoID" HeaderText="ID" SortExpression="TipoDocumentoID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                        <asp:BoundField DataField="Extension" HeaderText="Extensi&oacute;n" SortExpression="Extension" />
                        <asp:TemplateField HeaderText="Es editable">
                            <ItemTemplate>
                                <%# Boolean.Parse(Eval("EsEditable").ToString())? "Si": "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TipoDocumentoID")%>'
                                    ImageUrl="../../images/edit-button.png" ToolTip="Editar" Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TipoDocumentoID")%>'
                                    ImageUrl="../../images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                    Visible="false" />
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
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsTipoDocumento"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

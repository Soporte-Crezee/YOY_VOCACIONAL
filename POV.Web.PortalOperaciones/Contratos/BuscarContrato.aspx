<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarContrato.aspx.cs" Inherits="POV.Web.PortalOperaciones.Contratos.BuscarContrato" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .dato
        {
            height: 2.2em;
            width: 100%;
        }
        .dato span
        {
            float: left;
            padding-top: .4em;
            width: 5em;
        }
        .dato input
        {
            width: 15em;
        }
        #clave, #nombrecte
        {
            float: left;
        }
        #busqueda
        {
            padding-right: .5em;
        }
    </style>
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
        Cat&aacute;logo de contratos</h3>
    <div class="finder ui-widget-content">
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label ID="lblClave" runat="server" Text="Clave"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtClave" MaxLength="30" runat="server" CssClass="textoClave"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label ID="lblNombreCte" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombreCte" MaxLength="100" runat="server" CssClass="textoEnunciado"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div class="results">
        <div id="PnlCreate" class="nuevo" runat="server" visible="false">
            <a href="RegistrarContrato.aspx" id="lnkNuevoEstado" class="boton"><span class=" ui-icon ui-icon-circle-plus"
                style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                <asp:Label ID="lblNuevoEstado" runat="server" Text="Agregar nuevo contrato"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updPaises" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdContratos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="False" AllowPaging="True" EnableSortingAndPagingCallbacks="True"
                    PageSize="10" AllowSorting="True" OnRowCommand="grdEstados_RowCommand" Visible="false"
                    OnRowDataBound="grdContratos_DataBound" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Clave" HeaderText="Clave"></asp:BoundField>
                        <asp:BoundField DataField="InicioContrato" HeaderText="Fecha inicio" DataFormatString="{0:dd/MM/yyyy}">
                        </asp:BoundField>
                        <asp:BoundField DataField="FinContrato" HeaderText="Fecha fin" DataFormatString="{0:dd/MM/yyyy}">
                        </asp:BoundField>
                        <asp:CheckBoxField DataField="LicenciasLimitadas" HeaderText="Limitadas">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                        <asp:BoundField DataField="NumeroLicencias" HeaderText="Licencias">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="Consumidas" HeaderText="Consumidas">
                        </asp:BoundField>
                        <asp:BoundField DataField="Disponibles" HeaderText="Disponibles">
                        </asp:BoundField>
                        <asp:BoundField DataField="EstatusNombre" HeaderText="Estatus">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Ciclos escolares" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnConfig" runat="server" ToolTip="Configurar ciclos escolares"
                                    CommandName="config" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContratoID")%>'
                                    ImageUrl="../images/page_gear.png" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ejes o ámbitos" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAsignarEje" runat="server" ToolTip="Asignar ejes o ámbitos"
                                    CommandName="asignarEjes" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContratoID")%>'
                                    ImageUrl="../images/tick.png" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ToolTip="Editar contrato" CommandName="Editar"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContratoID")%>' ImageUrl="../images/edit-button.png"
                                    Visible="false" />
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="desactivar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContratoID")%>'
                                    ImageUrl="../images/minus-button.png" ToolTip="Desactivar" Visible="false" OnClientClick="return confirm('¿Está seguro que desea desactivar éste elemento?');" />
                                <asp:ImageButton ID="btnActivate" runat="server" CommandName="activar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ContratoID")%>'
                                    ImageUrl="../images/plus-button.png" ToolTip="Activar" OnClientClick="return confirm('¿Está seguro que desea activar éste elemento?');"
                                    Visible="false" />
                            </ItemTemplate>
                            <ItemStyle Width="20px" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                resultados</p>
                        </div>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="th" />
                    <PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="ResultadoContratos"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                    <RowStyle CssClass="td" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

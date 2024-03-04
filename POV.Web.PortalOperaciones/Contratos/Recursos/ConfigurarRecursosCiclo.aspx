<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurarRecursosCiclo.aspx.cs" Inherits="POV.Web.PortalOperaciones.Contratos.Recursos.ConfigurarRecursosCiclo" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();

        }
        function blockOnEditAction() {

            $('.divblockeado').block({ message: null });
            $('.divblockeado1').block({ message: null });
            $('.divblockeado2').block({ message: null });
        }

        function unblockOnEditAction() {

            $('.divblockeado').unblock();
            $('.divblockeado1').unblock();
            $('.divblockeado2').unblock();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:LinkButton ID="LnkBtnBack" runat="server" OnClick="BtnCancelar_OnClick">Volver</asp:LinkButton>/
        Configurar recursos ciclos orientaci&oacute;n vocacional
    </h3>
    <div class="ui-widget-content" style="padding: 5px;">
        <%-- BEGIN informacion de contratro --%>
        <br />
        <%-- END informacion de contratro --%>
        <%-- BEGIN informacion de ciclo escolar --%>
        <h2>
            Informaci&oacute;n del ciclo de orientaci&oacute;n vocacional</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <label>Nombre</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtNombreCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>Fecha inicio</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtInicioCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td class="td-label">
                    <label>Fecha fin</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtFinCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <%-- END informacion de ciclo escolar--%>
        <%-- BEGIN configuracion prueba pivote --%>
        <asp:Panel ID="pnlPruebaPivote" runat="server" GroupingText="Configurar prueba pivote">
            <div class="divblockeado">
                <div id="PnlAsignarPruebaPivote" class="nuevo" runat="server">
                    <asp:HyperLink ID="HpLinkAsignarPruebaPivote" runat="server" CssClass="boton" NavigateUrl="AsignarPruebaPivote.aspx">
                        <span class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                            margin-top: -5px;"></span>
                        <asp:Label ID="lblNuevaPruebaPivote" runat="server" Text="Asignar prueba pivote"></asp:Label>
                    </asp:HyperLink>
                </div>
                <br />
                <asp:GridView runat="server" ID="GrdViewPruebaPivote" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" Width="100%" AllowSorting="true"
                    OnRowCommand="GrdViewPruebaPivote_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" SortExpression="Modelo" />
                        <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="btndelete1" CommandName="eliminar" ImageUrl="~/Images/VOCAREER_suprimir.png"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PruebaContratoID") %>'
                                    OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>El ciclo escolar no
                                tiene prueba pivote asignada</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </asp:Panel>
        <%-- END configuracion prueba pivote --%>

    <%-- BEGIN configuracion pruebas --%>
    <asp:Panel ID="PnlPruebas" runat="server" GroupingText="Configurar pruebas">
        <div class="divblockeado2">
            <div id="PnlAsignarPruebas" class="nuevo" runat="server">
                <asp:HyperLink ID="hpLinkAsignarPrueba" runat="server" CssClass="btn-green" NavigateUrl="AsignarPrueba.aspx">
                    <span class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                        margin-top: -5px;"></span>
                    <asp:Label ID="lblAsignarPrueba" runat="server" Text="Asignar prueba"></asp:Label>
                </asp:HyperLink>
            </div>
            <asp:UpdatePanel ID="UpdPanelConfigPruebas" runat="server">
                <ContentTemplate>
                    <%-- OnRowCommand="GrdViewPaqueteJuegos_RowCommand"--%>
                    <asp:GridView runat="server" ID="GrdViewPruebas" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="100%" EnableSortingAndPagingCallbacks="True" AllowSorting="true" OnRowCommand="GrdViewPruebas_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" SortExpression="Modelo" />
                            <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" ID="btndelete3" CommandName="eliminar" ImageUrl="~/Images/VOCAREER_suprimir.png"
                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem,"PruebaContratoID") %>'
                                        OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?')" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>El ciclo de orientaci&oacute;n vocacional no
                                    tiene pruebas asignadas</p>
                            </div>
                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="dsRecursoPruebas"
                                DataSourceType="DataTable" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
   </asp:Panel>
    <%-- END configuracion pruebas --%>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(updateBlockPanelUI);

        //function updateBlockPanelUI() {
        //    if (hdnCommand.val() != '1')
        //        unblockOnEditAction();
        //    else
        //        blockOnEditAction();
        //}
    </script>
    <div class="line">
    </div>
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
        style="display: none;" />
    <asp:Button ID="BtnVolver" runat="server" Text="Volver" CssClass="btn-green" OnClick="BtnCancelar_OnClick" />
  </div>
</asp:Content>

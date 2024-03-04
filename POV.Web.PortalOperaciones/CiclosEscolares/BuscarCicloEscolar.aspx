<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="BuscarCicloEscolar.aspx.cs" Inherits="POV.Web.PortalOperaciones.CiclosEscolares.BuscarCicloEscolar" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable col-xs-13 col-md-13">
        <div class="ui-widget-header">
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">
                <%--<asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick">Volver</asp:LinkButton>/--%>
        Configurar ciclos de orientaci&oacute;n	vocacional
            </h3>
        </div>
        <div class="finder ui-widget-content" style="padding: 5px">
            <%-- BEGIN informacion de contratro--%>
            <%--<h2>
            Informaci&oacute;n del contrato</h2>
        <hr />--%>
            <br />
            <table>
                <tr style="display: none">
                    <td class="td-label">Fecha de contrato
                    </td>
                    <td>
                        <asp:TextBox ID="TxtFechaContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                    </td>
                    <td class="td-label">Clave
                    </td>
                    <td>
                        <asp:TextBox ID="TxtClave" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr style="display: none">
                    <td class="td-label">Fecha inicio
                    </td>
                    <td>
                        <asp:TextBox ID="TxtInicioContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                    </td>
                    <td class="td-label">Fecha fin
                    </td>
                    <td>
                        <asp:TextBox ID="TxtFinContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </table>
            <%-- END informacion de contratro--%>
            <br />
            <br />

        </div>
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="RegistrarCicloEscolar.aspx" id="lnkNuevoCicloEscolar" class="btn-green"><span
                    class="ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label ID="lblNuevoCicloEscolar" runat="server" Text="Agregar nuevo ciclo orientación vocacional"></asp:Label>
                </a>
            </div>
            <div class="table-responsive">
                <asp:UpdatePanel runat="server" ID="UpdCiclosEscolares">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="grdCiclosEscolar" CssClass="DDGridView" RowStyle-CssClass="td"
                            HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                            EnableSortingAndPagingCallbacks="True" OnRowDataBound="grdCiclosEscolar_DataBound"
                            AllowSorting="true" OnRowCommand="grdCiclosEscolar_RowCommand" Visible="false" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="CicloContratoID" HeaderText="ID" SortExpression="CicloContratoID" />
                                <asp:BoundField DataField="Titulo" HeaderText="Ciclo orientación vocacional" SortExpression="Titulo" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Fecha inicio" ID="lblheadFechaInicio"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblInicioCiclo" Text='<%# string.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem,"InicioCiclo")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" ID="lblheadFechaFin" Text="Fecha fin"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFinCiclo" Text='<%#string.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem,"FinCiclo")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" ItemStyle-CssClass="break_text" />
                                <asp:BoundField DataField="Liberado" HeaderText="Liberado" SortExpression="Liberado"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="EsActivo" HeaderText="Activo" SortExpression="EsActivo"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" ID="lblheadRecursos" Text="Recursos"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkBtnRecursos" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CicloContratoID")%>'
                                            CommandName="config" runat="server">
                                       <span>Configurar recursos</span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CicloContratoID")%>'
                                            ImageUrl="../images/VOCAREER_editar.png" Visible="false" />
                                        <asp:ImageButton runat="server" ID="btndelete" CommandName="eliminar" ImageUrl="../Images/VOCAREER_suprimir.png"
                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CicloContratoID") %>'
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?')"
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
                                <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="ciclosescolares"
                                    DataSourceType="DataSet" />
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
        </div>
    </div>
</asp:Content>

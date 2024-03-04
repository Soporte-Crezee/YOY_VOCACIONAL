<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarEstado.aspx.cs" Inherits="POV.Web.PortalOperaciones.Estados.BuscarEstado" %>

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
            <h3 class="ui-widget-header-label" style="margin: 0px 0px 0px 80px">Cat&aacute;logo de estados</h3>
        </div>
        <div class="finder ui-widget-content">
            <table class="finder">
                <tr>
                    <td class="label">
                        <asp:Label ID="lblPais" runat="server" Text="País"></asp:Label></td>
                    <td class="input">
                        <asp:DropDownList ID="ddlPais" runat="server"
                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td class="label">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="textoEnunciado"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="label">
                        <div>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-green" OnClick="btnBuscar_Click" />
                        </div>
                    </td>
                </tr>
            </table>


        </div>
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="RegistrarEstado.aspx" id="lnkNuevoEstado" class="btn-green">
                    <span class=" ui-icon ui-icon-circle-plus"
                        style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <asp:Label ID="lblNuevoEstado" runat="server" Text="Agregar nuevo estado"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updPaises" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdEstados" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10"
                        AllowPaging="true" Width="100%"
                        EnableSortingAndPagingCallbacks="True" AllowSorting="true"
                        OnRowCommand="grdEstados_RowCommand" OnSorting="grdEstados_Sorting" Visible="false"
                        OnRowDataBound="grdEstados_DataBound">
                        <Columns>
                            <asp:BoundField DataField="EstadoID" HeaderText="ID" SortExpression="EstadoID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" />
                            <asp:BoundField DataField="Pais" HeaderText="País" SortExpression="Pais" />
                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha de registro" SortExpression="FechaRegistro" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar"
                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EstadoID")%>' ImageUrl="../images/VOCAREER_editar.png"
                                        Visible="false" ToolTip="Editar" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar"
                                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "EstadoID")%>' ImageUrl="../images/VOCAREER_suprimir.png"
                                        OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" Visible="false" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>

                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>
                                    La búsqueda no produjo resultados
                                </p>
                            </div>

                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="Estados" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

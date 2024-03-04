<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarEventos.aspx.cs" Inherits="POV.Web.PortalUniversidad.Eventos.BuscarEventos" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            DoFormBlockUI();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default">
        <div class="panel-heading">
                Cat&aacute;logo de eventos
        </div>
        <div class="panel-body">
            <div class="col-lg-8 col-lg-offset-2">
                <div class="input-group">
                    <span class="hidden-xs hidden-sm input-group-addon btn-addon-find">Nombre</span>
                    <asp:TextBox ID="txtNombre" TabIndex="1" MaxLength="100" runat="server" CssClass="form-control text-find" placeholder="Nombre del evento"></asp:TextBox>
                    <span class="input-group-btn">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-green" />
                    </span>
                </div>
            </div>
        </div>
    </div>
        <div class="" style="padding: 20px 0px 0px 0px">
            <div class="">
                <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                    <a href="NuevoEvento.aspx" id="A1" class="btn-green"><span class=" ui-icon ui-icon-circle-plus"
                        style="display: inline-block; vertical-align: middle; margin-top: 0px;"></span>
                        <label class="" style="cursor: pointer;">Agregar nuevo evento</label>
                    </a>
                </div>
            </div>
            <div class="">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                            <div class="table-responsive">
                                <asp:GridView runat="server" ID="grdEventoUniversidades" CssClass="table table-bordered table-striped"
                                    AutoGenerateColumns="False" PageSize="10" AllowPaging="True"
                                    EnableSortingAndPagingCallbacks="True" AllowSorting="False" OnRowCommand="grdEventosUniversidad_RowCommand"
                                    Visible="false" OnRowDataBound="gv_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre">
                                            <ItemStyle Width="60%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FechaInicio" HeaderText="Fecha inicio" SortExpression="Fechainicio" />
                                        <asp:BoundField DataField="FechaFin" HeaderText="Fecha fin" SortExpression="Fechafin" />
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="btnEdit" CommandName="editar" ImageUrl="../images/VOCAREER_editar.png"
                                                    ToolTip="Editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"EventoUniversidadId") %>'
                                                    Visible="false" />
                                                <asp:ImageButton runat="server" ID="btnDel" CommandName="eliminar" ImageUrl="../images/VOCAREER_suprimir.png"
                                                    ToolTip="Eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"EventoUniversidadId")  %>'
                                                    OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                                    Visible="false" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="ui-state-highlight ui-corner-all">
                                            <p>
                                                <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                                La b&uacute;squeda no produjo resultados
                                            </p>
                                        </div>
                                    </EmptyDataTemplate>
                                    <HeaderStyle CssClass="th" />
                                    <RowStyle CssClass="td" />
                                    <PagerTemplate>
                                        <asp:GridViewPager ID="grdViewPager" runat="server" DataSourceType="DataSet" SessionName="EventoUniversidad" />
                                    </PagerTemplate>
                                </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server" style="display: none;" />
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls(sender, args) {
            $('.boton').button();
        }
    </script>
</asp:Content>

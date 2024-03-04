<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarCarreras.aspx.cs" Inherits="POV.Web.PortalUniversidad.Carreras.BuscarCarreras" %>

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
    <div class="col-md-12" style="padding: 0px 0px 0px 0px">
        <div class="col-xs-12 titulo_marco_general_principal">
            <div class="titulo_marco_general_principal" style="margin: 0px 0px 0px 80px"> ► Cat&aacute;logo de carreras </div>
        </div>
        <div class="col-xs-12 container_busqueda_general ui-widget-content">
            <div class="col-xs-12 col-sm-6">
                <div class="form-group">
                    <asp:Label runat="server" ID="Label1" Text="Nombre" CssClass="col-sm-4 control-label" ToolTip="Nombre carrera"></asp:Label>
                    <div class="col-sm-8">
                        <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblAreaConocimiento" Text="Área de conocimiento" CssClass="col-sm-4 control-label" ToolTip="Área de conocimiento"></asp:Label>
                    <div class="col-sm-8">
                        <asp:DropDownList ID="ddlAreaConocimiento" runat="server" CssClass="form-control">
                            <asp:ListItem Value="">Seleccionar área de conocimiento</asp:ListItem>
                        </asp:DropDownList>
                        <div class="help-block with-errors"></div>
                    </div>
                </div>
            </div>
            <div class="col-xs-offset-5 col-md-offset-5" style="padding: 5px 0 0 0">
                <div class="opciones_formulario">
                    <asp:Button runat="server" ID="btnBuscar" CssClass="btn-green" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="col-md-12" style="padding: 20px 0px 0px 0px">
        <div class="col-xs-12">
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="VincularCarreras.aspx" id="lnkVincularCarrera" class="btn-green"><span class=" ui-icon ui-icon-circle-plus"
                    style="display: inline-block; vertical-align: middle; margin-top: 0px;"></span>
                    <label class="label-helvetica" style="cursor: pointer;">Vincular carreras</label>
                </a>
            </div>
        </div>
        <div class="col-xs-12 container_busqueda_general ui-widget-content">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="col-xs-12 form-group center-block">
                        <div class="table-responsive">
                            <asp:GridView runat="server" ID="grdCarreras" CssClass="table table-bordered"
                                AutoGenerateColumns="False" PageSize="10" AllowPaging="True"
                                EnableSortingAndPagingCallbacks="True" AllowSorting="False" OnRowCommand="grdCarreras_RowCommand"
                                Visible="false" OnRowDataBound="grdCarreras_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NombreCarrera" HeaderText="Nombre de la carrera" SortExpression="Nombre" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripci&oacute;n de la carrera" SortExpression="Descripci&oacute;n" />
                                    <asp:BoundField DataField="Clasificador.Nombre" HeaderText="&Aacute;rea de conocimiento" SortExpression="&Aacute;rea" />
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="btnEdit" CommandName="editar" ImageUrl="../images/VOCAREER_editar.png"
                                                ToolTip="Editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CarreraID") %>'
                                                Visible="false" />
                                            <asp:ImageButton runat="server" ID="btnDel" CommandName="eliminar" ImageUrl="../images/VOCAREER_suprimir.png"
                                                ToolTip="Eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"CarreraID")  %>'
                                                OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                                Visible="false" />
                                        </ItemTemplate>
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
                                <HeaderStyle CssClass="th tabla_titulo_marco_general" />
                                <RowStyle CssClass="ui-widget-content td" />
                                <PagerTemplate>
                                    <asp:GridViewPager ID="grdViewPager" runat="server" DataSourceType="List<Carrera>" SessionName="LISTA_CARRERAS" />
                                </PagerTemplate>
                            </asp:GridView>
                        </div>
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

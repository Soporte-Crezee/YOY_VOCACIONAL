<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BuscarOrientadores.aspx.cs" Inherits="POV.Web.PortalUniversidad.Orientadores.BuscarOrientadores" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            Cat&aacute;logo de orientadores
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre" CssClass="col-sm-2 control-label" ToolTip="Nombre orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="80" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" ID="lblPrimerApellido" Text="Primer apellido" CssClass="col-sm-2 control-label" ToolTip="Primer apellido orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtPrimerApellido" TabIndex="2" MaxLength="50" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSegundoApellido" Text="Segundo apellido" CssClass="col-sm-2 control-label" ToolTip="Segundo apellido orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtSegundoApellido" TabIndex="3" CssClass="form-control" MaxLength="50"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" ID="lblCurp" Text="CURP" ToolTip="Curp orientador" CssClass="col-sm-2 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtCurp" TabIndex="4" MaxLength="500" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblSexo" Text="Sexo" CssClass="col-sm-2 control-label"></asp:Label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="CbxSexo" runat="server" TabIndex="5" CssClass="form-control">
                            <asp:ListItem Value="">Seleccionar</asp:ListItem>
                            <asp:ListItem Value="True">Hombre</asp:ListItem>
                            <asp:ListItem Value="False">Mujer</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Label runat="server" ID="lblCorreo" Text="Correo electrónico" CssClass="col-sm-2 control-label" ToolTip="Correo electrónico del orientador"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtCorreo" TextMode="Email" TabIndex="6" MaxLength="100" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-offset-2 col-md-offset-2 col-sm-offset-2 col-xs-1">
                        <asp:Button runat="server" ID="btnBuscar" CssClass="btn-green btn-form" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="" style="padding: 20px 0px 0px 0px">
        <div class="">
            <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                <a href="NuevoOrientador.aspx" id="lnkNuevoDocente" class="btn-green"><span class=" ui-icon ui-icon-circle-plus"
                    style="display: inline-block; vertical-align: middle; margin-top: -5px;"></span>
                    <label class="">Agregar nuevo orientador</label>
                </a>
            </div>
        </div>
        <div class="">
            <asp:UpdatePanel runat="server" ID="UpdDocenteEscuela">
                <ContentTemplate>
                    <div class="table-responsive">
                        <asp:GridView runat="server" ID="grdOrientadoresEscuela" CssClass="table table-bordered table-striped"
                            RowStyle-CssClass="td" HeaderStyle-CssClass="th"
                            AutoGenerateColumns="False" PageSize="10" AllowPaging="True"
                            EnableSortingAndPagingCallbacks="True" AllowSorting="False" OnRowCommand="grdOrientadoresEscuela_RowCommand"
                            Visible="false" OnRowDataBound="gv_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Curp" HeaderText="CURP" SortExpression="Curp" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs" />
                                <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre completo" SortExpression="NombreCompleto" />
                                <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha de nacimiento" SortExpression="FechaNacimiento" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs" />
                                <asp:BoundField DataField="Sexo" HeaderText="Sexo" SortExpression="Sexo" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs" />
                                <asp:BoundField DataField="Correo" HeaderText="Correo electrónico" SortExpression="Correo" HeaderStyle-CssClass="hidden-xs hidden-sm" ItemStyle-CssClass="hidden-sm hidden-xs" />
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="btnEdit" CommandName="editar" ImageUrl="../images/VOCAREER_editar.png"
                                            ToolTip="Editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"AsignacionDocenteEscuelaID") %>'
                                            Visible="false" />
                                        <asp:ImageButton runat="server" ID="btnDel" CommandName="eliminar" ImageUrl="../images/VOCAREER_suprimir.png"
                                            ToolTip="Eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"AsignacionDocenteEscuelaID")  %>'
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                            Visible="false" />
                                        <asp:ImageButton runat="server" ID="btnAdd" CommandName="asignar" ImageUrl="~/images/VOCAREER_agregar.png"
                                            ToolTip="Asignar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "DocenteID") %>'
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
                            <PagerTemplate>
                                <asp:GridViewPager ID="grdViewPager" runat="server" DataSourceType="DataSet" SessionName="Orientadoresescuela" />
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

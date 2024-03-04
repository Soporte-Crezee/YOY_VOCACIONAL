<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpedienteAlumnoUniversidad.aspx.cs" Inherits="POV.Web.PortalUniversidades.Pages.ExpedienteAlumnoUniversidad" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            DoFormBlockUI();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel panel-default" >
        <div class="panel-heading">
                Buscar expediente
        </div>
        <div class="panel-body">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblAreaConocimiento" Text="&Aacute;reas de conocimiento" class="col-sm-2 control-label" ToolTip="&Aacute;rea conocimiento"></asp:Label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlAreasConocimiento" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <asp:Label runat="server" ID="lblEscuela" Text="Escuela de procedencia" class="col-sm-2 control-label" ToolTip="Escuela procedencia"></asp:Label>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtEscuelaSearch" MaxLength="30" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblNivelEstudio" Text="Nivel de estudio" class="col-sm-2 control-label" ToolTip="Nivel estudio"></asp:Label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlSearchNivel" runat="server" CssClass="form-control">
                            <asp:ListItem Value="" Text="Seleccionar"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Semestre 1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Semestre 2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Semestre 3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Semestre 4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Semestre 5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Semestre 6"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Label runat="server" ID="lblEstado" Text="Estado" class="col-sm-2 control-label" ToolTip="Estado"></asp:Label>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlSearchEstado" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-offset-2 col-md-offset-2 col-sm-offset-2 col-xs-1">
                        <asp:Button ID="btnBuscarAlumno" runat="server" Text="Buscar" OnClick="btnBuscarAlumno_Click" CssClass="btn-green btn btn-form" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <!-- Grid de Alumnos -->
        <div class="panel-heading" id="div1" runat="server">
            Lista de estudiantes
        </div>
        <div class="panel-body">
            <asp:UpdatePanel runat="server" ID="UpdExpedienteAlumno">
                <ContentTemplate>
                    <!-- Grid Alumnos -->
                        <div class="table-responsive">
                            <asp:GridView AutoGenerateColumns="false" runat="server" ID="gvAlumnos"
                                CssClass="table table-bordered table-striped"
                                RowStyle-CssClass=" td" HeaderStyle-CssClass="th"
                                OnRowCommand="gvAlumnos_RowCommand"
                                Visible="false" PageSize="10" AllowPaging="true" AllowSorting="false" OnRowDataBound="gvAlumnos_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="Nombre del estudiante" DataField="Nombre"></asp:BoundField>
                                    <asp:BoundField HeaderText="Escuela" DataField="Escuela"></asp:BoundField>
                                    <asp:BoundField HeaderText="Nivel de estudio" DataField="Grado"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnVerExpediente" ToolTip="Ver expediente" runat="server"
                                                ImageUrl="~/Images/btn_search.png" Width="25px" Height="25px" CommandName="VerExpediente"
                                                CommandArgument='<%# Eval("AlumnoID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="ui-state-highlight ui-corner-all">
                                        <p>
                                            <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                            No se encontraron resultados
                                        </p>

                                    </div>
                                </EmptyDataTemplate>
                                <HeaderStyle CssClass="th" />
                                <RowStyle CssClass="td" />
                                <PagerTemplate>
                                    <asp:GridViewPager ID="grdViewPager" runat="server" DataSourceType="List<InfoAlumnoUsuario>" SessionName="ListAlumno" />
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
        function CerrarDialogo() {
            $('#MainContent_hdnDialogResultado').val('');
        }
    </script>
</asp:Content>

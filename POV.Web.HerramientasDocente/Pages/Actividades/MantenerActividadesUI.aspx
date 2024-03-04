<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="MantenerActividadesUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.MantenerActividadesDocenteUI" %>

<%-- Mantener Actividades --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='<%=Page.ResolveClientUrl("~/Content/Styles/gridview.css")%>' rel="stylesheet" />
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";

            if (confirm("¿Está seguro que desea asignar ésta actividad al área de conocimiento seleccionada?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function ConfirmReasignacion() {
            if (confirm("La actividad seleccionada ya se encuentra asignada ¿Está seguro que desea asignarla de nuevo?")) {
                window.location = "AsignarActividadGrupoUI.aspx";
            } else {

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-xs-12">
        <div class="titulo_marco">
            <asp:Label runat="server" ID="lblSubtitulo1" Text="Búsqueda de actividades"></asp:Label>
        </div>
        <div class="ui-widget-content">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                    <div class="form-group">
                        <label>Nombre</label>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtNombreActividad" MaxLength="30"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                    <div class="form-group">
                        <label>Instrucciones para el estudiante</label>
                        <asp:TextBox CssClass="form-control" runat="server" ID="txtDescripcionActividad" TextMode="MultiLine" MaxLength="30" Width="100%" Rows="3"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                    <div class="form-group">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_OnClick"
                            CssClass="button_clip_39215E pull-right" />
                    </div>
                </div>
            </div>
            <div class="clearfix"></div>
            <br />
        </div>
    </div>

    <div class="col-xs-12">
        <div id="PnlCreate">
            <a href="CrearActividadUI.aspx" id="lnkRegistrar" style="height: 40px;">
                <asp:Label ID="lblAgregarNuevo" CssClass="button_clip_39215E" runat="server" Text="Agregar actividad"></asp:Label>
            </a>
        </div>
        <div class="titulo_marco">
            <asp:Label runat="server" ID="lblResultados" Text="Resultados de la búsqueda"></asp:Label>
        </div>
        <div class="col-md-12 table-responsive">
            <asp:GridView ID="gvActividades" runat="server"
                AllowPaging="true" PageSize="15"
                CssClass="table table-bordered table-striped" HeaderStyle-Height="30px" RowStyle-Height="25px"
                RowStyle-CssClass="td" HeaderStyle-CssClass="th"
                AllowSorting="True" AutoGenerateColumns="False"
                Width="100%"
                SortedAscendingHeaderStyle-VerticalAlign="Top"
                OnPageIndexChanging="gvActividades_PageIndexChanging"
                OnRowDataBound="gvActividades_OnRowDataBound"
                OnRowCommand="gvActividades_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="Nombre" DataField="Nombre">
                        <HeaderStyle Width="260px" HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Instrucciones para el estudiante" DataField="Descripcion">
                        <HeaderStyle Width="360px" HorizontalAlign="Left" CssClass="hidden-sm hidden-xs" />
                        <ItemStyle HorizontalAlign="Left" CssClass="hidden-sm hidden-xs" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Área de conocimiento" DataField="Clasificador.Nombre">
                        <HeaderStyle Width="340px" HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Asignar a todos" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnAsignar" runat="server" CommandName="asignar_actividad"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActividadID") %>'
                                ImageUrl="~/Content/images/VOCAREER_page_gear.png" OnClientClick="Confirm()"
                                ToolTip="Asignar actividad a grupos" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEditar" runat="server" CommandName="editar_actividad"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActividadID") %>'
                                ImageUrl="~/Content/images/VOCAREER_editar.png"
                                ToolTip="Editar actividad" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEliminar" runat="server" CommandName="eliminar_actividad"
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ActividadID") %>'
                                ImageUrl="~/Content/images/VOCAREER_eliminar.png" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                ToolTip="Eliminar actividad" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div>
                        <span class="ui-icon ui-icon-notice"
                            style="float: left; margin: 0 7px 50px 0;"></span>No existen resultados para la consulta proporcionada.
                    </div>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="th"></HeaderStyle>

                <RowStyle CssClass="tr"></RowStyle>

                <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
            </asp:GridView>
        </div>

    </div>
</asp:Content>

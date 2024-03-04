<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="MantenerAsignacionesUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.MantenerAsignacionesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='<%=Page.ResolveClientUrl("~/Content/Styles/gridview.css")%>' rel="stylesheet" />
    <script type="text/javascript">

        $(function () {


            $("#chkTodasAsignaciones").change(function () {
                var tiene = $("#chkTodasAsignaciones").is(':checked');

                var grd = $('#<%=gvAsignaciones.ClientID%>');
                var trs = $(grd).find("tr:not(:has(table, th))");
                $.map(trs, function (value, index) {
                    var row = $(value);
                    var td1 = $("td", row).eq(0);
                    var chk = td1.find('input:checkbox');

                    if (tiene) {
                        chk.prop("checked", true);
                    } else {
                        chk.prop("checked", false);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titulo_marco">
        <asp:Label runat="server" ID="lblSubtitulo1" Text="Búsqueda de asignaciones"></asp:Label>
    </div>
    <div class="ui-widget-content">
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
            <div class="form-group">
                <label>Estudiante</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="txtNombreAlumno" MaxLength="200" ToolTip="Nombre completo del alumno"></asp:TextBox>
            </div>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
            <div class="form-group">
                <label>Actividad</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="txtNombreActividad" MaxLength="30" ToolTip="Nombre de la actividad"></asp:TextBox>
            </div>
        </div>
        <div class="clearfix"></div>
        <div style="text-align: right">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                CssClass="button_clip_39215E" />

        </div>
        <br />
    </div>

    <div style="margin-bottom: 20px">
        <div class="titulo_marco">
            <asp:Label runat="server" ID="lblResultados" Text="Resultados de la búsqueda"></asp:Label>
        </div>
        <asp:Button ID="btnEliminarTodos" runat="server" Text="Eliminar seleccionados"
            OnClientClick="return confirm('¿Está seguro que desea eliminar los elementos seleccionados?');" OnClick="btnEliminarTodos_Click"
            CssClass="btn-cancel" Style="margin: 10px 10px" />
        <br />
        <asp:GridView ID="gvAsignaciones" runat="server"
            AllowPaging="true" PageSize="25"
            CssClass="table table-striped table-bordered"
            AllowSorting="True" AutoGenerateColumns="False"
            Width="100%"
            SortedAscendingHeaderStyle-VerticalAlign="Top"
            OnPageIndexChanging="gvAsignaciones_PageIndexChanging"
            OnRowDataBound="gvAsignaciones_RowDataBound"
            OnRowCommand="gvAsignaciones_RowCommand">
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="text-center" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <input type="checkbox" id="chkTodasAsignaciones" class="chk-ctrl-todos" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSeleccionado" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actividad">
                    <ItemTemplate>
                        <asp:Label ID="lblActividad" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estudiante">
                    <ItemTemplate>
                        <asp:Label ID="lblAlumno" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&Aacute;rea de conocimiento" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs">
                    <ItemTemplate>
                        <asp:Label ID="lblArea" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnAsignacionId" runat="server" />
                        <asp:ImageButton ID="btnEliminar" runat="server" CommandName="eliminar_asignacion"
                            CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AsignacionActividadId") %>'
                            OnClientClick="return confirm('¿Está seguro que desea eliminar esta asignación?');"
                            ImageUrl="~/Content/images/VOCAREER_eliminar.png"
                            ToolTip="Eliminar" />
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
</asp:Content>

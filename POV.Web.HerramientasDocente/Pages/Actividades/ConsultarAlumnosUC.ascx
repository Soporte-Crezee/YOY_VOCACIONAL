<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultarAlumnosUC.ascx.cs" Inherits="POV.Web.Pages.Actividades.ConsultarAlumnosUC" %>

<script type="text/javascript">
    $(document).ready(
        function () {

        });
    function cerrar() {
        $(this).dialog("close");
    }

    function AbrirConfirmacion() {
        $("#dialog-detalleActividad").click();
    }

    function Ocultar() {
        var elemento = document.getElementById("btnColapse");
        if ((elemento).className == "btnUpAccodion") {
            $(elemento).removeClass("btnUpAccodion");
            $(elemento).addClass("btnDownAccordion");
        } else {
            $(elemento).removeClass("btnDownAccordion");
            $(elemento).addClass("btnUpAccodion");
        }
    }

    function seleccionarTodos() {

        var valorCheck = $("#chkbox-seleccionar-todos").is(':checked');
        $(".chk-ctrl-alumnos").each(function () {
            $(this).children("input[type='checkbox']").prop('checked', valorCheck);
        });
    }
</script>

<div class="titulo_marco">
    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblEncabezadoFiltro" runat="server" Text="Buscar estudiante" CssClass="TituloTalentos" Font-Bold="True"></asp:Label>
    <br />
</div>
<div class="ui-widget-content">
    <div class="col-lg-12">
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
            <div class="form-group">
                <label>&Aacute;rea de conocimiento</label>
                <asp:DropDownList CssClass="form-control" ID="ddlAreaConocimiento" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
            <div class="form-group">
                <label>Nombre del estudiante</label>
                <asp:TextBox ID="txtNombres" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="form-group">
                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="pull-right boton labelButton" OnClick="btnConsultar_Click" />
            </div>
        </div>
        <div class="clearfix"></div>
    </div>
    <div class="clearfix"></div>
</div>
<br />
<div class="titulo_marco">
    <asp:Label ID="lblEncabezadoGrid" runat="server" Text="Resultado de la búsqueda" CssClass="TituloTalentos" Font-Bold="True"></asp:Label>
</div>
<div class="ui-widget-content" style="margin-bottom: 20px;">
    <div class="col-lg-12" style="background-color: #f3f3f4;">
        <asp:Button ID="btnAsignarActividades" runat="server" Text="Asignar" OnClick="btnAsignarActividades_Click" CssClass="boton labelButton pull-right" />
    </div>
    <div class="clearfix"></div>
    <div class="" style="margin-top: 10px">
        <asp:GridView ID="gvAlumnosEnfasis" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped" RowStyle-CssClass="tr"
            HeaderStyle-CssClass="th" OnPageIndexChanging="gvAlumnosEnfasis_PageIndexChanging"
            OnPageIndexChanged="gvAlumnosEnfasis_PageIndexChanged" OnRowCommand="gvAlumnosEnfasis_RowCommand"
            OnRowDataBound="gvAlumnosEnfasis_RowDataBound" DataKeyNames="AlumnoID"
            SortedAscendingHeaderStyle-VerticalAlign="Top" BorderStyle="none" PageSize="35">
            <Columns>
                <asp:BoundField HeaderText="Id" DataField="AlumnoID" Visible="False">
                    <HeaderStyle Width="220px" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Estudiante" DataField="NombreCompletoAlumno">
                    <HeaderStyle Width="200px" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Actividades asignadas">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtDetalle" runat="server" ImageUrl="~/Content/Images/VOCAREER_buscar.png" ToolTip="Ver actividades" CommandName="Detalle"
                            CommandArgument='<%# Eval("AlumnoID")%>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="th" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="16px" Height="16px" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input id="chkbox-seleccionar-todos" onclick="seleccionarTodos();" type="checkbox" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" CssClass="chk-ctrl-alumnos" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="th text-center" BorderStyle="none" />
                    <ItemStyle CssClass="GridRowBorder" HorizontalAlign="Center"
                        VerticalAlign="Middle" Width="16px" Height="16px" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div>
                    <span class="ui-icon ui-icon-notice" style="float: left; margin: 0 7px 50px 0;"></span>No se encontraron coincidencias para los criterios de búsqueda proporcionados
                </div>
            </EmptyDataTemplate>
            <HeaderStyle CssClass="th"></HeaderStyle>

            <RowStyle CssClass="tr"></RowStyle>

            <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
        </asp:GridView>
    </div>
</div>

<button id="dialog-detalleActividad" type="button" class="btn btn-default" data-toggle="modal" data-target=".bs-example-modal-lg" style="display: none;">Large modal</button>
<div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myActividadModal" aria-hidden="true" data-keyboard="false" data-backdrop="static">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header color-info panel-heading">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                Aviso
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="container">
                        <div class="form-group">
                            <div class="col-xs-1 text-right">
                                <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Content/images/VOCAREER_buscar.png" />
                            </div>
                            <div class="col-xs-10">
                                <label class="labelTalentos">Actividades asignadas</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-12" style="overflow-y: scroll; height: 300px">
                                <asp:ListView ID="ltvActividades" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" runat="server">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </GroupTemplate>
                                    <ItemTemplate>
                                        <div id="head" onclick="Ocultar();" class="col-xs-12">
                                            <div class="form-group">
                                                <label class="col-xs-12 col-lg-3" style="font-family: Helvetica">Nombre de la actividad: </label>
                                                <label class="col-xs-12 col-lg-9" style="font-family: Helvetica"><%# Eval("Actividad.Nombre") %> </label>

                                                <div id="btnColapse" class="btnUpAccodion"></div>

                                                <asp:GridView BorderStyle="None" GridLines="None" Width="100%" ID="gvTareas" AutoGenerateColumns="False" runat="server"
                                                    DataSource='<%# Eval("Actividad.Tareas") %>' CssClass="table table-responsive table-striped table-bordered">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Tarea asignada" ItemStyle-HorizontalAlign="Left" DataField="Nombre" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div style="background-color: lightgray"></div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                            <div class="form-group" style="display: none">
                                                <label class="col-xs-12 col-lg-3" style="font-family: Helvetica">Fecha inicio:</label>
                                                <label class="col-xs-12 col-lg-9" style="font-family: Helvetica">
                                                    <%# Eval("FechaInicio", "{0:d}") %>
                                                </labe>
                                            </div>
                                            <div class="form-group" style="display: none;">
                                                <label class="col-xs-12" style="font-family: Helvetica">Fecha fin:</label>
                                                <label class="col-xs-12" style="font-family: Helvetica"><%# Eval("FechaFin", "{0:d}") %> </label>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <br />
                                        <label style="font-family: Helvetica">El alumno no tiene actividades asignadas</label><br />
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                <br />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="button_clip_39215E" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

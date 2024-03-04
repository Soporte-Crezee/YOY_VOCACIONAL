<%@ Page Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="AsignarActividadesUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.AsignarActividadesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {

            var dtpOptions = {
                changeYear: true,
                changeMonth: true,
                dateFormat: 'DD, d MM , yy',
                onSelect: function (date, instance) {
                    //se decodifica los html entities para las palabras acentuadas
                    var decoded = $('<div/>').html($(this).val()).text();
                    $(this).val(decoded);
                    var selectedDay = instance.selectedDay.toString().length == 1 ? "0" + instance.selectedDay : instance.selectedDay;
                    var selectedMonth = instance.selectedMonth.toString().length == 1 ? "0" + (instance.selectedMonth + 1) : (instance.selectedMonth + 1);
                    //se cambia el formato de la fecha 
                    var selectedDate = selectedDay + "/" + selectedMonth + "/" + instance.selectedYear;
                    $(this).next("input").val(selectedDate);
                }
            };

            cargarAcordeon();
            ocultarIconos();
        }

        function cargarAcordeon() {
            $(function () {
                $("#divAccordion").accordion({
                    collapsible: true,
                    active: true
                });
            });
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
        };

        function ocultarIconos() {
            $(function () {
                var icons = {
                    header: "ui-icon-circle-arrow-e",
                    activeHeader: "ui-icon-circle-arrow-s"
                };
                $("#divAccordion").accordion({
                    icons: icons
                });
                $("#divAccordion").accordion("option", "icons", null);
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <div class="divPrincipal">
            <br />
            <div class="titulo_principal_superior">
                &nbsp;<asp:Label ID="lblTitulo" runat="server" Text="Asignar actividades" CssClass="TituloTalentos" Font-Bold="True"></asp:Label>
            </div>
            <br />
            <div class="divContenido col-xs-12">
                <div class="col-xs-12 col-lg-6">
                    <%--Alumnos Seleccionados --%>
                    <div class="form-group ">
                        <div class="titulo_marco col-xs-12">
                            <asp:Label ID="lblSubTitulo1" runat="server" Text="Estudiantes seleccionados" CssClass="TituloTalentos col-xs-12 col-lg-6" Font-Bold="True"></asp:Label>
                            <div class="col-xs-12 col-lg-6" style="padding: 0px 0px 2px 0px;">
                                <asp:Button ID="btnAgregarMas" runat="server" Text="Nueva selección" OnClick="btnAgregarMas_OnClick" CssClass="boton btn-cancel" />
                            </div>
                        </div>
                        <div style="background-color: #f3f3f4;">
                            <asp:ListView ID="ltvGrado" ItemPlaceholderID="itemPlaceHolder" GroupPlaceholderID="groupPlaceHolder" runat="server">
                                <LayoutTemplate>
                                    <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <asp:GridView Width="100%" ID="gvAlumnos" AutoGenerateColumns="False" CssClass="SCGridView" runat="server" DataSource='<%# Eval("Alumnos") %>'
                                        RowStyle-CssClass="tr" HeaderStyle-CssClass="th" BorderStyle="None" RowStyle-BackColor="White">
                                        <Columns>
                                            <asp:BoundField HeaderText="Estudiante" DataField="Nombre" Visible="True">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="background-color: lightgray"></div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-6">
                    <%--Elección de Actividades --%>
                    <div class="form-group ">
                        <div class="titulo_marco">
                            <asp:Label ID="lblSubTitulo2" runat="server" Text="Elección de actividades" CssClass="TituloTalentos" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="background-color: #f3f3f4;" class="table table-responsive">
                            <asp:GridView ID="grvActividades" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" RowStyle-CssClass="tr" runat="server"
                                HeaderStyle-CssClass="th" OnPageIndexChanging="grvActividades_OnPageIndexChanging" OnPageIndexChanged="grvActividades_OnPageIndexChanged" OnRowCommand="grvActividades_OnRowCommand"
                                OnRowDataBound="grvActividades_OnRowDataBound" DataKeyNames="ActividadId" SortedAscendingHeaderStyle-VerticalAlign="Top"
                                BorderStyle="None" PageSize="10">
                                <Columns>
                                    <asp:BoundField HeaderText="Id" DataField="ActividadId" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Contenido">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtDetalle" runat="server" ImageUrl="~/Content/images/VOCAREER_buscar.png" ToolTip="Ver contenido"
                                                CommandName="Detalle" CommandArgument='<%#Eval("ActividadId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="th" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" Height="16px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Selección">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="th" BorderStyle="none" />
                                        <ItemStyle CssClass="GridRowBorder" HorizontalAlign="Center"
                                            VerticalAlign="Middle" Width="20px" Height="16px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="background-color: #f3f3f4;">No se encontraron actividades para los alumnos</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <br />
                        </div>
                    </div>
                    <div class="form-group">
                        <%--Detalle de la Actividad --%>
                        <div class="titulo_marco">
                            <asp:Label ID="lblSubTitulo3" runat="server" Text="Detalle de actividades" CssClass="TituloTalentos" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="background-color: #f3f3f4;" class="table table-responsive">
                            <asp:GridView ID="grvDetalleActividad" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" RowStyle-CssClass="tr" runat="server"
                                RowStyle-BackColor="White"
                                HeaderStyle-CssClass="th" OnPageIndexChanging="grvDetalleActividad_OnPageIndexChanging" OnPageIndexChanged="grvDetalleActividad_OnPageIndexChanged"
                                DataKeyNames="TareaId" SortedAscendingHeaderStyle-VerticalAlign="Top" PageSize="10">
                                <Columns>
                                    <asp:BoundField HeaderText="Id" DataField="TareaId" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Nombre" DataField="Nombre" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Instrucción" DataField="Instruccion" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Tipo tarea" DataField="Tipo" Visible="True" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="background-color: #f3f3f4;">No se ha seleccionado una actividad para ver su detalle</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <br />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="text-center">
                            <asp:Button ID="btnCancelar" runat="server" Text="Deseleccionar" OnClick="btnCancelar_OnClick" CssClass="btn-cancel" />
                            <asp:Button ID="btnAsignar" runat="server" Text="Asignar" OnClick="btnAsignar_OnClick" CssClass="button_clip_39215E" />
                        </div>
                        </>
                    <div class="form-group">
                        <%--Detalle de la Actividad --%>
                        <div class="titulo_marco">
                            <asp:Label ID="lblSubtitulo5" runat="server" Text="Actividades asignadas" CssClass="TituloTalentos" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="background-color: #f3f3f4;" class="table table-responsive">
                            <asp:GridView ID="grvPreAsignaciones" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" RowStyle-CssClass="tr" runat="server"
                                RowStyle-BackColor="White"
                                HeaderStyle-CssClass="th" OnPageIndexChanging="grvPreAsignaciones_OnPageIndexChanging" OnPageIndexChanged="grvPreAsignaciones_OnPageIndexChanged" OnRowCommand="grvPreAsignaciones_OnRowCommand"
                                DataKeyNames="AsignacionActividadId" SortedAscendingHeaderStyle-VerticalAlign="Top"
                                BorderStyle="None" PageSize="10">
                                <Columns>
                                    <asp:BoundField HeaderText="Id" DataField="AsignacionActividadId" Visible="False">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Estudiante">
                                        <ItemTemplate>
                                            <%# string.Format("{0} {1} {2}", Eval("Alumno.Nombre"), Eval("Alumno.PrimerApellido"), Eval("Alumno.SegundoApellido")) %>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="th" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Nombre actividad" DataField="ACtividad.Nombre" Visible="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Contenido">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtEliminar" runat="server" ImageUrl="~/Content/images/VOCAREER_eliminar.png" ToolTip="Eliminar"
                                                CommandName="Eliminar" CommandArgument='<%#Eval("AsignacionActividadId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="th" />
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="16px" Height="16px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div style="background-color: #f3f3f4;">No se han asignado actividades </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <br />
                        </div>
                    </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

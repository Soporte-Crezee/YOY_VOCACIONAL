<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="AsignarActividadGrupoUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.AsignarActividadGrupoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='<%=Page.ResolveClientUrl("~/Content/Styles/gridview.css")%>' rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {

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

            $(".datepicker-padre").datepicker(dtpOptions).keydown(function () { return false; });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="divPrincipal">
        <br />
        <div class="titulo_principal_superior">
            <asp:Label ID="lblTitulo" Text="Asignar actividad a grupos" runat="server"></asp:Label>
        </div>
        <div>
            <div>
                <div class="titulo_marco">
                    <asp:Label runat="server" ID="lblSubtitulo1" Text="Información general"></asp:Label>
                </div>
                <div class="ui-widget-content">
                    <table class="formulario_talentos_linea">
                        <tr class="form_group">
                            <td class="form_label">Nombre
                            </td>
                            <td class="form_control">
                                <asp:TextBox runat="server" ID="txtNombreActividad" ReadOnly="True" Enabled="false" MaxLength="100"></asp:TextBox>
                            </td>
                            <td class="form_label" style="width: 150px;"></td>
                            <td class="form_control"></td>
                        </tr>
                        <tr class="form_group">
                            <td class="form_label">Instrucciones para el alumno</td>
                            <td colspan="3">
                                <asp:TextBox runat="server" ID="txtDescripcionActividad" ReadOnly="True" Enabled="false" TextMode="MultiLine" MaxLength="500" Width="100%" Rows="3"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div style="display: none;">
            <div class="titulo_principal_superior">
                <asp:Label ID="Label1" Text="Grupos del docente" runat="server"></asp:Label>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvGrupos" runat="server" Width="100%" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
                    RowStyle-CssClass="tr" HeaderStyle-CssClass="th" BorderStyle="none" OnRowDataBound="gvGrupos_RowDataBound"
                    DataKeyNames="GrupoCicloEscolarID">
                    <Columns>
                        <asp:BoundField HeaderText="Id" DataField="GrupoCicloEscolarID" Visible="False">
                            <HeaderStyle Width="220px" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" Checked="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="th" BorderStyle="none" />
                            <ItemStyle CssClass="GridRowBorder" HorizontalAlign="Center"
                                VerticalAlign="Middle" Width="16px" Height="16px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grado">
                            <ItemTemplate>
                                <%#Eval("Grupo.Grado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grupo">
                            <ItemTemplate>
                                <%#Eval("Grupo.Nombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Inicio">
                            <ItemTemplate>
                                <asp:TextBox CssClass="ctr-input datepicker-padre" ID="txtFechaInicio" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdFechaInicio" runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Fin">
                            <ItemTemplate>
                                <asp:TextBox CssClass="ctr-input datepicker-padre" ID="txtFechaFin" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdFechaFin" runat="server"></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div>
                            <span class="ui-icon ui-icon-notice" style="float: left; margin: 0 7px 50px 0;"></span>No se encontraron grupos asignados
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <br />
        <div style="text-align: right">
            <asp:Button ID="btnCrearActividad" runat="server" Text="Guardar" OnClick="btnAsignarActividad_OnClick"
                CssClass="button_clip_39215E" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_OnClick" CssClass="button_clip_39215E" />
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnControlActividadId" />
    <asp:HiddenField runat="server" ID="hdnDialogResultado" />
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server" style="display: none" />
    <script type="text/javascript">

        $(document).ready(function () {

        });
        function CerrarDialogo() {
            $('#MainContent_hdnDialogResultado').val('');
        }
    </script>
</asp:Content>

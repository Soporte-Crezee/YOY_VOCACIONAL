<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgresoActividadesUC.ascx.cs" Inherits="POV.Web.Pages.Actividades.ProgresoActividadUC" %>
<%@ Register Src="~/Pages/Actividades/ProgresoTareasUC.ascx" TagPrefix="uc" TagName="ProgresoTareasUC" %>

<%--Visualizar Resultados Actividad--%>
<div class="divPrincipal">
    <div class="titulo-popup">
        <asp:Label ID="lblEncabezadoGrid" runat="server" Text="Avance Actividades"></asp:Label>
    </div>
    <div style="margin: 5px 4px 2px 4px ;">
        <label>Filtrar por fechas : </label>
        <asp:TextBox ID="txtFechaInicio" runat="server" Width="100px" tabindex="-1"></asp:TextBox> a
        <asp:TextBox ID="txtFechaFin" runat="server" Width="100px" tabindex="-1"></asp:TextBox>
        <asp:Button ID="btnBuscarActividades" runat="server" Text="Buscar" CssClass="button_clip_39215E" OnClick="btnBuscarActividades_OnClick"/>
    </div>
    <div>
        <asp:Label ID="lblErrorBusqueda" runat="server" CssClass="error"></asp:Label>
    </div>
    <div class="divFiltradoCont">

        <asp:GridView ID="gvActividades" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False"
            CssClass="DDGridView" BorderWidth="1" BorderStyle="Solid"
            HeaderStyle-Height="30px" RowStyle-Height="25px"
            RowStyle-CssClass="td" HeaderStyle-CssClass="th"
            BorderColor="#B2A4C4" HeaderStyle-BackColor="#cccccc" RowStyle-BackColor="Transparent"
            AlternatingRowStyle-BackColor="White"
            DataKeyNames="AsignacionActividadId"
            SortedAscendingHeaderStyle-VerticalAlign="Top" OnRowCommand="gvActividades_RowCommand" OnPageIndexChanging="gvActividades_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Id" DataField="AsignacionActividadId" Visible="False">
                    <HeaderStyle Width="220px" HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Nombre" DataField="Nombre">
                    <HeaderStyle Width="100px" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Inicio Vigencia" DataField="InicioVigencia">
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fin Vigencia" DataField="FinVigencia">
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Porcentaje Avance" DataField="PorcentajeAvance">
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtDetalle" runat="server" ImageUrl="~/Content/images/ico.buscar.png" ToolTip="Ver Tareas" CommandName="Detalle"
                            CommandArgument='<%# Eval("AsignacionActividadId")%>' />

                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="16px" Height="16px" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div>
                    <span class="ui-icon ui-icon-notice" style="float: left; margin: 0 7px 50px 0;"></span>Sin Actividades Asignadas
                </div>
            </EmptyDataTemplate>
            <HeaderStyle CssClass="th"></HeaderStyle>
            <RowStyle CssClass="tr"></RowStyle>
            <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
        </asp:GridView>

    </div>
</div>
<asp:HiddenField runat="server" ID="hdnDialogo" />

<div id="ProgresoTareasDialog" style="display: none;">
    <uc:ProgresoTareasUC runat="server" ID="UCProgresoTareas" />

</div>


<script type="text/javascript">

    $(document).ready(function () {
        $("#<%= txtFechaInicio.ClientID%>").datepicker({ changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy' });
        $("#<%= txtFechaFin.ClientID%>").datepicker({ changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy' });
        var showDialogTareas = $('#<%= hdnDialogo.ClientID %>').val();
	    if (showDialogTareas == "1") {
	        $('#AsignacionesDialog').dialog("close");
	        $('#ProgresoTareasDialog').dialog({
	            closeOnEscape: true,
	            resizable: false,
	            title: "",
	            width: 650,
	            modal: true,
	            close: function (event, ui) {
	                $('#<%= hdnDialogo.ClientID %>').val("");
				}
			}).parent().appendTo("form");

            }
	});
</script>

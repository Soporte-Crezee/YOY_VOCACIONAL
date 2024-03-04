<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgresoTareasUC.ascx.cs" Inherits="POV.Web.Pages.Actividades.ProgresoTareasUC" %>
<%-- Visualizar Resultados Actividad--%>

<div class="divPrincipal">
	<div class="titulo-popup">
		<asp:Label ID="lblTitulo" runat="server" Text="Titulo"></asp:Label><br />
		<strong >
	<asp:Label runat="server" ID="lblVigenciaInicio"></asp:Label>
	-
	<asp:Label runat="server" ID="lblVIgenciaFin"></asp:Label></strong>
	</div>
	<div class="divFiltradoCont">

		<asp:GridView ID="gvTareas" runat="server" AllowPaging="True" AllowSorting="True"
			AutoGenerateColumns="False"
			CssClass="DDGridView" BorderWidth="1px" BorderStyle="Solid" HeaderStyle-Height="30px" RowStyle-Height="25px"
			RowStyle-CssClass="td" HeaderStyle-CssClass="th"
			BorderColor="#B2A4C4" HeaderStyle-BackColor="#cccccc" RowStyle-BackColor="Transparent"
			AlternatingRowStyle-BackColor="White" Width="97%"
			SortedAscendingHeaderStyle-VerticalAlign="Top" OnPageIndexChanging="gvTareas_PageIndexChanging" >
<AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
			<Columns>
				<asp:BoundField HeaderText="Nombre" DataField="Nombre">
					<HeaderStyle Width="100px" HorizontalAlign="Center" />
					<ItemStyle HorizontalAlign="Left" />
				</asp:BoundField>
				<asp:BoundField HeaderText="Tipo" DataField="Tipo">
					<HeaderStyle Width="50px" HorizontalAlign="Center" />
					<ItemStyle Width="50px" HorizontalAlign="Center" />
				</asp:BoundField>
				<asp:BoundField HeaderText="Estatus" DataField="Estatus">
					<HeaderStyle Width="50px" HorizontalAlign="Center" />
					<ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
				</asp:BoundField>
				<asp:BoundField HeaderText="Fecha Inicio" DataField="FechaInicio">
					<HeaderStyle Width="50px" HorizontalAlign="Center" />
					<ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
				</asp:BoundField>
				<asp:BoundField HeaderText="Fecha Fin" DataField="FechaFin">
					<HeaderStyle Width="50px" HorizontalAlign="Center" />
					<ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
				</asp:BoundField>
                <asp:BoundField HeaderText="Resultado Prueba" DataField="ResultadoPrueba">
                    <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
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

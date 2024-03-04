<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultarEjesTematicosUC.ascx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.ConsultarEjesTematicosUC" %>
<%--Satisface al Caso de Uso CU025 Consultar Ejes Temáticos--%>

<div class="divPrincipal">
    <div class="titulo_marco">
        <asp:Label runat="server" ID="lblEncabezadoGrid" Text="Resultado de búsqueda" ></asp:Label>
    </div>
    <div class="divFiltradoCont">
        <div style="width: 100%;  overFlow-y: scroll; height:500px">
            <asp:GridView runat="server" ID="gvEjesTematicos" AllowPaging="True" AllowSorting="True"
                 AutoGenerateColumns="False" DataKeyNames="EjeTematicoId,ContenidoDigitalId,SituacionAprendizajeId,Clasificador"
                 CssClass="DDGridView" BorderWidth="1" BorderStyle="Solid" HeaderStyle-Height="30px" RowStyle-Height="25px"
                RowStyle-CssClass="td"  HeaderStyle-CssClass="th"
                BorderColor="#B2A4C4" HeaderStyle-BackColor="#cccccc" RowStyle-BackColor="Transparent"
                AlternatingRowStyle-BackColor="White"  Width="97%" PageSize="5"
                SortedAscendingHeaderStyle-VerticalAlign="Top" OnPageIndexChanged="gvEjesTematicos_OnPageIndexChanged" 
                OnPageIndexChanging="gvEjesTematicos_OnPageIndexChanging" OnRowDataBound="gvEjesTematicos_OnRowDataBound">
                
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="cbSeleccionado" ToolTip="Seleccionar"/>
                        </ItemTemplate>
                        <ItemStyle CssClass="GridRowBorder" HorizontalAlign="Center" VerticalAlign="Middle" Width="16px" Height="16px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Id" DataField="EjeTematicoId" Visible="False">
                        <HeaderStyle Width="20px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="ContenidoDigitalId" DataField="ContenidoDigitalId" Visible="False">
                        <HeaderStyle Width="20px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="SituacionAprendizajeId" DataField="SituacionAprendizajeId" Visible="False">
                        <HeaderStyle Width="20px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Asignatura" DataField="NombreArea">
                        <HeaderStyle Width="40px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                     <asp:BoundField HeaderText="Bloque" DataField="NombreMaterias">
                        <HeaderStyle Width="50px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                     <asp:BoundField HeaderText="Eje o ámbito" DataField="NombreEjeTematico">
                        <HeaderStyle Width="40px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Tema" DataField="NombreSituacion">
                        <HeaderStyle Width="50px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Contenido" DataField="Clasificador">
                        <HeaderStyle Width="50px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Competencias" DataField="Competencia">
                        <HeaderStyle Width="40px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Aprendizajes esperados" DataField="Aprendizaje">
                        <HeaderStyle Width="40px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Recurso didáctico" DataField="NombreContenido">
                        <HeaderStyle Width="40px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Tipo de archivo" DataField="NombreTipoDocumento">
                        <HeaderStyle Width="40px" HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <div>
                        <span class="ui-icon ui-icon-notice" style="float: left; margin: 0 7px 50px 0;"></span> No se encontraron coincidencias para los criterios de búsqueda proporcionados
                    </div>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="th"></HeaderStyle>
                <RowStyle CssClass="tr"></RowStyle>
                <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
            </asp:GridView>
        </div>
        <div style="text-align: right">
            <table style="width: 100%">
                <tr >
                    <td style="width:100px;vertical-align:text-top;"> 
                        <asp:Label ID="lblInstruccionesEjesTematicos" runat="server" Text="Instrucciones"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtInstruccionesEjes" runat="server" MaxLength="200"
                             TextMode="MultiLine" Width="100%" Rows="3"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblErrorAsignarEje" runat="server" CssClass="error"></asp:Label>
                        <asp:Button ID="btnAgregarEje" runat="server" Text="Agregar" OnClick="btnAgregarEje_OnClick"
                             CssClass="button_clip_39215E" />

                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
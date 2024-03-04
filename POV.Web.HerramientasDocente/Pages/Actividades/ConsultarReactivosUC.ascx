<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultarReactivosUC.ascx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.ConsultarReactivosUC" %>
<%--Satisface al Caso de Uso CU013 Consultar Reactivos--%>
<div class="divPrincipal" > 
    <div class="titulo_marco">
        <asp:Label ID="lblEncabezadoGrid" runat="server" Text="Resultado de búsqueda"></asp:Label>
    </div>
    <div class="divFiltradoCont">
        <div style="width: 100%; overflow-y: scroll;">
            <asp:GridView ID="gvReactivos" runat="server" AllowPaging="True" 
                 CssClass="DDGridView" BorderWidth="1" BorderStyle="Solid" HeaderStyle-Height="30px" RowStyle-Height="25px"
                RowStyle-CssClass="td"  HeaderStyle-CssClass="th"
                BorderColor="#B2A4C4" HeaderStyle-BackColor="#cccccc" RowStyle-BackColor="Transparent"
                AlternatingRowStyle-BackColor="White" 
                AllowSorting="True" AutoGenerateColumns="False" 
                DataKeyNames="ReactivoID" PageSize="5" Width="97%"
                SortedAscendingHeaderStyle-VerticalAlign="Top" 
                OnPageIndexChanged="gvReactivos_PageIndexChanged" 
                OnPageIndexChanging="gvReactivos_PageIndexChanging" OnRowDataBound="gvReactivos_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" />
                        </ItemTemplate>
                        <ItemStyle CssClass="GridRowBorder" HorizontalAlign="Center"
                            VerticalAlign="Middle" Width="16px" Height="16px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="ReactivoID" DataField="ReactivoID" Visible="False">
                        <HeaderStyle Width="40px" HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nombre" DataField="NombreReactivo">
                        <HeaderStyle Width="160px" HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Descripción" DataField="Descripcion">
                        <HeaderStyle Width="260px" HorizontalAlign="Left" />
                        <ItemStyle Width="260px" HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Estatus" DataField="Activo" >
                        <HeaderStyle Width="40px" HorizontalAlign="Left" />
                        <ItemStyle Width="40px" HorizontalAlign="Left" />
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
            </asp:GridView><br/>
        </div>
        <div style="text-align: right">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100px; vertical-align: text-top;">
                        <asp:Label ID="lblInstruccionesReactivo" runat="server" Text="Instrucciones"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtInstruccionesReactivos" runat="server" MaxLength="200"
                            TextMode="MultiLine" Width="100%" Rows="3"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblErrorAsignarReactivo" runat="server" CssClass="error"></asp:Label>
                        <asp:Button ID="btnAgregarReactivo" runat="server" Text="Agregar" OnClick="btnAgregarReactivo_Click"
                            CssClass="button_clip_39215E" />

                    </td>
                </tr>
            </table>
        </div>
     </div>
    
    </div>
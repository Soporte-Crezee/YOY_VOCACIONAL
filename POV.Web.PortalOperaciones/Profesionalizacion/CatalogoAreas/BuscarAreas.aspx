<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BuscarAreas.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAreas.BuscarAreas" %>
<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="<% =Page.ResolveClientUrl("~/Styles/gridview.css")%>" rel="stylesheet" type="text/css" />
     <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
     <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>

     <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();
            $("#frmMain").validate();
        }
    </script>
  <%--  <style type="text/css">
        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
        white-space: pre-line;
        }
        .wrap {
        white-space: nowrap;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
    Cat&aacute;logo asignaturas</h3>
<div class="finder ui-widget-content">
	<table class="finder">
		<tr>
            <td class="label">
                <asp:Label runat="server" ID="lblIdentificador" Text="ID" ToolTip="Identificador"></asp:Label>   </td>
            <td class="input">
	            <asp:TextBox runat="server" MaxLength="10" ID="txtIdentificador" TabIndex="1" ToolTip="Por favor, escribe un número entero válido." CssClass="number"></asp:TextBox>
            </td>
		</tr>
		<tr>
			<td class="label">
				<asp:Label runat="server" ID="lblNombre" Text="Nombre" ToolTip="Nombre"></asp:Label>
                </td>
			<td class="input">
				<asp:TextBox runat="server" ID="txtNombre" TabIndex="2" ToolTip="Nombre" MaxLength="100"></asp:TextBox>
			</td>
		</tr>
        <tr>
            <td class="label">
                <label>
                    Nivel educativo</label>
            </td>
            <td class="input">
                <asp:UpdatePanel ID="UpdNivelEducativo" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="cbNivelEducativo" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="cbNivelEducativo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="label">
                <label>
                    Grado</label>
            </td>
            <td class="input">
                <asp:UpdatePanel ID="UpdGrado" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="cbGradoAsignatura" runat="server">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="label"></td>
            <td>
                <div>
        <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" CssClass="boton" 
            onclick="BtnBuscar_Click" />
    </div>
            </td>
        </tr>
	</table>
    
</div>
<div class="results">
	<div id="PnlCreate" class="nuevo" runat="server" visible="True">
            <a href="RegistrarArea.aspx" id="lnkNuevaArea" class="boton">
                <span class=" ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                                                                                                                                                                                        margin-top: -5px;"></span>
                <asp:Label runat="server" Text="Agregar nueva asignatura" ID="lblNuevaArea"></asp:Label>
            </a>
        </div>
        <asp:UpdatePanel ID="updAreas" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdAreas" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="False" AllowPaging="True"
                    Width="100%" onrowcommand="grdAreas_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="AreaProfesionalizacionID" HeaderText="ID" SortExpression="AreaProfesionalizacionID" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ItemStyle-CssClass="break_text" >
                        <HeaderStyle />
                        <ItemStyle/>
                        </asp:BoundField>
                        <asp:BoundField DataField="NivelEducativo" HeaderText="Nivel Educativo" SortExpression="NivelEducativo" />
                        <asp:BoundField DataField="Grado" HeaderText="Grado" SortExpression="Grado" />

                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <%# Boolean.Parse(Eval("Activo").ToString())? "Activo": "Inactivo" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AreaProfesionalizacionID")%>'
                                    ImageUrl="~/Images/edit-button.png" ToolTip="Editar" Visible="True"/>
                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AreaProfesionalizacionID")%>'
                                    ImageUrl="~/Images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" 
                                    Visible="True"/>
                            </ItemTemplate>
                         <%--   <ControlStyle CssClass="wrap" />
                            <ItemStyle CssClass="wrap" />--%>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                resultados</p>
                        </div>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="th" />
                    <PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsAreaProfesionalizacion" DataSourceType="DataSet" />
                    </PagerTemplate>
                    <RowStyle CssClass="td" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
</div>
</asp:Content>

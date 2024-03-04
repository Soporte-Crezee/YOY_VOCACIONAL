<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurarSituacionesAprendizaje.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.ConfigurarSituacionesAprendizaje" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/gridview.css")%>" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {
            $(".boton").button();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:LinkButton runat="server" ID="lnkBack1" OnClick="lnkBack1_Click">Volver</asp:LinkButton>/Configurar
        temas
    </h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <h2>
            Informaci&oacute;n del eje o ámbito</h2>
            <div class="line"></div>
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblNivelEducativo" Text="Nivel educativo"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNivelEducativo" ReadOnly="True" Enabled="False"
                        TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblGrado" Text="Grado"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtGrado" ReadOnly="True" Enabled="False"
                        TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblAsignatura" Text="Asignatura"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAsignatura" ReadOnly="True" Enabled="False"
                        TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblBloque" Text="Bloque"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBloque" ReadOnly="True" Enabled="False"
                        TabIndex="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblEjeTematicoID" Text="ID"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEjeTematicoID" ReadOnly="True" Enabled="False"
                        TabIndex="5" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblEjeTematico" Text="Eje o ámbito"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEjeTematico" ReadOnly="True" Enabled="False" TabIndex="6"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div class="results">
            <div id="PnlCreate" class="nuevo" runat="server" visible="False">
                <a href="RegistrarSituacionAprendizaje.aspx" id="lnkNuevaSituacionAprendizaje" class="boton">
                    <span class=" ui-icon ui-icon-circle-plus" style="display: inline-block; vertical-align: middle;
                        margin-top: -5px;"></span>
                    <asp:Label runat="server" Text="Agregar nuevo tema" ID="lblNuevaSituacionAprendizaje"></asp:Label>
                </a>
            </div>
            <asp:UpdatePanel ID="updSituacionAprendizaje" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdSituacionAprendizaje" runat="server" CssClass="DDGridViewWrap"
                        RowStyle-CssClass="td" HeaderStyle-CssClass="th" AutoGenerateColumns="false"
                        PageSize="10" AllowPaging="true"
                        Visible="False" OnRowCommand="grdSituacionAprendizaje_RowCommand" OnRowDataBound="grdSituacionAprendizaje_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="SituacionAprendizajeID" HeaderText="ID" SortExpression="SituacionAprendizajeID">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ItemStyle-CssClass="break_text">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha registro" SortExpression="FechaRegistro"
                                DataFormatString="{0:dd/MM/yyyy}">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="90px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreEstatus" HeaderText="Estado" SortExpression="NombreEstatus">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Contenidos">
                                <ItemTemplate>
                                    <asp:Image ID="imgConsultarClasificadores" runat="server" ImageUrl="~/images/page_gear.png"
                                        Visible="False" />
                                    <asp:LinkButton runat="server" Visible="False" ID="lnkbtnConsultarClasificadores"
                                        Text="Consultar" CommandName="consultarclasificadores" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"SituacionAprendizajeID") %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CommandName="editar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "SituacionAprendizajeID")%>'
                                        ImageUrl="~/images/edit-button.png" ToolTip="Editar" Visible="False" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="eliminar" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "SituacionAprendizajeID")%>'
                                        ImageUrl="~/images/minus-button.png" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Visible="False" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle Width="20px" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                    resultados</p>
                            </div>
                        </EmptyDataTemplate>
                        <PagerTemplate>
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsSituacionesAprendizaje"
                                DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>

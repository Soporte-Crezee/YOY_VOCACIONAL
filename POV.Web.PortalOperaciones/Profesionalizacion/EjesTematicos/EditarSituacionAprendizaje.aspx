<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarSituacionAprendizaje.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.EjesTematicos.EditarSituacionAprendizaje" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.validate.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);
        function initPage() {

            $(".boton").button();

        }
        function ValidateFields() {
            $("#frmMain").validate();
        }

        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
    <style>
        
        table.DDGridView .th, table.DDGridView .td, table.DDListView .th, table.DDListView .td
        {
            white-space: pre-line;
        }
        .wrap
        {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server">Volver</asp:HyperLink>
        /Editar tema</h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <h2>
            Informaci&oacute;n del tema</h2>
        <div class="line">
        </div>
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
                    <asp:Label runat="server" ID="lblEjeTematicoId" Text="Eje o ámbito"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEjeTematico"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblSituacionID" Text="ID del tema"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSituacionID" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre del tema"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombre" CssClass="required" TextMode="MultiLine"
                        Columns="80" Width="500" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Estado</label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="DDLEstatusProfesionalizacion" />
                </td>
            </tr>
            <tr>
                <td class="td-label-texarea">
                    <asp:Label runat="server" ID="lblDescripcion" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" Columns="80" Width="500" Rows="5"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="Panel1" runat="server" GroupingText="Contenidos">
            <div>
                <asp:UpdatePanel ID="updClasificador" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" ID="lblClasificador" Text="Nombre"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtClasificador"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label" style="vertical-align: top">
                                    <asp:Label runat="server" ID="lblCompetencias" Text="Competencias"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtCompetencias" TextMode="MultiLine" Columns="80" Width="500" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label" style="vertical-align: top">
                                    <asp:Label runat="server" ID="lblAprendizajes" Text="Aprendizajes esperados"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtAprendizajes" TextMode="MultiLine" Columns="80" Width="500" Rows="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="td-label">
                                    <asp:Label runat="server" ID="lblPredeterminada" Text="Predeterminado"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEsDPredeterminado" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnAgregarClasificador" Text="Agregar contenido"
                                        CssClass="boton" OnClick="btnAgregarClasificador_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView runat="server" ID="grdClasificadores" CssClass="DDGridView" RowStyle-CssClass="td"
                            DataKeyNames="Orden" HeaderStyle-CssClass="th" AutoGenerateColumns="False" OnRowCancelingEdit="grdClasificadores_RowCancelingEdit"
                            OnRowEditing="grdClasificadores_RowEditing" OnRowUpdating="grdClasificadores_RowUpdating"
                            OnRowDeleting="grdClasificadores_Deleting" Width="1000px">
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" >
                                <ItemStyle Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Competencias" HeaderText="Competencias" >
                                <ItemStyle Width="250px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Aprendizajes" HeaderText="Aprendizajes Esperados" >
                                <ItemStyle Width="250px" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="EsPredeterminado" HeaderText="Es predeterminado" >
                                <HeaderStyle Width="100px" />
                                </asp:CheckBoxField>
                                <asp:CommandField ShowEditButton="True" HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center"/>
                                </asp:CommandField>
                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                            OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                            Text="Eliminar" Height="14px"></asp:LinkButton>&nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="ui-state-highlight ui-corner-all">
                                    <p>
                                        <span class="ui-icon ui-icon-info" style="float: left"></span>El tema
                                        no tiene contenidos asignados</p>
                                </div>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="th" />
                            <RowStyle CssClass="td" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <div class="line">
        </div>
        <table>
            <tr>
                <td colspan="2">
                    <div class="DDFloatRight">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click"
                            OnClientClick="ValidateFields()" />
                        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="~/Profesionalizacion/EjesTematicos/ConfigurarSituacionesAprendizaje.aspx"
                            CssClass="boton" OnClientClick="CancelValidate()"></asp:HyperLink>
                    </div>
                </td>
            </tr>
        </table>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

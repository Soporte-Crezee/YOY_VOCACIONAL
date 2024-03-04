<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="RegistrarArea.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAreas.RegistrarArea" %>

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
        function Validate() {
            $("#frmMain").validate(
				{
				    rules: {
				        '<%=txtNombre.UniqueID %>': { required: true, maxlength: 100 },
				        '<%=txtDescripcion.UniqueID %>': { required: false, maxlength: 500 },
				        '<%=txtNombreMateria.UniqueID %>': { required: false, maxlength: 50 },
				        '<%=cbNivelEducativo.UniqueID %>': { required: true },
				        '<%=cbGradoAsignatura.UniqueID %>': {required: true}
				    },
				    submitHandler: function (form) {
				        $('.main').block();
				        form.submit();
				    }
				});
        }
        function CancelValidate() {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/Profesionalizacion/CatalogoAreas/BuscarAreas.aspx">Volver</asp:HyperLink>/Registrar
        asignaturas
    </h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <%-- Empieza formulario AreaProfesionalizacion --%>
        <h2>
            Datos de la asignatura</h2>
        <div class="line">
        </div>
        <table>
            <tr>
                <td class="td-label">
                    <label>
                        <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" CssClass="required"
                        MaxLength="100" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label" style="vertical-align: top">
                    <label>
                        <asp:Label runat="server" ID="lblDescripcion" Text="Descripción"></asp:Label>
                    </label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" ReadOnly="False"
                        Enabled="True" MaxLength="500" TabIndex="2" Width="500px" Columns="80" Rows="5">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Nivel educativo</label>
                </td>
                <td>
                    <asp:DropDownList ID="cbNivelEducativo" runat="server" AutoPostBack="true" CssClass="required"
                        OnSelectedIndexChanged="cbNivelEducativo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Grado</label>
                </td>
                <td>
                    <asp:DropDownList ID="cbGradoAsignatura" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <%-- Finaliza formulario AreaProfesionalizacion --%>
        <%-- Empieza formulario MateriaProfesionalizacion --%>
        <asp:Panel ID="Panel1" runat="server" GroupingText="Bloques de la asignatura">
            <table>
                <tr>
                    <td class="td-label">
                        <label class="label">
                            <asp:Label runat="server" ID="lblNombreMateria" Text="Nombre"></asp:Label>
                        </label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtNombreMateria" ReadOnly="False" Enabled="True"
                            MaxLength="50" TabIndex="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                    </td>
                    <td>
                        <asp:Button ID="btnAgregar" Text="Agregar" runat="server" CssClass="boton" OnClick="BtnAgregar_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:UpdatePanel ID="updMaterias" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grdMaterias" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                        Width="600" Visible="True"
                        OnRowCancelingEdit="grdMaterias_RowCancelingEdit" OnRowDeleting="grdMaterias_RowDeleting"
                        OnRowEditing="grdMaterias_RowEditing" OnRowUpdating="grdMaterias_RowUpdating">
                        <Columns>
                            <asp:BoundField DataField="MateriaID" HeaderText="ID" SortExpression="MateriaID"
                                ReadOnly="true" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <%# Boolean.Parse(Eval("Activo").ToString())? "Activo": "Inactivo" %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                            </asp:CommandField>
                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                        OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Text="Eliminar"></asp:LinkButton>&nbsp;
                                </ItemTemplate>
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
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsAreaProfesionalizacion" DataSourceType="DataSet" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <%-- Finaliza formulario MateriaProfesionalizacion --%>
        <br />
        <div class="line">
        </div>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" CssClass="boton" OnClick="BtnGuardar_Click"
            OnClientClick="Validate();" />
        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="~/Profesionalizacion/CatalogoAreas/BuscarAreas.aspx"
            CssClass="boton" OnClientClick="CancelValidate();"></asp:HyperLink>
        <asp:UpdatePanel runat="server" ID="updEditar">
            <ContentTemplate>
                <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                    style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

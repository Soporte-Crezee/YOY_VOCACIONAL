<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditarArea.aspx.cs" Inherits="POV.Web.PortalOperaciones.Profesionalizacion.CatalogoAreas.EditarArea" %>

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

            $("#frmMain").validate(
				{
				    rules: {
				        '<%=txtNombre.UniqueID %>': { required: true, maxlength: 100 },
				        '<%=txtDescripcion.UniqueID %>': { required: false, maxlength: 500 },
				        '<%=cbNivelEducativo.UniqueID %>': { required: true },
				        '<%=cbGradoAsignatura.UniqueID %>': { required: true }
				    },
				    submitHandler: function (form) {
				        $('.main').block();
				        form.submit();
				    }
				});
        }

        function Cancelalidate(parameters) {
            var form = $('#frmMain').get(0);
            $.removeData(form, 'validator');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/Profesionalizacion/CatalogoAreas/BuscarAreas.aspx">Volver</asp:HyperLink>/Editar
        asignaturas
    </h3>
    <div class="main_div ui-widget-content" style="padding: 15px">
        <h2>
            Datos de la asignatura</h2>
        <div class="line">
        </div>
        <br />
        <%-- Empieza formulario AreaProfesionalizacion --%>
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblNombre" Text="Nombre"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" CssClass="required"
                        MaxLength="100" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label-texarea" style="vertical-align: top">
                    <asp:Label runat="server" ID="lblDescripcion" Text="Descripci&oacute;n"></asp:Label>
                </td>
                <td class="input">
                    <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" ReadOnly="False"
                        Enabled="True" MaxLength="500" TabIndex="2" Columns="80" Width="500" Rows="5">
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
            <tr>
                <td class="td-label">
                    <asp:Label runat="server" ID="lblEstatus" Text="Estatus"></asp:Label>
                </td>
                <td class="input">
                    <asp:DropDownList runat="server" ID="ddlEstatus">
                        <asp:ListItem Text="Activo" Value="True" />
                        <asp:ListItem Text="Inactivo" Value="False" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <%-- Finaliza formulario AreaProfesionalizacion --%>
        <%-- Empieza formulario MateriaProfesionalizacion --%>
        <asp:Panel runat="server" GroupingText="Bloques de la asignatura">
            <table>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblNombreMateria" Text="Nombre"></asp:Label>
                    </td>
                    <td class="input">
                        <asp:TextBox runat="server" ID="txtNombreMateria" ReadOnly="False" Enabled="True"
                            MaxLength="100" TabIndex="4"></asp:TextBox>
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
                        OnRowUpdating="grdMaterias_RowUpdating" OnRowCancelingEdit="grdMaterias_RowCancelingEdit"
                        OnRowEditing="grdMaterias_RowEditing" OnRowDeleting="grdMaterias_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="MateriaID" HeaderText="ID" SortExpression="MateriaID"
                                ReadOnly="True" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:TemplateField HeaderText="Estatus">
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
                                        Text="Eliminar"></asp:LinkButton>
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
                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsMateriasProf" DataSourceType="DataTable" />
                        </PagerTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <div class="line">
        </div>
        <%-- Finaliza formulario MateriaProfesionalizacion --%>
        <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" CssClass="boton" OnClick="BtnGuardar_Click" />
        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="~/Profesionalizacion/CatalogoAreas/BuscarAreas.aspx"
            CssClass="boton" OnClientClick="Cancelalidate();"></asp:HyperLink>
        <asp:UpdatePanel runat="server" ID="updEditar">
            <ContentTemplate>
                <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                    style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

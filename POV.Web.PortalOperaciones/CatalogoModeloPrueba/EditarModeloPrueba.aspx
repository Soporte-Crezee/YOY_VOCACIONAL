<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditarModeloPrueba.aspx.cs" Inherits="POV.Web.PortalOperaciones.CatalogoModeloPrueba.EditarModeloPrueba" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet" type="text/css" />
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_OnClick" OnClientClick="CancelValidate()">Volver</asp:LinkButton>/Editar modelo de prueba
    </h3>
    <div class="main_div ui-widget-content" style="padding: 5px;">
        <h2>Informaci&oacute;n del modelo de prueba</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label-texarea">
                    <label>Nombre</label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombre" ReadOnly="False" Enabled="True" CssClass="required" MaxLength="200" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label-texarea">
                    <label>Descripción</label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDescripcion" ReadOnly="False" Enabled="True" TextMode="MultiLine" MaxLength="1000" TabIndex="3"
                        Height="50px">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <h2>Informaci&oacute;n del m&eacute;todo de calificaci&oacute;n</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label-texarea">
                    <label>Nombre</label></td>
                <td>
                    <asp:DropDownList ID="DDLMetodoCalificacion" runat="server" Width="430px">
                        <asp:ListItem Text="SELECCIONE..." Value=""></asp:ListItem>
                        <asp:ListItem Text="CALIFICACIÓN POR PUNTOS" Value="0"></asp:ListItem>
                        <asp:ListItem Text="CALIFICACIÓN POR PORCENTAJE DE ACIERTOS" Value="1"></asp:ListItem>
                        <asp:ListItem Text="CALIFICACIÓN POR CLASIFICACIÓN" Value="2"></asp:ListItem>
                        <asp:ListItem Text="CALIFICACIÓN POR SELECCIÓN DE CLASIFICADORES" Value="3"></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="plPropiedades" runat="server" GroupingText="Propiedades de los clasificadores">
            <%-- BEGIN formulario Propiedades --%>
            <asp:UpdatePanel ID="UpdPanelPropiedades" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="td-label">
                                <label>
                                    Nombre</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNombrePropiedad" runat="server" MaxLength="100"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="td-label-texarea">
                                <label>
                                    Descripci&oacute;n</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescripcionPropiedad" runat="server" MaxLength="500"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="td-label">
                                <label>¿Es visible?</label>
                            </td>
                            <td style="width: 40px;">
                                <asp:CheckBox ID="chkEsVisible" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="BtnAgregarPropiedad" runat="server" Text="Agregar propiedad" CssClass="btn-green"
                                    OnClick="BtnAgregarPropiedad_OnClick" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="GridPropiedades" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        DataKeyNames="propID" HeaderStyle-CssClass="th" AutoGenerateColumns="false" OnRowCancelingEdit="GridPropiedades_RowCancelingEdit"
                        OnRowEditing="GridPropiedades_RowEditing" OnRowUpdating="GridPropiedades_RowUpdating" OnRowDeleting="GridPropiedades_RowDeleting"
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Propiedad" HeaderText="Propiedad">
                                <HeaderStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-Width="200px" />
                            <asp:CheckBoxField DataField="EsVisible" HeaderText="Es visible" />
                            <asp:CommandField ShowEditButton="True" HeaderText="Editar" ItemStyle-HorizontalAlign="Center" EditImageUrl="../images/edit-button.png"
                                ButtonType="Image" CancelImageUrl="../Images/hr.gif" UpdateImageUrl="../Images/save-icon.gif"></asp:CommandField>
                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');" ImageUrl="../images/minus-button.png" ToolTip="Eliminar"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>El modelo no tiene
                                propiedades disponibles
                                </p>
                            </div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <br />
        <%-- END formulario Propiedades --%>
        <div class="line"></div>
        <table>
            <tr>
                <td class="td-label"></td>
                <td class="label">
                    <asp:Button ID="BtnGuardar" Text="Guardar" runat="server" OnClick="BtnGuardar_OnClick"
                        CssClass="btn-green" OnClientClick="ValidateFields()" />
                </td>
                <td class="label">
                    <asp:Button ID="BtnCancelar" Text="Cancelar" runat="server" OnClick="BtnCancelar_OnClick"
                        CssClass="btn-cancel" OnClientClick="CancelValidate()" />
                </td>
                <td class="td-label"></td>
            </tr>
        </table>

        <span style=""></span>
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
    <asp:HiddenField ID="hdnNombreModeloPrueba" runat="server" />
</asp:Content>

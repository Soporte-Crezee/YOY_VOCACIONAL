<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurarMetodoClasificacion.aspx.cs" Inherits="POV.Web.PortalOperaciones.Pruebas.ConfigurarMetodoClasificacion" %>

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
    <style type="text/css">
        textarea
        {
            min-width: 240px;
            width: 240px;
        }
        .textarea_max
        {
            min-width: 300px;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_Click">Volver</asp:LinkButton>/Configurar
        método de calificación por clasificación</h3>
    <div class="main_div ui-widget-content" style="padding: 5px;">
        <h2>
            Información de la prueba</h2>
        <div class="line">
        </div>
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblClavePrueba" runat="server" Text="Clave"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtClavePrueba" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblModeloPrueba" runat="server" Text="Modelo de Prueba"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtModeloPrueba" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblMetodoCalificacion" runat="server" Text="Método de calificación"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMetodoCalificacion" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEstatusPrueba" runat="server" Text="Estatus"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEstadoPrueba" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        
       
        <h2>
            Configurar evaluaci&oacute;n</h2>
        <div class="line">
        </div>
        <asp:UpdatePanel ID="updRangos" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblRangoInicial" runat="server" Text="Rango inicial"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRangoInicial" runat="server" Width="40px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblRangoFinal" runat="server" Text="Rango final"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRangoFinal" runat="server" Width="40px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblRangoPredominante" runat="server" Text="Rango predominante"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chbRangoPredominante" runat="server" Text="" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblClasificador" runat="server" Text="Clasificador"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlClasificador" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="td-label-texarea">
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" Columns="30"
                                MaxLength="500" CssClass="textarea_max"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="BtnAgregarRango" runat="server" Text="Agregar Rango" CssClass="btn-green"
                                OnClick="BtnAgregarRango_Click" />
                        </td>
                    </tr>
                </table>
                <div class="line">
                </div>
                <h2>
                    Rangos configurados</h2>
                <div class="line">
                </div>
                <asp:GridView ID="grdRangos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="920" EnableSortingAndPagingCallbacks="True" AllowSorting="true" Visible="True"
                    OnRowDataBound="grdRangos_RowDataBound" OnRowCancelingEdit="grdRangos_RowCancelingEdit"
                    OnRowDeleting="grdRangos_RowDeleting" OnRowEditing="grdRangos_RowEditing" OnRowUpdating="grdRangos_RowUpdating"
                    TabIndex="10" ItemStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField DataField="PuntajeMinimo" HeaderText="Rango inicial" SortExpression="PuntajeMinimo"
                            ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="PuntajeMaximo" HeaderText="Rango final" SortExpression="PuntajeMaximo"
                            ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Predominante" ItemStyle-HorizontalAlign="Center" SortExpression="EsPredominante">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkEsPredominante" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem, "EsPredominante")%>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEsPredominante" runat="server" Text='<%# Bind("TextoPredominante") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Clasificador" SortExpression="Clasificador" ControlStyle-Width="120px">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlClasificador" runat="server">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClasificador" runat="server" Text='<%# Bind("Clasificador") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ControlStyle-Width="120px"
                            ItemStyle-Width="120px" ItemStyle-CssClass="break_text" />
                        <asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion" ControlStyle-Width="120px"
                            ItemStyle-Width="120px" ItemStyle-CssClass="break_text">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("Descripcion") %>'
                                    TextMode="MultiLine" Rows="3" Columns="20"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ButtonType="Image"
                            ItemStyle-HorizontalAlign="Center" HeaderText="Acción" EditImageUrl="~\images\VOCAREER_editar.png"
                            CancelImageUrl="~\images\hr.gif" UpdateImageUrl="~\images\save-icon.gif" DeleteImageUrl="~/images/VOCAREER_suprimir.png" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                                resultados</p>
                        </div>
                    </EmptyDataTemplate>
                    <PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DtRutas" DataSourceType="DataTable" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="line">
        </div>
        <asp:Button ID="BtnAceptar" runat="server" Text="Guardar" CssClass="btn-green" OnClick="BtnAceptar_Click" />
        <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="btn-cancel" OnClick="BtnCancelar_Click" />
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
     </div>
</asp:Content>

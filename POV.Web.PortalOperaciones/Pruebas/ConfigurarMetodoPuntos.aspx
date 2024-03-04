<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurarMetodoPuntos.aspx.cs" Inherits="POV.Web.PortalOperaciones.Pruebas.ConfigurarMetodoPuntos" %>

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
        textarea {
            min-width: 240px;
            width: 240px;
        }
        .textarea_max{
            min-width: 300px;
            
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="BuscarPruebas.aspx">Volver</asp:HyperLink>/Configurar
        método de calificación por puntos</h3>
    <div class="ui-widget-content" style="padding: 5px">
        <h2>
            Información de la prueba</h2><div class="line"></div>
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
                    <asp:Label ID="lblEstadoLiberacion" runat="server" Text="Estatus"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEstadoLiberacion" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br/>
        <h2>
            Configurar evaluación</h2><div class="line"></div>
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblPuntajeInicial" runat="server" Text="Puntuación inicial"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPuntajeInicial" runat="server" Width="40px" MaxLength="10"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblPuntajeFinal" runat="server" Text="Puntuación final"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPuntajeFinal" runat="server" Width="40px" MaxLength="10" TabIndex="1"></asp:TextBox>
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
                    <asp:CheckBox ID="chbRangoPredominante" runat="server" Text="" TabIndex="2" />
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
                    <asp:DropDownList runat="server" ID="ddlClasificador" TabIndex="3" />
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
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="200" TabIndex="4"></asp:TextBox>
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
                    <textarea id="txtaDescripcion" runat="server" cols="30" name="txtaDescripcion" rows="3"
                        tabindex="5" class="textarea_max"></textarea>
                </td>
                <td>
                    <asp:Button ID="BtnAgregarRango" runat="server" Text="Agregar Rango" CssClass="btn-green"
                        OnClick="BtnAgregarRango_Click" TabIndex="6" />
                </td>
            </tr>
        </table>
        <br/>
        <h2>
            Rangos configurados</h2><div class="line"></div>
        
                <asp:UpdatePanel ID="updRangos" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grdRangos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                    HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="10" AllowPaging="true"
                    Width="920" EnableSortingAndPagingCallbacks="True" AllowSorting="false" Visible="True"
                    OnRowDataBound="grdRangos_RowDataBound" OnRowCancelingEdit="grdRangos_RowCancelingEdit"
                    OnRowDeleting="grdRangos_RowDeleting" OnRowEditing="grdRangos_RowEditing" OnRowUpdating="grdRangos_RowUpdating"
                    TabIndex="10" ItemStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField DataField="PuntajeMinimo" HeaderText="Rango inicial" SortExpression="PuntajeMinimo" ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="PuntajeMaximo" HeaderText="Rango final" SortExpression="PuntajeMaximo" ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Predominante" ItemStyle-HorizontalAlign="Center" SortExpression="EsPredominante">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkEsPredominante" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEsPredominante" runat="server" Text='<%# Bind("EsPredominante") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Clasificador" SortExpression="Clasificador" ControlStyle-Width="120px" ItemStyle-HorizontalAlign="Center" >
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlClasificador" runat="server" >
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblClasificador" runat="server" Text='<%# Bind("Clasificador") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ControlStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-CssClass="break_text" />
                        <asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion" ControlStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="break_text" >
                            <EditItemTemplate>
                                <textarea id="txtaDescripcion" name="txtaDescripcion" runat="server" rows="3" cols="20"></textarea>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("Descripcion") %>'></asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ButtonType="Image"
                            ItemStyle-HorizontalAlign="Center" HeaderText="Acción" EditImageUrl="~/images/VOCAREER_editar.png"
                            CancelImageUrl="~/images/hr.gif" UpdateImageUrl="~/images/save-icon.gif" DeleteImageUrl="~/images/VOCAREER_suprimir.png" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="ui-state-highlight ui-corner-all">
                            <p>
                                <span class="ui-icon ui-icon-info" style="float: left"></span>La prueba no tiene rangos configurados</p>
                        </div>
                    </EmptyDataTemplate>
                    <PagerTemplate>
                        <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="escaladinamica"
                            DataSourceType="DataSet" />
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <div class="clear"></div>
        <br/>
        <div class="line">
        </div>
        <asp:Button ID="BtnAceptar" runat="server" Text="Guardar" CssClass="btn-green" OnClick="BtnAceptar_Click"
            TabIndex="7" />
        <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="btn-cancel" OnClick="BtnCancelar_Click"
            TabIndex="8" />
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
</asp:Content>

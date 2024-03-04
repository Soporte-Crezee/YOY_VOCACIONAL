<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfigurarMetodoSeleccion.aspx.cs" Inherits="POV.Web.PortalOperaciones.Pruebas.ConfigurarMetodoSeleccion" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/gridview.css")%>" rel="stylesheet" type="text/css" />
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

        .textarea_max {
            min-width: 300px;
        }

        .td-label {
            width: 170px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header ui-widget-header-label">
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="BtnCancelar_Click">Volver</asp:LinkButton>/Configurar m&eacute;todo de calificaci&oacute;n por selecci&oacute;n de áreas de conocimiento</h3>
    <div class="ui-widget-content" style="padding: 5px;">
        <h2>Informaci&oacute;n de la prueba</h2>
        <div class="line"></div>
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblClavePrueba" runat="server" Text="Clave" CssClass="label"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtClavePrueba" runat="server" Enabled="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblModeloPrueba" runat="server" Text="Modelo de Prueba" CssClass="label"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtModeloPrueba" runat="server" Enabled="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblMetodoCalificacion" runat="server" Text="Método de calificación" CssClass="label"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtMetodoCalificacion" runat="server" Enabled="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEstatusPrueba" runat="server" Text="Estatus" CssClass="label"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtEstadoPrueba" runat="server" Enabled="false"></asp:TextBox></td>
            </tr>
        </table>
        <br />
        <h2>Configurar evaluaci&oacute;n</h2>
        <div class="line"></div>
        <asp:UpdatePanel ID="updRangos" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblSeleccionInicial" runat="server" Text="Número respuestas inicial" CssClass="label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSeleccionInicial" runat="server" Width="80px" MaxLength="11"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblSeleccionFinal" runat="server" Text="Número de respuestas final" CssClass="label"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtSeleccionFinal" runat="server" Width="80px" MaxLength="11"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblRangoPredominante" runat="server" Text="Rango predominante" CssClass="label"></asp:Label></td>
                        <td>
                            <asp:CheckBox ID="chkRangoPredominante" runat="server" Text="" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblClasificador" runat="server" Text="Área de conocimiento" CssClass="label"></asp:Label></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlClasificadorForm" /></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td-label">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="label"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100"></asp:TextBox></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="td-label-texarea">
                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción" CssClass="label"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="textarea_max" TextMode="MultiLine" Rows="3" Columns="30" MaxLength="500"></asp:TextBox></td>

                    </tr>
                    <tr>
                        <td class="td-label"></td>
                        <td>
                            <asp:Button ID="BtnAgregarRango" runat="server" Text="Agregar Rango" CssClass="btn-green" OnClick="BtnAgregar_Click" /></td>
                    </tr>
                </table>


                <h2>Rangos configurados</h2>
                <div class="line"></div>

                <div class="table-responsive">
                    <asp:GridView ID="GrdRangos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                        HeaderStyle-CssClass="th" AutoGenerateColumns="false"
                        Width="920" EnableSortingAndPagingCallbacks="True" AllowSorting="false" Visible="True"
                        OnRowCancelingEdit="GrdRangos_RowCancelingEdit" OnRowDataBound="grdRangos_RowDataBound"
                        OnRowEditing="GrdRangos_RowEditing" OnRowUpdating="GrdRangos_RowUpdating" OnRowDeleting="GrdRangos_RowDeleting" ItemStyle-HorizontalAlign="Center">
                        <Columns>
                            <asp:BoundField DataField="PuntajeMinimo" HeaderText="Respuestas inicial"
                                SortExpression="PuntajeMinimo" ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PuntajeMaximo" HeaderText="Respuestas final"
                                SortExpression="PuntajeMaximo" ControlStyle-Width="40px" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Predominante" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblEsPredominante" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TextoPredominante") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkEsPredominante" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem, "EsPredominante")%>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Área de conocimiento">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreClasificador" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Clasificador")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlClasificador" runat="server" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ControlStyle-Width="120px" ItemStyle-CssClass="break_text" />
                            <asp:TemplateField HeaderText="Descripcion" ControlStyle-Width="170px" ItemStyle-CssClass="break_text">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescripcion" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Descripcion")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="3" Columns="20" Text='<%#DataBinder.Eval(Container.DataItem, "Descripcion")%>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:CommandField ShowEditButton="True" HeaderText="Editar" ItemStyle-HorizontalAlign="Center" ButtonType="Image"
                                EditImageUrl="~/images/VOCAREER_editar.png" CancelImageUrl="~/images/hr.gif" UpdateImageUrl="~/images/save-icon.gif" EditText="Editar"></asp:CommandField>

                            <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/VOCAREER_suprimir.png"
                                        OnClientClick="return confirm('¿Está seguro que desea eliminar este elemento?');"
                                        Text="Eliminar"></asp:ImageButton>&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="ui-state-highlight ui-corner-all">
                                <p>
                                    <span class="ui-icon ui-icon-info" style="float: left"></span>La búsqueda no produjo
                            resultados
                                </p>
                            </div>
                        </EmptyDataTemplate>

                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <br />
        <div class="line"></div>
        <table>
            <tr>
                <td class="td-label"></td>
                <td class="label">
                    <asp:Button ID="BtnGuardar" runat="server" Text="Guardar" CssClass="btn-green" OnClick="BtnGuardar_Click" />
                </td>
                <td class="label">
                    <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="btn-cancel" OnClick="BtnCancelar_Click" />
                </td>
                <td class="td-label"></td>
            </tr>
        </table>

        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
        <br />
        <br />
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="RegistrarEscuela.aspx.cs" Inherits="POV.Web.PortalOperaciones.Escuelas.RegistrarEscuela" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $(".boton").button();
            $("#frmMain").validate(
                {
                    rules: 
                        {
                        <%=fuArchivoEscuelas.UniqueID %>:
                            {
                                required: true
                            },
                        <%=cbContrato.UniqueID %>:
                            {
                                required: true
                            }
                            ,
                        <%=cbCicloEscolar.UniqueID %>:
                            {
                                required: true,
                                min: 1
                            }
                },
                messages:
                        {
                    
                    <%=cbCicloEscolar.UniqueID %>: {
                        min: "Seleccione un Ciclo Escolar"
                    }
                },
                submitHandler: function(form) {
                    $(form).block();
                    form.submit();
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink ID="lnkBack" runat="server"></asp:HyperLink>Importar excel de escuelas</h3>
    <div class="ui-widget-content" style="padding:5px">
        <h2>
            Informaci&oacute;n del contrato</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblClave" runat="server" Text="Clave contrato"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="cbContrato" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbContrato_SelectedIndexChanged"
                        AppendDataBoundItems="True" />
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    Nombre del cliente
                </td>
                <td colspan="3">
                    <asp:TextBox ID="TxtNombreCliente" runat="server" ReadOnly="true" Enabled="false" Width="99%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    Representante
                </td>
                <td colspan="3">
                    <asp:TextBox ID="TxtNombreRepresentante" runat="server" ReadOnly="true" 
                        Enabled="false" Width="99%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    Fecha inicio
                </td>
                <td>
                    <asp:TextBox ID="TxtInicioContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td class="td-label">
                    Fecha fin
                </td>
                <td>
                    <asp:TextBox ID="TxtFinContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    Licencias ilimitadas
                </td>
                <td>
                    <asp:TextBox ID="TxtLicenciasIlimitadas" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td class="td-label">
                    N&uacute;mero de licencias
                </td>
                <td>
                    <asp:TextBox ID="TxtNumeroLicencias" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    Licencias disponibles
                </td>
                <td>
                    <asp:TextBox ID="TxtLicenciasDisponibles" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Informaci&oacute;n del ciclo escolar</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblCicloEscolar" runat="server" Text="Ciclo escolar"></asp:Label>
                </td>
                <td >
                    <asp:DropDownList ID="cbCicloEscolar" runat="server" AppendDataBoundItems="True"
                        AutoPostBack="true" OnSelectedIndexChanged="cbCiclo_SelectedIndexChanged" />
                </td>
               
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblPais" runat="server" Text="País"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtPais" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 115px;">
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                </td>
                <td style="width: auto;">
                    <asp:TextBox ID="TxtEstado" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td style="width: 70px;">
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="td-label">
                    <label>
                        Fecha inicio</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtInicioCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td class="td-label">
                    <label>
                        Fecha fin</label>
                </td>
                <td>
                    <asp:TextBox ID="TxtFinCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td-label">
                    <label>
                        Descripci&oacute;n</label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="TxtDescripcionCiclo" runat="server" ReadOnly="true" Enabled="false"
                        Width="99%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Modulos funcionales de las escuelas</h2>
        <hr />
        <asp:GridView ID="grdModulosFuncionales" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
            HeaderStyle-CssClass="th" AutoGenerateColumns="false"  
            Width="800" >
            <Columns>
                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                <asp:TemplateField HeaderText="Seleccionar">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" />
                        <asp:HiddenField id="hdnModuloId" runat="server" Value='<%# Bind("ModuloFuncionalID") %>'/>
                    </ItemTemplate>
                    <HeaderStyle CssClass="th" BorderStyle="none" />
                    <ItemStyle CssClass="GridRowBorder" HorizontalAlign="Center"
                        VerticalAlign="Middle" Width="26px" Height="16px" />
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="ui-state-highlight ui-corner-all">
                    <p>
                        <span class="ui-icon ui-icon-info" style="float: left;"></span>La b&uacute;squeda
                        no produjo resultados
                    </p>
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <h2>
            Excel de escuelas</h2>
        <hr />
        <br />
        <table>
            <tr>
                <td class="td-label">
                    <asp:Label ID="lblEscuelas" runat="server" Text="Escuelas"></asp:Label>
                </td>
                <td style="width: 400px;">
                    <asp:FileUpload ID="fuArchivoEscuelas" runat="server" Style="width: 100%" />
                </td>
                <td style="width: 70px;">
                    <asp:Button ID="btnCargar" runat="server" Text="Cargar" Style="width: 100%" CssClass="boton"
                        OnClick="btnCargar_OnClick" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
            style="display: none;" />
    </div>
    &nbsp;
    <br />
    <br />
    <br />
    <br />
    &nbsp;
</asp:Content>

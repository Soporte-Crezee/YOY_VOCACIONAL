<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ActualizarLicenciaEscuela.aspx.cs" Inherits="POV.Web.PortalOperaciones.Escuelas.ActualizarLicenciaEscuela" %>

<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/Controls/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>gridview.css" rel="stylesheet"
        type="text/css" />
    <script src="../Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="../Scripts/messages_es.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            $('#acordion').accordion({ active: -1, collapsible: true, autoHeight: false });

        };

        function validateEscuela() {
            var rules = {

                '<%=cbContrato.UniqueID %>': { required: true }

            };

            jQuery.extend(jQuery.validator.messages, {
                min: jQuery.validator.format("Este campo es obligatorio")
            });

            $('#frmMain').validate({
                rules: rules,
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

        function seleccionDirector(valorW) {
            $("#hdnVistaConsultar").val(valorW);

        }
    </script>
    <style type="text/css">
        .textareachica
               {
          min-width:232px;
        }        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="ui-widget-header">
        <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="BuscarEscuelas.aspx">
		Volver
        </asp:HyperLink>
        /Actualizar licencia de escuela
    </h3>
    <div class="ui-widget-content" style="padding: 5px">
        <h2>
            Informaci&oacute;n de la escuela</h2>
        <hr />
        <br />
        <table class="finder">
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblClaveEscuela" Text="Clave escuela"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtClaveEscuela" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblTurno" Text="Turno"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtTurno" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblNombreEscuela" Text="Nombre"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNombreEscuela" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblAmbito" Text="Ámbito"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAmbito" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblControl" Text="Control"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtControl" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
                <td class="label">
                    <asp:Label runat="server" ID="lblTipoServicio" Text="Tipo de servicio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtTipoServicio" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="label">
                    <asp:Label runat="server" ID="lblUbicacion" Text="Ubicación"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtUbicacion" ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <h2>
                Informaci&oacute;n del contrato</h2>
            <hr />
            <br />
            <asp:UpdatePanel ID="UpdPnlContrato" runat="server">
                <ContentTemplate>
                    <table class="finder">
                        <tr>
                            <td class="label">
                                Clave contrato
                            </td>
                            <td colspan="1">
                                <asp:DropDownList ID="cbContrato" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbContrato_SelectedIndexChanged"
                                    AppendDataBoundItems="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="label">
                                Nombre de cliente
                            </td>
                            <td colspan="1">
                                <asp:TextBox ID="TxtNombreCliente" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="label">
                                Representante
                            </td>
                            <td colspan="1">
                                <asp:TextBox ID="TxtNombreRepresentante" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">
                                Fecha inicio
                            </td>
                            <td>
                                <asp:TextBox ID="TxtInicioContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="label">
                                Fecha fin
                            </td>
                            <td>
                                <asp:TextBox ID="TxtFinContrato" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">
                                Licencias ilimitadas
                            </td>
                            <td>
                                <asp:TextBox ID="TxtLicenciasIlimitadas" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="label">
                                N&uacute;mero de licencias
                            </td>
                            <td>
                                <asp:TextBox ID="TxtNumeroLicencias" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="label">
                                Licencias disponibles
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="TxtLicenciasDisponibles" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: left; width: 59%">
            <asp:Panel ID="pnlCiclosContrato" runat="server" GroupingText="Lista de ciclos escolares del contrato">
                <asp:UpdatePanel runat="server" ID="updCiclosContrato">
                    <ContentTemplate>
                        <asp:GridView runat="server" ID="grdCiclosContrato" CssClass="DDGridView" RowStyle-CssClass="td"
                            HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="5" AllowPaging="true"
                            Width="100%" Visible="true" OnRowCommand="grdCiclos_RowCommand" OnRowDataBound="grdCiclos_DataBound">
                            <Columns>
                                <asp:BoundField DataField="CicloEscolarID" HeaderText="ID" SortExpression="CicloEscolarID" />
                                <asp:BoundField DataField="Titulo" HeaderText="Nombre" SortExpression="Titulo" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" Visible="false" />
                                <asp:BoundField DataField="InicioCiclo" HeaderText="Fecha inicio" SortExpression="InicioCiclo"/>
                                <asp:BoundField DataField="FinCiclo" HeaderText="Fecha fin" SortExpression="FinCiclo" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Seleccionar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAsig" runat="server" CommandName="SelecContenidos" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CicloEscolarID")%>'
                                            ImageUrl="../Images/plus-button.png" ToolTip="Seleccionar" />
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
                                <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsCiclos" DataSourceType="DataSet" />
                            </PagerTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div style="float: right; width: 40%">
           <asp:Panel ID="pnlCicloActual" runat="server" GroupingText="Ciclo escolar actual">
                <asp:UpdatePanel ID="UpdPnlCicloEscolar" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="width: 80px; text-align:right"">
                                    <label>
                                        ID</label>
                                </td>
                                <td >
                                    <asp:TextBox runat="server" ID="txtID" ReadOnly="true" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px; text-align:right">
                                    Nombre
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombreCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px; text-align:right">
                                    <label>
                                        Inicio</label>
                                </td>
                                <td >
                                    <asp:TextBox ID="TxtInicioCiclo" runat="server" ReadOnly="true" Enabled="false" ></asp:TextBox>
                                </td>
                                </tr>
                                <tr>
                                <td style="width: 80px; text-align:right">
                                    <label>
                                        Fin</label>
                                </td>
                                <td >
                                    <asp:TextBox ID="TxtFinCiclo" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; text-align:right">
                                    <label>
                                        Descripci&oacute;n</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtDescripcionCiclo" runat="server" ReadOnly="true" Enabled="false"
                                      TextMode="MultiLine" CssClass="textareachica"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
           </asp:Panel>
        </div>
        <div style="clear: both;">
        </div>
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
                            <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" Checked='<%# Bind("Seleccionado") %>' />
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
        <div class="line" style="clear: both;">
        </div>
        <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
        <asp:HyperLink ID="HpLnkCancelar" Text="Cancelar" runat="server" NavigateUrl="BuscarEscuelas.aspx"
            CssClass="boton"></asp:HyperLink>
    </div>
    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
        style="display: none;" />
</asp:Content>

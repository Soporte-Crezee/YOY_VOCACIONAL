<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ConfigurarBancoReactivos.aspx.cs" Inherits="POV.Web.PortalOperaciones.Pruebas.ConfigurarBancoReactivos" %>

<%@ Register Src="~/Controls/GridViewPager.ascx" TagName="GridViewPager" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Styles/gridview.css")%>" rel="stylesheet"
        type="text/css" />
    <script src="<% =Page.ResolveClientUrl("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/messages_es.js")%>" type="text/javascript"></script>
    <style>
        .selected {
            background-color: lightblue;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            bindControls();
        }

        function bindControls() {
            var grd = $('#<%=grdBancoReactivos.ClientID%>');
            var trs = $(grd).find("tr:not(:has(table, th))");

            $(trs).css("cursor", "pointer").on('click', function (e) {
                var row = $(this);
                row.addClass("selected");
                var td4 = $("td", row).eq(4);
                var hdnReactivoId = td4.find('input:hidden');
                $('#<%=hdnBancoSelectedRow.ClientID %>').val(hdnReactivoId.val());
                var gr = $('#<%=grdBancoReactivos.ClientID%>');
                var tr = $(gr).find("tr:not(:has(table, th))");

                $.map($(tr).filter('.selected'), function (value, index) {
                    var rw = $(value);
                    var t4 = $("td", rw).eq(4);
                    var hdnselected = $('#<%=hdnBancoSelectedRow.ClientID %>');
                    var hdnreactivo = t4.find('input:hidden');
                    if (hdnselected.val() != hdnreactivo.val()) {
                        rw.removeClass("selected");
                    }
                });
            });

            var count = 0;
            $.map(trs, function (value, index) {
                var row = $(value);
                var td1 = $("td", row).eq(0);
                var td4 = $("td", row).eq(4);
                var chk = td1.find('input:checkbox');
                var hdnreactivo = td4.find('input:hidden');
                var hdnselected = $('#<%=hdnBancoSelectedRow.ClientID %>');
                var txtcountselect = $("#<%= txtTotalReactivosSeleccionados.ClientID%>");
                if (hdnselected.val() != hdnreactivo.val()) {
                    row.removeClass("selected");
                } else {
                    row.addClass("selected");
                }
                if (chk.is(':checked')) {
                    count++;
                }
                if (txtcountselect) {
                    txtcountselect.val(count);
                }
                chk.on('click', function (e) {
                    var chkself = $(e.target);
                    var txtselected = $("#<%= txtTotalReactivosSeleccionados.ClientID%>");
                    var countselected;
                    if (txtcountselect && txtselected.val() != "") {
                        countselected = parseInt(txtselected.val());
                        if (chkself.is(':checked')) { countselected++; }
                        else {
                            if (countselected >= 1) { countselected--; }
                        }
                        txtcountselect.val(countselected.toString());
                    }
                });
            });
        }
    </script>
        <h3 class="ui-widget-header ui-widget-header-label">
            <asp:HyperLink ID="lnkBack" NavigateUrl="BuscarPruebas.aspx" runat="server">Volver</asp:HyperLink>/Configurar
        banco reactivos de la prueba
        </h3>
        <div class="ui-widget-content" style="margin: auto; padding: 5px;">
            <h2 class="border-bottom-gray1">Informaci&oacute;n de la prueba
            </h2>
            <div class="line"></div>
            <table>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblClavePrueba" Text="Clave" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" Enabled="False" ID="txtClavePrueba" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblNombrePrueba" Text="Nombre" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" Enabled="False" ID="txtNombrePrueba" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td-label">
                        <asp:Label runat="server" ID="lblModeloPrueba" Text="Modelo de la prueba" CssClass="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox runat="server" Enabled="False" ID="txtModeloPrueba" Width="200"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <br />
            <div class="TCContainer">
                <div class="form-group">
                    <div class="col-xs-12 col-md-6">
                        <div class="TCLeftColumn" style="width: 100%">
                        <fieldset style="width: 95%; margin: auto">
                            <legend>Cat&aacute;logo de reactivos</legend>
                            <asp:UpdatePanel runat="server" ID="updReactivos">
                                <ContentTemplate>
                                    <div class="finder ui-widget-content">
                                        <table>
                                            <tr>
                                                <td>
                                                    <br />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td class="td-label">
                                                    <asp:Label runat="server" ID="lblClaveReactivo" Text="Clave" CssClass="label"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtClaveReactivo"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="td-label">
                                                    <asp:Label runat="server" ID="lblNombreReactivo" Text="Nombre" CssClass="label"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtNombreReactivo"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Button runat="server" ID="btnBuscarReactivos" Text="Buscar" CssClass="btn-green"
                                                        OnClick="btnBuscarReactivos_Click" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td colspan="3"></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="line" style="width: 100%">
                                    </div>
                                    <div style="height: 520px; width: 100%; overflow: scroll">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btnAsignarTodos" CssClass="btn-green" Text="Asignar todos los seleccionados"
                                                        OnClick="btnAsignarTodos_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="grdReactivos" runat="server" CssClass="DDGridView" RowStyle-CssClass="td"
                                                        HeaderStyle-CssClass="th" AutoGenerateColumns="false" PageSize="15" AllowPaging="true"
                                                        Width="99%" ItemStyle-HorizontalAlign="Center">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkTodos" AutoPostBack="True" OnCheckedChanged="chkTodos_CheckedChanged" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkUsar" Checked='<%# DataBinder.Eval(Container.DataItem,"Usar")  %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Clave">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblClave" Text='<%# DataBinder.Eval(Container.DataItem,"Clave")  %>'>
                                                                    </asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdnReactivoID" Value='<%# DataBinder.Eval(Container.DataItem,"ReactivoID")  %>' />
                                                                    <asp:HiddenField runat="server" ID="hdnTipoReactivo" Value='<%# DataBinder.Eval(Container.DataItem,"TipoReactivo")  %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Nombre">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" CssClass="break_text" ID="lblNombre" Text='<%# DataBinder.Eval(Container.DataItem,"NombreReactivo")  %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" Text="Asignar" CssClass="label" ID="lnkbtnCopiar" OnClick="lnkbtnCopiarReactivo_Click"></asp:LinkButton>
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
                                                        <PagerTemplate>
                                                            <asp:GridViewPager ID="grdViewPager" runat="server" SessionName="DsReactivosModelo"
                                                                DataSourceType="DataSet" />
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </div>
                    </div>
                    <div class="col-xs-12 col-md-6">
                          <div class="TCRightColumn" style="width: 100%">
                        <fieldset style="width: 95%; margin: auto">
                            <legend>Banco de reactivos de la prueba</legend>
                            <asp:UpdatePanel runat="server" ID="updConfiguracionBancoReactivos">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td style="width: 150px;">
                                                <h3>
                                                    <asp:Label runat="server" ID="lblSeleccion" Text="Opciones de selección:"></asp:Label></h3>

                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <asp:RadioButton runat="server" ID="rdUsarTotal" Text="Usar total de reactivos: "
                                                    AutoPostBack="True" OnCheckedChanged="rdUsarTotal_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtTotalReactivos" Width="50" ReadOnly="True" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <asp:RadioButton runat="server" ID="rdUsarAleatorio" Text="Usar selección aleatoria: "
                                                    AutoPostBack="True" OnCheckedChanged="rdUsarAleatorio_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:Panel runat="server" ID="plSeleccionAleatoria" Visible="False">
                                                    <asp:TextBox runat="server" ID="txtTotalReactivosAleatorios" Width="50"></asp:TextBox>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px;">
                                                <asp:RadioButton runat="server" ID="rdUsarSeleccionados" Text="Usar reactivos seleccionados: "
                                                    AutoPostBack="True" OnCheckedChanged="rdUsarSeleccionados_CheckedChanged" />
                                            </td>
                                            <td style="width: 55px;">
                                                <asp:Panel runat="server" ID="plSeleccionEspecifica" Visible="False">
                                                    <asp:TextBox runat="server" ID="txtTotalReactivosSeleccionados" ReadOnly="True" Enabled="False"
                                                        Width="50"></asp:TextBox>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <h3>
                                                    <asp:Label runat="server" ID="lblTipoOrdenamiento" Text="Tipo de ordenamiento:"></asp:Label></h3>

                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 270px;">
                                                <label>Reactivos por página</label></td>
                                            <td>
                                                <asp:TextBox TextMode="Number" ID="txtReactivosPorPagina" runat="server" Width="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td style="width: 270px;">
                                                <br />
                                                <asp:CheckBox runat="server" ID="chkPorGrupo" Text="Ordenar reactivos por grupo" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 270px;">
                                                <br />
                                                <asp:CheckBox runat="server" ID="chkOrdenamiento" Text="Ordenar reactivos aleatoriamente" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="line" style="width: 100%">
                            </div>
                            <asp:UpdatePanel runat="server" ID="updBancoReactivos">
                                <ContentTemplate>
                                    <div style="width: 100%;">
                                        <div style="width: 92%; float: left; height: 500px; overflow: scroll">
                                            <asp:HiddenField ID="hdnBancoSelectedRow" runat="server" />
                                            <table style="width: 100%">
                                                <caption>
                                                    &nbsp;<br />
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="grdBancoReactivos" runat="server" AutoGenerateColumns="false" CssClass="DDGridView"
                                                                HeaderStyle-CssClass="th" RowStyle-CssClass="td" ItemStyle-HorizontalAlign="Center"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkUsar" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"EstaSeleccionado")  %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Orden">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOrden" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Orden")  %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Clave">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox runat="server" ID="txtReactivoID" Visible="False" CssClass="key" Value='<%# DataBinder.Eval(Container.DataItem,"Reactivo.ReactivoID")  %>' />
                                                                            <asp:Label ID="lblClave" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Reactivo.Clave")  %>'>
                                                                            </asp:Label>
                                                                            <asp:HiddenField ID="hdnReactivoID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Reactivo.ReactivoID")  %>' />
                                                                            <asp:HiddenField ID="hdnTipoReactivo" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Reactivo.TipoReactivo")  %>' />
                                                                            <asp:HiddenField ID="hdnReactivoBancoID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ReactivoBancoID")  %>' />
                                                                            <asp:HiddenField ID="hdnReactivoOriginalID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ReactivoOriginal.ReactivoID")  %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Nombre">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNombre" CssClass="break_text" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Reactivo.NombreReactivo")  %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Acción" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkbtnQuiter" runat="server" CssClass="label" OnClick="lnkbtnQuitar_Click"
                                                                                Text="Quitar"></asp:LinkButton>
                                                                            <asp:HiddenField ID="hdnReactivoKey" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Reactivo.ReactivoID")  %>' />
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
                                                        </td>
                                                    </tr>
                                                </caption>
                                            </table>
                                        </div>
                                        <div style="width: 3%; float: left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <br />
                                                        <asp:ImageButton ID="btnSubir" runat="server" CssClass="boton" ImageUrl="~/Images/arrow_up.png"
                                                            OnClick="btnSubir_Click" ToolTip="Subir" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="btnBajar" runat="server" CssClass="boton" ImageUrl="~/Images/arrow_down.png"
                                                            OnClick="btnBajar_Click" ToolTip="Bajar" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <script type="text/javascript">

                                Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequest);
                                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
                                function initializeRequest(sender, args) {
                                }
                                function endRequest(sender, args) {
                                    bindControls();
                                }

                            </script>
                        </fieldset>
                    </div>
                    </div>
                </div>
                    
              
                </div>
            <br />
            <div>
                <asp:UpdatePanel runat="server" ID="updGuardar">
                    <ContentTemplate>
                        <div class="line"></div>
                        <table>
                            <tr>
                                <td class="td-label"></td>
                                <td class="label">
                                    <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="btn-green"
                            OnClick="btnGuardar_Click" />
                                </td>
                                <td class="label">
                                    <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn-cancel"
                        OnClick="btnCancelar_Click" />
                                </td>
                                <td class="td-label"></td>
                            </tr>
                        </table>
                        
                        &nbsp;&nbsp;
                    
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server"
                style="display: none;" />
        </div>

</asp:Content>

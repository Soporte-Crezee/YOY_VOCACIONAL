<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VincularCarreras.aspx.cs" Inherits="POV.Web.PortalUniversidad.Carreras.VincularCarreras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            DoFormBlockUI();
            iniciarCotrolesGrid();
        }

        function reloadControlsGrid() {
            setTimeout(function () {
                iniciarCotrolesGrid();
            }, 300);
        }

        function iniciarCotrolesGrid() {
            $('#<%=grdCarreras.ClientID%> tbody tr.tr').mouseover(function () {
                if (!$(this).hasClass('selectedRow'))
                    $(this).addClass('selectRow');
            }).mouseout(function () {
                $(this).removeClass('selectRow');
            });

            $(".chk-ctrl-carreras").children("input[type='checkbox']").each(function (i) {
                if ($(this).is(':checked')) $(".tr").eq(i).toggleClass('selectedRow');
            });

            $('#<%=grdCarreras.ClientID%>').delegate('tbody tr.tr', 'click', function () {
                if ($(this).hasClass('tr')) {
                    var row = $(this).index() - 1;
                    $(this).toggleClass('selectedRow');

                    //no seleccionado
                    if ($(this).hasClass('selectedRow')) {
                        $(this).removeClass('selectRow');
                        $(".chk-ctrl-carreras").children("input[type='checkbox']").each(function (i) {
                            if (i == row) $(this).prop('checked', true);
                        });
                    }

                        //Seleccionado
                    else {
                        $(this).addClass('selectRow');
                        $(".chk-ctrl-carreras").children("input[type='checkbox']").each(function (i) {
                            if (i == row) $(this).prop('checked', false);
                        });
                    }
                }
            });
        }

        function seleccionarTodos() {

            var valorCheck = $("#chkbox-seleccionar-todos").is(':checked');

            $(".chk-ctrl-carreras").children("input[type='checkbox']").each(function (i) {
                $(this).prop('checked', valorCheck);

                var tRow = $(".tr").eq(i);
                if (!tRow.hasClass('selectedRow') && valorCheck)
                    tRow.addClass('selectedRow');
                else if (tRow.hasClass('selectedRow') && !valorCheck)
                    tRow.removeClass('selectedRow');
            });
        }
    </script>
    <style type="text/css">
        .hidden_col {
            display: none;
            width: 0px !important;
            margin: 0px;
            padding: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="tBienvenida">
        <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Carreras/BuscarCarreras.aspx" CssClass="tBienvenidaLabel">
            Volver
        </asp:HyperLink>
        ► Vincular carreras
    </h1>
    <div class="col-xs-12 col-md-12">
        <div class="row">
            <div class="col-md-12">
                <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
            </div>
            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                <div class="col-xs-12 titulo_marco_general">
                    Informaci&oacute;n de la carrera
                </div>
                <div class="col-xs-12 container_busqueda_general ui-widget-content">
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="Label1" Text="Nombre" CssClass="col-sm-4 control-label" ToolTip="Nombre carrera"></asp:Label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNombre" TabIndex="1" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblAreaConocimiento" Text="Área de conocimiento" CssClass="col-sm-4 control-label" ToolTip="Área de conocimiento"></asp:Label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlAreaConocimiento" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="">Seleccionar área de conocimiento</asp:ListItem>
                                </asp:DropDownList>
                                <div class="help-block with-errors"></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-offset-5 col-md-offset-5" style="padding: 5px 0 0 0">
                        <div class="opciones_formulario">
                            <asp:Button runat="server" ID="btnBuscar" CssClass="btn-green" Text="Buscar" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12" style="padding: 20px 0px 0px 0px">
                <div class="col-xs-12">
                    <div id="PnlCreate" class="nuevo" runat="server" visible="false">
                        <a href="NuevaCarrera.aspx" id="lnkNuevaCarrera" class="btn-green"><span class=" ui-icon ui-icon-circle-plus"
                            style="display: inline-block; vertical-align: middle; margin-top: 0px;"></span>
                            <label class="label-helvetica" style="cursor: pointer;" >Agregar nueva carrera</label>
                        </a>
                    </div>
                </div>
                <div class="col-xs-12 container_busqueda_general ui-widget-content">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <div class="col-xs-12 form-group center-block">
                                <div class="table-responsive">
                                    <asp:GridView runat="server" ID="grdCarreras" CssClass="table table-bordered"
                                        AutoGenerateColumns="False" PageSize="10" AllowPaging="True"
                                        EnableSortingAndPagingCallbacks="True" AllowSorting="True" DataKeyNames="CarreraID"
                                        Visible="false" OnRowDataBound="grdCarreras_RowDataBound" OnRowCreated="grdCarreras_RowCreated"
                                        OnPageIndexChanging="grdCarreras_PageIndexChanging" OnPageIndexChanged="grdCarreras_PageIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="CarreraID" HeaderText="Clave" SortExpression="Clave" HeaderStyle-CssClass="hidden_col" ItemStyle-CssClass="hidden_col" />
                                            <asp:BoundField DataField="NombreCarrera" HeaderText="Nombre de la carrera" />
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripci&oacute;n de la carrera" />
                                            <asp:BoundField DataField="Clasificador.Nombre" HeaderText="&Aacute;rea de conocimiento" />
                                            <asp:TemplateField HeaderText="">
                                                <HeaderTemplate>
                                                    <input id="chkbox-seleccionar-todos" onclick="seleccionarTodos();" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkSeleccionado" ToolTip="Seleccionar" CssClass="chk-ctrl-carreras" />
                                                </ItemTemplate>
                                                <HeaderStyle BorderStyle="none" />
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="16px" Height="16px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div class="ui-state-highlight ui-corner-all">
                                                <p>
                                                    <span class="ui-icon ui-icon-info" style="display: inline-block; vertical-align: middle; margin-top: 0px"></span>
                                                    La b&uacute;squeda no produjo resultados
                                                </p>
                                            </div>
                                        </EmptyDataTemplate>
                                        <PagerTemplate>
                                            <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" style="width: 60%;">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ToolTip="Primera Página" CommandName="Page" CommandArgument="First" runat="server" ID="ImgeBtnFirst" CssClass="boton" ImageUrl="~/Controls/Images/PgFirst.gif" />
                                                                </td>

                                                                <td>
                                                                    <asp:ImageButton ToolTip="Página Anterior" CommandName="Page" CommandArgument="Prev" runat="server" ID="ImgbtnPrevious" CssClass="boton" ImageUrl="~/Controls//Images/PgPrev.gif" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblpageindx" CssClass="labelBold" Text="Página : " runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ToolTip="Goto Page" ID="ddlPageSelector" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSelector_SelectedIndexChanged" CssClass="combo_common_nowidth" Visible="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ToolTip="Siguiente Página" CommandName="Page" CommandArgument="Next" runat="server" CssClass="boton" ID="ImgbtnNext" ImageUrl="~/Controls/Images/PgNext.gif" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ToolTip="Última Página" CommandName="Page" CommandArgument="Last" runat="server" CssClass="boton" ID="ImgbtnLast" ImageUrl="~/Controls/Images/PgLast.gif" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelRows" runat="server" Text="Resultados por página:" />
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DropDownListPageSize" runat="server" AutoPostBack="true" CssClass="DDControl form-control" Style="cursor: pointer;" OnSelectedIndexChanged="DropDownListPageSize_SelectedIndexChanged">
                                                                        <asp:ListItem Value="5" />
                                                                        <asp:ListItem Value="10" />
                                                                        <asp:ListItem Value="15" />
                                                                        <asp:ListItem Value="20" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 40%" align="right"></td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                        <HeaderStyle CssClass="th tabla_titulo_marco_general" />
                                        <RowStyle CssClass="tr" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-xs-8 col-sm-7 col-md-7 col-xs-offset-3 col-sm-offset-5 col-md-offset-5">
                    <div class="opciones_formulario">
                        <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-green btn-md" Text="Vincular" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="6" class="btn btn-cancel btn-md" OnClick="btnCancelar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <input type="text" id="txtRedirect" clientidmode="Static" value="" runat="server" style="display: none;" />
    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(loadControls);

        function loadControls(sender, args) {
            $('.boton').button();
        }
    </script>
</asp:Content>

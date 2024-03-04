<%@ Page Title="" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="VincularExpediente.aspx.cs" Inherits="POV.Web.PortalUniversidad.Pages.VincularExpediente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/gridview.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();
            iniciarControlesGrid();
        }

        function reloadControlsGrid() {
            setTimeout(function () {
                iniciarControlesGrid();
            }, 300);
        }

        function iniciarControlesGrid() {
            $('#<%=grdAlumnos.ClientID%> tbody tr.tr').mouseover(function () {
                if (!$(this).hasClass('selectedRow'))
                    $(this).addClass('selectRow');
            }).mouseout(function () {
                $(this).removeClass('selectRow');
            });

            $(".chk-ctrl-alumnos").children("input[type='checkbox']").each(function (i) {
                if ($(this).is(':checked')) $(".tr").eq(i).toggleClass('selectedRow');
            });

            $('#<%=grdAlumnos.ClientID%>').delegate('tbody tr.tr', 'click', function () {
                if ($(this).hasClass('tr')) {
                    var row = $(this).index() - 1;
                    $(this).toggleClass('selectedRow');

                    //no seleccionado
                    if ($(this).hasClass('selectedRow')) {
                        $(this).removeClass('selectRow');
                        $(".chk-ctrl-alumnos").children("input[type='checkbox']").each(function (i) {
                            if (i == row) $(this).prop('checked', true);
                        });
                    }

                        //Seleccionado
                    else {
                        $(this).addClass('selectRow');
                        $(".chk-ctrl-alumnos").children("input[type='checkbox']").each(function (i) {
                            if (i == row) $(this).prop('checked', false);
                        });
                    }
                }
            });
        }

        function seleccionarTodos() {

            var valorCheck = $("#chkbox-seleccionar-todos").is(':checked');

            $(".chk-ctrl-alumnos").children("input[type='checkbox']").each(function (i) {
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
    <div id="contenidoPrincipal" runat="server">
        <ol class="breadcrumb">
            <li>
                <asp:HyperLink runat="server" ID="lnkBack" NavigateUrl="~/Orientadores/BuscarOrientadores.aspx" Style="font-size: 30px !important;">Volver</asp:HyperLink>
            </li>
            <li style="font-size: 30px !important;">Vincular expediente a orientador</li>
        </ol>
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                        Orientador seleccionado
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6 col-lg-6">
                        <asp:Button ID="btnNuevaSeleccion" runat="server" Text="Nueva selección" CssClass="btn-cancel" OnClick="btnNuevaSeleccion_Click" />
                    </div>
                </div>
            </div>
            <div class="panel-body">
                <div class="table-responsive">
                    <asp:Table ID="Table1" runat="server" CssClass="table table-bordered table-striped">
                        <asp:TableRow>
                            <asp:TableCell CssClass="td col-xs-6 boldText">
                                <asp:Label ID="lbl1" runat="server" CssClass="label-control" Text="Nombre (s)"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell CssClass="td col-xs-6">
                                <asp:Label ID="lblNombre" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell CssClass="td col-xs-6 boldText">
                                <asp:Label ID="lbl2" runat="server" CssClass="label-control" Text="Apellido (s)"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell CssClass="td col-xs-6">
                                <asp:Label ID="lblApellido" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell CssClass="td col-xs-6 boldText">
                                <asp:Label ID="lbl3" runat="server" CssClass="label-control" Text="Correo"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell CssClass="td col-xs-6">
                                <asp:Label ID="lblCorreo" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell CssClass="td col-xs-6 boldText">
                                <asp:Label ID="lbl4" runat="server" CssClass="label-control" Text="Nombre de usuario"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell CssClass="td col-xs-6">
                                <asp:Label ID="lblUsuario" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Elecci&oacute;n de estudiantes
            </div>
            <div class="panel-body">
                <asp:UpdatePanel runat="server" ID="UpdatPanel1">
                    <ContentTemplate>
                        <div class="">
                            <div class="table-resposive">
                                <asp:GridView ID="grdAlumnos" runat="server" AllowPaging="true" AllowSorting="true" 
                                    AutoGenerateColumns="false" CssClass="table table-bordered table-striped" 
                                    OnPageIndexChanging="grdAlumnos_PageIndexChanging"
                                    OnPageIndexChanged="grdAlumnos_PageIndexChanged" 
                                    SortedAscendingHeaderStyle-VerticalAlign="Top" BorderStyle="None" PageSize="10" Visible="false" 
                                    OnRowDataBound="grdAlumnos_RowDataBound" DataKeyNames="AlumnoID">
                                    <Columns>
                                        <asp:BoundField HeaderText="Id" DataField="AlumnoID" HeaderStyle-CssClass="hidden_col" ItemStyle-CssClass="hidden_col" />
                                        <asp:BoundField HeaderText="Nombre" DataField="NombreCompletoAlumno">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                                <div style="text-align:center">
                                                <input id="chkbox-seleccionar-todos" onclick="seleccionarTodos();" type="checkbox" />
                                                    </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSeleccionado" runat="server" ToolTip="Seleccionar" CssClass="chk-ctrl-alumnos" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="th" />
                                            <ItemStyle  HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div>No se encontraron estudiantes para mostrar</div>
                                    </EmptyDataTemplate>
                                    <HeaderStyle CssClass="th" />
                                    <RowStyle CssClass="tr" />
                                    <SortedAscendingCellStyle VerticalAlign="Top" />
                                </asp:GridView>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="col-xs-12 form-group center-block">
                    <asp:Button runat="server" ID="btnAsignar" Text="Asignar" CssClass="btn-green" OnClick="btnAsignar_Click" />
                    <asp:Button runat="server" ID="btnCancelar" Text="Deseleccionar" CssClass="btn-cancel" OnClick="btnCancelar_Click" />

                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                Estudiantes asignados
            </div>
            <div class="panel-body">
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <div class="">
                            <div class="table-responsive">
                                <asp:GridView ID="grdPreAsignaciones" runat="server" AllowPaging="true" AllowSorting="true" 
                                    AutoGenerateColumns="false" CssClass="table table-bordered table-striped" RowStyle-CssClass="tr" 
                                    HeaderStyle-CssClass="th" OnPageIndexChanging="grdPreAsignaciones_PageIndexChanging"
                                    SortedAscendingHeaderStyle-VerticalAlign="Top" BorderStyle="None" PageSize="10"
                                     OnRowDataBound="grdPreAsignaciones_RowDataBound" OnRowCommand="grdPreAsignaciones_RowCommand" 
                                    OnPageIndexChanged="grdPreAsignaciones_PageIndexChanged">
                                    <Columns>
                                        <asp:BoundField HeaderText="Nombre" DataField="NombreCompletoAlumno">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/images/VOCAREER_suprimir.png" ToolTip="Eliminar" CommandName="Eliminar" CommandArgument='<%#Eval("AlumnoID") %>' Visible="false" />
                                            </ItemTemplate>
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
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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

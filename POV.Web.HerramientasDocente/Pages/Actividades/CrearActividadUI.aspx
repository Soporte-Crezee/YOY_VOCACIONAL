<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="CrearActividadUI.aspx.cs" Inherits="POV.Web.HerramientasDocente.Pages.Actividades.CrearActividadUI" %>

<%@ Register Src="AgregarContenidoDigitalUC.ascx" TagPrefix="uc1" TagName="AgregarContenidoUC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<% =Page.ResolveClientUrl("~/Content/Styles/gridview.css")%>" rel="stylesheet" />
    <link href="<% =Page.ResolveClientUrl("~/Content/Styles/fileUpload.css")%>" rel="stylesheet" />
    <script src="<% =Page.ResolveClientUrl("~/Content/Scripts/fileUpload.js")%>" type="text/javascript"></script>

    <style type="text/css">
        .selector_view {
            display: block;
            text-decoration: none;
        }

        tr.tab_selector {
            font-size: 14pt;
            font-weight: bold;
        }

            tr.tab_selector td {
                padding: 5px 0 5px 8px;
                margin: 0;
                border: 1px solid #ccc;
                color: #aaa;
                width: 16%;
            }

                tr.tab_selector td.seleccionado {
                    background-color: #77aea9;
                    color: #fff !important;
                }

                    tr.tab_selector td.seleccionado a {
                        color: #fff !important;
                    }

        .divPrincipal {
            padding-bottom: 20px !important;
        }
        #contenedorfinal {
            height:auto !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            var dtpOptions = {
                changeYear: true,
                changeMonth: true,
                dateFormat: 'DD, d MM , yy',
                onSelect: function (date, instance) {
                    //se decodifica los html entities para las palabras acentuadas
                    var decoded = $('<div/>').html($(this).val()).text();
                    $(this).val(decoded);
                    var selectedDay = instance.selectedDay.toString().length == 1 ? "0" + instance.selectedDay : instance.selectedDay;
                    var selectedMonth = instance.selectedMonth.toString().length == 1 ? "0" + (instance.selectedMonth + 1) : (instance.selectedMonth + 1);
                    //se cambia el formato de la fecha 
                    var selectedDate = selectedDay + "/" + selectedMonth + "/" + instance.selectedYear;
                    $(this).next("input").val(selectedDate);
                }
            };

            $(".datepicker-padre").datepicker(dtpOptions).keydown(function () { return false; });

        });
        $(function () {
            var tabSeleccionado = "#" + $("#<%# hdnTabSeleccionado.ClientID %>").val();

            $(tabSeleccionado).addClass("seleccionado");

            $.blockUI.defaults.overlayCSS.backgroundColor = "white";
            $.blockUI.defaults.message = '<h1 style="font-size:20px;">Registrando, por favor espere...</h1>';

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bodyadaptable">
        <div class="divPrincipal col-xs-12">
            <div class="titulo_principal_superior col-xs-12">
                <asp:Label ID="lblTitulo" Text="Agregar nueva actividad" runat="server"></asp:Label>
            </div>
            <div class="divContenido col-xs-12">
                <div class="col-xs-12 col-md-6" style="padding: 0px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco">
                        <asp:Label runat="server" ID="lblSubtitulo1" Text="Información general"></asp:Label>
                    </div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <br />
                        <div class="col-xs-12 form-group form-horizontal">
                            <label class="col-sm-4 control-label">Nombre</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtNombreActividad" MaxLength="100" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-12 form-group form-horizontal">
                            <label class="col-sm-4 control-label">Instrucciones para el estudiante</label>
                            <div class="col-sm-8">
                                <asp:TextBox runat="server" ID="txtDescripcionActividad" TextMode="MultiLine" MaxLength="500" Width="100%" CssClass="form-control" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-md-6" style="padding: 0px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco">
                        <asp:Label runat="server" ID="lblTituloGrupos" Text="Asignación automática"></asp:Label>
                    </div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <br />
                        <div class="col-xs-12 form-group">
                            <label class="col-sm-4 control-label">
                                <asp:Label ID="lblGrupo" runat="server" Text="Area de conocimiento  " CssClass="labelTalentos"></asp:Label></label>
                            <div class="col-sm-8">
                                <asp:DropDownList CssClass="form-control" ID="ddlArea" runat="server" Width="90%"></asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <br />
                        <br />
                    </div>
                </div>
            </div>
            <div class="divContenido col-xs-12">
                <div class="col-xs-12 col-md-12" style="padding: 0px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco">
                        <asp:Label runat="server" ID="lblSubtitulo2" Text="Agregar contenido"></asp:Label>
                    </div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <div id="tabs" class="ui-widget-content">
                            <asp:HiddenField ID="hdnTabSeleccionado" runat="server" Value="td-juegos" />
                            <table width="100%" border="0" cellspacing="0">
                                <tr class="tab_selector">

                                    <td id="td-subir" style="display: none;">
                                        <asp:LinkButton ID="lnkBtnAgregarcontenido" runat="server" OnClick="changeView_OnClick" CssClass="selector_view" Enabled="false"> <i class="icon_36 icon_subir_contenido36"></i>Subir contenido </asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <asp:MultiView runat="server" ID="mtvTareas" ActiveViewIndex="1">
                                <asp:View runat="server" ID="vieGhost">
                                </asp:View>
                                <asp:View runat="server" ID="viewAgregarContenido">
                                    <div style="text-align: center; margin: 20px auto;">
                                        <asp:Button ID="Button1" runat="server" Text="Subir contenido" OnClick="btnAgrarContenido_OnClick" CssClass="button_clip_39215E" Style="padding: 5px 10px;" />
                                    </div>
                                </asp:View>
                            </asp:MultiView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divContenido col-xs-12">
                <div class="col-xs-12 col-md-12" style="padding: 0px 0px 0px 0px">
                    <div class="col-xs-12 titulo_marco">
                        <asp:Label runat="server" ID="lblSubtitulo3" Text="Tarea a asignar"></asp:Label>
                    </div>
                    <div class="col-xs-12 container_busqueda_general ui-widget-content">
                        <br />
                        <asp:GridView ID="gvTareasActividad" runat="server"
                            AllowPaging="false"
                            CssClass="table table-bordered table-striped" HeaderStyle-Height="30px" RowStyle-Height="25px"
                            RowStyle-CssClass="td" HeaderStyle-CssClass="th"
                            AllowSorting="True" AutoGenerateColumns="False"
                            Width="100%"
                            SortedAscendingHeaderStyle-VerticalAlign="Top"
                            OnRowDataBound="gvTareasActividad_OnRowDataBound"
                            OnRowCommand="gvTareasActividad_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="100px">
                                    <HeaderTemplate>Tipo</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipoTarea" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre">
                                    <HeaderStyle Width="260px" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Instrucción" DataField="Instruccion">
                                    <HeaderStyle Width="360px" HorizontalAlign="Left" />
                                    <ItemStyle Width="260px" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Quitar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnQuitar" runat="server" CommandName="quitar"
                                            CommandArgument='<%# Container.DataItemIndex %>' ImageUrl="~/Content/images/VOCAREER_eliminar.png"
                                            OnClientClick="return confirm('¿Está seguro que desea quitar este elemento?');" ToolTip="Quitar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div>
                                    <span class="ui-icon ui-icon-notice"
                                        style="float: left; margin: 0 7px 50px 0;"></span>No existen tareas asignadas a la actividad, por favor asigne tareas para continuar con el registro.
                                </div>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="th"></HeaderStyle>

                            <RowStyle CssClass="tr"></RowStyle>

                            <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-xs-12">
                <div style="text-align: right">
                    <asp:Button ID="btnCrearActividad" runat="server" Text="Guardar" OnClick="btnCrearActividad_OnClick" CssClass="button_clip_39215E" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_OnClick" CssClass="btn-cancel" />
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hdnDialogResultado" />


        <button id="AgregarContenidoDigital" type="button" class="btn btn-default" data-toggle="modal" data-target=".bs-example-modal-lg" style="display: none;">Large modal</button>
        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myActividadModal" aria-hidden="true" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header color-info panel-heading">
                        <button type="button" onclick="cerrar();" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        Agregar Contenido Digital
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <uc1:AgregarContenidoUC runat="server" ID="AgregarContenidoUC" PaginaContenedora="CrearActividadUI" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">

            $(document).ready(function () {
                var dialog = $('#<%= hdnDialogResultado.ClientID %>').val();
                if (dialog == "AgregarContenidoDigital") {
                    $("#AgregarContenidoDigital").click();
                }

            });
            function CerrarDialogo() {
                $('#MainContent_hdnDialogResultado').val('');
            }

            function cerrar() {
                $('#<%= hdnDialogResultado.ClientID %>').val('');
            }
        </script>
    </div>
</asp:Content>

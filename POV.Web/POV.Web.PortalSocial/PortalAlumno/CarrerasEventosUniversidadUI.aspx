<%@ Page Title="YOY - ESTUDIANTE" Language="C#" MasterPageFile="~/PortalAlumno/PortalAlumno.master" AutoEventWireup="true" CodeBehind="CarrerasEventosUniversidadUI.aspx.cs" Inherits="POV.Web.PortalSocial.PortalAlumno.CarrerasEventosUniversidadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head_selected" runat="server">
    <style type="text/css">
        select {
            border: 0 solid #acacac;
        }

        .descripcion {
            border: 0px solid #ccc !important;
            resize: none;
            overflow-y: scroll !important;
            background-color: #fff !important;
            text-align: justify;
        }

        option {
            cursor: pointer;
            margin-right: 0.1em;
            overflow: visible;
            padding: 7px;
            position: relative;
            text-align: justify;
            text-decoration: none !important;
            background: #f6f6f6 url("../Styles/ui-lightness/images/ui-bg_glass_100_f6f6f6_1x400.png") repeat-x scroll 50% 50%;
            border: 1px solid #d7deef;
            color: #242525 !important;
            border-radius: 4px;
        }

            option:hover {
                background: #f1f1f1;
                border: 1px solid #33acfd;
                color: #1c94c4;
            }

            option::selection {
                color: #1c94c4;
            }
    </style>

    <script type="text/javascript">
        $(function () {

            $("#tabs").tabs();
            $("#tabs").removeClass('ui-widget-content');

            $("option").on('click', function () {
                $(this).attr('style', 'background: #f6f6f6;');
            });
        });

        function cargarLista() {
            setTimeout(function () {
                $('option[selected="selected"]').attr('style', 'background: #f6f6f6;');
            }, 300);
        }

        $(document).ready(initPage);

        function initPage() {
            $('.boton').button();

            $("#btnClose").on("click", function () {
                $('#<%=txtDescripcion.ClientID %>').val('No se pudo visualizar la descripción del evento');
            });
        }

        function viewModal() {
            window.location = "#btnVerDescripcion";
            setTimeout(function () {
                $("#btnVerDescripcion").click();
            }, 500);
            setTimeout(function () {
                $('option[selected="selected"]').attr('style', 'background: #f6f6f6;');
            }, 300);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_selected" runat="server">
    <div id="info_perfil">
        <h1 class="tBienvenida">Eventos de la escuela
        </h1>
    </div>

    <button type="button" id="btnVerDescripcion" class="button_clip_39215E" data-toggle="modal" style="display: none;" data-target=".dialog-md">Modal Dialog</button>
    <div class="modal fade dialog-md" tabindex="-1"
        data-keyboard="false" data-backdrop="static"
        role="dialog" aria-labelledby="ventanaModalLabel" id="ventanaModal">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <div runat="server" id="divTitulo" class="col-xs-12 modal_titulo_marco_general center-block"></div>
                </div>
                <div class="modal-body container_busqueda_general ui-widget-content">
                    <div class="">
                        <div class="row" style="border-bottom: 0;">
                            <div class="col-md-12" style="padding: 0px 0px 0px 0px">
                                <div class="col-xs-12">
                                    <div class="col-xs-12 form-group">
                                        <asp:TextBox runat="server" ID="txtDescripcion" TextMode="MultiLine" MaxLength="2000" Rows="15" ReadOnly="true" CssClass="form-control descripcion"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="form-group">
                        <div class="col-md-6 pull-right">
                            <button type="button" class="btn-green" id="btnClose" data-dismiss="modal">Aceptar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="col-xs-12 col-md-12 col-lg-12" style="padding: 20px 0px 0px 0px">
        <div class="col-xs-12 titulo_marco_general">
            <asp:Label ID="lbUniversidades" runat="server" CssClass="text-color"></asp:Label>
        </div>
        <div class="col-xs-12 form-group center-block container_busqueda_general ui-widget-content" style="background-color: #fbf9f9">
            <div class="table table-responsive" style="padding: 20px 0px 0px 0px">
                <asp:GridView ID="grvEventos" AllowPaging="True" AllowSorting="True" CssClass="table table-bordered table-responsive"
                    RowStyle-CssClass="tr" runat="server"
                    HeaderStyle-CssClass="th" OnPageIndexChanging="grvEventos_OnPageIndexChanging"
                    DataKeyNames="EventoUniversidadId" SortedAscendingHeaderStyle-VerticalAlign="Top"
                    AutoGenerateColumns="False" PageSize="10"
                    OnRowCommand="grvEventos_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="Id" DataField="TareaId" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Fecha incio" DataField="FechaInicio" Visible="True">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Fecha fin" Visible="True" DataField="FechaFin"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imbVerDescripcion" runat="server"
                                    CommandName="Ver_Descripcion"
                                    ImageUrl="~/Images/icons/VOCAREER_buscar.png"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EventoUniversidadId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <Columns>
                    </Columns>
                    <EmptyDataTemplate>
                        <div style="background-color: #f3f3f4;">La escuela no tiene eventos programados</div>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="th tabla_titulo_marco_general" />
                    <RowStyle CssClass="tr" />
                    <SortedAscendingHeaderStyle VerticalAlign="Top"></SortedAscendingHeaderStyle>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnSocialHubID" runat="server" />
    <asp:HiddenField ID="hdnUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnSessionSocialHubID" runat="server" />
    <asp:HiddenField ID="hdnSessionUsuarioSocialID" runat="server" />
    <asp:HiddenField ID="hdnFuente" runat="server" Value="A" />
</asp:Content>

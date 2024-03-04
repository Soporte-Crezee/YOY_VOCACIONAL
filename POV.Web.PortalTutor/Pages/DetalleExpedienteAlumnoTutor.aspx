<%@ Page MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="DetalleExpedienteAlumnoTutor.aspx.cs" Inherits="POV.Web.PortalTutor.Pages.DetalleExpedienteAlumnoTutor" %>

<asp:Content ID="Conten1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.tab-item').css('cursor', 'pointer');
        });

        $(function () {
            $('div.panel-heading.clickable').click(function () {
                var $panel = $(this).parent();
                var $panelContainerBody = $panel.find('div.panel-collapse');
                var $panelBody = $panelContainerBody.find('.panel-body');
                if ($panelContainerBody.hasClass('in')) {
                    $panelContainerBody.removeClass('in');
                    $panelBody.slideUp();
                } else {
                    $panelContainerBody.addClass('in');
                    $panelBody.slideDown();
                }
            });
        });

        $(function () {

            $.blockUI.defaults.overlayCSS.backgroundColor = "white";

        });

        $(function () {
            $('#tabs a').click(function (e) {
                e.preventDefault()
                $(this).tab('show')
            });

        });

        $(function () {
            $("div.accordion").accordion({
                collapsible: true,
                active: false
            });

            $('.tab-item').click(function () {
                window.location = $(this).find("a").first().attr("href");
            });
        });
    </script>

    <style type="text/css">
        .clickable {
            cursor: pointer;
        }

        .selector_view {
            display: block;
            text-decoration: none;
        }

        tr.tab_selector {
            font-size: 14px;
            font-weight: bold;
        }

            tr.tab_selector td {
                padding: 5px 0 5px 8px;
                margin: 0;
                border: 1px solid #ccc;
                color: #aaa;
                width: 16%;
            }

                tr.tab_selector td:hover {
                    font-size: 14px;
                    font-weight: bold;
                    background: #f1f1f1;
                    color: #23CC9D;
                }

                tr.tab_selector td.seleccionado {
                    background-color: #0067ac;
                    color: #fff !important;
                }

                    tr.tab_selector td.seleccionado a {
                        color: #fff !important;
                    }

        .divContenido {
            background-color: #F3F3F5;
            width: 97%;
            margin: 0 auto;
        }

        tr.tab {
            height: auto;
            /*width: 200px;*/
            font-size: 14px;
            font-weight: bold;
            border: 1px solid #ccc;
            color: #aaa;
            margin: 0;
            padding: 5px 0 5px 8px;
        }

        td.tabtr {
            padding: 5px 0 5px 8px;
            margin: 0;
            border: 1px solid #ccc;
            color: #aaa;
            /*width: 16%;
            height: auto;
            width: 200px;*/
        }

        td.tabRotter {
            padding: 5px 0 5px 8px;
            margin: 0;
            color: #aaa;
            /*width: 16%;
            height: auto;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="page_content" runat="server">
    <ol class="breadcrumb">
        <li>
            <asp:HyperLink ID="HplVolver" runat="server" ToolTip="Volver" CssClass="tusuario pconfirmacorreo " NavigateUrl="ExpedienteAlumnoTutor.aspx">Volver</asp:HyperLink>
        </li>
        <li>Ver expediente</li>
    </ol>
    <div id="info_alumno" runat="server" class="panel panel-default" visible="false">
        <div class="panel-heading">
            <asp:Label ID="LblNombreUsuario" runat="server" Text="" Visible="false"></asp:Label>
            <h1 class="" visible="false">Expediente del estudiante</h1>
        </div>
        <div class="panel-body">
            <asp:Label ID="LblNombreGrupo" runat="server" Text="" Visible="false"></asp:Label>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            Informaci&oacute;n del hijo
        </div>
        <div class="panel-body">
            <div>
                <ul class="nav nav-tabs" role="tablist" id="tabs">
                    <li role="presentation" class="active">
                        <a href="#infoPersonal" aria-controls="infoPersonal" role="tab" data-toggle="tab">Informaci&oacute;n personal</a>
                    </li>
                    <li role="presentation">
                        <a href="#historialServicios" aria-controls="historialServicios" role="tab" data-toggle="tab">Historial de servicios</a>
                    </li>
                </ul>
            </div>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="infoPersonal">
                    <div class="form-horizontal" style="padding-top: 20px;">
                        <div class="form-group">
                            <!-- B1 Nombre -->
                            <label class="col-sm-2 control-label">Nombre</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtNombre" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <!-- B4 Escuela de Procedencia -->
                            <label class="col-sm-2 control-label">Escuela de procedencia</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtEscuela" MaxLength="30" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="text-center">
                                <label class="label-header">&Aacute;reas de conocimiento</label>
                            </div>
                            <div class="table-responsive col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8">
                                <!-- B7 Areas d concomiento -->
                                <asp:GridView AutoGenerateColumns="false" runat="server"
                                    ID="gvAreasConocimiento" CssClass="table table-bordered table-striped"
                                    RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                    <Columns>
                                        <asp:BoundField HeaderText="&Aacute;reas de conocimiento del estudiante" DataField="Nombre">
                                            <HeaderStyle Width="460px" HorizontalAlign="Left" />
                                            <ItemStyle Width="460px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Historial de Servicios -->
                <div role="tabpanel" class="tab-pane" id="historialServicios">
                    <div class="form-horizontal" style="padding-top: 20px;">
                        <!-- A1 Intereses -->
                        <div class="form-group">
                            <div class="text-center">
                                <h3>
                                    <label class="label-header">Intereses</label>
                                </h3>
                            </div>
                            <div class="table-responsive col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8">
                                <asp:GridView AutoGenerateColumns="false" runat="server" ID="gvIntereses" CssClass="table table-bordered table-striped"
                                    RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                    <Columns>
                                        <asp:BoundField HeaderText="Intereses del estudiante" DataField="NombreInteres">
                                            <HeaderStyle Width="460px" HorizontalAlign="Left" />
                                            <ItemStyle Width="460px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <%-- Tabla de la prueba de habitos de estudio --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultados">
                            <div class="text-center">
                                <h2 class="label-header">Resultado de pruebas realizadas</h2>
                            </div>
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionHabitos" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingHabitos">
                                        <h3 class="panel-title">
                                            <label class="label-header" runat="server" id="lbResultadoGratis1" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                        </h3>
                                    </div>
                                    <div id="collapseHabitos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingHabitos">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label runat="server" ID="lblPuntaje" Text="Puntaje"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label runat="server" ID="lblNiveles" Text="Niveles"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label runat="server" ID="lblRecomendaciones" Text="Recomendaciones"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td050">
                                                            <asp:Label runat="server" ID="lblp1" Text="0 - 50"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdMal">
                                                            <asp:Label runat="server" ID="lbln1" Text="Malo"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdMBajo" style="text-align: left;">
                                                            <asp:Label runat="server" ID="lblr1" Text="Muy Bajo (Necesita cambiar y aprender)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td5175">
                                                            <asp:Label runat="server" ID="lblp2" Text="51 - 75"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdReg">
                                                            <asp:Label runat="server" ID="lbln2" Text="Regular"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdBajo" style="text-align: left;">
                                                            <asp:Label runat="server" ID="lblr2" Text="Bajo (Necesita cambiar y aprender)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td7695">
                                                            <asp:Label runat="server" ID="lblp3" Text="76 - 95"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdBue">
                                                            <asp:Label runat="server" ID="lbln3" Text="Bueno"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdMedio" style="text-align: left;">
                                                            <asp:Label runat="server" ID="lblr3" Text="Medio (Debe aprender a mejorar)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td96115">
                                                            <asp:Label runat="server" ID="lblp4" Text="96 - 115"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdMuy">
                                                            <asp:Label runat="server" ID="lbln4" Text="Muy bueno"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdAlto" style="text-align: left;">
                                                            <asp:Label runat="server" ID="lblr4" Text="Alto (Adecuado)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td116150">
                                                            <asp:Label runat="server" ID="lblp5" Text="116 - 150"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdExc">
                                                            <asp:Label runat="server" ID="lbln5" Text="Excelente"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="tdMAlto" style="text-align: left;">
                                                            <asp:Label runat="server" ID="lblr5" Text="Muy Alto (Adecuado)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Tabla de la prueba de dominos --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosDominos">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionDominos" role="tablist" aria-multiselectable="true">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingDominos">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionDominos" href="#collapseDominos" aria-expanded="true" aria-controls="collapseDominos">-->
                                            <label class="label-header" runat="server" id="lbResultadoPremium1" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseDominos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingDominos">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="Label1" runat="server" Text="Percentiles"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="Label2" runat="server" Text="Rangos"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td1a">
                                                            <asp:Label runat="server" ID="Label3" Text="95"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td1b">
                                                            <asp:Label runat="server" ID="Label6" Text="Superior"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td2a">
                                                            <asp:Label runat="server" ID="Label2b" Text="90 - 75"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td2b">
                                                            <asp:Label runat="server" ID="Label14" Text="Superior al término medio"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td3a">
                                                            <asp:Label runat="server" ID="Label17" Text="50"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td3b">
                                                            <asp:Label runat="server" ID="Label18" Text="Término medio"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td4a">
                                                            <asp:Label runat="server" ID="Label19" Text="25 - 10"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td4b">
                                                            <asp:Label runat="server" ID="Label20" Text="Inferior al término medio"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td5a">
                                                            <asp:Label runat="server" ID="Label21" Text="5"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td5b">
                                                            <asp:Label runat="server" ID="Label22" Text="Deficiente"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Tabla de la prueba de terman --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosTerman">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionTerman" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingTerman">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionTerman" href="#collapseTerman" aria-expanded="false" aria-controls="collapseTerman">-->
                                            <label class="label-header" runat="server" id="lblResultadoTerman" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseTerman" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTerman">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="table-responsive">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="Label5" runat="server" Text="Serie"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="Label7" runat="server" Text="Categoría"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="Label25" runat="server" Text="Puntuación"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="Label26" runat="server" Text="Rango"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td1">
                                                            <asp:Label runat="server" ID="Label8" Text="I"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td2">
                                                            <asp:Label runat="server" ID="Label9" Text="Información"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td11">
                                                            <asp:Label runat="server" ID="lblPunt1Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td12">
                                                            <asp:Label runat="server" ID="lblRango1Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td3">
                                                            <asp:Label runat="server" ID="Label10" Text="II"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td4">
                                                            <asp:Label runat="server" ID="Label11" Text="Juicio"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td13">
                                                            <asp:Label runat="server" ID="lblPunt2Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td14">
                                                            <asp:Label runat="server" ID="lblRango2Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td5">
                                                            <asp:Label runat="server" ID="Label12" Text="III"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td6">
                                                            <asp:Label runat="server" ID="Label13" Text="Vocabulario"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td15">
                                                            <asp:Label runat="server" ID="lblPunt3Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td16">
                                                            <asp:Label runat="server" ID="lblRango3Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td7">
                                                            <asp:Label runat="server" ID="Label15" Text="IV"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td8">
                                                            <asp:Label runat="server" ID="Label16" Text="Síntesis"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td17">
                                                            <asp:Label runat="server" ID="lblPunt4Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td18">
                                                            <asp:Label runat="server" ID="lblRango4Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td9">
                                                            <asp:Label runat="server" ID="Label23" Text="V"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td10">
                                                            <asp:Label runat="server" ID="Label24" Text="Concentración"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td19">
                                                            <asp:Label runat="server" ID="lblPunt5Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td20">
                                                            <asp:Label runat="server" ID="lblRango5Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td21">
                                                            <asp:Label runat="server" ID="Label35" Text="VI"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td22">
                                                            <asp:Label runat="server" ID="Label36" Text="Análisis"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td23">
                                                            <asp:Label runat="server" ID="lblPunt6Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td24">
                                                            <asp:Label runat="server" ID="lblRango6Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td25">
                                                            <asp:Label runat="server" ID="Label39" Text="VII"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td26">
                                                            <asp:Label runat="server" ID="Label40" Text="Abstracción"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td27">
                                                            <asp:Label runat="server" ID="lblPunt7Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td28">
                                                            <asp:Label runat="server" ID="lblRango7Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td29">
                                                            <asp:Label runat="server" ID="Label43" Text="VIII"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td30">
                                                            <asp:Label runat="server" ID="Label44" Text="Planeación"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td31">
                                                            <asp:Label runat="server" ID="lblPunt8Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td32">
                                                            <asp:Label runat="server" ID="lblRango8Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td33">
                                                            <asp:Label runat="server" ID="Label47" Text="IX"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td34">
                                                            <asp:Label runat="server" ID="Label48" Text="Organización"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td35">
                                                            <asp:Label runat="server" ID="lblPunt9Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td36">
                                                            <asp:Label runat="server" ID="lblRango9Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td37">
                                                            <asp:Label runat="server" ID="Label51" Text="X"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td38">
                                                            <asp:Label runat="server" ID="Label52" Text="Atención"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td39">
                                                            <asp:Label runat="server" ID="lblPunt10Terman"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td40">
                                                            <asp:Label runat="server" ID="lblRango10Terman"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="text-center">
                                                <h3>Tabla C.I.</h3>
                                            </div>
                                            <div class="table-responsive">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label27" runat="server" Text="Puntos totales"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="PuntosTerman" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td41">
                                                            <asp:Label runat="server" ID="Label29" Text="Rango"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td42">
                                                            <asp:Label runat="server" ID="RangoCI"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" runat="server" id="td43">
                                                            <asp:Label runat="server" ID="Label31" Text="C.I."></asp:Label>
                                                        </td>
                                                        <td class="tabtr" runat="server" id="td44">
                                                            <asp:Label runat="server" ID="ValorCI"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Tabla de la prueba de Raven --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadoRaven">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="accordionRaven" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingRaven">
                                        <h3 class="panel-title">
                                            <label class="label-header" runat="server" id="lblResultadoRaven" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                        </h3>
                                    </div>
                                    <div id="collapseRaven" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingRaven">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="text-center">
                                                <h2>Obtenci&oacute;n de discrepancias</h2>
                                            </div>
                                            <div class="">
                                                <table class="table" style="text-align: center;">
                                                    <tr class="tab">
                                                        <td style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label61" runat="server" Style="font-weight: bolder;" Text="A"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label53" runat="server" Style="font-weight: bolder;" Text="B"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label57" runat="server" Style="font-weight: bolder;" Text="C"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label58" runat="server" Style="font-weight: bolder;" Text="D"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label59" runat="server" Style="font-weight: bolder;" Text="E"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label60" runat="server" Style="font-weight: bolder;" Text="Total"></asp:Label></td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="">
                                                            <asp:Label ID="Label4" runat="server" Style="font-weight: bolder;" Text="Puntuación Directa"> </asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpDirectaA" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpDirectaB" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpDirectaC" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpDirectaD" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpDirectaE" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenTotal" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="">
                                                            <asp:Label ID="Label30" runat="server" Style="font-weight: bolder;" Text="Puntuación Esperada"> </asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpEsperadaA" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpEsperadaB" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpEsperadaC" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpEsperadaD" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenpEsperadaE" runat="server"></asp:Label></td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="">
                                                            <asp:Label ID="Label69" runat="server" Style="font-weight: bolder;" Text="Discrepancia"> </asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenDiscrepanciaA" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenDiscrepanciaB" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenDiscrepanciaC" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenDiscrepanciaD" runat="server"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblRavenDiscrepanciaE" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="">
                                                <table class="table" style="text-align: center;">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" id="tdvalidezraben" runat="server" style="text-align: center;">
                                                            <asp:Label ID="lblValidezRaven" runat="server" Style="font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="4" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="">
                                                <table class="table" style="text-align: center;">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="3" style="border-bottom-color: white; border-top-color: white; border-left-color: white; border-right-color: white;"></td>
                                                        <td class="tabtr" colspan="3" style="border-bottom-color: white; border-top-color: white; border-left-color: white;"></td>
                                                        <td class="tabtr" colspan="3">
                                                            <asp:Label ID="Label33" runat="server" Text="Edad:" Style="font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" colspan="3">
                                                            <asp:Label ID="lblEdadRaven" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="">
                                                <table class="table" style="text-align: center;">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="Label28" runat="server" Text="Análisis cuantitativo" Style="font-weight: bolder;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="font-weight: bolder;">
                                                            <asp:Label ID="Label32" runat="server" Text="Puntaje Escalar"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label runat="server" ID="lblRavenPuntaje"></asp:Label></td>
                                                        <td class="tabtr" style="font-weight: bolder;">
                                                            <asp:Label ID="Label37" runat="server" Text="Percentil"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label runat="server" ID="lblRavenPercentil"></asp:Label></td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="font-weight: bolder;">
                                                            <asp:Label ID="Label62" runat="server" Text="Discrepancia (SI/NO)"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label runat="server" ID="lblRavenDiscrepancia"></asp:Label></td>
                                                        <td class="tabtr" style="font-weight: bolder;">
                                                            <asp:Label ID="Label63" runat="server" Text="Rango"></asp:Label></td>
                                                        <td class="tabtr">
                                                            <asp:Label runat="server" ID="lblRavenRango"></asp:Label></td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="Label65" runat="server" Text="Diagnóstico" Style="font-weight: bolder;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblRavenDiagnostico" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>                        

                        <%-- Tabla de la prueba de Sacks --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosSacks">
                            <div class="panel-group  col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionSacks" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingSacks">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionSacks" href="#collapseSacks" aria-expanded="false" aria-controls="collapseSacks">-->
                                            <label class="label-header" runat="server" id="lblResultadoSacks" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseSacks" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingSacks">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1; text-align: center;">
                                                            <asp:Label ID="lblTituloSacks" runat="server" Text="Interpretaci&oacute;n de resultados"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloMadurez" runat="server" Text="Madurez"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblMadurez" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloRealidad" runat="server" Text="Nivel de realidad"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblNivelRealidad" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloConflictos" runat="server" Text="Manera en que los conflictos son expresados"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblConflictos" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Tabla de la prueba Cleaver --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosCleaver">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionCleaver" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingCleaver">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionCleaver" href="#collapseCleaver" aria-expanded="false" aria-controls="collapseCleaver">-->
                                            <label class="label-header" runat="server" id="lblResultadoCleaver" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseCleaver" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingCleaver">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="3" style="background-color: #f1f1f1; text-align: center;">
                                                            <asp:Label ID="lblTituloCleaver" runat="server" Text="Interpretaci&oacute;n Cleaver"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloCaracteristica" runat="server" Text="Caracter&iacute;stica"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloTag" runat="server" Text="Tags"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloTexto" runat="server" Text="Texto"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trDMore" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAE" runat="server" Text="Alto en empuje"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblDmas" runat="server" Text="D+"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextDmas" runat="server" Text="Le apasionan los retos. Puede ser considerado temerario por los dem&aacute;s. Siempre listo a la 
                                                competencia. Cuando algo esta en juego, sale lo mejor de &eacute;l. Tiene respeto por aquellos que ganan contra todas las expectativas."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trDLess" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblBE" runat="server" Text="Bajo en empuje"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblDmenos" runat="server" Text="D-"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextDmenos" runat="server" Text="Son personas apacibles que buscan la paz y la armon&iacute;a. En 
                                                donde existen problemas, ellos preferirán que sean otros los que inicien la acci&oacute;n, quiz&aacute; hasta sacrificando 
                                                su propio inter&eacute;s para adaptarse a las soluciones impuestas."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trIMore" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAI" runat="server" Text="Alto en influencia"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblImas" runat="server" Text="I+"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextImas" runat="server" Text="Abierto, persuasivo y sociable. Generalmente optimista, puede ver algo bueno en cualquier 
                                                situaci&oacute;n. Interesado principalmente en la gente, sus problemas y actividades. Dispuesto a ayudar a otros a promover sus proyectos, 
                                                as&iacute; como los suyos propios."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trILess" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblBI" runat="server" Text="Bajo  en influencia"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblImenos" runat="server" Text="I-"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextImenos" runat="server" Text="L&oacute;gicas y objetivas en todo lo que hacen, con frecuencia se acusa a estas personas de no gustar de 
                                                la gente. El problema no es de sentir atracci&oacute;n o afecto, sino lo que hacen al respecto."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trSMore" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAC" runat="server" Text="Alto en constancia"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblSmas" runat="server" Text="S+"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextSmas" runat="server" Text="Generalmente amable, tranquilo y llevadero. Es poco demostrativo y controlado. Ya que 
                                                no es de naturaleza explosiva de pronta reacci&oacute;n, puede ocultar sus sentimientos y ser rencoroso."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trSLess" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblBC" runat="server" Text="Bajo en constancia"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblSmenos" runat="server" Text="S-"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextSmenos" runat="server" Text="Flexibles, variables y activos, estas personas ponen las cosas en movimiento. La variedad 
                                                es el condimento de la vida; Adem&aacute;s, es difícil pegarle a un blanco en constante movimiento."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trCMore" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblACu" runat="server" Text="Alto en cumplimiento"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblCmas" runat="server" Text="C+"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextCmas" runat="server" Text="Es generalmente pac&iacute;fico y se adapta a las situaciones con el fin de evitar antagonismos. 
                                                Siendo sensible, busca apreciaci&oacute;n y es f&aacute;cilmente herido por otros. Es humilde leal y d&oacute;cil, tratando de hacer siempre 
                                                las cosas lo mejor posible."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab" id="trCLess" runat="server" visible="false">
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblBCu" runat="server" Text="Bajo en cumplimiento"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblCmenos" runat="server" Text="C-"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblTextCmenos" runat="server" Text="Independientes, desinhibidos y aventureros; estos esp&iacute;ritus libres disfrutan de la vida. 
                                                Cualquier cosa nueva y diferente les emociona."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Table de la prueba Chaside --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosChaside">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionChaside" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingChaside">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionChaside" href="#collapseChaside" aria-expanded="false" aria-controls="collapseChaside">-->
                                            <label class="label-header" runat="server" id="lblResultadoChaside" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseChaside" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingChaside">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="text-center">
                                                <h2 class="label-header">Tus intereses</h2>
                                            </div>
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblIntLetraUno" runat="server" Style="font-size: 80px; font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblIntCarreraUno" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblIntDescripcionUno" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblIntLetraDos" runat="server" Style="font-size: 80px; font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblIntCarreraDos" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblIntDescripcionDos" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div class="text-center">
                                                <h2>Tus aptitudes</h2>
                                            </div>
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblAptLetraUno" runat="server" Style="font-size: 80px; font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAptCarreraUno" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAptDescripcionUno" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblAptLetraDos" runat="server" Style="font-size: 80px; font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAptCarreraDos" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblAptDescripcionDos" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                         <%-- Tabla de la prueba de kuder --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosKuder">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionKuder" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingKuder">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionKuder" href="#collapseKuder" aria-expanded="false" aria-controls="collapseKuder">-->
                                            <label class="label-header" runat="server" id="lblResultadoKuder" style="cursor: pointer;">Resultado</label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseKuder" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingKuder">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="text-align: center; background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTitulo" runat="server" Text="Cuadro de actividades ocupacionales"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td45" class="tabtr" runat="server" colspan="4" style="text-align: center; background-color: #f1f1f1;">
                                                            <asp:Label runat="server" ID="Label34" Text="NOTA: La tabla que aparece a continuación, menciona únicamente algunas ocupaciones de tipo profesional y unas pocas de índole semiprofesional."></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td46" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblplantilla1"></asp:Label>
                                                        </td>
                                                        <td id="Td47" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lbldescripcion1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td48" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblplantilla2"></asp:Label>
                                                        </td>
                                                        <td id="Td49" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lbldescripcion2"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td51" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblplantilla3"></asp:Label>
                                                        </td>
                                                        <td id="Td52" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lbldescripcion3"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                         <%-- Tabla de la prueba de Frases Incompletas Vocacionales --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosFrasesVocacionales">
                            <div class="panel-group  col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionFrasesVocacionales" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingFrasesVocacionales">
                                        <h3 class="panel-title">
                                            <label class="label-header" runat="server" id="lblResultadoFrasesVocacionales" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                        </h3>
                                    </div>
                                    <div id="collapseFrasesVocacionales" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFrasesVocacionales">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1; text-align: center;">
                                                            <asp:Label ID="lblTituloFrasesVocacionales" runat="server" Text="Interpretaci&oacute;n de resultados"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloOrganizacion" runat="server" Text="Organización de personalidad y conducta de elección"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblOrganizacion" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloPerspectiva" runat="server" Text="Perspectiva de las opciones profesionales y ocupacionales"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblPerspectiva" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="background-color: #f1f1f1;">
                                                            <asp:Label ID="lblTituloFuentes" runat="server" Text="Fuentes de conflicto para la elección"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4">
                                                            <asp:Label ID="lblFuentes" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Tabla de la prueba de allport --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosAllport">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionAllport" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingAllport">
                                        <h3 class="panel-title">
                                            <!--<a role="button" data-toggle="collapse" data-parent="#acordionAllport" href="#collapseAllport" aria-expanded="false" aria-controls="collapseAllport">-->
                                            <label class="label-header" runat="server" id="lblResultadoAllport" style="cursor: pointer;">Resultado</label>
                                            <!--</a>-->
                                        </h3>
                                    </div>
                                    <div id="collapseAllport" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingAllport">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="4" style="text-align: center; background-color: #f1f1f1;">
                                                            <asp:Label ID="lbltituloAllport" runat="server" Text="Descripciones de valores según Allport"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td53" class="tabtr" runat="server" colspan="2" style="text-align: center; background-color: #f1f1f1;">
                                                            <asp:Label runat="server" ID="lblSub1" Text="Valor"></asp:Label>
                                                        </td>
                                                        <td id="Td54" class="tabtr" runat="server" colspan="2" style="text-align: center; background-color: #f1f1f1; s">
                                                            <asp:Label runat="server" ID="lblSub2" Text="Meta"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td55" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblplantillaAll1"></asp:Label>
                                                        </td>
                                                        <td id="Td56" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblObjetivoAll1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td57" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblplantillaAll2"></asp:Label>
                                                        </td>
                                                        <td id="Td58" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblObjetivoAll2"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td id="Td59" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblplantillaAll3"></asp:Label>
                                                        </td>
                                                        <td id="Td60" class="tabtr" runat="server" colspan="2" style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblObjetivoAll3"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                       <%-- Tabla de la prueba de Zavic  --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadosZavic">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="acordionZavic" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="Div3">
                                        <h3 class="panel-title">
																																																  
                                            <label class="label-header" runat="server" id="lblResultadoZavic" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                        </h3>
                                    </div>
                                    <div id="collapseZavic" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingZavic">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabtr" style="background-color: #f1f1f1; text-align: center;" colspan="2">
                                                            <asp:Label ID="Label45" runat="server" Style="font-weight: bolder;" Text="VALORES"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="background-color: #f1f1f1; text-align: center;" colspan="2">
                                                            <asp:Label ID="Label49" runat="server" Style="font-weight: bolder;" Text="INTERESES"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="Label54" runat="server" Style="font-weight: bolder;" Text="Moral"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicMoral" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label56" runat="server" Style="font-weight: bolder;" Text="Económico"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicEconomico" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="Label38" runat="server" Style="font-weight: bolder;" Text="Legalidad"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicLegalidad" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label55" runat="server" Style="font-weight: bolder;" Text="Político"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicPolitico" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="Label64" runat="server" Style="font-weight: bolder;" Text="Indiferencia"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicIndiferencia" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label66" runat="server" Style="font-weight: bolder;" Text="Social"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicSocial" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="Label68" runat="server" Style="font-weight: bolder;" Text="Corrupto"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicCorrupto" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="Label70" runat="server" Style="font-weight: bolder;" Text="Religioso"></asp:Label>
                                                        </td>
                                                        <td class="tabtr">
                                                            <asp:Label ID="lblZavicReligioso" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <%-- Tabla de la prueba de Rotter --%>
                        <div class="form-group" visible="false" runat="server" id="divTablaResultadoRotter">
                            <div class="panel-group col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8" id="accordionRotter" role="tablist" aria-multiselectable="false">
                                <div class="panel panel-default">
                                    <div class="panel-heading clickable" role="tab" id="headingRotter">
                                        <h3 class="panel-title">
                                            <label class="label-header" runat="server" id="lblResultadoRotter" style="cursor: pointer;">
                                                Resultado
                                            </label>
                                        </h3>
                                    </div>
                                    <div id="collapseRotter" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingRotter">
                                        <div class="panel-body" style="padding: 0px !important;">
                                            <div class="text-center">
                                                <h2>Locus de control</h2>
                                            </div>
                                            <div class="">
                                                <table class="table">
                                                    <tr class="tab">
                                                        <td class="tabRotter" style="text-align: right">
                                                            <asp:Label ID="lblLocusCapital" runat="server" Style="font-size: 80px; font-weight: bolder;"></asp:Label>
                                                        </td>
                                                        <td class="tabRotter" style="text-align: left;">
                                                            <asp:Label ID="lblLocusIndice" runat="server" Style="font-size: 38px; font-weight: bolder; margin-left: -15px;"></asp:Label>
                                                        </td>
                                                        <td class="tabtr" style="text-align: center;">
                                                            <asp:Label ID="lblLocusTexto" runat="server" Style="font-size: 30px;"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="tab">
                                                        <td class="tabtr" colspan="3">
                                                            <asp:Label ID="lblLocusDescripcion" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- A4 Orientador -->
                        <div class="form-group">
                            <div class="text-center">
                                <h3>
                                    <label class="label-header">Orientador(es)</label>
                                </h3>
                            </div>
                            <div class="table-responsive col-md-offset-2 col-md-8 col-lg-offset-2 col-lg-8">
                                <asp:GridView AutoGenerateColumns="false" runat="server" ID="grdOrientadores"
                                    CssClass="table table-bordered table-striped"
                                    RowStyle-CssClass="td" HeaderStyle-CssClass="th">
                                    <Columns>
                                        <asp:BoundField HeaderText="Orientador(es) asignado(s)" DataField="NombreCompletoOrientador">
                                            <HeaderStyle Width="260px" HorizontalAlign="Left" />
                                            <ItemStyle Width="260px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Tipo de orientador" DataField="NombreUniversidad">
                                            <HeaderStyle Width="260px" HorizontalAlign="Left" />
                                            <ItemStyle Width="260px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdnDialogResultado" />
        </div>
    </div>
    <script type="text/javascript">
        function CerrarDialogo() {
            $('#MainContent_hdnDialogResultado').val('');
        }
    </script>
</asp:Content>

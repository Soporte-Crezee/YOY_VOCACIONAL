<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/PruebaMaster.Master"
    AutoEventWireup="true" CodeBehind="ContestarPrueba.aspx.cs" Inherits="POV.Web.Portal.Pruebas.Pages.ContestarPrueba" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>json.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.blockUI.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.shared.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.dialogs.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.reactivo.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>jquery.waitforimages.js" type="text/javascript"></script>


    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>alertify.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>alertify.bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>alertify.default.css" rel="stylesheet" type="text/css" />
    <link href="<% =Page.ResolveClientUrl("~/Styles/")%>alertify.semantic.css" rel="stylesheet" type="text/css" />

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>date.format.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>alertify.js" type="text/javascript"></script>

    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>api.orientacionvocacional.js" type="text/javascript"></script>
    <script src="<% =Page.ResolveClientUrl("~/Scripts/")%>core.orientacionvocacional.js" type="text/javascript"></script>
    <script type="text/javascript">
        seleccionesTerman4 = 0;
        function getTermanSerie4(pregunta, preguntaid, name, check) {
            seleccionesTerman4 = 0;
            var selected = document.querySelectorAll('input[name="' + name + '"]:checked');
            if (selected.length > 0)
                seleccionesTerman4 = selected.length;

            $("#pre-" + pregunta).each(function (i) {
                if (seleccionesTerman4 > 2) {
                    check.checked = false;
                    seleccionesTerman4 -= 1;
                }
            });
        }

        function getDomino(preguntaid, opcionid) {
            var item = 0;
            var divImgId = $("#opcion-img-" + opcionid);
            var checkboxId = $("#opcion-chk-" + preguntaid + "-" + opcionid);

            $("input.check-" + preguntaid).each(function (i) {
                var checkId = $(this).attr('id');
                if (checkId == checkboxId.attr('id')) item = i;
            });

            if (item <= 6) {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i <= 6) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i <= 6) {
                        var divCheck = $(this).hasClass("domino-selected");
                        if (divCheck) {
                            $(this).removeClass("domino-selected");
                            $(this).addClass("domino");
                            var cls = $(this).addClass("domino").val();

                        }
                    }
                });
            }
            else {

                $("input.check-" + preguntaid).each(function (i) {
                    if (i > 6) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i > 6) {
                        var divCheck = $(this).hasClass("domino-selected");
                        if (divCheck) {
                            $(this).removeClass("domino-selected");
                            $(this).addClass("domino");
                            var cls = $(this).addClass("domino").val();
                        }
                    }
                });
            }

            divImgId.removeClass("domino");
            divImgId.addClass("domino-selected");

            var marcado = (checkboxId.prop("checked")) ? checkboxId.prop("checked", false) : checkboxId.prop("checked", true);
        }

        var contadorRespuestaInteres = [];
        var contArray = {};
        var checksTotal = 0;
        var itmSel = 0;
        var itmCol = -1;
        var item = 0;

        for (var i = 0; i < 4; i++) {
            contArray = {};
            for (var j = 0; j < 4; j++) {
                contArray[j] = checksTotal;
                checksTotal++;
            }
            contadorRespuestaInteres.push(contArray);
        }

        function validarRepetidos(preguntaid) {
            for (var i = 0; i < 16; i++) {
                if (i == item) itmSel = i;
            }
            for (var i = 0; i < 4; i++) {
                for (var j = 0; j < 4; j++) {
                    if (contadorRespuestaInteres[i][j] == itmSel) {
                        itmCol = j;
                        break;
                    }
                }
            }
            for (var i = 0; i < 4; i++) {
                for (var j = 0; j < 4; j++) {
                    if (contadorRespuestaInteres[i][j] == itmSel) {
                        continue;
                    }

                    if (itmCol >= 0) {
                        var itmTemp = contadorRespuestaInteres[i][itmCol];
                        var check = $("div.check-" + preguntaid).eq(itmTemp);
                        var divCheck = check.hasClass("interes-selected");
                        if (divCheck) {
                            check.removeClass("interes-selected");
                            check.addClass("interes");
                        }
                    }
                }
            }
        }

        function getInteres(preguntaid, opcionid) {
            item = 0;
            var divImgId = $("#opcion-img-" + opcionid);
            var checkboxId = $("#opcion-chk-" + preguntaid + "-" + opcionid);

            $("input.check-" + preguntaid).each(function (i) {
                var checkId = $(this).attr('id');
                if (checkId == checkboxId.attr('id')) item = i;
            });


            if (item < 4) {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i < 4) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i < 4) {
                        var divCheck = $(this).hasClass("interes-selected");
                        if (divCheck) {
                            $(this).removeClass("interes-selected");
                            $(this).addClass("interes");
                        }
                    }
                });
                validarRepetidos(preguntaid);
            }
            else if (item >= 4 && item < 8) {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i >= 4 && i < 8) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i >= 4 && i < 8) {
                        var divCheck = $(this).hasClass("interes-selected");
                        if (divCheck) {
                            $(this).removeClass("interes-selected");
                            $(this).addClass("interes");
                        }
                    }
                });

                validarRepetidos(preguntaid);
            }
            else if (item >= 8 && item < 12) {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i >= 8 && i < 12) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i >= 8 && i < 12) {
                        var divCheck = $(this).hasClass("interes-selected");
                        if (divCheck) {
                            $(this).removeClass("interes-selected");
                            $(this).addClass("interes");
                        }
                    }
                });
                validarRepetidos(preguntaid);
            }
            else if (item >= 12 && item < 16) {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i >= 12 && i < 16) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i >= 12 && i < 16) {
                        var divCheck = $(this).hasClass("interes-selected");
                        if (divCheck) {
                            $(this).removeClass("interes-selected");
                            $(this).addClass("interes");
                        }
                    }
                });
                validarRepetidos(preguntaid);
            }

            divImgId.removeClass("interes");
            divImgId.addClass("interes-selected");

            var marcado = (checkboxId.prop("checked")) ? checkboxId.prop("checked", false) : checkboxId.prop("checked", true);
        }

        function getRaven(preguntaid, opcionid) {
            var item = 0;
            var divImgId = $("#opcion-img-" + opcionid);
            var checkboxId = $("#opcion-rad-" + preguntaid + "-" + opcionid);

            $("input-rad-" + preguntaid).each(function (i) {
                var checkId = $(this).attr('id');
                if (checkId == checkboxId.attr('id')) {
                    item = i;
                }
            });

            if (item == 0 || item == 1 || item == 2 || item == 3 || item == 4 || item == 5 || item == 6 || item == 7) {
                $("input-rad-" + preguntaid).each(function (i) {
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7) $(this).prop("checked", false);
                });

                $("div.rad-" + preguntaid).each(function (i) {
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 7) {
                        var divCheck = $(this).hasClass("raven-selected");
                        if (divCheck) {
                            $(this).removeClass("raven-selected");
                            $(this).addClass("raven");
                        }
                    }
                });
            }

            divImgId.removeClass("raven");
            divImgId.addClass("raven-selected");

            var marcado = (checkboxId.prop("checked")) ? checkboxId.prop("checked", false) : checkboxId.prop("checked", true);
        }


        var sesionesAlumno = [{ sesionorientacionid: undefined }];
        var cargaSesionesAlumno = false;
        var openAlert = false;
        $(function () {
            var alumnoid = parseInt($('#<%=hdnAlumnoID.ClientID%>').val());
            alumnoFechaNacimiento = $("#<%=hdnAlumnoFechaNacimiento.ClientID%>").val();
            alumnoNombre = $("#<%=hdnAlumnoNombre.ClientID%>").val();
            alumnoPrimerApellido = $("#<%=hdnAlumnoPrimerApellido.ClientID%>").val();
            alumnoCurp = $("#<%=hdnAlumnoCurp.ClientID%>").val();
            var sesionesAl = {};
            var dto = {};
            dto.alumnoid = alumnoid;
            sesionesAl.dto = dto;

            if (alumnoid != '') {
                getSesionOrientacionAlumno(sesionesAl);
                var verificarSesiones = setInterval(startStatusSesiones, 9000000);
                var cargaStatusSesiones = setInterval(statusSesiones, 500);
            }
            function startStatusSesiones() {
                getSesionOrientacionAlumno(sesionesAl);
                cargaStatusSesiones = setInterval(statusSesiones, 500);
            }

            function statusSesiones() {
                if (alumnoid != undefined) {
                    if (cargaSesionesAlumno) {
                        SesionOrientacionAlumno();
                        clearInterval(cargaStatusSesiones);
                        cargaSesionesAlumno = false;
                    }
                }
            }

            function SesionOrientacionAlumno() {

                if (sesionesAlumno.length != undefined) {
                    var sAux = "";
                    var fecha = new Date();
                    var nowhrs = fecha.format('H:i');

                    for (var i = 0; i < sesionesAlumno.length; i++) {
                        var horaSaveFn = sesionesAlumno[i].fin.split('T')[1];
                        if (nowhrs < horaSaveFn) {
                            sAux += "Sesion de orientación" + "<br/>";
                            sAux += "Fecha: " + sesionesAlumno[i].fecha + "<br/>";
                            sAux += "Inicio: " + sesionesAlumno[i].inicio.split('T')[1] + "<br/> ";
                            sAux += "Fin: " + sesionesAlumno[i].fin.split('T')[1] + " <br/>";
                            sAux += "<br/>";
                            openAlert = true;
                        }
                    }

                    var pre = document.createElement('pre');
                    var texto = pre.appendChild(document.createTextNode(sAux.toString()));

                    //launch it.
                    if (openAlert == true) {
                        var notificationDocente = alertify.notify(texto.data, 'success', 10, function () { console.log('dismissed'); });
                    }
                }
                else {
                    clearInterval(verificarSesiones);
                    cargaSesion = false;
                }
            }
        });
    </script>
    <style type="text/css">
        .intruccionesTermanMerril {
            box-shadow: 0 0 0 2px #f1f1f1 inset;
            border-radius: 8px;
            text-align: center;
            border: 1px solid transparent;
            background-image: none;
            line-height: 1.42857;
            margin-bottom: 0;
            padding: 6px 12px;
            background-color: #05AED9;
            color: #fff !important;
            vertical-align: middle;
            white-space: nowrap;
        }

        .seccion {
            font-size: 16pt !important;
            font-weight: bold;
        }

        @media screen and (max-width:768px) {
            .letrasresponsivas {
                width: 100%;
            }
        }

        .bg-espacio-alumno {
            margin-left: -15px;
        }

        .boton_siguiente.btn-entrar {
            font-family: Roboto-Bold;
            background-color: #FFA936 !important;
            box-shadow: none !important;
            color: #FFFFFF !important;
            font-style: italic !important;
        }

        .btn-primary {
            background-color: #05aed9 !important;
            border-color: #05aed9 !important;
            font-family: Helvetica Neue LT Pro;
            font-weight: bolder !important;
            color: #fff;
            -moz-user-select: none;
            background-image: none;
            border-radius: 8px;
            cursor: pointer;
            display: inline-block;
            font-size: 12pt;
            font-weight: normal;
            line-height: 1.42857;
            margin-bottom: 0;
            padding: 6px 12px;
            text-align: center;
            vertical-align: middle;
            white-space: nowrap;
            box-shadow: inset 0 0 0 2px #f1f1f1;
            text-transform: capitalize;
        }

        .counter {
            display: inline;
        }

        .digit {
            display: inline-block;
            width: 15px;
        }

        .separator {
            display: inline-block;
            width: 1px;
        }

        .part {
            display: inline;
        }

        .content-progress {
            height: 30px;
            background-image: -webkit-linear-gradient(top, #ebebeb 0%, #e4e4e4 100%);
            background-image: -o-linear-gradient(top, #ebebeb 0%, #e4e4e4 100%);
            background-image: -webkit-gradient(linear, left top, left bottom, from(#ebebeb), to(#e4e4e4));
            background-image: linear-gradient(to bottom, #ebebeb 0%, #e4e4e4 100%);
        }

        .progress-text-style {
            line-height: 30px;
            font-size: 18pt;
        }

        .progress-bar-yoy {
            background-color: #05ADD9;
        }
    </style>
    <script id="reactivo-tmpl" type="text/x-jquery-tmpl">
        <input id="hdnReactivoID" type="hidden" value="${reactivoid}" />
        <input id="hdnRespuestaReactivoID" type="hidden" value="${respuestareactivoid}" />
        <input id="hdnTipoReactivo" type="hidden" value="${tipo}" />
        <input id="hdnHashReactivo" type="hidden" value="${hash}" />
        {{if tipopresentacion != 0}}
                <div id="reactivo-panel" class="espacio_reactivo">
                    {{if tipopresentacion == 1 || tipopresentacion == 3}}
                    <div id="reactivo-texto" class="texto_reactivo elemento_consalto">${texto}</div>
                    {{/if}}
                    {{if tipopresentacion == 2 || tipopresentacion == 3}}
                    <div id="reactivo-img" style="text-align: center;">
                        <img alt="imagen" src="${imagenurl}" class="img_reactivo" />
                    </div>
                    {{/if}}
                </div>
        <div class="separador-reactivo"></div>
        {{/if}}
            
    </script>
    <script id="pregunta-opcion-unica-tmpl" type="text/x-jquery-tmpl">
        <li id="pre-${respuestareactivoid}-${preguntaid}" class="espacio_pregunta">
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnTipoPregunta" type="hidden" value="${tiporespuesta}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnPreguntaID" type="hidden" value="${preguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnRespuestaPreguntaID" type="hidden" value="${respuestapreguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnHash" type="hidden" value="${hash}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnModo" type="hidden" value="${tiposeleccion}" />
            <div class="head_pregunta">
                {{if tipopresentacion == 1 || tipopresentacion == 3}}
                        <div id="pregunta-texto-${preguntaid}" class="texto_pregunta elemento_consalto">${texto}</div>
                {{/if}}
                 {{if tipopresentacion == 2 || tipopresentacion == 3}}
                        <div id="pregunta-img-${preguntaid}" style="text-align: center;">
                            {{if tipopruebapresentacion == 15}}
                             <img alt="imagen" src="${imagenurl}" class="img_pregunta img-thumbnail img-responsive" />
                            {{else}}
                            <img alt="imagen" src="${imagenurl}" class="img_pregunta " />
                            {{/if}}
                        </div>
                {{/if}}
            </div>

            <div class="espacio_respuestas">
                <div class="row" style="background: transparent">
                    {{if (tipopruebapresentacion == 15)}}
                    <div class="separador-pregunta"></div>
                    <div class="row">
                        <div class="col-md-12">
                            <div id="opcion-content-${preguntaid}" class="row">
                                {{each(i,op) opciones}}
										{{if (i == 0 || i % 8 === 0) }}                                         
                                {{/if}}
                             {{if (i==0 || i==2 || i==4 || i==6)}} 
                            <div class="col-md-3 col-xs-3">
                                {{/if}}
                                <div id="opcion-panel-${opcionid}" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                    <div class="opciones_container_cols checkbox">
                                        <div class="opciones_input">
                                            {{if tipopresentacion == 0 }}
                                            <input type="radio" id="opcion-${preguntaid}-${opcionid}" name="input-${preguntaid}" value="${opcionid}" onclick="verificarError('pre-${respuestareactivoid}-${preguntaid}')" />
                                            {{/if}}
                                            {{if tipopresentacion == 1 }}
                                            <input type="radio" style="margin-left: 10px; visibility: hidden;" value="${opcionid}" class="rad-${preguntaid}" name="input-${preguntaid}" id="opcion-rad-${preguntaid}-${opcionid}" />
                                            {{/if}}
                                        </div>
                                        <div class="opciones_text">
                                            {{if tipopresentacion == 0 }}
														<div id="opcion-texto-${opcionid}">
                                                            <label for="opcion-${preguntaid}-${opcionid}" onclick="verificarError('pre-${respuestareactivoid}-${preguntaid}')">${texto}</label>
                                                        </div>
                                            {{/if}}
													 {{if tipopresentacion == 1 }}
														<div onclick="javascript:getRaven('${preguntaid}','${opcionid}');verificarError('pre-${respuestareactivoid}-${preguntaid}')" class="raven rad-${preguntaid}" id="opcion-img-${opcionid}">
                                                            <img alt="imagen" src="${imagenurl}" class="img_opcion img-responsive raven" />
                                                        </div>
                                            {{/if}}
                                        </div>
                                    </div>
                                </div>
                                {{if (i==1 || i==3 || i==5 ||i==7)}} 
                            </div>
                                {{/if}} 
                                {{/each}}
                            </div>
                        </div>
                    </div>
                    <div class="separador-pregunta"></div>
                    {{else}}
                             <div id="opcion-content-${preguntaid}" class="row">
                                 <div class="col-xs-12 espacio_opcion">
                                     <ul id="opcion-content-${preguntaid}">
                                         {{each(i,op) opciones}}
                                         {{if tipopruebapresentacion == 19 || tipopruebapresentacion == 20 || tipopruebapresentacion == 21 ||
                                tipopruebapresentacion == 22 || tipopruebapresentacion == 23 || tipopruebapresentacion == 24 ||
                                tipopruebapresentacion == 17 || tipopruebapresentacion == 25 || tipopruebapresentacion == 26 ||
                                tipopruebapresentacion == 27 ||tipopruebapresentacion == 28 }}  
                                {{if (i > 0 && i % 5 === 0) }}
                            </ul>
                        </div>
                        <div class="col-xs-12 espacio_opcion">
                            <ul id="opcion-content-${preguntaid}">
                                {{/if}}
                                {{else}}       
										{{if (i > 0 && i % 3 === 0) }}
                                     </ul>
                                 </div>
                                 <div class="col-xs-12 espacio_opcion">
                                     <ul id="opcion-content-${preguntaid}">
                                         {{/if}}
                                         {{/if}}
                                {{if tipopruebapresentacion == 12 || tipopruebapresentacion == 29}}
                                <li id="opcion-panel-${opcionid}" class="col-xs-12 col-md-12 espacio_opcion" style="display: inline-block;">
                                    {{else}}
                                    {{if tipopruebapresentacion == 19 || tipopruebapresentacion == 20 || tipopruebapresentacion == 21 ||
                                tipopruebapresentacion == 22 || tipopruebapresentacion == 23 || tipopruebapresentacion == 24 ||
                                tipopruebapresentacion == 17 || tipopruebapresentacion == 25 || tipopruebapresentacion == 26 ||
                                tipopruebapresentacion == 27 ||tipopruebapresentacion == 28 }}  
                                    <li id="opcion-panel-${opcionid}" class="col-xs-12 col-md-2 espacio_opcion_bullying" style="display: inline-block;">
                                        {{else}}
										<li id="opcion-panel-${opcionid}" class="col-xs-6 col-md-4 espacio_opcion" style="display: inline-block;">
                                            {{/if}}
                                            {{/if}}
                                            <div class="opciones_container_cols checkbox">
                                                <div class="opciones_input">
                                                    <input id="opcion-${preguntaid}-${opcionid}" name="input-${preguntaid}" type="radio" value="${opcionid}" onclick="verificarError('pre-${respuestareactivoid}-${preguntaid}')" />
                                                </div>
                                                <div class="opciones_text">
                                                    {{if tipopresentacion == 0 }}
														<div id="opcion-texto-${opcionid}">
                                                            <label for="opcion-${preguntaid}-${opcionid}" onclick="verificarError('pre-${respuestareactivoid}-${preguntaid}')">${texto}</label>
                                                        </div>
                                                    {{/if}}
													 {{if tipopresentacion == 1 }}
														<div id="opcion-img-${opcionid}">
                                                            <img alt="imagen" src="${imagenurl}" class="img_opcion" />
                                                        </div>
                                                    {{/if}}
                                                </div>
                                            </div>
                                        </li>
                                         {{/each}}
                                     </ul>
                                 </div>
                             </div>
                    {{/if}}                    
                </div>
            </div>
        </li>
    </script>
    <script id="pregunta-opcion-multiple-tmpl" type="text/x-jquery-tmpl">
        <li id="pre-${respuestareactivoid}-${preguntaid}" class="espacio_pregunta">
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnTipoPregunta" type="hidden" value="${tiporespuesta}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnPreguntaID" type="hidden" value="${preguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnRespuestaPreguntaID" type="hidden" value="${respuestapreguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnHash" type="hidden" value="${hash}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnModo" type="hidden" value="${tiposeleccion}" />
            <div class="head_pregunta">
                {{if tipopresentacion == 1 || tipopresentacion == 3}}
                        <div id="pregunta-texto-${preguntaid}" class="texto_pregunta elemento_consalto">${texto}</div>
                {{/if}}
                    {{if tipopresentacion == 2 || tipopresentacion == 3}}
                        <div id="pregunta-img-${preguntaid}" style="text-align: center;">
                            <img alt="imagen" src="${imagenurl}" class="img_pregunta img-thumbnail img-responsive" />
                        </div>
                {{/if}}
            </div>
            <div class="separador-pregunta"></div>

            <div class="espacio_respuestas">
                <div class="row" style="background: transparent">
                    <div id="opcion-content-${preguntaid}" class="row">
                        {{if (tipopruebapresentacion == 2)}}
							 <div class="row centradodominos" id="opcion-content" style="margin-bottom: 10px">
                                 {{each(i,op) opciones}}                                                                     
                                 {{if (i==0 || i % 4===0) }}           
                                 <!--<div class="col-md-3 col-xs-3">-->
                                 {{/if}}
                                     {{if (i===0 || i==7) }}
                                        <div class="row perrow" style="margin-top: 15px">
                                            <div class="col-md-1 col-sm-1 col-xs-1 adapterxs"></div>
                                            {{/if}}                
                                         <div class="col-md-1 col-sm-1 col-xs-1 adapter adapterxs" id="opcion-panel-${opcionid}">
                                             <div class="checkbox opciones_container_cols">
                                                 <div class="opciones_input">
                                                     <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="${opcionid}" class="check-${preguntaid}" name="input-${preguntaid}" id="opcion-chk-${preguntaid}-${opcionid}" />
                                                 </div>
                                                 <div class="opciones_text">
                                                     {{if tipopresentacion == 0 }}
                                                        <div id="opcion-texto-${opcionid}">
                                                            <label for="opcion-chk-${preguntaid}-${opcionid}">${texto}</label>
                                                        </div>
                                                     {{/if}}                                                   
                                                     {{if tipopresentacion == 1 }}
                                                        <div id="opcion-img-${opcionid}" class="domino check-${preguntaid}" onclick="javascript:getDomino('${preguntaid}','${opcionid}');verificarError('pre-${respuestareactivoid}-${preguntaid}')">
                                                            <img alt="imagen" src="${imagenurl}" class="img_opcion img-responsive dominos" />
                                                        </div>
                                                     {{/if}}
                                                 </div>
                                             </div>
                                         </div>
                                            {{if (i==6 || i==13) }}  
                                            <div class="col-md-2"></div>
                                        </div>
                                 {{/if}}
                                     {{if (i>0 && (i==3 || i==7 || i==11 || i==13))}}        
                                 <!--</div>-->
                                 {{/if}}
                                 {{/each}}
                             </div>
                        {{else}}
                        {{if (tipopruebapresentacion == 8 || tipopruebapresentacion == 14)}}
                         <div class="">
                             {{each(i,op) opciones}}                                    
                             {{if (i == 0 || i % 4===0)}}    
                             <div class="row">
                                 {{/if}}
                                    <div class="col-xs-3 col-md-3 sinpadding">
                                        <div class="col-md-3 col-sm3 col-xs-3"></div>
                                        <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                            <div class="checkbox opciones_container_cols">
                                                <div class="opciones_input">
                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="${opcionid}" class="check-${preguntaid}" name="input-${preguntaid}" id="opcion-chk-${preguntaid}-${opcionid}" />
                                                </div>
                                                <div class="opciones_text">
                                                    {{if tipopresentacion == 0 }}
														<div id="opcion-texto-${opcionid}">
                                                            <label for="opcion-chk-${preguntaid}-${opcionid}">${texto}</label>
                                                        </div>
                                                    {{/if}}
												{{if tipopresentacion == 1 }}
                                                        <div id="opcion-img-${opcionid}" style="text-align: center" class="interes check-${preguntaid}" onclick="javascript:getInteres('${preguntaid}','${opcionid}');verificarError('pre-${respuestareactivoid}-${preguntaid}')">
                                                            <img alt="imagen" src="${imagenurl}" class="img_opcion img-responsive letrasresponsivas interes" />
                                                        </div>
                                                    {{/if}}                                               
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-sm3 col-xs-3"></div>
                                    </div>
                                 {{if (i>0 && (i==3 || i==7 || i==11 || i==15))}}                                   
                             </div>
                             {{/if}}
                             {{/each}}                             
                         </div>
                        {{else}}                        
                         <div class="col-xs-12 espacio_opcion">
                             <ul>
                                 {{each(i,op) opciones}}
										{{if (i > 0 && i % 3 === 0) }}
                             </ul>
                         </div>
                        <div class="col-xs-12 espacio_opcion">
                            <ul>
                                {{/if}}
										<li id="opcion-panel-${opcionid}" class="col-xs-12 col-sm-12 col-md-4 espacio_opcion" style="display: inline-block;">
                                            <div class="checkbox opciones_container_cols">
                                                <div class="opciones_input">
                                                    {{if clasificadorid == 14 }}                                                 
                                                    <input id="opcion-chk-${preguntaid}-${opcionid}" name="input-${preguntaid}" type="checkbox" value="${opcionid}" style="margin-left: 10px" onclick="getTermanSerie4('${respuestareactivoid}-${preguntaid}', '${preguntaid}', 'input-${preguntaid}', this)" />
                                                    {{else}}
                                                    <input id="opcion-chk-${preguntaid}-${opcionid}" name="input-${preguntaid}" type="checkbox" value="${opcionid}" style="margin-left: 10px" />
                                                    {{/if}}
                                                </div>
                                                <div class="opciones_text">
                                                    {{if tipopresentacion == 0 }}
														<div id="opcion-texto-${opcionid}">
                                                            <label for="opcion-chk-${preguntaid}-${opcionid}">${texto}</label>
                                                        </div>
                                                    {{/if}}
													{{if tipopresentacion == 1 }}
														<div id="opcion-img-${opcionid}">
                                                            <img alt="imagen" src="${imagenurl}" class="img_opcion" />
                                                        </div>
                                                    {{/if}}
                                                </div>
                                            </div>
                                        </li>
                                {{/each}}
                            </ul>
                            {{/if}}
                        {{/if}}
                        </div>
                    </div>
                </div>
                <div class="separador-pregunta"></div>
            </div>
    </script>
    <script id="pregunta-numerica-tmpl" type="text/x-jquery-tmpl">
        <li id="pre-${respuestareactivoid}-${preguntaid}" class="espacio_pregunta">
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnTipoPregunta" type="hidden" value="${tiporespuesta}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnPreguntaID" type="hidden" value="${preguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnRespuestaPreguntaID" type="hidden" value="${respuestapreguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnHash" type="hidden" value="${hash}" />
            <div class="head_pregunta">
                {{if tipopresentacion == 1 || tipopresentacion == 3}}
                        <div id="pregunta-texto-${preguntaid}" class="texto_pregunta elemento_consalto">${texto}</div>
                {{/if}}
                    {{if tipopresentacion == 2 || tipopresentacion == 3}}
                        <div id="pregunta-img-${preguntaid}" style="text-align: center;">
                            <img alt="imagen" src="${imagenurl}" class="img_pregunta" />
                        </div>
                {{/if}}
            </div>
            <div class="separador-pregunta"></div>
            <div class="espacio_respuestas">
                Tu respuesta:
                <input id="pre-${respuestareactivoid}-${preguntaid}-txtRespuesta" type="text" /></br>
                    <label id="pre-${respuestareactivoid}-${preguntaid}-lblError" class="error_textopregunta" style="display: none;">Se espera un valor num&eacute;rico.</label>
                <div>
                    <div class="separador-pregunta"></div>
        </li>
    </script>
    <script id="pregunta-corta-tmpl" type="text/x-jquery-tmpl">
        <li id="pre-${respuestareactivoid}-${preguntaid}" class="espacio_pregunta">
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnTipoPregunta" type="hidden" value="${tiporespuesta}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnPreguntaID" type="hidden" value="${preguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnRespuestaPreguntaID" type="hidden" value="${respuestapreguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnHash" type="hidden" value="${hash}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-escorta" type="hidden" value="1" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-min" type="hidden" value="${minimocaracteres}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-max" type="hidden" value="${maximocaracteres}" />
            <div class="head_pregunta">
                {{if tipopresentacion == 1 || tipopresentacion == 3}}
                            <div id="pregunta-texto-${preguntaid}" class="texto_pregunta elemento_consalto">${texto}</div>
                {{/if}}
                        {{if tipopresentacion == 2 || tipopresentacion == 3}}
                            <div id="pregunta-img-${preguntaid}" style="text-align: center;">
                                <img alt="imagen" src="${imagenurl}" class="img_pregunta" />
                            </div>
                {{/if}}
            </div>
            <div class="separador-pregunta"></div>
            <div class="espacio_respuestas">
                Tu respuesta:
                <input id="pre-${respuestareactivoid}-${preguntaid}-txtRespuesta" type="text" maxlength="${maximocaracteres}" style="width: 100%;" />
                </br>
                    <label id="pre-${respuestareactivoid}-${preguntaid}-lblError" class="error_textopregunta" style="display: none;">Se espera un texto entre ${minimocaracteres} y ${maximocaracteres} de caracteres .</label>
                <div>
                    <div class="separador-pregunta"></div>
        </li>
    </script>
    <script id="pregunta-larga-tmpl" type="text/x-jquery-tmpl">
        <li id="pre-${respuestareactivoid}-${preguntaid}-pregunta-panel" class="espacio_pregunta">
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnTipoPregunta" type="hidden" value="${tiporespuesta}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnPreguntaID" type="hidden" value="${preguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnRespuestaPreguntaID" type="hidden" value="${respuestapreguntaid}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-hdnHash" type="hidden" value="${hash}" />
            <input id="pre-${respuestareactivoid}-${preguntaid}-escorta" type="hidden" value="0" />
            <div class="head_pregunta">
                {{if tipopresentacion == 1 || tipopresentacion == 3}}
                        <div id="pregunta-texto-${preguntaid}" class="texto_pregunta elemento_consalto">${texto}</div>
                {{/if}}
                    {{if tipopresentacion == 2 || tipopresentacion == 3}}
                        <div id="pregunta-img-${preguntaid}">
                            <img alt="imagen" src="${imagenurl}" class="img_pregunta" />
                        </div>
                {{/if}}
            </div>
            <div class="separador-pregunta"></div>
            <div class="espacio_respuestas">
                Tu respuesta:
                <textarea id="pre-${respuestareactivoid}-${preguntaid}-txtRespuesta" style="width: 100%"></textarea>
            </div>
            <div class="separador-pregunta"></div>
        </li>
    </script>
    <script type="text/javascript">
        var apims = new MessageApi();
        var reloj;
        var tiempoBase = null;
        var tiempo = new Object();
        tiempo.Minuto = 0;
        tiempo.Segundo = 0;
        var tiempoTotal = 0;
        var tiempoConvertido = 0;

        var errorPregunta = [];
        var totalReactivos = 0;
        var reactivos = 0;
        var reactivosAnterior = [];
        var reactivosActual = [];
        var repetidos = 0;
        var tipopruebapresentacion = 0;
        var clasificadorid = 0;

        var alumnoFechaNacimiento = "";

        var alumnoNombre = "";
        var alumnoPrimerApellido = "";
        var alumnoCurp = "";

        $(document).ready(initPage);

        function initPage() {
            loadReactivos();
            startTimer();
        }

        //cargar todos los reactivos sin terminar
        function loadReactivos() {
            var reactivoApi = new ReactivoApi();
            var data = { dto: {} };
            var dataString = $.toJSON(data);
            displayFondoCargando();
            $("#btnResponder").css("display", "none");
            reactivoApi.getNext({
                success: function (result) {
                    displayFondoCargando();
                    totalReactivos = result.d.length;

                    for (var index = 0; index < totalReactivos; index++) {
                        reactivosAnterior.push(result.d[index].reactivoid);
                        tipopruebapresentacion = result.d[index].tipopruebapresentacion;
                        onLoadReactivoComplete(result.d[index]);
                        if (index + 1 == totalReactivos) {
                            getTotales();
                        }
                    }

                }, error: function (result) {
                    onLoadReactivoError(true);
                }
            }, dataString);
        }

        //Obtiene el total de reactivos, reactivos contestados y no contestados.
        function getTotales() {
            var reactivoApi = new ReactivoApi();
            var data = { dto: {} };
            var dataString = $.toJSON(data);
            reactivoApi.getTotalReactivos({
                success: function (result) {
                    var percent = (result.d.TotalReactivosContestados / result.d.TotalReactivos) * 100;
                    $("#reactivosPercent").css("width", percent + "%");
                    $("#percentText").text(parseInt(percent) + "%");
                }, error: function (result) {
                    onLoadReactivoError(true);
                }
            }, dataString);
        }

        function compareResult(resultAnterior, resultActual) {
            var exists = false;
            for (var i = 0; i < resultAnterior.length; i++) {
                if ($.inArray(resultAnterior[i], resultActual) !== -1) {
                    exists = true;
                    return exists;
                }
            }

            return exists;
        }

        //Responder todos los Reactivos
        function responderReactivos() {
            errorPregunta = [];

            // se valida la presentacion de Terman
            if (tipopruebapresentacion == 6) {
                $("input#hdnRespuestaReactivoID").each(function (i) {
                    var respuestaReactivoID = $("input#hdnRespuestaReactivoID").eq(i).val();
                    var reactivoID = $("input#hdnReactivoID").eq(i).val();
                    var hashReactivo = $("input#hdnHashReactivo").eq(i).val();
                    responderReactivo(respuestaReactivoID, reactivoID, hashReactivo, true);
                });
            } else {
                //se valida que se han respondido todos los reactivos
                $("input#hdnRespuestaReactivoID").each(function (i) {
                    var respuestaReactivoID = $("input#hdnRespuestaReactivoID").eq(i).val();
                    var reactivoID = $("input#hdnReactivoID").eq(i).val();
                    var hashReactivo = $("input#hdnHashReactivo").eq(i).val();
                    responderReactivo(respuestaReactivoID, reactivoID, hashReactivo, false);
                });

                //se guarda todas las respuestas de los reactivos
                if (errorPregunta.length < 1) {
                    $("input#hdnRespuestaReactivoID").each(function (i) {
                        var respuestaReactivoID = $("input#hdnRespuestaReactivoID").eq(i).val();
                        var reactivoID = $("input#hdnReactivoID").eq(i).val();
                        var hashReactivo = $("input#hdnHashReactivo").eq(i).val();
                        responderReactivo(respuestaReactivoID, reactivoID, hashReactivo, true);
                    });
                }
            }
        }

        function verificarError(idContent) {
            var liContent = $("#" + idContent);
            if (liContent.hasClass('error_pregunta')) {
                liContent.removeClass('error_pregunta');
                errorPregunta.pop();
            }
        }

        //Responder reactivo
        function responderReactivo(respuestaReactivoID, reactivoID, hashReactivo, isInsert) {

            var respuestaReactivoID = respuestaReactivoID;
            var reactivoID = reactivoID;
            var hashReactivo = hashReactivo;

            var respuestas = [];

            var isValid = true;
            $('li[id|="pre-' + respuestaReactivoID + '"]').each(function (index) {
                var currentid = $(this).attr('id');

                $(this).removeClass("error_pregunta");

                var tipo = $("#" + currentid + "-hdnTipoPregunta").val();
                var preguntaID = $("#" + currentid + "-hdnPreguntaID").val();
                var respuestaPreguntaID = $("#" + currentid + "-hdnRespuestaPreguntaID").val();
                var hashPregunta = $("#" + currentid + "-hdnHash").val();
                if (tipopruebapresentacion == 6) {
                    var varsValid = currentid != "" && tipo != '' && preguntaID != '';
                }
                else {
                    var varsValid = currentid != "" && tipo != '' && preguntaID != '' && respuestaPreguntaID != '';
                }

                if (!varsValid) {
                    isValid = false;
                    $(this).addClass("error_pregunta");
                    errorPregunta.push("error");
                    return;
                }

                if (tipo == 1) { //abierta
                    var textoRespuesta = $("#" + currentid + "-txtRespuesta").val().trim();

                    if (textoRespuesta == '') {
                        isValid = false;
                        $(this).addClass("error_pregunta");
                        errorPregunta.push("error");
                        return;
                    }


                    var tipoCorta = $("#" + currentid + "-escorta").val();
                    if (tipoCorta == 1) {
                        var minimo = $("#" + currentid + "-min").val();
                        var maximo = $("#" + currentid + "-max").val();
                        if (textoRespuesta.length >= minimo && textoRespuesta.length <= maximo) {
                            $("#" + currentid + "-lblError").css("display", "none");
                        } else {
                            $(this).addClass("error_pregunta");
                            errorPregunta.push("error");
                            isValid = false;
                            $("#" + currentid + "-lblError").css("display", "block");
                            return;
                        }
                    }

                    var respuestaPregunta = {
                        "preguntaid": preguntaID,
                        "respuestapreguntaid": respuestaPreguntaID,
                        "hash": hashPregunta,
                        "textorespuesta": textoRespuesta
                    };

                    respuestas[index] = respuestaPregunta;

                } else if (tipo == 2) { //opcion
                    var modo = $("#" + currentid + "-hdnModo").val();

                    var respuestasOpcion = [];

                    if (modo == 1) { //multiple
                        $("input[name='input-" + preguntaID + "']:checked").each(
                            function (inputindex) {
                                var seleccionMultiple = $(this).attr('value');
                                respuestasOpcion[inputindex] = {
                                    "opcionid": seleccionMultiple
                                };
                            }
                        );
                    } else if (modo == 2) {// unica	                      	
                        var seleccion = $("input[name='input-" + preguntaID + "']:checked").attr('value');
                        if (seleccion != undefined) {

                            respuestasOpcion[0] = {
                                "opcionid": seleccion
                            };
                        }
                    } else {
                        onLoadReactivoError(false, "No se puede registrar su respuesta.");
                        return;
                    }

                    if (tipopruebapresentacion != 6) {
                        if (respuestasOpcion.length == 0) {
                            isValid = false;
                            $(this).addClass("error_pregunta");
                            errorPregunta.push("error");
                            return;
                        }
                    }

                    if (tipopruebapresentacion == 2) {
                        if (respuestasOpcion.length < 2) {
                            isValid = false;
                            $(this).addClass("error_pregunta");
                            errorPregunta.push("error");
                            return;
                        }
                    }

                    if (tipopruebapresentacion == 8 || tipopruebapresentacion == 14) {
                        if (modo == 1) {
                            if (respuestasOpcion.length < 4) {
                                isValid = false;
                                $(this).addClass("error_pregunta");
                                errorPregunta.push("error");
                                return;
                            }
                        }
                    }
                    var respuestaPregunta = {
                        "preguntaid": preguntaID,
                        "respuestapreguntaid": respuestaPreguntaID,
                        "hash": hashPregunta,
                        opciones: respuestasOpcion
                    };

                    respuestas[index] = respuestaPregunta;
                } else if (tipo == 3) {//numerica
                    var textoRespuesta = $("#" + currentid + "-txtRespuesta").val();

                    if (textoRespuesta == '') {
                        isValid = false;
                        $(this).addClass("error_pregunta");
                        errorPregunta.push("error");
                        return;
                    }

                    var esNumero = validaFloat(textoRespuesta);
                    if (!esNumero) {
                        isValid = false;
                        $(this).addClass("error_pregunta");
                        errorPregunta.push("error");
                        $("#" + currentid + "-lblError").css("display", "block");
                        return;
                    }

                    $("#" + currentid + "-lblError").css("display", "none");
                    var respuestaPregunta = {
                        "preguntaid": preguntaID,
                        "respuestapreguntaid": respuestaPreguntaID,
                        "hash": hashPregunta,
                        "valorespuesta": parseFloat(textoRespuesta)
                    };

                    respuestas[index] = respuestaPregunta;
                } else {
                    onLoadReactivoError(false, "No se puede registrar su respuesta.");
                    return;
                }
            });

            if (isValid && isInsert && errorPregunta.length < 1) {
                var tiempo = tiempoConvertido;
                var data = {
                    dto: {
                        "reactivoid": reactivoID,
                        "respuestareactivoid": respuestaReactivoID,
                        "hash": hashReactivo,
                        "respuestas": respuestas,
                        "tiempo": tiempo,
                        "tipopruebapresentacion": tipopruebapresentacion
                    }
                };

                var reactivoApi = new ReactivoApi();
                var dataString = $.toJSON(data);
                displayFondoCargando();
                reactivos++;
                reactivoApi.responder({
                    success: function (result) {
                        if (result.d != null) {
                            if (result.d == 0) {
                                onLoadReactivoError(false);
                            } else if (result.d == 1) {
                                //si no ocurrio algun error la pagina carga los siguientes datos
                                if (errorPregunta.length < 1 && reactivos == totalReactivos) {
                                    $("#reactivo-panel").empty();
                                    $("#pregunta-content").empty();
                                    reactivos = 0;
                                    setTimeout(function () {
                                        displayFondoCargando();
                                        ObtenerSiguiente();
                                    }, 800);
                                }
                                //ObtenerSiguiente();                         	
                            } else {
                                location.href = "bienvenida.aspx";
                            }
                        } else {
                            location.href = "bienvenida.aspx";
                        }

                    },
                    error: function (result) {
                        onLoadReactivoError(false);
                        errorPregunta.push("error");
                    }
                }, dataString);
            } else {
                if (errorPregunta.length > 0) {
                    if (tipopruebapresentacion == 2)
                        onLoadReactivoError(false, "Una o más preguntas no tienen respuesta completa, por favor responda.");
                    else
                        onLoadReactivoError(false, "Una o más preguntas no tienen respuesta, por favor responda.");
                }
                return;
            }
        }

        function validaFloat(numero) {

            if (!/^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$/.test(numero))
                return false;
            return true;
        }

        function ObtenerSiguiente() {
            var reactivoApi = new ReactivoApi();
            var data = { dto: {} };
            var dataString = $.toJSON(data);
            $("#btnResponder").css("display", "none");
            reactivoApi.getNext({
                success: function (result) {
                    totalReactivos = result.d.length;
                    for (var i = 0; i < totalReactivos; i++) {
                        reactivosActual.push(result.d[i].reactivoid);
                    }
                    var cmp = compareResult(reactivosAnterior, reactivosActual);
                    if (cmp != true) {
                        reactivosAnterior = [];
                        reactivosActual = [];
                        repetidos = 0;
                        for (var i = 0; i < totalReactivos; i++) {
                            reactivosAnterior.push(result.d[i].reactivoid);
                            tipopruebapresentacion = result.d[i].tipopruebapresentacion;
                            onLoadReactivoComplete(result.d[i]);
                            if (i + 1 == totalReactivos) {
                                getTotales();
                            }
                        }
                    }
                    else {
                        if (repetidos < 5) {
                            repetidos++;
                            reactivosActual = [];
                            setTimeout(function () {
                                ObtenerSiguiente();
                            }, 1200);
                        }
                        else {
                            repetidos = 0;
                            location.reload();
                        }
                    }
                },
                error: function (result) {
                    onLoadReactivoError(false);
                }
            }, dataString);
        }

        function onLoadReactivoComplete(result) {
            if (result != null) {
                if (result.reactivoid != null) {

                    if (tipopruebapresentacion == 6) {
                        setTime();
                        startTimer();
                        $("#relojero").show();
                    }
                    else {

                        $("#relojero").hide();
                    }

                    if (tipopruebapresentacion == 0)
                        $("#btnSuspender").hide();
                    else
                        $("#btnSuspender").show();

                    if (tipopruebapresentacion == 2)
                        $("#instruccionesDominos").show();

                    if (tipopruebapresentacion == 9)
                        $("#instruccionesSacks").show();

                    if (tipopruebapresentacion == 10)
                        $("#instruccionesCleaver").show();

                    if (tipopruebapresentacion == 11)
                        $("#instruccionesChaside").show();

                    if (tipopruebapresentacion == 7)
                        $("#instruccionesKuder").show();

                    if (tipopruebapresentacion == 1)
                        $("#instruccionesHabitos").show();

                    if (tipopruebapresentacion == 15)
                        $("#instruccionesRaven").show();

                    if (tipopruebapresentacion == 19 || tipopruebapresentacion == 20 || tipopruebapresentacion == 21 || tipopruebapresentacion == 22 ||
                    tipopruebapresentacion == 23 || tipopruebapresentacion == 24 || tipopruebapresentacion == 17 || tipopruebapresentacion == 25 ||
                    tipopruebapresentacion == 26 || tipopruebapresentacion == 27 || tipopruebapresentacion == 28 || tipopruebapresentacion == 29)
                        $("#instruccionesBullying").show();

                    dibujarReactivo(result);
                    return;
                }
                if (result.esfinal && result.reactivoid == null) {
                    location.href = "Final.aspx";
                    //redireccionar
                    return;
                }
            } else {
                //ocurrió un error en la carga
                onLoadReactivoError(false);
            }
            hideFondoCargando();
        }

        //Dibujar reactivo
        function dibujarReactivo(data) {
            $("#btnResponder").css("display", "block");
            $("#reactivo-tmpl").tmpl(data).appendTo("#reactivo-panel");
            var reactivoID = data.reactivoid;
            var elementPregunta = "#pregunta-content";
            for (var index = 0; index < data.preguntas.length; index++) {
                clasificadorid = data.preguntas[index].clasificadorid;
                var tipo = data.preguntas[index].tiporespuesta;
                if (tipo == 1) { //abierta
                    var esCorta = data.preguntas[index].escorta;
                    if (esCorta) {
                        $("#pregunta-corta-tmpl").tmpl(data.preguntas[index]).appendTo(elementPregunta);
                    } else {
                        $("#pregunta-larga-tmpl").tmpl(data.preguntas[index]).appendTo(elementPregunta);
                    }
                } else if (tipo == 2) { //opcion
                    var tipoSeleccion = data.preguntas[index].tiposeleccion;
                    if (tipoSeleccion == 1)
                        $("#pregunta-opcion-multiple-tmpl").tmpl(data.preguntas[index]).appendTo(elementPregunta);
                    else if (tipoSeleccion == 2)
                        $("#pregunta-opcion-unica-tmpl").tmpl(data.preguntas[index]).appendTo(elementPregunta);
                } else if (tipo == 3) { //numerica
                    $("#pregunta-numerica-tmpl").tmpl(data.preguntas[index]).appendTo(elementPregunta);
                }
            }
            if (tipopruebapresentacion == 6) {
                if (clasificadorid != null) {
                    if (clasificadorid == 11) {
                        $("#instruccionesTerman1").show(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 12) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").show(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 13) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").show(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 14) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").show();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 15) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").show(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 16) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").show(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 17) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").show(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 18) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").show();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 19) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").show(); $("#instruccionesTerman10").hide();
                    }
                    if (clasificadorid == 20) {
                        $("#instruccionesTerman1").hide(); $("#instruccionesTerman2").hide(); $("#instruccionesTerman3").hide(); $("#instruccionesTerman4").hide();
                        $("#instruccionesTerman5").hide(); $("#instruccionesTerman6").hide(); $("#instruccionesTerman7").hide(); $("#instruccionesTerman8").hide();
                        $("#instruccionesTerman9").hide(); $("#instruccionesTerman10").show();
                    }
                }
            }

            if (tipopruebapresentacion == 8) {
                if (tipoSeleccion == 2) {
                    $("#instruccionesAllportParte1").show();
                    $("#instruccionesAllportParte2").hide();
                }
                else {
                    $("#instruccionesAllportParte2").show();
                    $("#instruccionesAllportParte1").hide();
                }
            }

            if (tipopruebapresentacion == 14)
                $("#instruccionesAllportParte2").show();

            $("#container").waitForImages(function () {
                hideFondoCargando();
            });
        }

        function onLoadReactivoError(resetReloj, error) {
            var strerror = error ? error : "Ocurrió un error durante la carga del reactivo.Por favor,actualice la página";
            //apims.PrepareDialog();
            apims.CreateMessage(strerror, "ERROR");
            apims.Show();
            if (resetReloj) {
                setTime();
                hideFondoCargando();
            }
        }

        function displayFondoCargando() {
            $("#fondo_cargando").css("display", "block");
            $('#prueba_container').block({ message: null });
            $("#prueba_container").css("display", "none");
        }

        function hideFondoCargando() {
            $("#fondo_cargando").css("display", "none");
            $('#prueba_container').unblock({ message: null });
            $("#prueba_container").css("display", "block");
        }

        function suspenderPrueba() {
            stopTimer();
            var strconfirmacion = "Estás seguro de querer abandonar la prueba, los cambios realizados a esta página no se guardaran." + "<br/>" +
            "Te sugerimos responder esta página y hacer clic en el boton siguiente, en la siguiente página hacer clic en suspender la prueba.";
            apims.CreateQuestionMessage(strconfirmacion, function () {
                redireccion();
            }, function () {
                startTimer();
            });
            apims.ShowQuestion();
        }

        function redireccion() {
            var reactivoApi = new ReactivoApi();
            var data = {
                alumno: {
                    "nombre": alumnoNombre,
                    "primerapellido": alumnoPrimerApellido,
                    "curp": alumnoCurp,
                    "fechanacimiento": alumnoFechaNacimiento
                }
            };
            var dataString = $.toJSON(data);
            displayFondoCargando();
            reactivoApi.redireccionSocial({
                success: function (result) {
                    displayFondoCargando();
                    if (typeof result.d.redirect != 'Undefined' && result.d.redirect.trim().length > 0)
                        window.location = result.d.redirect.trim();
                }, error: function (result) {
                    onLoadRedireccionError(true);
                }
            }, dataString);

        }

        function onLoadRedireccionError(resetReloj, error) {
            var strerror = error ? error : "Ocurrió un error durante la redireccion.Por favor, actualice la página e intente de nuevo";
            apims.CreateMessage(strerror, "ERROR");
            apims.Show();
            hideFondoCargando();
        }

        function startTimer() {
            tiempoBase = setInterval(function () {
                // Segundo
                tiempo.Segundo++;
                if (tiempo.Segundo >= 60) {
                    tiempo.Segundo = 0;
                    tiempo.Minuto++;
                }

                $("#Minuto").text(tiempo.Minuto < 10 ? '0' + tiempo.Minuto : tiempo.Minuto);
                $("#Segundo").text(tiempo.Segundo < 10 ? '0' + tiempo.Segundo : tiempo.Segundo);

                var timeMin = $("#Minuto").text();
                var timeDiv = $("#Divider").text();
                var timeSeg = $("#Segundo").text();
                tiempoTotal = timeMin + timeDiv + timeSeg;
                var conversion = 0;

                if (tiempo.Minuto >= 1) {
                    conversion = tiempo.Minuto * 60;
                    tiempoConvertido = conversion + tiempo.Segundo;
                } else {
                    tiempoConvertido = conversion + tiempo.Segundo;
                }

                if (tipopruebapresentacion == 6) {
                    if (clasificadorid == 11 && tiempoTotal == "02:00")
                        responderReactivos();

                    if (clasificadorid == 12 && tiempoTotal == "03:00")
                        responderReactivos();

                    if (clasificadorid == 13 && tiempoTotal == "03:00")
                        responderReactivos();

                    if (clasificadorid == 14 && tiempoTotal == "04:00")
                        responderReactivos();

                    if (clasificadorid == 15 && tiempoTotal == "05:00")
                        responderReactivos();

                    if (clasificadorid == 16 && tiempoTotal == "02:00")

                        responderReactivos();

                    if (clasificadorid == 17 && tiempoTotal == "02:00")
                        responderReactivos();

                    if (clasificadorid == 18 && tiempoTotal == "04:00")
                        responderReactivos();

                    if (clasificadorid == 19 && tiempoTotal == "04:00")
                        responderReactivos();

                    if (clasificadorid == 20 && tiempoTotal == "05:00")
                        responderReactivos();
                }
            }, 1000);
        }

        /* stops and resets the timer if conditions are met */
        function stopTimer() {
            clearInterval(tiempoBase);
        }

        function setTime() {
            /* setup the new clock, assign variables */
            stopTimer();
            $("#Minuto").text("00");
            $("#Divider").text(":");
            $("#Segundo").text("00");
            tiempoBase = 0;
            tiempoConvertido = 0;
            tiempoTotal = 0;
            tiempo.Minuto = 0;
            tiempo.Segundo = 0;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:HiddenField ID="hdnAlumnoID" runat="server" />
    <div id="fondo_cargando" style="display: none;"></div>
    <div id="prueba_container">
        <div class="bodyadaptable">
            <div class="row">
                <div class="col-xs-12 text-center">
                    <div class="col-sm-6 col-sm-offset-1 col-md-4 col-md-offset-2 col-lg-4 col-lg-offset-2">
                        <div class="content" id="progressBar" runat="server">
                            <p>Porcentaje de avance</p>
                            <div class="progress content-progress">
                                <div id="reactivosPercent" class="progress-bar progress-bar-yoy progress-bar-striped active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;">
                                    <span id="percentText" class="progress-text-style"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-4 col-md-4 col-lg-4">
                        <br />
                        <button type="button" onclick="suspenderPrueba();" class="boton_Finalizar btn-cancel" runat="server" id="btnSuspender">
                            <i>&nbsp;&nbsp;&nbsp;&nbsp;</i> Suspender prueba
                        </button>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-lg-6 col-sm-8 col-md-6">
                    <div class="form-group" style="margin: 0px!important">
                        <div id="work-space">
                            <div class="bg-espacio-alumno">
                                <div class="label-alumno">
                                    <asp:Label ID="lblTipoAlumno" runat="server" Text="Estudiante:"></asp:Label><br />
                                    <asp:Label ID="lblNombreAlumno" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-6 col-sm-4 col-md-6">
                    <div class="espacio-reloj" id="relojero" style="display: none; padding-top: 35px; padding-left: 15px;">
                        <div class="part">
                            <div id="Minuto" class="digit">00</div>
                            <div id="Divider" class="separator">:</div>
                            <div id="Segundo" class="digit">00</div>
                        </div>
                    </div>
                </div>
                <%--<div class="col-xs-12 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3 col-lg-6 col-lg-offset-3">
                    <div class="content" style="margin-top: 15px;">
                        <p>Total de Reactivos: <span id="totalReactivos"></span></p>
                        <p>Total de Reactivos Contestados: <span id="totalReactivosContestados"></span></p>
                        <p>Total de Reactivos No Contestados: <span id="totalReactivosNoContestados"></span></p>
                        <div class="progress" style="height: 30px;">
                            <div id="reactivosPercent" class="progress-bar progress-bar-success progress-bar-striped active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                                <span id="percentText" style="line-height: 30px; font-size: 18pt;"></span>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <div class="col-xs-12">
                    <div id="reactivo-content" class="border_div_contenedor background_div_contenedor reactivo_content bodyadaptable">
                        <div id="instruccionesTerman" class="col-xs-12 col-lg-6 col-md-6 col-sm-8">
                            <div id="instruccionesTerman1" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n I</h1>
                                <asp:Label runat="server" ID="Label10" Text="Seleccione la palabra que complete correctamente la oraci&oacute;n." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label90" Text="Tiempo : 2 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman2" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n II</h1>
                                <asp:Label runat="server" ID="Label11" Text="Lea cada cuesti&oacute;n y seleccione la mejor respuesta." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label1" Text="Tiempo : 3 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman3" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n III</h1>
                                <asp:Label runat="server" ID="Label12" Text="Cuando las dos palabras signifiquen lo mismo, selecciona la letra (I) de igual, cuando signifiquen lo opuesto, selecciona la letra (O) de opuesto" CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label2" Text="Tiempo : 3 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman4" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n IV</h1>
                                <asp:Label runat="server" ID="Label13" Text="Seleccione las dos opciones que indican algo que siempre tiene el sujeto." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label3" Text="Tiempo : 4 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman5" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n V</h1>
                                <asp:Label runat="server" ID="Label14" Text="Encuentre las respuestas lo m&aacute;s pronto posible a los problemas que se presentan." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label4" Text="Tiempo : 5 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman6" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n VI</h1>
                                <asp:Label runat="server" ID="Label15" Text="Seleccione la contestaci&oacute;n correcta." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label5" Text="Tiempo : 2 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman7" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n VII</h1>
                                <asp:Label runat="server" ID="Label16" Text="Seleccione la respuesta correcta." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label6" Text="Tiempo : 2 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman8" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n VIII</h1>
                                <asp:Label runat="server" ID="Label17" Text="Las palabras de cada una de las oraciones siguientes están mezcladas, ordene cada una.  " CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label79" Text="Si el significado de la oración es verdadero, selecciona la letra (V); si el significado es falso, selecciona la letra (F)." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label7" Text="Tiempo : 4 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman9" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n IX</h1>
                                <asp:Label runat="server" ID="Label18" Text="Seleccione la palabra que no corresponda con las dem&oacute;s." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label8" Text="Tiempo : 4 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesTerman10" style="display: none;" class="intruccionesTermanMerril">
                                <h1 class="seccion">Secci&oacute;n X</h1>
                                <asp:Label runat="server" ID="Label58" Text="Seleccione los dos n&uacute;meros que deban seguir en la serie." CssClass="elemento_consalto"></asp:Label><br />
                                <asp:Label runat="server" ID="Label9" Text="Tiempo : 5 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesDominos" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionDomino" Text="Selecciona la combinaci&oacute;n que corresponda." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesSacks" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesSacks" Text="Continua tan r&aacute;pido como te sea posible." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesCleaver" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesCleaver" Text="Selecciona M&Aacute;S o MENOS seg&uacute;n sea el caso." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesChaside" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesChaside" Text="Selecciona “SI” o “NO” seg&uacute;n sea el caso." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>

                            <div id="instruccionesAllportParte1" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesAllport1" Text="Selecciona la opción que consideres." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesAllportParte2" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesAllport2" Text="Señala 4 para opción más importante, 3 a la que le sigue, 2 la siguiente y 1 para la menos importante." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesKuder" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesKuder" Text="Indica, en cada caso, si te agrada m&aacute;s o menos." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesHabitos" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesHabitos" Text="Selecciona la alternativa que consideres." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesRaven" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="lblInstruccionesRaven" Text="“Observa esta imagen, es una figura a la cual le falta una parte...“" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                                <asp:Label runat="server" ID="lblInstruccionRaven2" Text="Selecciona para cada ejercicio la respuesta que complete correctamente la imagen que se presenta." Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesBullying" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="Label19"
                                    Text="Lee con atención cada afirmación y selecciona la opción correspondiente según la frecuencia que corresponda en cada caso." 
                                    Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                            <div id="instruccionesSocioeconomico" style="display: none; margin-left: 10px;" class="intruccionesTermanMerril">
                                <asp:Label runat="server" ID="Label20"
                                    Text="Lee con atención cada afirmación y selecciona la opción correspondiente según la frecuencia que corresponda en cada caso." 
                                    Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div id="reactivo-panel" class="col-xs-12">
                        <br />
                    </div>
                    <div class="col-xs-12">
                        <ul id="pregunta-content">
                        </ul>
                    </div>
                    <div class="col-xs-12">
                        <div id="btnResponder" style="margin: 15px auto; width: 200px;">
                            <button type="button" onclick="responderReactivos();" class="boton_siguiente btn-entrar" id="btn-reactivo-siguiente">
                                <i>&nbsp;&nbsp;&nbsp;&nbsp;</i> Siguiente
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>










    <!--<div id="dialog" title="Mensaje">
        <span id="message"></span>
    </div>-->
    <div class="modal fade" tabindex="-1" role="dialog" id="modalMessage" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="modalHeader">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="color: #FFF; opacity: 1; filter: none;" onclick="startTimer();">
                        <span aria-hidden="true" style="font-size: 25px; opacity: 1;">&times;</span>
                    </button>
                    <h4 class="modal-title" style="color: white;">YOY - ESTUDIANTES
                    </h4>
                </div>
                <div class="modal-body">
                    <p id="message"></p>
                </div>
                <div class="modal-footer" id="modalFooter">
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnAlumnoFechaNacimiento" runat="server" />
    <asp:HiddenField ID="hdnAlumnoNombre" runat="server" />
    <asp:HiddenField ID="hdnAlumnoPrimerApellido" runat="server" />
    <asp:HiddenField ID="hdnAlumnoCurp" runat="server" />
</asp:Content>

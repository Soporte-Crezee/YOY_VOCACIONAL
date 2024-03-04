<%@ Page Title="" Language="C#" MasterPageFile="~/Content/MasterPages/PruebaMaster.Master"
    AutoEventWireup="true" CodeBehind="Bienvenida.aspx.cs" Inherits="POV.Web.Portal.Pruebas.Pages.Bienvenida" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">
        var tipoPrueba = $('#tipopruebapresentacion').val();
        $(document).ready(function () {

        });

        function iniciarContestarPrueba() {
            location.href = "ContestarPrueba.aspx";
            //redireccionar
            return;
        }


        function getDomino(preguntaid, opcionid) {
            var item = 0;
            var divImgId = $("#opcion-img-" + opcionid);
            var checkboxId = $("#opcion-chk-" + preguntaid + "-" + opcionid);

            $("input.check-" + preguntaid).each(function (i) {
                var checkId = $(this).attr('id');
                if (checkId == checkboxId.attr('id')) {
                    item = i;
                }
            });

            if (item == 0 || item == 1 || item == 4 || item == 5 || item == 8 || item == 9 || item == 12) {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i == 0 || i == 1 || i == 4 || i == 5 || i == 8 || i == 9 || i == 12) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i == 0 || i == 1 || i == 4 || i == 5 || i == 8 || i == 9 || i == 12) {
                        var divCheck = $(this).hasClass("domino-selected");
                        if (divCheck) {
                            $(this).removeClass("domino-selected");
                            $(this).addClass("domino");
                        }
                    }
                });
            }
            else {
                $("input.check-" + preguntaid).each(function (i) {
                    if (i == 2 || i == 3 || i == 6 || i == 7 || i == 10 || i == 11 || i == 13) $(this).prop("checked", false);
                });

                $("div.check-" + preguntaid).each(function (i) {
                    if (i == 2 || i == 3 || i == 6 || i == 7 || i == 10 || i == 11 || i == 13) {
                        var divCheck = $(this).hasClass("domino-selected");
                        if (divCheck) {
                            $(this).removeClass("domino-selected");
                            $(this).addClass("domino");
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
            /*padding: 6px 12px;*/
            background-color: #33acfd;
            color: #fff !important;
            vertical-align: middle;
            white-space: nowrap;
        }

        .seccion {
            font-size: 16pt !important;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <div id="ContenedorPincipal" class="bodyadaptable border_div_contenedor background_div_contenedor reactivo_content">
        <div class="bodyadaptable">
            <asp:HiddenField ID="tipopruebapresentacion" runat="server" />
            <div id="bienvenida_nombre_alumno" style="text-align: center; margin-top: 35px">
                <asp:Label runat="server" CssClass="label-alumno-bienvenida" Text="Bienvenido(a): "></asp:Label>
                <asp:Label ID="lblNombreAlumno" runat="server" class="label-alumno-bienvenida" Text=""></asp:Label>
            </div>
            <div id="bienvenida_instruccion_texto" class="col-xs-12 label-instrucciones-bienvenida" style="margin-top: 10px">
                <div class="">
                    <p>
                        <asp:Label runat="server" ID="lblInstrucciones" Text="" CssClass="elemento_consalto"></asp:Label>
                    </p>
                    <div class="" id="instruccionesInicial" style="text-align: justify;" runat="server">
                        <p class="label-instrucciones-bienvenida">
                            Para iniciar tu proceso de orientaci&oacute;n te pedimos que respondas una breve encuesta. Es importante
                            que sepas que no hay respuestas correctas, se trata de conocer un poco acerca de tus intereses, por lo que
                            te pedimos que respondas con sinceridad. Al terminar la encuesta inicial deber&aacute;s confirmar tu correo
                            y completar tu perfil para poder continuar.
                        </p>
                        <p class="label-instrucciones-bienvenida">INSTRUCCIONES:</p>
                        <p class="label-instrucciones-bienvenida">
                            A continuaci&oacute;n, se presenta una lista de intereses de los cuales debes seleccionar s&oacute;lo los que
                            se ajusten m&aacute;s a tu forma de ser, trata de responder de forma consciente. Se sugiere que no marques
                            m&aacute;s de 8 opciones.
                        </p>
                        <br />
                    </div>

                </div>

            </div>

            <div runat="server" id="pruebaDominosExample" visible="false">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-10">
                        <p class="label-instrucciones-bienvenida">
                            La prueba que se presenta a continuaci&oacute;n se llama <b>Domin&oacute;s</b> y es una prueba no verbal de inteligencia que tiene como objetivo 
                        valorar la capacidad de una persona para conceptualizar y aplicar el razonamiento l&oacute;gico a nuevos problemas.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            La prueba consta de 48 ejercicios en orden de dificultad creciente, y no tiene l&iacute;mite de tiempo, aunque se estima una duraci&oacute;n de entre
                        30 y 45 minutos para completarla.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            En esta prueba encontrar&aacute;s una serie de fichas de Domin&oacute; que guardan una cierta relaci&oacute;n entre s&iacute;. Dentro de cada 
                        mitad los puntos var&iacute;an entre 0 y 6. En cada uno de los grupos hay siempre una ficha completamente vac&iacute;a, que tendr&aacute;s que 
                        llenar seg&uacute;n sea el caso.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Observa bien cada grupo y calcula qu&eacute; n&uacute;meros le corresponden a cada ficha en blanco, para responder es necesario seleccionar una de 
                        las fichas de cada grupo de respuestas (Superior e Inferior).
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Mira el siguiente ejemplo, si te fijas, la secuencia para las fichas de arriba mantiene una relaci&oacute;n de espejo con las de abajo; por lo que 
                        la respuesta para la ficha en blanco es 4 (arriba) y 6 (abajo). Selecciona la respuesta correcta en cada caso y cuando est&eacute;s listo inicia la 
                        prueba.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>[ejercicio de ejemplo]</b>
                        </p>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="">
                        <div id="pregunta-content">
                            <div class="" id="pre-358-1158">
                                <input type="hidden" value="2" id="pre-358-1158-hdnTipoPregunta" />
                                <input type="hidden" value="1158" id="pre-358-1158-hdnPreguntaID" />
                                <input type="hidden" value="358" id="pre-358-1158-hdnRespuestaPreguntaID" />
                                <input type="hidden" value="" id="pre-358-1158-hdnHash" />
                                <input type="hidden" value="1" id="pre-358-1158-hdnModo" />
                                <div class="head_pregunta">
                                    <div style="text-align: center;" id="pregunta-img-1158">
                                        <img runat="server" id="imgPregunta" class="img_pregunta img-thumbnail img-responsive" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/daf753d733514e30baa6e262010ffb85.png" alt="imagen" />
                                    </div>
                                </div>
                                <div class="separador-pregunta"></div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row centradodominos" id="opcion-content-1158" style="padding-left: 10%">
                                            <div class="col-md-3 col-xs-3">
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1576">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1576" class="check-1158" name="input-1158" id="opcion-chk-1158-1576" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1576'); " class="domino check-1158" id="opcion-img-1576">
                                                                    <img runat="server" id="imgRespuesta0" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/a495bc59772547d99af6dba02ae080c7.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1577">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1577" class="check-1158" name="input-1158" id="opcion-chk-1158-1577" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1577'); " class="domino check-1158" id="opcion-img-1577">
                                                                    <img runat="server" id="imgRespuesta1" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/650b07033db24de9a31ec2169f7ff621.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1583">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1583" class="check-1158" name="input-1158" id="opcion-chk-1158-1583" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1583'); " class="domino check-1158" id="opcion-img-1583">
                                                                    <img runat="server" id="imgRespuesta7" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/b8c82206ff7e467d9858e1d2688e253f.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1584">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1584" class="check-1158" name="input-1158" id="opcion-chk-1158-1584" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1584'); " class="domino check-1158" id="opcion-img-1584">
                                                                    <img runat="server" id="imgRespuesta8" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/15be8b65563f4882b943537edc671c6d.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-3">
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1578">

                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1578" class="check-1158" name="input-1158" id="opcion-chk-1158-1578" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1578'); " class="domino check-1158" id="opcion-img-1578">
                                                                    <img runat="server" id="imgRespuesta2" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/6b6ecbd0339648a2baa86ea4a1e78eea.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1579">

                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1579" class="check-1158" name="input-1158" id="opcion-chk-1158-1579" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1579'); " class="domino check-1158" id="opcion-img-1579">
                                                                    <img runat="server" id="imgRespuesta3" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/d54cb2a08a6041e096a7a76d392aa1ed.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1585">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1585" class="check-1158" name="input-1158" id="opcion-chk-1158-1585" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1585'); " class="domino check-1158" id="opcion-img-1585">
                                                                    <img runat="server" id="imgRespuesta9" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/2ee0124878d04616950b66ec15c432f4.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1586">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1586" class="check-1158" name="input-1158" id="opcion-chk-1158-1586" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1586'); " class="domino check-1158" id="opcion-img-1586">
                                                                    <img runat="server" id="imgRespuesta10" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/c4321e3c47d34d428b5d87ea2c9824e8.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-3">
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1580">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1580" class="check-1158" name="input-1158" id="opcion-chk-1158-1580" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1580'); " class="domino check-1158" id="opcion-img-1580">
                                                                    <img runat="server" id="imgRespuesta4" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/1fd144418d194eb2b887cd3808cb41fb.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1581">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1581" class="check-1158" name="input-1158" id="opcion-chk-1158-1581" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1581'); " class="domino check-1158" id="opcion-img-1581">
                                                                    <img runat="server" id="imgRespuesta5" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/16a1dd0dd2cb472a8742351f192f529e.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1587">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1587" class="check-1158" name="input-1158" id="opcion-chk-1158-1587" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1587'); " class="domino check-1158" id="opcion-img-1587">
                                                                    <img runat="server" id="imgRespuesta11" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/c4d46eb73c8f4448a500f6f33d8929eb.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1588">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1588" class="check-1158" name="input-1158" id="opcion-chk-1158-1588" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1588'); " class="domino check-1158" id="opcion-img-1588">
                                                                    <img runat="server" id="imgRespuesta12" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/0befeb3ea00a4cb0a0d1e6d22b037e9f.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-3">
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1582">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1582" class="check-1158" name="input-1158" id="opcion-chk-1158-1582" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1582'); " class="domino check-1158" id="opcion-img-1582">
                                                                    <img runat="server" id="imgRespuesta6" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-top: 15px">
                                                    <div class="col-md-6 col-sm-6 col-xs-6" id="opcion-panel-1589">
                                                        <div class="checkbox opciones_container_cols">
                                                            <div class="opciones_input">
                                                                <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="1589" class="check-1158" name="input-1158" id="opcion-chk-1158-1589" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div onclick="javascript:getDomino('1158','1589'); " class="domino check-1158" id="opcion-img-1589">
                                                                    <img runat="server" id="imgRespuesta13" class="img_opcion img-responsive dominos" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/885f48788e9041fb82044b4a96336e9d.png" alt="imagen" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="separador-pregunta"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div runat="server" id="pruebaTermanExample" visible="false">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-10 col-xs-12">
                        <p class="label-instrucciones-bienvenida">
                            La prueba que se presenta a continuaci&oacute;n se llama <b>Terman</b> y su objetivo es determinar el cociente intelectual, factor general, mediante 
                            la evaluaci&oacute;n de la habilidad mental y conocimientos. La prueba consta de 10 partes o subpruebas, con un promedio de 17 reactivos por cada 
                            subprueba. Las tareas de cada subprueba son de habilidad matem&aacute;tica y ling&uuml;&iacute;stica, con un tiempo de duraci&oacute;n total de 
                            30 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Al iniciar cada subprueba, revisa la indicaci&oacute;n y resuelve seg&uacute;n corresponda. Considera los tiempos de cada subprueba y responde de 
                            manera r&aacute;pida. Mira los ejemplos de cada una a continuaci&oacute;n.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>[cuadro de ejemplos]</b>
                        </p>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row form-group">
                    <div class="col-xs-12 col-md-6 col-lg-6 separador">
                        <div id="instruccionesTerman1" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n I</h1>
                            <asp:Label runat="server" ID="Label1" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label2" Text="Seleccione la palabra que complete correctamente la oración." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label20" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label21" Text="El iniciador de nuestra guerra de independencia fue:" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio1" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label22" Text=" Morelos   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio2" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label62" Text=" Zaragoza   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio3" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label63" Text=" Iturbide   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio4" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label64" Text=" Hidalgo" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label23" Text="Respuesta :" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio14" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label24" Text=" Hidalgo" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label90" Text="Tiempo : 2 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-6 col-lg-6">
                        <div id="instruccionesTerman2" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n II</h1>
                            <asp:Label runat="server" ID="Label3" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label4" Text="Lea cada cuestión y seleccione la mejor respuesta." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label25" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label26" Text="¿Por qué compramos relojes? Por que: " CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio5" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label27" Text=" No gustan oírlos sonar.   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio6" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label30" Text=" Tienen manecillas.   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio7" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label31" Text=" Nos indican las horas." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label28" Text="Respuesta: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio13" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label29" Text=" Nos indican las horas." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label91" Text="Tiempo : 3 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-xs-12 col-md-6 col-lg-6 separador">
                        <div id="instruccionesTerman3" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n III</h1>
                            <asp:Label runat="server" ID="Label5" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label6" Text="Cuando las dos palabras signifiquen lo mismo, selecciona la letra (I) de igual, cuando signifiquen lo opuesto, selecciona la letra (O) de opuesto" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label32" Text="Ejemplo: " CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label33" Text="    Tirar  -  Arrojar ....................(I)" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label34" Text="    Norte  -  Sur     ....................(O)" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label92" Text="Tiempo : 3 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-6 col-lg-6">
                        <div id="instruccionesTerman4" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n IV</h1>
                            <asp:Label runat="server" ID="Label7" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label8" Text="Seleccione las dos opciones que indican algo que siempre tiene el sujeto." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label35" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label36" Text="Un hombre siempre tiene:" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio8" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label37" Text=" Cuerpo   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio9" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label65" Text=" Gorra   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio10" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label66" Text=" Guantes   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio11" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label67" Text=" Boca   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio12" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label68" Text=" Dinero" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label38" Text="Respuesta: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio15" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label69" Text="Cuerpo  " CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label70" Text="  -  " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio16" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label39" Text="  Boca" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label93" Text="Tiempo : 4 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-xs-12 col-md-6 col-lg-6 separador">
                        <div id="instruccionesTerman5" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n V</h1>
                            <br />

                            <asp:Label runat="server" ID="Label9" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <br />
                            <asp:Label runat="server" ID="Label10" Text="Encuentre las respuestas lo más pronto posible a los problemas que se presentan." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label94" Text="Tiempo : 5 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-6 col-lg-6">
                        <div id="instruccionesTerman6" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n VI</h1>
                            <asp:Label runat="server" ID="Label11" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label12" Text="Seleccione la contestación correcta como se muestra en los ejemplos." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label40" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label41" Text="Se hace el carbón de madera                        .......... Si" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label42" Text="Tienen todos los hombres 1.70 mts. de altura .......... No" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label95" Text="Tiempo : 2 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-xs-12 col-md-6 col-lg-6 separador">
                        <div id="instruccionesTerman7" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n VII</h1>
                            <asp:Label runat="server" ID="Label13" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label14" Text="Seleccione la respuesta correcta." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label47" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label43" Text="Oído es a oír como el ojo es a:" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio17" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label45" Text=" Mesa   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio18" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label71" Text=" Ver   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio19" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label72" Text=" Mano   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio20" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label73" Text=" Jugar" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label46" Text="Respuesta: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio21" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label74" Text=" Ver" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label44" Text="El sombrero es a la cabeza como zapato es a:" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio22" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label75" Text=" Brazo   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio23" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label76" Text=" Abrigo   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio24" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label77" Text=" Pie   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio25" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label48" Text=" Pierna" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label49" Text="Respuesta: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio26" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label78" Text=" Pie" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label96" Text="Tiempo : 2 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-6 col-lg-6">
                        <div id="instruccionesTerman8" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n VIII</h1>
                            <asp:Label runat="server" ID="Label15" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label16" Text="Las palabras de cada una de las oraciones siguientes están mezcladas, ordene cada una. " CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label79" Text="Si el significado de la oración es verdadero, selecciona la letra (V); si el significado es falso, selecciona la letra (F)." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label50" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label51" Text="Oír son para los oídos         ..................(V)" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label52" Text="Comer para pólvora la buena es ..........(F)" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label97" Text="Tiempo : 4 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-xs-12 col-md-6 col-lg-6 separador">
                        <div id="instruccionesTerman9" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n IX</h1>
                            <asp:Label runat="server" ID="Label17" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label18" Text="Seleccione la palabra que no corresponda con las demás." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label53" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio27" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label54" Text=" Bala   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio28" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label80" Text=" Cañón   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio29" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label81" Text=" Pistola   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio30" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label82" Text="  Espada   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio31" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label83" Text=" Lápiz" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label55" Text="Respuesta: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio37" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label84" Text=" Lápiz" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="Radio32" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label56" Text=" Canadá   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio33" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label85" Text=" Sonora   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio34" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label86" Text=" China   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio35" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label87" Text=" India   " CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio36" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label88" Text=" Francia" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label57" Text="Respuesta: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <input id="Radio38" name="input-${preguntaid}" type="radio" value="${opcionid}" disabled style="cursor: auto !important;" />
                            <asp:Label runat="server" ID="Label89" Text=" Sonora" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label98" Text="Tiempo : 4 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                        </div>
                    </div>
                    <div class="col-xs-12 col-md-6 col-lg-6">
                        <div id="instruccionesTerman10" class="intruccionesTermanMerril">
                            <h1 class="seccion">Secci&oacute;n X</h1>
                            <br />
                            <asp:Label runat="server" ID="Label19" Text="Instrucciones: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <asp:Label runat="server" ID="Label58" Text="Seleccione los dos números que deban seguir en la serie." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label59" Text="Ejemplo: " Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <br />
                            <asp:Label runat="server" ID="Label60" Text="5   10   15   20   25       .......... 30   35" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label61" Text="20   18   16   14   12  .......... 10   8" CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <asp:Label runat="server" ID="Label99" Text="Tiempo : 5 minutos" Style="font-weight: bold;" CssClass="elemento_consalto"></asp:Label>
                            <br />
                        </div>
                    </div>
                </div>
            </div>

            <div id="pruebaSacksExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La siguiente prueba se llama <b>Frases incompletas de Sacks</b> y tiene como objetivo identificar aspectos y rasgos de personalidad mediante la 
                            asociaci&oacute;n de palabras. La prueba consta de 60 frases incompletas, y aunque no es una prueba de tiempo, se pide que la realices de forma 
                            r&aacute;pida, entre 20 y 45 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Lee la frase y completa escribiendo la primera idea que te venga en mente, y continuar con las siguientes tan a prisa como te sea posible hasta 
                            concluir, sin detenerte demasiado a pensar tu respuesta.
                        </p>
                    </div>
                </div>
            </div>

            <div id="pruebaCleaverExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La siguiente prueba se llama <b>Cleaver</b> y tiene como objetivo identificar aspectos y rasgos mediante la selecci&oacute;n de 
                            caracter&iacute;sticas de personalidad. La prueba consta de 96 reactivos, no es una prueba de tiempo, aunque se pide que la realices de forma 
                            r&aacute;pida, entre 20 y 45 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Examina las palabras y selecciona “M&Aacute;S” si consideras que la caracter&iacute;stica de personalidad te define y te describe mejor o 
                            “MENOS” si consideras que la caracter&iacute;stica no te define o te describe menos.
                        </p>
                    </div>
                </div>
            </div>

            <div id="pruebaChasideExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La prueba que se presenta a continuaci&oacute;n se llama <b>CHASIDE</b> y tiene como objetivo que el estudiante pueda definir el campo de 
                            conocimiento para el que tiene mayor inter&eacute;s y aptitud. La prueba consta de 98 reactivos, no es una prueba de tiempo, aunque se pide que la 
                            realices de forma r&aacute;pida, entre 20 y 45 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            A continuaci&oacute;n se presentan una serie de cuestionamientos acerca de lo que te gusta e interesa y lo que no. Lee cada una y trata de responder con la mayor 
                            sinceridad, marcando “SI” o “NO” seg&uacute;n sea el caso; recuerda responder todas las preguntas.
                        </p>
                    </div>
                </div>
            </div>

            <div runat="server" id="pruebaAllportExample" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La siguiente prueba se conoce como <b>Estudio de Valores de Allport</b> y tiene como objetivo medir la importancia relativa de seis intereses o 
                        motivos b&aacute;sicos en la personalidad. La prueba consta de 45 reactivos en dos partes, y no es una prueba de tiempo, aunque se pide que la 
                        realices de forma r&aacute;pida entre 20 y 45 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            En la primera parte de la prueba, de 30 reactivos, debes tomar una postura escogiendo una de las dos opciones presentadas. 
                        Observa el primer ejemplo de reactivo y resp&oacute;ndelo seg&uacute;n consideres. Algunas de las opciones pueden parecerte igualmente 
                        atractivas o desagradables, sin embargo, escoge una de ellas aunque sólo te parezca relativamente aceptable.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            En la segunda parte, tendr&aacute;s que responder jerarquizando un grupo de cuatro enunciados, mira las indicaciones espec&iacute;ficas y el ejemplo:
                        </p>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <ul id="Ul1">
                            <li class="espacio_pregunta">
                                <input type="hidden" value="2" id="pre-3793-1933" />
                                <input type="hidden" value="1933" id="pre-3793-1933-hdnPreguntaID" />
                                <input type="hidden" value="3793" id="pre-3793-1933-hdnRespuestaPreguntaID" />
                                <input type="hidden" value="" id="pre-3793-1933-hdnHash" />
                                <input type="hidden" value="2" id="pre-3793-1933-hdnModo" />
                                <div class="head_pregunta">
                                    <div class="texto_pregunta elemento_consalto" id="pregunta-texto-1933">
                                        <p>Dando por hecho que usted tiene la habilidad necesaria, preferiría ser.</p>
                                    </div>
                                </div>
                                <div class="espacio_respuestas">
                                    <div class="row">
                                        <div class="" id="opcion-content-1933">
                                            <div class="col-xs-12 espacio_opcion">
                                                <ul id="Ul2">
                                                    <li style="display: inline-block;" class="col-xs-6 col-md-4 espacio_opcion" id="opcion-panel-5503">
                                                        <div class="opciones_container_cols checkbox">
                                                            <div class="opciones_input">
                                                                <input type="radio" value="5503" name="input-1933" id="opcion-1933-5503" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div id="opcion-texto-5503">
                                                                    <label for="opcion-1933-5503">Banquero</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                    <li style="display: inline-block;" class="col-xs-6 col-md-4 espacio_opcion" id="opcion-panel-5504">
                                                        <div class="opciones_container_cols checkbox">
                                                            <div class="opciones_input">
                                                                <input type="radio" value="5504" name="input-1933" id="opcion-1933-5504" />
                                                            </div>
                                                            <div class="opciones_text">
                                                                <div id="opcion-texto-5504">
                                                                    <label for="opcion-1933-5504">Político</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <ul id="Ul3">
                        <li class="espacio_pregunta" id="pre-359-1935">
                            <input type="hidden" value="2" id="pre-359-1935-hdnTipoPregunta" />
                            <input type="hidden" value="1158" id="pre-359-1935-hdnPreguntaID" />
                            <input type="hidden" value="358" id="pre-359-1935-hdnRespuestaPreguntaID" />
                            <input type="hidden" value="" id="pre-359-1935-hdnHash" />
                            <input type="hidden" value="1" id="pre-359-1935-hdnModo" />
                            <div class="head_pregunta">
                                <div style="text-align: center;" id="pregunta-img-1935">
                                    <img runat="server" id="imgInstruccionesPart2" class="img_pregunta img-thumbnail img-responsive" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/daf753d733514e30baa6e262010ffb85.png" alt="imagen" />
                                </div>
                            </div>
                            <div class="separador-pregunta"></div>
                            <div class="espacio_respuestas">
                                <div class="row">
                                    <div class="" id="opcion-content-1935">
                                        <div class="col-xs-12 espacio_opcion">
                                            <div class="row">
                                                <div class="col-md-2"></div>
                                                <div class="col-md-2 col-sm-3 col-xs-3">
                                                    <div class="row">
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5521" class="check-1935" name="input-1935" id="opcion-chk-1935-5521" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5521'); " class="interes check-1935" id="opcion-img-5521" style="text-align: center">
                                                                        <img runat="server" id="imgpreguntaAllport_A4" class="img_opcion img-responsive interes" src="http://localhost:55803/Medios/imagenesReactivos/Dinamico/a495bc59772547d99af6dba02ae080c7.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                       <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5525" class="check-1935" name="input-1935" id="opcion-chk-1935-5525" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5525'); " class="interes check-1935" id="opcion-img-5525" style="text-align: center">
                                                                        <img runat="server" id="imgpreguntaAllport_B4" class="img_opcion img-responsive interes" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/1fd144418d194eb2b887cd3808cb41fb.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5529" class="check-1935" name="input-1935" id="opcion-chk-1935-5529" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5529'); " class="interes check-1935" id="opcion-img-5529" style="text-align: center">
                                                                        <img runat="server" id="imgpreguntaAllport_C4" class="img_opcion img-responsive interes" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5533" class="check-1935" name="input-1935" id="opcion-chk-1935-5533" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5533'); " class="interes check-1935" id="opcion-img-5533" style="text-align: center">
                                                                        <img runat="server" id="imgpreguntaAllport_D4" class="img_opcion img-responsive interes" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2 col-sm-3 col-xs-3">
                                                    <div class="row">
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5522" class="check-1935" name="input-1935" id="opcion-chk-1935-5522" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5522'); " class="interes check-1935" id="opcion-img-5522">
                                                                        <img runat="server" id="imgpreguntaAllport_A3" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/650b07033db24de9a31ec2169f7ff621.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5526" class="check-1935" name="input-1935" id="opcion-chk-1935-5526" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5526'); " class="interes check-1935" id="opcion-img-5526">
                                                                        <img runat="server" id="imgpreguntaAllport_B3" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/16a1dd0dd2cb472a8742351f192f529e.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5530" class="check-1935" name="input-1935" id="opcion-chk-1935-5530" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5530'); " class="interes check-1935" id="opcion-img-5530">
                                                                        <img runat="server" id="imgpreguntaAllport_C3" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5534" class="check-1935" name="input-1935" id="opcion-chk-1935-5534" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5534'); " class="interes check-1935" id="opcion-img-5534">
                                                                        <img runat="server" id="imgpreguntaAllport_D3" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2 col-sm-3 col-xs-3">
                                                    <div class="row">
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5523" class="check-1935" name="input-1935" id="opcion-chk-1935-5523" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5523'); " class="interes check-1935" id="opcion-img-5523">
                                                                        <img runat="server" id="imgpreguntaAllport_A2" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/6b6ecbd0339648a2baa86ea4a1e78eea.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5527" class="check-1935" name="input-1935" id="opcion-chk-1935-5527" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5527'); " class="interes check-1935" id="opcion-img-5527">
                                                                        <img runat="server" id="imgpreguntaAllport_B2" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5531" class="check-1935" name="input-1935" id="opcion-chk-1935-5531" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5531'); " class="interes check-1935" id="opcion-img-5531">
                                                                        <img runat="server" id="imgpreguntaAllport_C2" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5535" class="check-1935" name="input-1935" id="opcion-chk-1935-5535" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5535'); " class="interes check-1935" id="opcion-img-5535">
                                                                        <img runat="server" id="imgpreguntaAllport_D2" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2 col-sm-3 col-xs-3">
                                                    <div class="row">
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5524" class="check-1935" name="input-1935" id="opcion-chk-1935-5524" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5524'); " class="interes check-1935" id="opcion-img-5524">
                                                                        <img runat="server" id="imgpreguntaAllport_A1" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/d54cb2a08a6041e096a7a76d392aa1ed.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5528" class="check-1935" name="input-1935" id="opcion-chk-1935-5528" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5528'); " class="interes check-1935" id="opcion-img-5528">
                                                                        <img runat="server" id="imgpreguntaAllport_B1" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5532" class="check-1935" name="input-1935" id="opcion-chk-1935-5532" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5532'); " class="interes check-1935" id="opcion-img-5532">
                                                                        <img runat="server" id="imgpreguntaAllport_C1" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="col-xs-9 sinpadding">
                                                            <div class="checkbox opciones_container_cols">
                                                                <div class="opciones_input">
                                                                    <input type="checkbox" style="margin-left: 10px; visibility: hidden;" value="5536" class="check-1935" name="input-1935" id="opcion-chk-1935-5536" />
                                                                </div>
                                                                <div class="opciones_text">
                                                                    <div onclick="javascript:getInteres('1935','5536'); " class="interes check-1935" id="opcion-img-5536">
                                                                        <img runat="server" id="imgpreguntaAllport_D1" class="img_opcion img-responsive interes" style="text-align: center" src="http://localhost:60861/Medios/imagenesReactivos/Dinamico/84ec4f0485bf4f2fa5e7d06770495da9.png" alt="imagen" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="col-md-2"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="separador-pregunta"></div>
                        </li>
                    </ul>
                </div>
            </div>

            <div id="pruebaKuderExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La siguiente prueba se conoce como <b>Kuder de Preferencias Vocacionales</b> y tiene como objetivo descubrir las &aacute;reas generales donde se 
                            sit&uacute;an los intereses y las preferencias del individuo, respecto a la vocaci&oacute;n; no tiene tiempo l&iacute;mite, aunque la mayor&iacute;a 
                            de las personas la contesta entre 30 y 55 minutos. La prueba consta de 504 reactivos y puede ser respondida en dos o m&aacute;s sesiones de trabajo; 
                            la recomendaci&oacute;n son dos sesiones como m&aacute;ximo.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            A continuaci&oacute;n se presenta una serie de enunciados en los que deber&aacute;s indicar si te agrada m&aacute;s o menos. No existen respuestas 
                            correctas o incorrectas, se trata de señalar las situaciones con las que te sientes m&aacute;s a gusto y m&aacute;s feliz y con las que no. 
                            Programa tus sesiones para completar esta prueba.
                        </p>
                    </div>
                </div>
            </div>

            <div id="pruebaHabitosExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La prueba que se presenta a continuaci&oacute;n se llama <b>H&aacute;bitos de estudio</b> y tiene como objetivo identificar el nivel de 
                            h&aacute;bitos de estudio que tienes, de tal forma que se puedan identificar las &aacute;reas de oportunidad y mejora del estudiante. La prueba 
                            consta de 50 reactivos, no es una prueba de tiempo, aunque se pide que la realices de forma r&aacute;pida, entre 20 y 45 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            A continuaci&oacute;n se presentan una serie de preguntas, lee detenidamente cada una y selecciona la alternativa que consideres, tomando en cuenta 
                            la siguiente escala:
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            a) Siempre
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            b) A veces
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            c) Nunca
                        </p>
                    </div>
                </div>
            </div>

            <div id="pruebaRotterExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La prueba que se presenta a continuaci&oacute;n se llama test de Locus de control <b>Rotter</b> y es ampliamente utilizado en la psicolog&iacute;a en el campo de 
                            la autorregulaci&oacute;n para identificar de d&oacute;nde surge la motivaci&oacute;n de la persona para alcanzar sus objetivos y cubrir sus necesidades
                            personales. La prueba consta de 29 reactivos, y no es una prueba de tiempo, aunque se pide que la realices de manera &aacute;gil. El tiempo de respuesta 
                            est&aacute; entre 15 y 30 minutos.                   
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            A continuaci&oacute;n, encontrar&aacute;s una serie de enunciados.<br />
                            Ninguna respuesta es correcta o incorrecta. Selecciona la que m&aacute;s se adecue a su forma de pensar.<br />
                            <asp:Label ID="lblRotPrg1" runat="server" Text="1.-" CssClass="elemento_consalto"></asp:Label><br />
                            <input id="radRot1" name="input-${preguntaid}" type="radio" value="${opcionid}" />
                            <asp:Label ID="lblOpc1" runat="server" Text="a) Hay ciertas personas que simplemente no son buenas personas." CssClass="elemento_consalto"></asp:Label>
                            <br />
                            <input id="radRot2" name="input-${preguntaid}" type="radio" value="${opcionid}" />
                            <asp:Label ID="lblOpc2" runat="server" Text="b) Siempre hay algo bueno en todas las personas." CssClass="elemento_consalto"></asp:Label>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            La informaci&oacute;n que proporciones es confidencial y solo sera utilizado para fines de investigaci&oacute;n acad&eacute;mica.
                        </p>
                    </div>
                </div>
            </div>

            <div id="pruebaRavenExample" runat="server" visible="true">
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-10">
                        <p class="label-instrucciones-bienvenida">
                            El test de <b>Raven</b>, es una prueba no verbal donde deben analizarse matrices y encontrar las piezas faltantes. Esta prueba 
                            obliga a poner en marcha el razonamiento analógico, la percepción y la capacidad de abstracción y es uno de 
                            los más conocidos relacionados con la medición de la inteligencia y el coeficiente de IQ.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Consiste en encontrar la pieza faltante en una serie de figuras que se irán mostrando. Se debe analizar la serie que se le presenta y siguiendo la secuencia horizontal y vertical, escoger uno de las seis piezas sugeridas, la que encaje perfectamente en ambos sentidos, tanto en el horizontal como en el vertical.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Se trata de un test de inteligencia no verbal en el que no suele utilizarse límite de tiempo, pero dura 
                            aproximadamente 60 minutos. 
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>[ejercicio de ejemplo]</b>
                        </p>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <div class="row">
                    <div class="">
                        <div id="pregunta-content">
                            <div id="pre-21383-2651" class="">
                                <input id="pre-21383-2651-hdnTipoPregunta" value="2" type="hidden">
                                <input id="pre-21383-2651-hdnPreguntaID" value="2651" type="hidden">
                                <input id="pre-21383-2651-hdnRespuestaPreguntaID" value="21382" type="hidden">
                                <input id="pre-21383-2651-hdnHash" value="" type="hidden">
                                <input id="pre-21383-2651-hdnModo" value="2" type="hidden">
                                <div class="head_pregunta">
                                    <div id="pregunta-img-2651" style="text-align: center;">
                                        <img id="imgpregravenexample" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/1514c81400bd4b22b0d8d370e7f972c0.png" class="img_pregunta img-thumbnail img-responsive">
                                    </div>
                                </div>
                                <div class="separador-pregunta"></div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row" id="opcion-content-2651">
                                            <div class="col-md-3 col-xs-3">
                                                <div id="opcion-panel-7581" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="opciones_container_cols radio">
                                                        <div class="opciones_input">
                                                            <input type="radio" style="margin-left: 10px; visibility: hidden;" value="7581" class="rad-2651" name="input-2651" id="opcion-rad-2651-7581">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div onclick="javascript:getRaven('2651','7581');" class="raven rad-2651" id="opcion-img-7581">
                                                                <img id="respraven01" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/ac9790ff47ca4ce488772d582485aaee.png" class="img_opcion img-responsive raven">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="opcion-panel-7582" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="opciones_container_cols radio">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" id="opcion-rad-2651-7582" class="rad-2651" name="input-2651" value="7582" onclick="    verificarError('pre-21383-2651')" type="radio">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div onclick="javascript:getRaven('2651','7582');" class="raven rad-2651" id="opcion-img-7582">
                                                                <img id="respraven02" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/28b9652bc4d04d16bd3486daf5343806.png" class="img_opcion img-responsive raven">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-3">
                                                <div id="opcion-panel-7583" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="opciones_container_cols radio">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" id="opcion-rad-2651-7583" name="input-2651" class="rad-2651" value="7583" onclick="    verificarError('pre-21383-2651')" type="radio">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div onclick="javascript:getRaven('2651','7583');" class="raven rad-2651" id="opcion-img-7583">
                                                                <img id="respraven03" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/118e81f3618c4b968c36f7f4b17085c0.png" class="img_opcion img-responsive raven">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="opcion-panel-7584" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="opciones_container_cols radio">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" id="opcion-rad-2651-7584" class="rad-2651" name="input-2651" value="7584" onclick="    verificarError('pre-21383-2651')" type="radio">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div onclick="javascript:getRaven('2651','7584');" class="raven rad-2651" id="opcion-img-7584">
                                                                <img id="respraven04" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/7307395eefb44502b6479d3c59e1febc.png" class="img_opcion img-responsive raven">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-3">
                                                <div id="opcion-panel-7585" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="opciones_container_cols radio">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" id="opcion-rad-2651-7585" class="rad-2651" name="input-2651" value="7585" onclick="    verificarError('pre-21383-2651')" type="radio">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div onclick="javascript:getRaven('2651','7585');" class="raven rad-2651" id="opcion-img-7585">
                                                                <img id="respraven05" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/244b3b990f6544d0afd813964f3f0cc8.png" class="img_opcion img-responsive raven">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="opcion-panel-7586" class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="opciones_container_cols radio">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" id="opcion-rad-2651-7586" class="rad-2651" name="input-2651" value="7586" onclick="    verificarError('pre-21383-2651')" type="radio">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div onclick="javascript:getRaven('2651','7586');" class="raven rad-2651" id="opcion-img-7586">
                                                                <img id="respraven06" runat="server" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/f5b11ca2ed2d47e796319f9c08b68379.png" class="img_opcion img-responsive raven">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-xs-3">
                                        </div>
                                    </div>
                                </div>
                                <div class="separador-pregunta"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="pruebaFrasesVocacionalesExample" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            La siguiente prueba se llama <b>Frases incompletas vocacionales</b> y tiene como objetivo identificar aspectos y rasgos de personalidad mediante la 
                            asociaci&oacute;n de palabras. La prueba consta de 60 frases incompletas, y aunque no es una prueba de tiempo, se pide que la realices de forma 
                            r&aacute;pida, entre 20 y 45 minutos.
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            <b>INSTRUCCI&Oacute;N GENERAL:</b>
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Lee la frase y completa escribiendo la primera idea que te venga en mente, y continuar con las siguientes tan a prisa como te sea posible hasta 
                            concluir, sin detenerte demasiado a pensar tu respuesta.
                        </p>
                    </div>
                </div>
            </div>

            <div id="pruebaZavicExample" runat="server" visible="false">
                <div class="row">
                    <li id="pre-23846-2779" class="espacio_pregunta">
                        <input id="pre-23846-2779-hdnTipoPregunta" value="2" type="hidden">
                        <input id="pre-23846-2779-hdnPreguntaID" value="2779" type="hidden">
                        <input id="pre-23846-2779-hdnRespuestaPreguntaID" value="23845" type="hidden">
                        <input id="pre-23846-2779-hdnHash" value="" type="hidden">
                        <input id="pre-23846-2779-hdnModo" value="1" type="hidden">
                        <div class="head_pregunta">
                            <div id="pregunta-img-2779" style="text-align: center;">
                                <img runat="server" id="imginstruccioneszavic" alt="imagen" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/059ce8f92d8e4eabb84f221cddec9b6f.png" class="img_pregunta img-thumbnail img-responsive">
                            </div>
                        </div>
                        <div class="separador-pregunta"></div>
                        <div class="espacio_respuestas">
                            <div class="row" style="background: transparent">
                                <div id="opcion-content-2779" class="row">
                                    <div class="">
                                        <div class="row">
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8411" class="check-2779" name="input-2779" id="opcion-chk-2779-8411" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8411" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8411');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_A4" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/2dd2e7ee87ee407eb1e0f540dbcd12f3.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8412" class="check-2779" name="input-2779" id="opcion-chk-2779-8412" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8412" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8412');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_A3" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/3e818bb946284392bba95c7b41d7f6ea.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8413" class="check-2779" name="input-2779" id="opcion-chk-2779-8413" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8413" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8413');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_A2" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/1b39b3f7728244cfbba3cf9a4752b735.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8414" class="check-2779" name="input-2779" id="opcion-chk-2779-8414" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8414" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8414');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_A1" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/6ade36602b514f3f923fc13edf6f5ea8.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8415" class="check-2779" name="input-2779" id="opcion-chk-2779-8415" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8415" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8415');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_B4" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/b3b716a0f8cd49d29aca092b055b4e45.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8416" class="check-2779" name="input-2779" id="opcion-chk-2779-8416" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8416" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8416');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_B3" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/cbb7b94deaae4bb9af365ecc0fdc5337.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8417" class="check-2779" name="input-2779" id="opcion-chk-2779-8417" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8417" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8417');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_B2" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/22d383b4961e4784ac66c3e1dc08a411.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8418" class="check-2779" name="input-2779" id="opcion-chk-2779-8418" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8418" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8418');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_B1" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/d2e8dc860c644619b8fdf8875ec08d0e.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8419" class="check-2779" name="input-2779" id="opcion-chk-2779-8419" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8419" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8419');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_C4" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/9d59e0022429456fa5b862459bfcf4f7.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8420" class="check-2779" name="input-2779" id="opcion-chk-2779-8420" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8420" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8420');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_C3" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/2e47d6917aa247b88f92ecb2b40245aa.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8421" class="check-2779" name="input-2779" id="opcion-chk-2779-8421" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8421" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8421');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_C2" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/6f1c29fbdada45ba9d50e0e82f4bb83b.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8422" class="check-2779" name="input-2779" id="opcion-chk-2779-8422" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8422" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8422');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_C1" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/eda07c5f6772433785885c15ea2644a8.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8423" class="check-2779" name="input-2779" id="opcion-chk-2779-8423" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8423" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8423');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_D4" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/f0fc0ed2a6274d6e991ec39ad518e3ee.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8424" class="check-2779" name="input-2779" id="opcion-chk-2779-8424" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8424" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8424');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_D3" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/dbe806eba49d426ebaba366e9a4c6e3c.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8425" class="check-2779" name="input-2779" id="opcion-chk-2779-8425" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8425" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8425');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_D2" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/3a4f7f3eb9984abe8a0275d827f96a69.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                            <div class="col-xs-3 col-md-3 sinpadding">
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                                <div class="col-md-6 col-sm-6 col-xs-6" style="display: inline-block;">
                                                    <div class="checkbox opciones_container_cols">
                                                        <div class="opciones_input">
                                                            <input style="margin-left: 10px; visibility: hidden;" value="8426" class="check-2779" name="input-2779" id="opcion-chk-2779-8426" type="checkbox">
                                                        </div>
                                                        <div class="opciones_text">
                                                            <div id="opcion-img-8426" style="text-align: center" class="interes check-2779" onclick="javascript:getInteres('2779','8426');verificarError('pre-23846-2779')">
                                                                <img runat="server" alt="imagen" id="imgpreguntaZavic_D1" src="http://testpov.grupoplenum.com/Operaciones/Medios/imagenesReactivos/Dinamico/45b7b3dba91a4e9e93832372e6720663.png" class="img_opcion img-responsive letrasresponsivas interes">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm3 col-xs-3"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="separador-pregunta"></div>
                    </li>
                </div>
            </div>

            <div id="pruebaEstilosdeApredizajeExample" runat="server" visible="false">
                   <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                           Elige la opción con la que más te identifiques de cada una de las preguntas y márcala con una X. 
                            Lee detenidamente y trata de escoger alguna de las opciones que se plantean.

                        </p>
                        
                      <div id="ejemploEstilos">
                          <p>A continuación se presenta una pregunta de ejemplo</p>
                     
                       <p > Cuando estás en clase y el profesor explica algo que está escrito en la pizarra o en tu libro, te es más fácil seguir las explicaciones...</p>
                      <div class="row">
                       
                          <input type="radio" name="estilos" id="opcion_a"/>
                          <label for="opcion_a">Escuchando al profesor.</label>
                            <input type="radio"name="estilos"  id="opcion_b"/>
                          <label for="opcion_b">Me aburro y espero a que me den algo para hacer.</label>
                            <input type="radio"name="estilos" id="opcion_c"/>
                          <label for="opcion_c">Leyendo el libro o la pizarra.</label>
                     
                      </div>
                          <br />
                      </div>
                        <p>
                            Responde a cada una de las preguntas y si tienes alguna duda dirígete al aplicador o a soporte@yoyvocacional.com.
                        </p>
                    </div>
                </div>
            </div>

             <div id="pruebaInteligenciasMultiplesExample" runat="server" visible="false">
                   <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                          Este test te ayudará a que puedas conocerte mejor y, también, a que pueda identificar las áreas más sobresalientes de su inteligencia. 
                            Por lo tanto, lee cuidadosamente cada una de las afirmaciones siguientes y responde utilizando en siguiente criterio de “Si” y “No”. <br />
                        </p>
                         <ul>
                                <li>
                                    Si crees que refleja una característica tuya y te parece que la afirmación es verdadera, escribe "Si".

                                </li>
                                <li>
                                    Si crees que no refleja una característica tuya y te parece que la afirmación es falsa, escribe una "No".
                                </li>
                            </ul>
                        <p>Recuerda responder con mucha honestidad y sinceridad.
</p>
                        
                        <div id="ejerciciodeejemplointeligencias">
                            <p>a continuación se muestra una pregunta de ejemplo</p>
                            <p>Tengo buena memoria para los nombres de lugares, personas, fechas y otras cosas aunque parezcan triviales.</p>
                              <input type="radio" name="inteligencias" id="Radio39"/>
                          <label for="Radio39">Si</label><br />
                            <input type="radio"name="inteligencias"  id="Radio40"/>
                          <label for="Radio40">no</label>
                         
                        </div>
                        <p>
                            Responde a cada una de las preguntas y si tienes alguna duda dirígete al aplicador o a soporte@yoyvocacional.com.
                        </p>
                    </div>
                </div>
            </div>

             <div id="InventariodeInteresesExample" runat="server" visible="false">
                   <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                       
                        <p class="label-instrucciones-bienvenida">
                            Esta no es una prueba, sino solamente una medida de tu interés en algunos campos profesionales.<br/> 
                            No hay respuestas correctas, lo único importante es tu franca opinión. <br/>

	                        A continuaci&oacute;n encontrar&aacute;s una serie de actividades o cosas por hacer. <br/>
                            Por favor indica a cada actividad si te gusta o desagrada
                        </p>
                        <p>por ejemplo <br /> Asistir a un partido de futboll</p>
                        
                                <input type="radio" name="karl" id="Radio41"/>
                        <label for="Radio41">Me desagrada mucho</label>
                               <input type="radio" name="karl" id="Radio42"/>
                        <label for="Radio42">No me gusta</label>
                              <input type="radio" name="karl" id="Radio43"/>
                        <label for="Radio43">Me es indiferente</label>
                        <input type="radio" name="karl" id="Radio44"/>
                               <label for="Radio44">Me gusta</label>
                     <input type="radio" name="karl" id="Radio45"/> 
                       <label for="Radio45">Me gusta mucho</label>
                       
                      
                       
                    
                        <p>
                            Responde a cada una de las preguntas y si tienes alguna duda dirígete al aplicador o a soporte@yoyvocacional.com.
                        </p>
                    </div>
                </div>
            </div>

              <div id="pruebaInventarioHerreraMontes" runat="server" visible="false">
                   <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                       
                        <p class="label-instrucciones-bienvenida">
                          En la medida que vayas leyendo cada pregunta, piensa ¿qué tanto te gustaría hacer ___________?, posteriormente, 
                            responde selecciona alguna de las opciones según la escala que aparece a continuación: <br />
                        4 Me gusta mucho <br />
                        3 Me gusta algo o en parte <br />
                        2 Me es indiferente, pues ni me gusta, ni me disgusta <br />
                        1 Me desagrada algo o en parte <br />
                        0 Me desagrado mucho o totalmente <br />

                        </p>
                        <p>por ejemplo <br /> Asistir a un partido de futboll</p>
                        
                                <input type="radio" name="karl" id="Radio46"/>
                        <label for="Radio41">Me desagrada mucho</label>
                               <input type="radio" name="karl" id="Radio47"/>
                        <label for="Radio42">No me gusta</label>
                              <input type="radio" name="karl" id="Radio48"/>
                        <label for="Radio43">Me es indiferente</label>
                        <input type="radio" name="karl" id="Radio49"/>
                               <label for="Radio44">Me gusta</label>
                     <input type="radio" name="karl" id="Radio50"/> 
                       <label for="Radio45">Me gusta mucho</label>
                    
                        <p>
                            Responde a cada una de las preguntas y si tienes alguna duda dirígete al aplicador o a soporte@yoyvocacional.com.
                        </p>
                    </div>
                </div>
            </div>


            <div id="bateriabullying" runat="server" visible="false">
                <div class="row">
                    <div class="col-md-10 col-md-offset-1">
                        <p class="label-instrucciones-bienvenida">
                            A continuaci&oacute;n, se presenta una serie de afirmaciones acerca de situaciones q
                            ue pudieron ocurrirles a ti, a tus compañeros o a alguien en tu escuela en los
                            &uacute;ltimos dos meses.  
                        </p>
                        <p class="label-instrucciones-bienvenida">
                            Estas pruebas no son de conocimientos, por lo que no existen respuestas correctas o incorrectas.                            
                        </p>
                        <p>
                            Responde a cada una de las preguntas y si tienes alguna duda dirígete al aplicador o a soporte@yoyvocacional.com.
                        </p>
                    </div>
                </div>
            </div>

            <div class="col-xs-12">
                <div id="bienvenida_boton_iniciar" style="text-align: center; margin-top: 10px;">
                    <div style="text-align: center">
                        <button id="btnIniciar" type="button" class="boton_iniciarBienvenida" onclick="iniciarContestarPrueba()">
                            Iniciar
                        </button>
                    </div>
                </div>
                <br />
            </div>

            <div id="welcomeSpace3" style="margin: 15px auto; height: 10px;"></div>
        </div>
    </div>
</asp:Content>

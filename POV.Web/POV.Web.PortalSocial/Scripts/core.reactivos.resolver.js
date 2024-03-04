var api = new ReactivoApi();
$(document).ready(initPage);

function initPage() {
    $("#informacion_reactivo").show();
    $("#panel_reactivo").empty();
    $("#panel_resultado").empty();
    $("#panel_reactivo").html('<div class="content_reactivo"><div id="wrapReactivo" class=""></div></div><div id="panel_preguntas"></div>');
    loadReactivo();


}

function loadReactivo() {
    if (reactivoid.val() != '') {

        var tipoDocente = comandoActivo.val() == "TD" ? 1 : 0;
       
        var tipoDocente = comandoActivo.val() == "TD" ? 1 : 0;

        var data = {
            dto: { "reactivoid": reactivoid.val(), "tipo": tipoDocente }
        }

        var dataString = $.toJSON(data);
        myApiBlockUI.loading();
        api.LoadReactivo({ success: function (result) { printReactivo(result); myApiBlockUI.unblockContainer(); } }, dataString);
        
    }
}

function printReactivo(data) {


    var dataString = $.toJSON(data);

    $("#panel_preguntas").empty();
    $("#wrapReactivo").empty();
    $("#reactivoViewTmpl").tmpl(data.d).appendTo("#panel_reactivo");
    var titulo = data.d.nombrereactivo;

    if (data.d.presentacionplantilla != 0) {
        if (data.d.presentacionplantilla == 2) {
            var wrap = $("#wrapReactivo");
            wrap.load('../Files/ResourceProxy.aspx?url=' + data.d.plantilla, function () {
                myApiBlockUI.unblockContainer();
                //esperamos que se cargue por completo la imagen para hacer los calculos de dimensiones
                $("#Div1").html('<div class="div_reactivo_result_title2">' + titulo + '</div>');
                $("#reactivo-image").load(function () {
                    calcularDimensionesContenidoReactivo();
                });

                printReferenciasReactivo();
            });
        } else if (data.d.presentacionplantilla == 3) {
            var wrap = $("#wrapReactivo");
            var texto = data.d.descripcion;
            wrap.load('../Files/ResourceProxy.aspx?url=' + data.d.plantilla, function () {
                myApiBlockUI.unblockContainer();
                //esperamos que se cargue por completo la imagen para hacer los calculos de dimensiones
                $("#enunciado-reactivo-container").prepend('<p>' + texto + '</p>');
                $("#Div1").html('<div class="div_reactivo_result_title2">' + titulo + '</div>');
                $("#reactivo-image").load(function () {
                    calcularDimensionesContenidoReactivo();
                });

                printReferenciasReactivo();
            });
        } else if (data.d.presentacionplantilla == 1) {
            var texto = data.d.descripcion;
            var element = '<div id="reactivo-contenido" class="reactivo_column_left1 box_style_reactivo_element" style="float:none">' +
                               '<div id="Div1" class="reactivo_column_left1">' +
                                    '<div class="div_reactivo_result_title2">' + titulo + '</div>' +
                                '</div>' +
                                '<div id="enunciado-reactivo-container">' + texto + '</div>' +
                          '</div>';
            $("#wrapReactivo").append(element);
        }
    } else {
        $("#informacion_reactivo").css({ "margin-top": "25px" });
    }
}

function calcularDimensionesContenidoReactivo() {
    var elementMenuRecursos = document.getElementById("reactivo-recursos-container");
    
    if (elementMenuRecursos) {
        var heightContenido = $("#reactivo-image").height();
        var minHeight = 500;
        var heightinforeactivo = $("#informacion_reactivo").height();
        
        if (heightContenido <= minHeight) {
            var auxHeight = minHeight;
            var auxMargin = (auxHeight - heightContenido - 30) / 2;
            $("#reactivo-contenido").css({ "height": auxHeight + "px" });
            $("#reactivo-image").css({ "margin-top": auxMargin + "px" });
            $("#reactivo-menu-recursos").css({ "height": ((minHeight - 30) - heightinforeactivo) + "px", "overflow": "auto" });

        } else {

            $("#reactivo-menu-recursos").css({ "height": ((heightContenido - 5) - heightinforeactivo) + "px", "overflow": "auto" });
        }
   }
}
function printReferenciasReactivo() {
    var menu = document.getElementById('reactivo-recursos-container');
    if (menu) {
        var $menu = $(menu);
        var ref = $menu.find('#reactivo-menu-recursos img.reflink');
        if (ref) {
            $menu.find("#reactivo-menu-recursos img.reflink").YouTubePopup({ idAttribute: 'id' });
        }

    }
}


function loadResource(element, url) {
    return "";
}

function getTipoPlantilla(tipo) {
    if (tipo == TIPO_ABIERTA.val()) {
        return "#abiertaTmpl";
    } else if (tipo == TIPO_OPCIONMULTIPLE.val()) {
        return "#opcionTmpl";
    }
}

function registrar() {

    var isValid = true;

    var respuestas = [];
    var index = 0;
    $('li[id|="' + reactivoid.val() + '"]').each(function (index) {
        var currentid = $(this).attr('id');

        var optionsdiv = document.getElementById("options_" + currentid);
        var abiertadiv = document.getElementById("abierta_" + currentid);

        if (optionsdiv != undefined) {
            var seleccion = $("#options_" + currentid + " input:radio:checked");
            var first = $("#options_" + currentid + " input:radio:first").attr('id');
            if (seleccion.val() != null) {

                $(this).css('border', '1px solid #E5E5E5');
                var respuestaOpcion = {
                        "preguntaid": seleccion.attr('name'),
                        "opcionseleccionadaid": seleccion.val()
                };
                respuestas[index] = respuestaOpcion;
            } else if (first != null) {
                $(this).css('border', '1px solid red');
                isValid = false;
            }
        } else if (abiertadiv != undefined) {
            var textoabierta = $("#abierta_" + currentid + " textarea");

            if (textoabierta.val() != '') {
                $(this).css('border', '1px solid #E5E5E5');
                var respuestaAbierta = {
                    "preguntaid": textoabierta.attr('id'),
                    "textoabierta": textoabierta.val()

                };
                respuestas[index] = respuestaAbierta;
            } else {
                $(this).css('border', '1px solid red');
                isValid = false;
            }
        }

        index++;
    });

    if (isValid) {
        if (comandoActivo.val() != "T" && comandoActivo.val() != "TD") {
            var data = {
                dto: {
                    "preguntas": respuestas,
                    "reactivoid": reactivoid.val()
                }
            };
            var dataString = $.toJSON(data);
            myApiBlockUI.send();
            api.SaveAnswers({
                success: function (result) {
                    myApiBlockUI.unblockContainer();
                    printResultRespuesta(result.d);

                }
            }, dataString);
        } else {
            $(btnFin).click();
        }
    } else {
        $("#txtRedirect").val('');
        $(hdnmessageinputid).val("Debe completar el ejercicio de práctica");
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}

function printResultRespuesta(data) {
    if (data.success == true) {
        $("#panel_reactivo").empty();
        $("#panel_resultado").empty();
        $("#informacion_reactivo").hide();
        $("#resultadoTmpl").tmpl(data).appendTo("#panel_resultado");
    } else {
        alert(data.error);
    }
}

function publicar() {
    var contenido = $("#txtPublicacion").val();
    contenido = $.trim(contenido);
    if (contenido != '') {
        var data = {
            publicaciondto: {
                "contenido": contenido,
                "usuariosocialid": usrse.val(),
                "tipo": 1,
                "socialhubid": hub.val()
            }
        };

        var dataString = $.toJSON(data);
        api.Share({ success: function (result) {
            window.location = "../Reactivos/Sugerencias.aspx";
        } }, dataString);

    }
}

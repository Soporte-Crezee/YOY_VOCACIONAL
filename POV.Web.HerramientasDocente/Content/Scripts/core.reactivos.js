
var reactivoapi = new ReactivoApi();
$(function () {
    applyButtonStyle();
    $.get('../../Content/Scripts/tmpl/reactivos.tmpl.htm', function (templates) {
        $('head').append(templates);
        if ($(reactivo_id).val() == null) {
            agregarOpcionMultiple();
        }
    });
});

function applyButtonStyle() {
    $(".boton").button();
}

function agregarAbierta() {
    var currentDate = new Date();
    var num = currentDate.getTime();
    var data = { preguntaid: num };
    $("#new-abierta-tmpl").tmpl(data).appendTo("#preguntas_list");
    applyButtonStyle();

    $("#error-r-p").html('');
}

function agregarOpcionMultiple() {
    var currentDate = new Date();
    var num = currentDate.getTime();
    var data = { preguntaid: num };
    $("#new-opt-multiple-tmpl").tmpl(data).appendTo("#preguntas_list");
    applyButtonStyle();
    num++;
    $("#error-r-p").html('');
}

function eliminarPregunta(id) {
    var obj = document.getElementById("p-" + id);
    $(obj).remove();
    containsPreguntas();
}

function agregarOpcion(id) {
    var currentDate = new Date();
    var number = currentDate.getTime();

    var data = { preguntaid: id, opcionid: number };
    $("#new-option-tmpl").tmpl(data).appendTo("#options-stream-p-" + id);
    applyButtonStyle();
    $("#option-text-p-" + id + "-" + number).focus();
    isValidPreguntas();
}

function eliminarOpcion(id, number) {
    var obj = document.getElementById("option-li-p-" + id + "-" + number);

    $(obj).remove();
    isValidPreguntas();
}
/*Load reactivo*/

function loadReactivo() {
    if ($(reactivo_id).val() != null && $(reactivo_id).val() != '') {

        var data = {
            dto: {
                "reactivoid": $(reactivo_id).val(),
                "docenteid": parseInt(docenteId.val()),
                "tipodocente": true

            }
        }
        var dataString = $.toJSON(data);
        reactivoapi.LoadReactivo({ success: function (result) { OnLoadReactivoComplete(result); } }, dataString);
    }
}

function OnLoadReactivoComplete(data) {
    var reactivo = data.d;
    $("#txtNombre").val(reactivo.nombrereactivo);
    $("#txtUrl").val(reactivo.plantilla);
    $("#txtValor").val(reactivo.valor);
    $("#txtDescripcion").val(reactivo.descripcion);
    $("#txtRetroalimentacion").val(reactivo.retroalimentacion);
    for (var index = 0; index < reactivo.preguntas.length; index++) {
        var pregunta = reactivo.preguntas[index];
        if (pregunta.tipoplantilla == TIPO_ABIERTA.val()) {
            $("#edit-abierta-tmpl").tmpl(pregunta).appendTo("#preguntas_list");
        } else if (pregunta.tipoplantilla == TIPO_OPCIONMULTIPLE.val()) {
            $("#edit-opt-multiple-tmpl").tmpl(pregunta).appendTo("#preguntas_list");
        }
    }
    applyButtonStyle();

}

var preguntas_deleted = [];
var opciones_deleted = [];

function eliminarEditPregunta(id) {
    var apiMessages = new MessageApi();
    var message = "¿Está seguro de eliminar este registro?";
    apiMessages.CreateQuestionMessage(message, function () {
        var reactivoid = '';
        if (reactivo_id != null && $(reactivo_id).val() != '')
            reactivoid = $(reactivo_id).val();
        var data = {
            "preguntaid": id,
            "reactivoid": reactivoid
        };
        preguntas_deleted[preguntas_deleted.length] = data;
        $("#dialog-question").dialog('close');
        eliminarPregunta(id);

    });

    apiMessages.ShowQuestion();
}

function processResponseDeletePregunta(data) {
    $("#dialog-question").dialog('close');
    if (data.d.success == true) {
        $(hdnmessageinputid).val("La pregunta se elimino con éxito.");
        $(hdnmessagetypeinputid).val('3');
        mostrarInfo();
        eliminarPregunta(data.d.preguntaid);
    } else if (data.d.success == false) {
        $(hdnmessageinputid).val(data.d.error);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}
function eliminarEditOpcion(id, number) {
    var apiMessages = new MessageApi();
    var message = "¿Est&aacute; seguro de eliminar este registro?";
    apiMessages.CreateQuestionMessage(message, function () {

        var data = {
            "preguntaid": id,
            "opcionid": number
        };
        opciones_deleted[opciones_deleted.length] = data;
        $("#dialog-question").dialog('close');
        eliminarOpcion(id, number);

    });

    apiMessages.ShowQuestion();
}

function processResponseDeleteOpcionPregunta(data) {
    $("#dialog-question").dialog('close');
    if (data.d.success == true) {
        $(hdnmessageinputid).val("La opción se elimino con éxito.");
        $(hdnmessagetypeinputid).val('3');
        mostrarInfo();
        eliminarOpcion(data.d.preguntaid, data.d.opcionid);
    } else if (data.d.success == false) {
        $(hdnmessageinputid).val("No se pudo eliminar el registro.");
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}
////////////////

function guardar() {
    var isValid = true;
    isValid = isValid && isValidForm();
    isValid = isValid && isValidPreguntas();
    isValid = isValid && containsPreguntas();

    if (isValid) {

        var reactivo = createReactivo();

        var data = {
            dto: reactivo
        };
        var dataString = $.toJSON(data);

        reactivoapi.SaveComplete({ success: function (result) { processResponse(result); } }, dataString);
    }
}

function processResponse(data) {
    if (data.d.success == true) {
        $("#txtRedirect").val('BuscarReactivos.aspx');
        $(hdnmessageinputid).val("El reactivo se registro con exito.");
        $(hdnmessagetypeinputid).val('3');
        mostrarInfo();
    } else if (data.d.success == false) {
        $(hdnmessageinputid).val(data.d.errors[0]);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}

function createPreguntas() {
    var preguntas = []
    var i = 0;
    $('li[id|="p"]').each(function (index) {
        var id = $(this).attr('id').split('-')[1]
        var type = parseInt($("#p-" + id + "-type").val());
        var opciones = []

        if (type == TIPO_ABIERTA.val()) {

        } else if (type = TIPO_OPCIONMULTIPLE.val()) {
            var j = 0;
            $('li[id|="option-li-p-' + id + '"]').each(function (index) {
                var optionid = $(this).attr('id').split('-')[4];
                var bd_id;
                if ($("#option-li-p-" + id + "-" + optionid + "-ID").val() != '')
                    bd_id = parseInt($("#option-li-p-" + id + "-" + optionid + "-ID").val());

                var check = !$("#option-radio-p-" + id + "-" + optionid + ":checked").val() ? false : true;

                var opcion = {
                    "opcionid": bd_id,
                    "texto": $("#option-text-p-" + id + "-" + optionid).val(),
                    "check": check
                };
                opciones[j] = opcion;
                j++;
            });
        }

        var idpregunta;
        var idplantilla;
        if ($("#p-" + id + "-ID").val() != '')
            idpregunta = parseInt($("#p-" + id + "-ID").val());
        if ($("#p-" + id + "-plantilla-ID").val() != '')
            idplantilla = parseInt($("#p-" + id + "-plantilla-ID").val());
        var pregunta = {
            "preguntaid": idpregunta,
            "plantillaid": idplantilla,
            "textopregunta": $("#descripcion-p-" + id).val(),
            "tipoplantilla": type,
            "opciones": opciones
        }

        preguntas[i] = pregunta;

        i++;
    });



    return preguntas;
}

function createReactivo() {
    var preguntas = createPreguntas();
    var id = '';
    if (reactivo_id != null && $(reactivo_id).val() != '')
        id = $(reactivo_id).val();
    var reactivo = {
        "reactivoid": id,
        "presentacionplantilla": parseInt($(presentacion).val()),
        "nombrereactivo": $("#txtNombre").val(),
        "plantilla": $("#txtUrl").val(),
        "descripcion": $("#txtDescripcion").val(),
        "retroalimentacion": $("#txtRetroalimentacion").val(),
        "tipocomplejidadid": parseInt($(complejidad_select_id).val()),
        "areaaplicacionid": parseInt($(area_select_id).val()),
        "preguntas": preguntas,
        "preguntasdeleted": preguntas_deleted,
        "opcionesdeleted": opciones_deleted,
        "docenteid": parseInt(docenteId.val()),
        "tipodocente": true
    };

    return reactivo;
}
function isValidForm() {
    return $("#frmMain").validate().form();
}

function isValidPreguntas() {
    var isValid = true;
    $('ul[id|="options-stream-p"]').each(function (index) {
        var currentid = $(this).attr('id');
        var size = $("#" + currentid + " li").size();

        if (size < 2) {
            $("#" + currentid + "-error").html('La pregunta debe tener al menos dos opciones');
            isValid = false;
        } else {
            $("#" + currentid + "-error").html('');
        }
    });

    return isValid;
}

function containsPreguntas() {
    var size = $("#preguntas_list li").size();

    if (size < 1) {
        $("#error-r-p").html('El reactivo debe tener al menos una pregunta');
        return false;
    } else {
        $("#error-r-p").html('');
        return true;
    }
}
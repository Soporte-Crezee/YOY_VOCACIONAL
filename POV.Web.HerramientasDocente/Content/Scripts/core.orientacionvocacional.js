
var orientationVocacionalApi = new OrientacionVocacionalApi();

function getSesionOrientacionDocente(data) {

    orientationVocacionalApi.GetSesionOrientacionDocente({
        success: function (result) {
            loadSesionesDocente(result.d);
        },
    });
}

function loadSesionesDocente(result) {
    sesionesDocente = [];
    if (result.length > 0) {
        for (var i = 0; i < result.length; i++) {

            var getEvent = new Object();
            getEvent.sesionorientacionid = result[i].sesionorientacionid;
            getEvent.inicio = result[i].inicio;
            getEvent.fin = result[i].fin;
            getEvent.fecha = result[i].fecha;
            getEvent.asistenciaaspirante = result[i].asistenciaaspirante;
            getEvent.asistenciaorientador = result[i].asistenciaorientador;
            getEvent.alumnoid = result[i].alumnoid;
            getEvent.docenteid = result[i].docenteid;
            getEvent.estatussesion = result[i].estatussesion;
            getEvent.encuestacontestada = result[i].encuestacontestada;
            getEvent.horafinalizada = result[i].horafinalizada;

            sesionesDocente.push(getEvent);
            cargaSesionesDocente = true;
        }
    }
    cargaSesionesDocente = true;
}

function saveSesionOrientacion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.UpdateSesionOrientacion({
        success: function (result) {
        },
        error: function (result) {
        }
    }, dataString);
}

function finSesionOrientacion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.FinalizarSesionOrientacion({
        success: function (result) {
            if (result.d.Success != '') {
                if (result.d.asistenciaorientador == 0) {
                }
                showMessage('La sesion a finalizado', 'nonRedirection');
            }
        },
        error: function (result) {
        }
    }, dataString);
}

function sendReembolso(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.EnviarCorreoReembolso({
        success: function (result) {
        },
        error: function (result) {
        }
    }, dataString);
}


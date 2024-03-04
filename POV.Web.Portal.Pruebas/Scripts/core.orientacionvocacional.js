
var orientationVocacionalApi = new OrientacionVocacionalApi();

function getSesionOrientacionAlumno(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.GetSesionOrientacionAlumno({
        success: function (result) {
            if (result != '') {
                loadSesionesAlumno(result.d);
            } else {
            }
        },
        error: function (result) {
        }
    }, dataString);
}

function loadSesionesAlumno(result) {
    sesionesAlumno = [];
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
            getEvent.estatusesion = result[i].estatusesion;
            getEvent.encuestacontestada = result[i].estatusesion;
            getEvent.horafinalizada = result[i].horafinalizada;

            sesionesAlumno.push(getEvent);
            cargaSesionesAlumno = true;
        }   
        cargaSesionesAlumno = true;
}
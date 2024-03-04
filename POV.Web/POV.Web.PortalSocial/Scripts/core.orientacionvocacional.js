
var orientationVocacionalApi = new OrientacionVocacionalApi();

function saveConfigCalendar(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.GuardarConfiguracionAgenda({
        success: function (result) {  
            if (result.d.Success != '')
            showMessage(result.d.Success);
            if (result.d.Error != '') {
                showMessage(result.d.Error, 'nonRedirector');
                $("#modalCloseButton").unbind('click');
                $("#modalCloseButton").click(function () {
                    $("#divConfigCalendar").click();
                });
            }
        },
        error: function (result) {
            showMessage("Ha ocurrido un error al intentar guardar los cambios, intente más tarde.");
        }
    }, dataString);
}

function getConfigCalendar(data) {
    myApiBlockUI.loading();
    var dataString = $.toJSON(data);
    orientationVocacionalApi.GetConfiguracionAgenda({
        success: function (result) {
            if (result.d.Success != '') {
                loadConfigCalendar(result.d);
            }
            if (result.d.Error != '') {
                myApiBlockUI.unblockContainer();
                showMessage(result.d.Error, 'nonRedirection');
            }
        },
        error: function (result) {
            myApiBlockUI.unblockContainer();
            showMessage("Ha ocurrido un error al cargar la configuración del calendario, intente más tarde.");
        }
    }, dataString);
}

function getConfigCalendarOrientador(data) {
    myApiBlockUI.loading();
    var dataString = $.toJSON(data);

    orientationVocacionalApi.GetConfiguracionAgendaOrientador({
        success: function (result) {
            if (result.d.Success != '')
                loadConfigCalendar(result.d);
            $("#noCalendar").hide();
            if (result.d.Error != '') {
                myApiBlockUI.unblockContainer();
                var htm = '<span>No se ha creado una configuraci&oacute;n de calendario</span>';
                $("#noCalendar").show();
                $("#noCalendar").empty();
                $("#noCalendar").html(htm);
            }
        },
        error: function (result) {
            myApiBlockUI.unblockContainer();
            showMessage("Ha ocurrido un error al cargar la configuración del calendario, intente más tarde.");
        }
    }, dataString);
}

function loadConfigCalendar(result) {
    businessHours = [];
    var bHours = new Object();;
    var dow = [];

    var dowArray = parseInt(result.diaslaborales.split(',').length);
    for (var i = 0; i < dowArray ; i++)
        dow.push(parseInt(result.diaslaborales.split(',')[i]));

    if (result.iniciodescanso != '' && result.iniciodescanso != undefined) {
        bHours.configcalendarid = result.configcalendarid;
        bHours.dow = dow;
        bHours.start = result.iniciotrabajo;
        bHours.end = result.iniciodescanso;
        businessHours.push(bHours);

        bHours = new Object();
        bHours.dow = dow;
        bHours.start = result.findescanso;
        bHours.end = result.fintrabajo;
        businessHours.push(bHours);
        cargaCompleta = true;
    }
    else {
        bHours.configcalendarid = result.configcalendarid;
        bHours.dow = dow;
        bHours.start = result.iniciotrabajo;
        bHours.end = result.fintrabajo;
        businessHours.push(bHours);
        cargaCompleta = true;
    }
}

function saveEventCalendar(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.GuardarEvento({
        success: function (result) {
            if (result.d.Success != '') {
                sendConfirmacion(data);
                showMessage(result.d.Success);
            }
            if(result.d.Error != ''){
                showMessage(result.d.Error,'',"ERROR");
            }
        },
        error: function (result) {
            showMessage("Ha ocurrido un error al intentar guardar los cambios, intente más tarde.",'',"ERROR");
        }
    }, dataString);
}

function saveEventCalendarAlumno(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.GuardarEventoAlumno({
        success: function (result) {
            if (result.d.Success != '') {
                sendCorreo(data);
                showMessage(result.d.Success);
            }
            if (result.d.Error != '')
                showMessage(result.d.Error, 'nonRedirection', "ERROR");
        },
        error: function (result) {
            showMessage("Ha ocurrido un error al intentar guardar los cambios, intente más tarde.",'',"ERROR");
        }
    }, dataString);
}

function sendCorreo(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.EnviarCorreoSolicitud({
        success: function (result) {
        },
        error: function (result) {
        }
    }, dataString);
}

function sendConfirmacion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.EnviarCorreoConfirmacion({
        success: function (result) {
        },
        error: function (result) {
        }
    }, dataString);
}

function getEventCalendar(data) {
    var dataString = $.toJSON(data);

    orientationVocacionalApi.GetEvento({
        success: function (result) {
            if (result.d.Success != '') {
                loadEventOrientador(result.d);
            }
        },
        error: function (result) {
            showMessage("Ha ocurrido un error al cargar los eventos del calendario, intente mas tarde.");
        }
    }, dataString);
}

function loadEvent(result) {
    eventos = [];
    var alid = alumno.val();
    for (var i = 0; i < result.length; i++) {
        var getEvent = new Object();
        getEvent.title = result[i].asunto;
        getEvent.start = result[i].hrsinicio;
        getEvent.end = result[i].hrsfin;
        getEvent.fecha = result[i].fecha;
        getEvent.eventcalendarid = result[i].eventcalendarid;
        getEvent.alumnoid = result[i].alumnoid;
        getEvent.usuarioid = result[i].usuarioid;
        if (result[i].alumnoid != alid) {
            getEvent.title = "";
            getEvent.start = result[i].hrsinicio;
            getEvent.end = result[i].hrsfin;
            getEvent.color = '#ff9f89'; //color del evento
        }
        eventos.push(getEvent);
        cargaCompletaEvent = true;
    }
    cargaCompletaEvent = true;
}

function loadEventOrientador(result) {
    eventos = [];
        for (var i = 0; i < result.length; i++) {

            var getEvent = new Object();
            getEvent.title = result[i].asunto;
            getEvent.nombrecompletoalumno = result[i].nombrecompletoalumno;
            getEvent.start = result[i].hrsinicio;
            getEvent.end = result[i].hrsfin;
            getEvent.fecha = result[i].fecha;
            getEvent.eventcalendarid = result[i].eventcalendarid;
            getEvent.alumnoid = result[i].alumnoid;
            getEvent.usuarioid = result[i].usuarioid;
            getEvent.cantidadhoras = result[i].cantidadhoras;

            eventos.push(getEvent);
            cargaCompletaEvent = true;
        }
        cargaCompletaEvent = true;
}

function getEventCalendarOrientador(data) {
    var dataString = $.toJSON(data);

    orientationVocacionalApi.GetEventoOrientador({
        success: function (result) {
            if (result != '')
                loadEvent(result.d);
            else {
                showMessage(result.d.Error, 'nonRedirect');
            }
        },
        error: function (result) {
            showMessage("Ha ocurrido un error al cargar los eventos del calendario, intente mas tarde.");
        }
    }, dataString);
}

function loadSolicitudes(result) {
    solicitudes = [];   
    if (result.length > 0) {
        for (var i = 0; i < result.length; i++) {

            var getEvent = new Object();
            getEvent.title = result[i].asunto;
            getEvent.nombrecompletoalumno = result[i].nombrecompletoalumno;
            getEvent.start = result[i].hrsinicio;
            getEvent.end = result[i].hrsfin;
            getEvent.fecha = result[i].fecha;
            getEvent.eventcalendarid = result[i].eventcalendarid;
            getEvent.alumnoid = result[i].alumnoid;
            getEvent.usuarioid = result[i].usuarioid;
            getEvent.cantidadhoras = result[i].cantidadhoras;

            $("#tblEventos tbody").append("<tr><td class='td-label'>" + getEvent.title + "</td>" +
                                                                  "<td>" + getEvent.fecha + "</td>" +
                                                                  "<td>" + getEvent.nombrecompletoalumno + "</td>" +
                                                                  "<td>" + getEvent.start.split('T')[1].split(':')[0] + ':00' + "</td>" +
                                                                  "<td>" + getEvent.end.split('T')[1].split(':')[0] + ':00' + "</td>" +
                                                                  "<td style='display: none;'>" + getEvent.eventcalendarid + "</td>" +
                                                                  "<td><input type='button' onclick='verDetalleEvento(" + JSON.stringify(getEvent) + ")' id='btnSolic' class='btnDetalle button_clip_39215E' value='Ver' /></td></tr>");

            solicitudes.push(getEvent);
            cargaCompletaSolicitudes = true;
        }
    }
    else
    {
        $("#tblEventos tbody").append("<tr><td class='td-label' colspan='5' style='text-align:center'>Sin solicitudes pendientes</td></tr>");
        cargaCompletaSolicitudes = true;
    }
    cargaCompletaSolicitudes = true;
}

function getSolicitudes(data) {
    var dataString = $.toJSON(data);

    orientationVocacionalApi.GetSolicitudes({
        success: function (result) {
            if (result.d.Success != '') {
                loadSolicitudes(result.d);
            }
        },
        error: function (result) {
            showMessage("Ha ocurrido un error al cargar las solicitudes, intente mas tarde.",'',"ERROR");
        }
    }, dataString);
}

function deleteSesion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.EliminarSesion({
        success: function (result) {
            if (result.d.Success != '') {
                sendCancelacion(data);
                showMessage(result.d.Success);
            } else {
                showMessage(result.d.Error);
            }
        },
        error: function (result) {
        }
    }, dataString);
}

function sendCancelacion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.EnviarCorreoCancelacion({
        success: function (result) {
        },
        error: function (result) {
        }
    }, dataString);
}

function getSesionOrientacionAlumno() {
        orientationVocacionalApi.GetSesionOrientacionAlumno({
            success: function (result) {
                if (result.d.Success != '') {
                    // comparar fecha para mostrar ventana de asistencia
                    loadSesionesAlumno(result.d);                    
                }
            },
            error: function (result)
            {
            }
        });
}

function loadSesionesAlumno(result) {
    sesionesAlumno = [];
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

            sesionesAlumno.push(getEvent);
            cargaSesionesAlumno = true;            
        }      
    }
    cargaSesionesAlumno = true;
}

function getSesionOrientacionDocente(data) {

    orientationVocacionalApi.GetSesionOrientacionDocente({
        success: function (result) {
            if (result.d.Success != '') {
                loadSesionesDocente(result.d);
            }
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
        success: function (result)
        {
        },
        error: function (result)
        {
        }
    }, dataString);
}

function finSesionOrientacion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.FinalizarSesionOrientacion({
        success: function (result) {
            if (result.d.Success != '') {
                if (result.d.asistenciaorientador == 0) {
                    showMessage(result.d.Success, 'nonRedirection');
                }
                showMessage(result.d.Success, 'nonRedirection');
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

function saveEncuestaSatisfaccion(data) {
    var dataString = $.toJSON(data);
    orientationVocacionalApi.GuardarEncuestaSatisfaccion({
        success: function (result) {
            if (result.d.Success != '') {
                showMessage(result.d.Success, 'nonRedirection');
            }
            if (result.d.Error != '')
                showMessage(result.d.Error, 'nonRedirection');
        },
        error: function (result) {
        }
    }, dataString);
}
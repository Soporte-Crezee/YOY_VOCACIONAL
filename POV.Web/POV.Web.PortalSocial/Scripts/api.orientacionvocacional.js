var OrientacionVocacionalApi = function () { };

OrientacionVocacionalApi.prototype.GuardarConfiguracionAgenda = function (options, configs) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GuardarConfiguracionAgenda"),
        type: 'POST',
        data: configs,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetConfiguracionAgenda = function (options, configs) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetConfiguracionAgenda"),
        type: 'POST',
        data: configs,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetConfiguracionAgendaOrientador = function (options, configs) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetConfiguracionAgendaOrientador"),
        type: 'POST',
        data: configs,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GuardarEvento = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GuardarEvento"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.EnviarCorreoSolicitud = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/EnviarCorreoSolicitud"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.EnviarCorreoConfirmacion = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/EnviarCorreoConfirmacion"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GuardarEventoAlumno = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GuardarEventoAlumno"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetEvento = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetEvento"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetEventoOrientador = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    GetEventoOrientador = $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetEventoOrientador"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetSolicitudes = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetSolicitudes"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.EliminarSesion = function (options, eventos) {
    
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/EliminarSesion"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.EnviarCorreoCancelacion = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/EnviarCorreoCancelacion"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetSesionOrientacionAlumno = function (options, sesiones) {
    var sesion = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetSesionOrientacionAlumno"),
        type: 'POST',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { sesion.success(result); },
        error: function (result) { sesion.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GetSesionOrientacionDocente = function (options, sesiones) {
    var sesion = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf+"OrientacionVocacionalService.svc/GetSesionOrientacionDocente"),
        type: 'POST',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { sesion.success(result); },
        error: function (result) { sesion.error(result); }
    });
};

OrientacionVocacionalApi.prototype.UpdateSesionOrientacion = function (options, sesiones) {
    var sesion = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/UpdateSesionOrientacion"),
        type: 'POST',
        data: sesiones,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { sesion.success(result); },
        error: function (result) { sesion.error(result); }
    });
};

OrientacionVocacionalApi.prototype.FinalizarSesionOrientacion = function (options, sesiones) {
    var sesion = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/FinalizarSesionOrientacion"),
        type: 'POST',
        data: sesiones,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { sesion.success(result); },
        error: function (result) { sesion.error(result); }
    });
};

OrientacionVocacionalApi.prototype.EnviarCorreoReembolso = function (options, eventos) {
    var evento = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/EnviarCorreoReembolso"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};

OrientacionVocacionalApi.prototype.GuardarEncuestaSatisfaccion = function (options, respuestas) {
    var respuesta = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GuardarEncuestaSatisfaccion"),
        type: 'POST',
        data: respuestas,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { respuesta.success(result); },
        error: function (result) { respuesta.error(result); }
    });
};
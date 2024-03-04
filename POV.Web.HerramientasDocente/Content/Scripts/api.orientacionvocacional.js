var OrientacionVocacionalApi = function () { };

OrientacionVocacionalApi.prototype.GetSesionOrientacionDocente = function (options, sesiones) {
    var sesion = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "OrientacionVocacionalService.svc/GetSesionOrientacionDocente"),
        type: 'POST',
        data: sesiones,
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
        url: encodeURI(url_wcf + "OrientacionVocacionalService.svc/UpdateSesionOrientacion"),
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
        url: encodeURI(url_wcf + "OrientacionVocacionalService.svc/FinalizarSesionOrientacion"),
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
        url: encodeURI(url_wcf + "OrientacionVocacionalService.svc/EnviarCorreoReembolso"),
        type: 'POST',
        data: eventos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { evento.success(result); },
        error: function (result) { evento.error(result); }
    });
};
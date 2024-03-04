var OrientacionVocacionalApi = function () { };

OrientacionVocacionalApi.prototype.GetSesionOrientacionAlumno = function (options, sesiones) {
    var sesion = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/OrientacionVocacionalService.svc/GetSesionOrientacionAlumno"),
        type: 'POST',
        data: sesiones,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { sesion.success(result); },
        error: function (result) { sesion.error(result); }
    });
};
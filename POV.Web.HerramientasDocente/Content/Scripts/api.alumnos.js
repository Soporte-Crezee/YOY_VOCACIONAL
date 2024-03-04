var AlumnosApi = function () { };

AlumnosApi.prototype.ValidateAlumnoAsignadoEscuela = function (options,alumno) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/AlumnoService.svc/ValidateAlumnoAsignadoEscuela"),
        type: 'POST',
        data: alumno,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        async: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
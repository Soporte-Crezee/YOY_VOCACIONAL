var AsistenciaApi = function () { };

AsistenciaApi.prototype.GetInformacionContenido = function(options, asistencia) {
    var config = $.extend({
        success: function() {
        },
        error: function() {
        }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "AsistenciaService.svc/GetInformacionContenido"),
        type: 'POST',
        data: asistencia,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function(result) { config.success(result); },
        error: function(result) { config.error(result); }
    });
};

AsistenciaApi.prototype.SearchAsistencia = function (options, asistencia) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "AsistenciaService.svc/SearchAsistencia"),
        type: 'POST',
        data: asistencia,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
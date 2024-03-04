var CursosApi = function () { };
CursosApi.prototype.GetCursos = function (options, curso) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);
 
    $.apiCall({
        url: encodeURI(url_wcf + "CursosService.svc/GetCursos"),
        type: 'POST',
        data: curso,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
CursosApi.prototype.GetDetalleCurso = function (options, curso) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "CursosService.svc/GetInformacionCurso"),
        type: 'POST',
        data: curso,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
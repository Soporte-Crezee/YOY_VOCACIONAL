var EjesTematicosApi = function () { };

EjesTematicosApi.prototype.GetInformacionEjeTematico = function (options, ejetematico) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "EjesTematicosService.svc/GetInformacionEjeTematico"),
        type: 'POST',
        data: ejetematico,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

/**CU193-Consultar Eje Temático Red Social**/

EjesTematicosApi.prototype.SearchEjesTematico = function (options, ejeTematico) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "EjesTematicosService.svc/SearchEjesTematico"),
        type: 'POST',        
        data: ejeTematico,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

/**CU193-Consultar Eje Temático Red Social**/
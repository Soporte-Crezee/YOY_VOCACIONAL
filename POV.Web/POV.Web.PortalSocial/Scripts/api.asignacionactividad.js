var AsignacionActividadApi = function () { };

AsignacionActividadApi.prototype.GetTotalAsignacionActividad = function (options) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "NotificacionService.svc/GetTotalAsignacionActividad"),
        type: 'POST',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
var NotificacionesApi = function () { };

NotificacionesApi.prototype.GetTotalNotificaciones = function (options) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "NotificacionService.svc/GetTotalNotificaciones"),
        type: 'POST',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

NotificacionesApi.prototype.GetNotificaciones = function (options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

 GetNotificaciones = $.apiCall({
     url: encodeURI(url_wcf + "NotificacionService.svc/GetNotificaciones"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

NotificacionesApi.prototype.DeleteNotificacion = function (options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "NotificacionService.svc/DeleteNotificacion"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};


NotificacionesApi.prototype.ConfirmNotificacion = function (options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "NotificacionService.svc/ConfirmNotificacion"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
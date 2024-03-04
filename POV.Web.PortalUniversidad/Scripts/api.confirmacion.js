var ConfirmacionApi = function () { };

ConfirmacionApi.prototype.GetInfoConfirmacion = function (options) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ConfirmarUniversidadService.svc/GetInfoConfirmacion"),
        type: 'POST',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ConfirmacionApi.prototype.ConfirmarInformacionUniversidad = function (options, confirmacion) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ConfirmarUniversidadService.svc/ConfirmarInformacionUniversidad"),
        type: 'POST',
        data: confirmacion,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
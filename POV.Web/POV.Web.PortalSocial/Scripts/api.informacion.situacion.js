var InformacionSituacionApi = function () { };

InformacionSituacionApi.prototype.Search = function (options, contenido) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ContenidosDigitalesService.svc/ConsultarContenidoInformacionSituacion"),
        type: 'POST',
        data: contenido,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
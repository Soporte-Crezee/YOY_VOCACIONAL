var ContenidosApi = function () { };

ContenidosApi.prototype.Search = function (options, contenidos) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "ContenidosDigitalesService.svc/SearchContenidoDigital"),
        type: 'POST',
        data: contenidos,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
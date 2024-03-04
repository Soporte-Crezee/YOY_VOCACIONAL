var FavoritosApi = function () { };

FavoritosApi.prototype.GuardarFavorito = function (options, favorito) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/FavoritosService.svc/GuardarFavorito"),
        type: 'POST',
        data: favorito,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

FavoritosApi.prototype.EliminarFavorito = function (options, favorito) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/FavoritosService.svc/EliminarFavorito"),
        type: 'POST',
        data: favorito,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
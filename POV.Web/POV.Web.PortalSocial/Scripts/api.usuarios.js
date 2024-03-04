var UsuariosApi = function () { };

UsuariosApi.prototype.ValidateUsuario = function (options, usuario) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/UsuarioService.svc/ValidateUsuario"),
        type: 'POST',
        data: usuario,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        async: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
var ReactivoApi = function () { };

ReactivoApi.prototype.Search = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/SearchReactivos"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReactivoApi.prototype.ListByUser = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/GetReactivosUsuario"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReactivoApi.prototype.Join = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/SuscribirReactivo"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReactivoApi.prototype.Leave = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/QuitarSuscripcion"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReactivoApi.prototype.LoadReactivo = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/GetReactivo"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};


ReactivoApi.prototype.LoadAnswers = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/GetRespuestaReactivoUsuario"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReactivoApi.prototype.SaveAnswers = function (options, reactivo) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReactivosService.svc/RegistrarRespuestas"),
        type: 'POST',
        data: reactivo,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReactivoApi.prototype.Share = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/PublicacionesService.svc/SaveSugerencia"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
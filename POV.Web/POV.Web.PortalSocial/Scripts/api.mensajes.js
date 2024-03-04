var MensajeApi = function () { };
//Consulta los mensajes
MensajeApi.prototype.LoadMensajes = function (options, mensaje) {

    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/MensajesService.svc/GetMensajes"),
        type: 'POST',
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
       
    });
    

};

//Consulta los utimos mensajes
MensajeApi.prototype.LoadLastMensajes = function (options, mensaje) {

    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/MensajesService.svc/GetLastMensajes"),
        type: 'POST',
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
       
    });


};
MensajeApi.prototype.LoadPageMensajes = function (options, mensaje) {

    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/MensajesService.svc/GetMensajesPaginados"),
        type: 'POST',
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
       
    });
    

};
MensajeApi.prototype.Send = function (options, mensaje) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/MensajesService.svc/SaveMensaje"),
        type: 'POST',
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
MensajeApi.prototype.SendComment = function (options, mensaje) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/MensajesService.svc/SaveRespuestaMensaje"),
        type: 'POST',
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
MensajeApi.prototype.Delete = function (options, mensaje) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/MensajesService.svc/RemoveAsociadoMensaje"),
        type: 'POST',
        data: mensaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};


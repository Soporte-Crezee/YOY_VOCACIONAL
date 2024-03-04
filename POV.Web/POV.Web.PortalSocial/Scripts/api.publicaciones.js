var PublicacionApi = function () { };
//Consulta publicaciones iniciales
PublicacionApi.prototype.LoadPubs = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPublicaciones"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};


PublicacionApi.prototype.LoadSugerencias = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetSugerencias"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LoadPubsMuroDocente = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPublicacionesMuroDocente"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LoadDudasMuroDocente = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetDudasMuroDocente"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LoadPubsMuroMiDocente = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPublicacionesMuroDocente"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LoadPubsMisDocentes = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPublicacionesDocentes"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
PublicacionApi.prototype.LoadPubsSocial = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPublicacionesContactos"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LoadPubsVoted = function (options, publication) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetMasVotadas"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
PublicacionApi.prototype.Share = function(options, publication) {
    var config = $.extend({
            success: function() { },
            error: function() { }
        }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/SavePublicacion"),
        type: 'POST',
        data: publication,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
        });
};
PublicacionApi.prototype.Comment = function (options, comment) {
    var config = $.extend({
            success: function() { },
            error: function() { }
        }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/SaveComentario"),
            type: 'POST',
            data: comment,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processData: false,
            success: function(result) { config.success(result); },
            error: function(result) { config.error(result); }
        });
};
PublicacionApi.prototype.RemovePub = function(options, pub) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/RemovePublicacion"),
        type: 'POST',
        data: pub,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result);},
        error: function (result) { config.error(result);}
    });
};
PublicacionApi.prototype.RemoveComment = function(options, comment) {
    var config = $.extend({
            success: function() { },
            error: function() { }
        }, options);

    $.apiCall({
            url: encodeURI(url_wcf + "PublicacionesService.svc/RemoveComentario"),
            type: 'POST',
            data: comment,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processData: false,
            success: function(result) { config.success(result);},
            error: function(result) { config.error(result); }
        });
    };

    PublicacionApi.prototype.GetPub = function (options, pub) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPublicacion"),
        type: 'POST',
        data: pub,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LikePub = function (options, pub) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/MasUnoPublicacion"),
        type: 'POST',
        data: pub,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.LikeComment = function (options, comment) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/MasUnoComentario"),
        type: 'POST',
        data: comment,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

PublicacionApi.prototype.GetPeople = function (options, ranking) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI(url_wcf + "PublicacionesService.svc/GetPeople"),
        type: 'POST',
        data: ranking,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};


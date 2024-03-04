var ReporteAbusoApi = function () { };

ReporteAbusoApi.prototype.GetReportesAbuso = function (options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReporteAbusoService.svc/GetReportesAbuso"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReporteAbusoApi.prototype.ConfirmReporteAbuso = function(options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReporteAbusoService.svc/ConfirmReporteAbuso"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReporteAbusoApi.prototype.GetPublicacionReporteAbuso = function (options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReporteAbusoService.svc/GetCompleteReporteAbuso"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReporteAbusoApi.prototype.DeleteReporteAbuso = function(options, data) {

    var config = $.extend({
        success: function() {
        },
        error: function() {
        }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReporteAbusoService.svc/DeleteReporteAbuso"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function(result) { config.success(result); },
        error: function(result) { config.error(result); }
    });
};

ReporteAbusoApi.prototype.AddReporteAbuso = function (options, data) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReporteAbusoService.svc/InsertReporteAbuso"),
        type: 'POST',
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};

ReporteAbusoApi.prototype.validateReportarAbuso = function (options) {
    var config = $.extend({
        success: function () { },
        error: function () { }
    }, options);

    $.apiCall({
        url: encodeURI("../wcf/ReporteAbusoService.svc/ValidateInsertReporteAbuso"),
        type: 'POST',
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function (result) { config.success(result); },
        error: function (result) { config.error(result); }
    });
};
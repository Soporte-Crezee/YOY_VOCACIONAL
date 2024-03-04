
var SituacionApi = function () { };

/*Consultar situaciones de aprendizaje Red Social. */
SituacionApi.prototype.SearchSituacionesAprendizaje = function(options, situacionaprendizaje) {
    var config = $.extend({
        success: function(parameters) {
        },
        error: function() {
        }
    }, options);
    $.apiCall({
        url: encodeURI(url_wcf + "SituacionesService.svc/SearchSituacionesAprendizaje"),
        type: 'POST',
        data: situacionaprendizaje,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: false,
        success: function(result) { config.success(result); },
        error: function(result) { config.error(result); }
    });
};
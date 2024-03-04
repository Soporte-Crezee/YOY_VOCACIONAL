var oAsignacionActividadApi = new AsignacionActividadApi();

function getTotalAsignacionActividad() {
    oAsignacionActividadApi.GetTotalAsignacionActividad({ success: function (result) { printTotalAsignacionActividad(result); }, error: function (result) {  return; } });
}

function printTotalAsignacionActividad(data) {
    if (data.d != null && data.d.totalactividades != null && data.d.totalactividades > 0) {
        var totalActividades = data.d.totalactividades != null && data.d.totalactividades > 0 ? '(' + data.d.totalactividades + ')' : '(0)';
        var totalUniversidad = data.d.universidad != null && data.d.universidad > 0 ? '(' + data.d.universidad + ')' : '(0)';
        var totalOrientador = data.d.orientador != null && data.d.orientador > 0 ? '(' + data.d.orientador + ')' : '(0)';
        var totalAreasConocimiento = data.d.areasconocimiento != null && data.d.areasconocimiento > 0 ? '(' + data.d.areasconocimiento + ')' : '(0)';
        
        $("#actividadesSummary").text(totalActividades);
        $("#universidadSummary").text(totalUniversidad);
        $("#orientadorSummary").text(totalOrientador);
        $("#areasConocimientoSummary").text(totalAreasConocimiento);
       
    } else {
       
        $("#actividadesSummary").text('(0)');
        $("#universidadSummary").text('(0)');
        $("#orientadorSummary").text('(0)');
        $("#areasConocimientoSummary").text('(0)');
    }
}
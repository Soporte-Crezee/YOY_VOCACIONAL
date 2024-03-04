var api = new AsistenciaApi();
var currentPage = 1, pageSize = 10;

function GetInformacionContenido(asistenciaid) {
    var data = {
        dto: {
            "pagesize": pageSize,
            "currentpage": currentPage,
            "asistenciaid": asistenciaid
        }
    };
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetInformacionContenido({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}
function LoadInformacionAsistencia(asistenciaid) {
    var data = {
        dto: {
            "pagesize": pageSize,
            "currentpage": currentPage,
            "asistenciaid": asistenciaid
        }
    };
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetInformacionAsistencia({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}
function initResults(data, append) {

    var dataString = $.toJSON(data);

    if (!append) {
        $("#resultados_asistencia").empty();
    }

    if (data.d.asistencia.length <= 0) {
        if (currentPage == 1) {
            var htm = '<span>No se encontraron resultados.</span>';
            $("#mas_informacion").empty();
            $("#mas_informacion").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#mas_informacion").empty();
            $("#mas_informacion").html(htm);
        }
    } else {
        $("#verInformacionAsistenciaTmpl").tmpl(data.d.asistencia).appendTo("#resultados_asistencia");

        if (data.d.asistencia.length < (pageSize - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#mas_informacion").empty();
            $("#mas_informacion").html(htm);
        } else {
            $("#mas_informacion").html('<a class="link_blue" onclick="javascript:LoadInformacionAsistencia('+data.d.situacionid+');" >Mostrar m&aacute;s resultados.</a>');
        }
    }
    currentPage++;
}
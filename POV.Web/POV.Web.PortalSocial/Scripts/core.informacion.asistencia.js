var currentPage = 1, page_size = 3, predeterminadoid = 0, tipopredeterminado = 0, asistencia = 0;
var api = new InformacionAsistenciaApi();
function InformacionAsistenciaCore(agrupadorID, tipoClasificador, asistenciaidjs) {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "agrupadorID": agrupadorID,
            "tipoAgrupador": tipoClasificador,
            "ejeID": asistenciaidjs
        }
    }
    $("#agrupadorid").val(agrupadorID);
    $("#tipoagrupador").val(tipoClasificador);
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResultsInfoAsistenciaCore(result, false, agrupadorID, tipoClasificador, asistenciaidjs); myApiBlockUI.unblockContainer(); } }, dataString);
}

function loadInformacionAsistencia() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "agrupadorID": $("#agrupadorid").val(),
            "tipoAgrupador": $("#tipoagrupador").val(),
            "ejeID": asistenciaidjs
        }
    }
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResultsInfoAsistenciaCore(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResultsInfoSituacionCore(data, append, agrupadorID, tipoClasificador, asistenciaidjs) {
    var dataString = $.toJSON(data);

    if (!append) {
        $("#DivDatosInformacion").empty();
    }

    if (data.d.length <= 0) {
        if (currentPage == 1) {
            var htm = '<span>No se encontraron resultados.</span>';
            $("#more_asistencia").empty();
            $("#more_asistencia").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_asistencia").empty();
            $("#more_asistencia").html(htm);
        }
    } else {
        $("#informacionAsistenciaTmpl").tmpl(data.d).appendTo("#DivDatosInformacion");
        if (data.d.length < (page_size - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_asistencia").empty();
            $("#more_asistencia").html(htm);
        } else {
            $("#more_asistencia").html('<a class="link_blue" onclick="javascript:loadInformacionAsistencia();" >Mostrar m&aacute;s resultados.</a>');
        }
    }

    currentPage++;
}
// Visualizar asistencia red social.
var currentPage = 1, current_page = 1, temaID, tipodocumentoID, nombreAsistencia, page_size = 10, pageSizeContenido = 5;

function BuscarAsistencia() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "temaid": temaID,
            "tipodocumentoid": tipodocumentoID,
            "nombre": nombre
        }
    };
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.SearchAsistencia({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}

function loadResult() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "temaid": temaID,
            "tipodocumentoid": tipodocumentoID,
            "nombre": nombre
        }
    };
    myApiBlockUI.loading();
    var dataString = $.toJSON(data);
    api.SearchAsistencia({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResults(data, append) {

    var dataString = $.toJSON(data);
    if (!append) {
        $("#resultados_asistencia").empty();
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
        $("#asistenciaTmpl").tmpl(data.d).appendTo("#resultados_asistencia");

        if (data.d.length < (page_size - 1)) {
            htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_asistencia").empty();
            $("#more_asistencia").html(htm);
        } else {
            $("#more_asistencia").html('<a class="link_blue" onclick="javascript:loadResult();" >Mostrar m&aacute;s resultados.</a>');
        }
    }
    currentPage++;
}

function LoadMoreDetalleAsistencia(asistenciaid, currentpage) {
    var data = {
        dto: {
            "asistenciaid": asistenciaid,
            "currentpage": currentpage,
            "pagesize": pageSizeContenido
        }
    }

    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetInformacionContenido({ success: function (result) { resultsasistencia(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function cargarContenidos(asistenciaid, currentpage) {
    var data = {
        dto: {
            "asistenciaid": asistenciaid,
            "pagesize": pageSizeContenido,
            "currentpage": currentpage
        }
    };
    current_page = 1;
    myApiBlockUI.search();
    var dataString = $.toJSON(data);

    api.GetInformacionContenido({ success: function (result) { resultsasistencia(result,false); myApiBlockUI.unblockContainer(); } }, dataString);
}

function resultsasistencia(data, append) {
    var current_page = data.d.currentpage;
    var dataString = $.toJSON(data);

    var more = "#more_" + data.d.asistenciaid;
    var ocultar = "#ocultar_" + data.d.asistenciaid;
    $(ocultar).hide();
    $("#resultado_detalle_asistencia_" + data.d.asistenciaid).show();
    if (data.d.contenidos == null) {
        if (current_page == 1) {
            var htm = '<span>No hay contenidos digitales.</span>';
            $(more).empty();
            $(more).html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s contenidos digitales.</span>';
            $(more).empty();
            $(more).html(htm);
        }
    }
    if (data.d.contenidos != null) {
        if (data.d.contenidos.length <= 0) {
            if (current_page == 1) {
                var htm = '<span>No existen contenidos digitales.</span>';
                $(more).empty();
                $(more).html(htm);
            } else {
                var htm = '<span>No existen m&aacute;s contenidos digitales.</span>';
                $(more).empty();
                $(more).html(htm);
            }
        } else {
            var incrustar = "#espacioasistencia_" + data.d.asistenciaid;
            if (!append) {
                $(incrustar).empty();
            }
                
            $("#contenidosTmpl").tmpl(data.d.contenidos).appendTo(incrustar);

            if (data.d.contenidos.length < (pageSizeContenido - 1)) {
                htm = '<span>No existen m&aacute;s resultados.</span>';
                $(more).empty();
                $(more).html(htm);
            } else {
                $(more).html('<a class="link_blue" onclick="javascript:LoadMoreDetalleAsistencia(' + data.d.asistenciaid + ', ' + data.d.currentpage + ');" >Mostrar m&aacute;s contenidos.</a>');
            }
        }
    }
}
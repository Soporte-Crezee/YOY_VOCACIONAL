// Consultar.Contenido.Digital.Red.Social
var currentPage = 1, nombreContenido, page_size = 10;

function buscarContenidos() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "nombrecontenido": nombreContenido
        }
    };
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}


function loadResult() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "nombrecontenido": nombreContenido
        }
    };
    myApiBlockUI.loading();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResults(data, append) {

    var dataString = $.toJSON(data);

    if (!append) {
        $("#resultados_contenidos").empty();
    }

    if (data.d.length <= 0) {
        if (currentPage == 1) {
            var htm = '<span>No se encontraron resultados.</span>';
            $("#more_contenidos").empty();
            $("#more_contenidos").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_contenidos").empty();
            $("#more_contenidos").html(htm);
        }
    } else {
        $("#contenidoTmpl").tmpl(data.d).appendTo("#resultados_contenidos");

        if (data.d.length < (page_size - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_contenidos").empty();
            $("#more_contenidos").html(htm);
        } else {
            $("#more_contenidos").html('<a class="link_blue" onclick="javascript:loadResult();" >Mostrar m&aacute;s resultados.</a>');
        }
    }
    currentPage++;

    $(controlsViewState.hdnCurrentPage).val(currentPage);
}
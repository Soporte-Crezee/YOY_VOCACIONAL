
var currentPage = 1, areaID, tipoID, nombreReactivo, page_size = 10; 

function BuscarReactivos() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "nombrereactivo": nombreReactivo,
            "tipocomplejidadid": tipoID,
            "areaaplicacionid": areaID
        }
    }
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}


function loadResult() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "nombrereactivo": nombreReactivo,
            "tipocomplejidadid": tipoID,
            "areaaplicacionid": areaID
        }
    }
    myApiBlockUI.loading();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResults(data, append) {

    var dataString = $.toJSON(data);

    if (!append) {
        $("#resultados_reactivos").empty();
    }

        if (data.d.reactivos.length <= 0) {
            if (currentPage == 1) {
                var htm = '<span>No se encontraron resultados.</span>';
                $("#more").empty();
                $("#more").html(htm);
            } else {
                var htm = '<span>No existen m&aacute;s resultados.</span>';
                $("#more").empty();
                $("#more").html(htm);
            }
        } else {
            $("#reactivoTmpl").tmpl(data.d.reactivos).appendTo("#resultados_reactivos");

            if (data.d.reactivos.length < (page_size - 1)) {
                var htm = '<span>No existen m&aacute;s resultados.</span>';
                $("#more").empty();
                $("#more").html(htm);
            } else {
                $("#more").html('<a class="link_blue" onclick="javascript:loadResult();" >Mostrar m&aacute;s resultados.</a>');
            }
        }
    currentPage++;
}
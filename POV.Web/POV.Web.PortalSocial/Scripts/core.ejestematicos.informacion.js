var api = new EjesTematicosApi();
var currentPage = 1, pageSize = 10;

function GetInformacionEjeTematico(ejetematicoid) {
    var data = {
        dto: {
            "pagesize": pageSize,
            "currentpage": currentPage,
            "ejetematicoid": ejetematicoid
           
            
        }
    }
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
     api.GetInformacionEjeTematico({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}
function LoadInformacionEjeTematico(ejetematicoid) {
    var data = {
        dto: {
            "pagesize": pageSize,
            "currentpage": currentPage,
            "ejetematicoid": ejetematicoid
        }
    }
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetInformacionEjeTematico({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}
function initResults(data, append) {

    var dataString = $.toJSON(data);

    if (!append) {
        $("#resultados_situaciones").empty();
    }

    if (data.d.situaciones.length <= 0) {
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
        $("#verInformacionEjeTematicoTmpl").tmpl(data.d.situaciones).appendTo("#resultados_situaciones");

        if (data.d.situaciones.length < (pageSize - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#mas_informacion").empty();
            $("#mas_informacion").html(htm);
        } else {
            $("#mas_informacion").html('<a class="link_blue" onclick="javascript:LoadInformacionEjeTematico(' + data.d.ejetematicoid + ');" >Mostrar m&aacute;s resultados.</a>');
        }
    }
    currentPage++;
}
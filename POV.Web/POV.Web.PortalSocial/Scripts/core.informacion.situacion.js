var currentPage = 1, page_size = 10, predeterminadoid = 0, tipopredeterminado=0, eje=0;
var api = new InformacionSituacionApi();
function InformacionSituacionCore(agrupadorID, tipoClasificador, ejeidjs) {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "agrupadorID": agrupadorID,
            "tipoAgrupador": tipoClasificador,
            "ejeID" : ejeidjs
        }
    }
    $("#agrupadorid").val(agrupadorID);
    $("#tipoagrupador").val(tipoClasificador);
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResultsInfoSituacionCore(result, false, agrupadorID, tipoClasificador, ejeidjs); myApiBlockUI.unblockContainer(); } }, dataString);
}


function loadInformacionSituacion() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "agrupadorID": $("#agrupadorid").val(),
            "tipoAgrupador": $("#tipoagrupador").val(),
            "ejeID": ejeidjs
        }
    }
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.Search({ success: function (result) { initResultsInfoSituacionCore(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResultsInfoSituacionCore(data, append, agrupadorID, tipoClasificador, ejeidjs) {
    var dataString = $.toJSON(data);

    if (!append) {
        $("#DivDatosInformacion").empty();
    }

    if (data.d.length <= 0) {
        if (currentPage == 1) {
            if (predeterminadoid == -1) {
                var htm = '<span>El contenido no tiene recursos didácticos.</span>';
                $("#more_cd").empty();
                $("#more_cd").html(htm);
            }
            else {
                var htm = '<span>No se encontraron resultados.</span>';
                $("#more_cd").empty();
                $("#more_cd").html(htm);
            }
        }
        else {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_cd").empty();
            $("#more_cd").html(htm);
        }
    } else {
        $("#informacionSituacionTmpl").tmpl(data.d).appendTo("#DivDatosInformacion");
        if (data.d.length < (page_size - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_cd").empty();
            $("#more_cd").html(htm);
        } else {
            $("#more_cd").html('<a class="link_blue" onclick="javascript:loadInformacionSituacion();" >Mostrar m&aacute;s resultados.</a>');
        }
    }

    currentPage++;
}
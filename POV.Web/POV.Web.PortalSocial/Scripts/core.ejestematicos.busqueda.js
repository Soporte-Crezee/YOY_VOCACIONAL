//Consultar Eje Temático Red Social

var currentPage = 1, nivelID, grado, areaID, materiaID, nombreTema, competencia, aprendizaje, page_size = 10;

function BuscarEjeTema() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "nivelid": nivelID,
            "grado": grado,
            "areaid": areaID,
            "materiaid": materiaID,
            "nombretema": nombreTema,
            "competencia": competencia,
            "aprendizaje": aprendizaje
        }
    };
    myApiBlockUI.search();
    var dataString = $.toJSON(data);    
    api.SearchEjesTematico({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}

function loadResult() {
    var data = {
        dto: {
            "pagesize": page_size,
            "currentpage": currentPage,
            "nivelid": nivelID,
            "grado": grado,
            "areaid": areaID,
            "materiaid": materiaID,
            "nombretema": nombreTema,
            "competencia": competencia,
            "aprendizaje": aprendizaje
        }
    };
    myApiBlockUI.loading();
    var dataString = $.toJSON(data);
    api.SearchEjesTematico({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResults(data, append) {

    var dataString = $.toJSON(data);    
    if (!append) {
        $("#resultados_ejestematicos").empty();
    }

    if (data.d.length <= 0) {
        if (currentPage == 1) {
            var htm = '<span>No se encontraron resultados.</span>';
            $("#more_ejestematicos").empty();
            $("#more_ejestematicos").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_ejestematicos").empty();
            $("#more_ejestematicos").html(htm);
        }
    } else {
        $("#ejestematicosTmpl").tmpl(data.d).each(function (index) {
            if (data.d[index])
                initResultsSituacion(data.d[index].situacionoutputdto, this);
        }).appendTo("#resultados_ejestematicos");
        
        if (data.d.length < (page_size - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#more_ejestematicos").empty();
            $("#more_ejestematicos").html(htm);
        } else {
            $("#more_ejestematicos").html('<a class="link_blue" onclick="javascript:loadResult();" >Mostrar m&aacute;s resultados.</a>');
        }
    }
    currentPage++;
}

function initResultsSituacion(data, tmplItem) {
    $("#situacionesTmpl").tmpl(data.situaciones).each(function (index) {
        if (data.situaciones[index]) {
            initResultsAgrupador(data.situaciones[index], this);
        }
    }).appendTo($("#espacioeje_" + data.ejetematicoid, tmplItem));
    $('.eje_item_' + data.ejetematicoid).show();
}

function initResultsAgrupador(data, tmplItem) {
    $("#contenidosTmpl").tmpl(data.agrupadorcontenido.agrupadores).each(function (index) {
        initResultsContenido(data.agrupadorcontenido.agrupadores[index], this);
    }).appendTo($("#espaciosituacion_" + data.situacionid, tmplItem));
}

function initResultsContenido(data, tmplItem) {
    if (data.contenidosdigitales.length > 0) 
        $("#recursosTmpl").tmpl(data.contenidosdigitales).each(function (index) {
            $(".labelrecurso", this).html('<a href="../ContenidosDigitales/VerContenidoDigital.aspx?src=eje&amp;ejeid=' + data.ejeid + '&amp;contid=' + data.contenidosdigitales[index].contenidodigitalid + '" class="link_blue">' + data.contenidosdigitales[index].nombrecontenidodigital + '</a>');
        }).appendTo($("#espaciocontenido_" + data.agrupadorid, tmplItem));
    else 
        $("#espaciocontenido_" + data.agrupadorid, tmplItem).html("Sin recursos");
}
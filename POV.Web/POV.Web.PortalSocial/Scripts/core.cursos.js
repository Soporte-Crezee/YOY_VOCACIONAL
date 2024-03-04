var api = new CursosApi();
var currentPage = 1,current_page = 1, pageSize = 10,pageSizeContenido = 5;
function GetCursos(nombre, presencial, temaid) {
    var data = {
        dto: {
            "pagesize": pageSize,
            "currentpage": currentPage,
            "cursonombre": nombre,
            "cursotemaid": temaid,
            "cursopresencial":presencial
        }
    }
    

    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    
    api.GetCursos({ success: function (result) { initResults(result, false); myApiBlockUI.unblockContainer(); } }, dataString);
}

function GetDetalleCurso(cursoid,currentpage) {
    var data = {
        dto: {
            "cursoid": cursoid,
            "currentpage": currentpage,
            "pagesize": pageSizeContenido  
        }
    }
    current_page = 1;
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetDetalleCurso({ success: function (result) { IncrustarResultado(result,false); myApiBlockUI.unblockContainer(); } }, dataString);
}
function LoadMoreDetalleCurso(cursoid, currentpage) {
    var data = {
        dto: {
            "cursoid": cursoid,
            "currentpage": currentpage,
            "pagesize": pageSizeContenido
        }
    }
  
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetDetalleCurso({ success: function (result) { IncrustarResultado(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function LoadMoreCursos(nombre, presencial, temaid) {

    var data = {
        dto: {
            "pagesize": pageSize,
            "currentpage": currentPage,
            "cursonombre": nombre,
            "cursotemaid": temaid,
            "cursopresencial": presencial
        }
    }
    myApiBlockUI.search();
    var dataString = $.toJSON(data);
    api.GetCursos({ success: function (result) { initResults(result, true); myApiBlockUI.unblockContainer(); } }, dataString);
}

function initResults(data, append) {

    var dataString = $.toJSON(data);

    if (!append) {
        $("#resultados_cursos").empty();
    }
  
    if (data.d.length <= 0) {
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
        $("#cursoTmpl").tmpl(data.d).appendTo("#resultados_cursos");

        if (data.d.length < (pageSize - 1)) {
            var htm = '<span>No existen m&aacute;s resultados.</span>';
            $("#mas_informacion").empty();
            $("#mas_informacion").html(htm);
        } else {
            $("#mas_informacion").html('<a class="link_blue" onclick="javascript:GetMoreCursos();" >Mostrar m&aacute;s resultados.</a>');
        }
    }
    currentPage++;
}
function IncrustarResultado(data,append) {
    var current_page = data.d.currentpage;
    var dataString = $.toJSON(data);
   
    var more = "#more_" + data.d.cursoid;
    var ocultar = "#ocultar_"+ data.d.cursoid;
    $(ocultar).hide();
    $("#resultado_detalle_contenidos_" + data.d.cursoid).show();
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
            
            var incrustar = "#resultado_curso_detalle_" + data.d.cursoid;
            if (!append) {
                $(incrustar).empty();
            }
           
            $("#cursoDetalleTmpl").tmpl(data.d.contenidos).appendTo(incrustar);

            if (data.d.contenidos.length < (pageSizeContenido - 1)) {
                var htm = '<span>No existen m&aacute;s contenidos digitales.</span>';
                $(more).empty();
                $(more).html(htm);
            } else {
                $(more).html('<a class="link_blue element_3A2F8B" onclick="javascript:LoadMoreDetalleCurso(' + data.d.cursoid + ',' + data.d.currentpage + ');" >Mostrar m&aacute;s contenidos digitales <i class="icon icon_show_more"></i></a>');
            }
        } 
    }
    
}
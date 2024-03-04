var oNotificacionesApi = new NotificacionesApi();

function getTotalNotificaciones() {
    oNotificacionesApi.GetTotalNotificaciones({
        success: function (result) {
            var localSessionCorreo = localStorage['session_correo_confirmado'];
            if (localSessionCorreo != "False") {
                printTotalNotificaciones(result);
            }
        }
        , error: function (result) { return; }
    });
}

function printTotalNotificaciones(data) {
    if (data.d != null && data.d.total != null && data.d.total > 0) {
        var strtotal = data.d.total != null && data.d.total > 0 ? '(' + data.d.total + ')' : '(0)';

        $("#social_summary").text(strtotal);
       

    } else {
       

        $("#social_summary").text('(0)');
    }
    if (data.d != null && data.d.reportesabuso != null && data.d.reportesabuso > 0) {
        var strabusotetotal = '(' + data.d.reportesabuso + ')';
        $("#reporte_summary").text(strabusotetotal);
    } else {
        $('#reporte_summary').text('(0)');
    }
}

function loadNotificaciones() {
    var data = { dto: {
        "pagesize": page_size,
        "currentpage": current_page
        }
    };
    var dataString = $.toJSON(data);
myApiBlockUI.loading();
setTimeout(function(){
    oNotificacionesApi.GetNotificaciones({
    success: function (result) { 
        printNotificaciones(result);
        myApiBlockUI.unblockContainer(); 
    }
}, dataString);
},2000);
  
}

function printNotificaciones(data) {
    if (data.d.length <= 0) {
        if (current_page == 1) {
            var htm = '<span>No tienes notificaciones</span>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s notificaciones</span>';
            $("#more").empty();
            $("#more").html(htm);
        }
    } else {

        $("#notificacionTmpl").tmpl(data.d,
                { getCss: getCss
                }).appendTo("#notificaciones_stream");

                getTotalNotificaciones();
        if (data.d.length < page_size) {
            var htm = '<span>No existen m&aacute;s notificaciones</span>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            $("#more").html('<a class="link_blue pconfirmacorreo" onclick="javascript:loadNotificaciones();" >Mostrar m&aacute;s notificaciones</a>');
        }
        ChangeBtnDeleteNotificacion();

         //actualizar el estado de las notificaciones

    }
    if(current_page==1){
        $(document).trigger('coreNotificaciones/printNotificaciones');
    }
    
    current_page++;
}

function deleteNotificacion(id) {
    if (id != null && id != "") {

        var apiMessages = new MessageApi();
        var message = "Se eliminar&aacute;n los datos de la notificaci&oacute;n <br />¿Desea Continuar?";
        apiMessages.CreateQuestionMessage(message, function () {
            $("#dialog-question").dialog("close");
            removeNotificacionFromServer(id);

        });

        apiMessages.ShowQuestion();
    }
}
function removeNotificacionFromServer(id) {
    var data = { dto: {
        "notificacionid": id
    }
    };
var dataString = $.toJSON(data);
myApiBlockUI.process();
$("#btnRechazarMensaje").click();
    oNotificacionesApi.DeleteNotificacion({
        success: function (result) { deleteNotificacionFromUI(result); myApiBlockUI.unblockContainer(); }
    }, dataString);

}
function ConfirmNotificacionFromServer() {
    oNotificacionesApi.ConfirmNotificacion({ success: function (result) { }}, null);

}
function deleteNotificacionFromUI(data) {
    if (data != null && data.d != null && data.d.notificacionid != null) {
        var obj = document.getElementById(data.d.notificacionid);
        $(obj).parent().remove();
    }
}

function getCss(estatus) {
    if (estatus == 1)
        return "new_notification";
    return "";
}

function ChangeBtnDeleteNotificacion() {
    $('button[id|="btn-del"]').each(function (index) {
        $(this).button({
            icons: {
                primary: "ui-icon-close"
            },
            text: false
        });
    });
}

$(document).bind('coreNotificaciones/printNotificaciones',function(){
    ConfirmNotificacionFromServer();
});

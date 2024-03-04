var apiPub = new PublicacionApi();
$(function () {
    CargaPublicaciones();
});
function CargaPublicaciones() {
    myApiBlockUI.loading();
    var data = {
        dto:{
            "pagesize": 30,
            "currentpage": curpage,
            "socialhubid": hub.val(),
            "estatus": 1
        }
    };

    var dataString = $.toJSON(data);
    apiPub.LoadSugerencias({ success: function (result) { printPublicaciones(result); myApiBlockUI.unblockContainer(); } }, dataString);

}
function printPublicaciones(data) {

    var dataString = $.toJSON(data);
    if (data.d == null || data.d.length <= 0) {
        if (curpage == 1) {
            var htm = '<span>No se ha compartido ninguna sugerencia</span>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s sugerencias</span>';
            $("#more").empty();
            $("#more").html(htm);
        }
    } else {
        for (var x = 0; x < data.d.length; x++) {
            var pub = document.getElementById(data.d[x].publicacionid);
            if (!pub) {
                $("#pubTmpl").tmpl(data.d[x],
                { urlFiles: getFilesUrl }).appendTo("#PublicacionStream");
            } else {
                var html_pub = $("#pubTmpl").tmpl(data.d[x], { urlFiles: getFilesUrl }).html();

                $("#" + data.d[x].publicacionid).html(html_pub);
            }
        }
        ChangeBtnDelete();
        if (data.d.length >= 30)
            $("#more").html('<a class="link_blue" onclick="javascript:CargaPublicaciones();" >Mostrar m&aacute;s publicaciones</a>');
        else {
            var htm = '<span>No existen m&aacute;s sugerencias</span>';
            $("#more").empty();
            $("#more").html(htm);
        }
    }
    curpage++;

}

function LikePub(publicacionid, type) {
    if (publicacionid != null && publicacionid != '') {
        var data = { dto: { "publicacionid": publicacionid, "usuarioid": usrse.val(), "vote": type} };
        var dataString = $.toJSON(data);

        apiPub.LikePub({ success: function (result) { updateVotesPublicacion(result); } }, dataString);
    }
}

function updateVotesPublicacion(data) {
    if (data.d != null) {
        $("#votes0_" + data.d.publicacionid).attr('title', data.d.numvotes1 + " excelente");
        $("#votes1_" + data.d.publicacionid).attr('title', data.d.numvotes2 + " bien");
        $("#votes2_" + data.d.publicacionid).attr('title', data.d.numvotes3 + " regular");
        $("#votes0_" + data.d.publicacionid).attr("value", "(" + data.d.numvotes1 + ")");
        $("#votes1_" + data.d.publicacionid).attr("value", "(" + data.d.numvotes2 + ")");
        $("#votes2_" + data.d.publicacionid).attr("value", "(" + data.d.numvotes3 + ")");
    }
}




function showPeople(rankingID, type) {
    if (rankingID != '') {
        var data = { dto: { "rankingid": rankingID, "vote": type} };
        var dataString = $.toJSON(data);
        myApiBlockUI.loading();
        apiPub.GetPeople({ success: function (result) {


            $("#people-stream").empty();
            if (result.d.peoples.length > 0) {
                $("#peopleTmpl").tmpl(result.d, {
                    urlFiles: getFilesUrl
                }).appendTo("#people-stream");
                var typeTitle = result.d.vote == '0' ? 'Excelente' : (result.d.vote == '1' ? 'Bien' : 'Regular');

                $("#dialog-people-likes").dialog({
                    autoOpen: false,
                    minHeight: 200,
                    maxHeight: 500,
                    width: 400,
                    resizable: false,
                    modal: true,
                    title: typeTitle
                });

                $('.ui-widget-overlay').live('click', function () {
                    $('#dialog-people-likes').dialog("close");
                });
                $("#dialog-people-likes").dialog("open");
            }

            myApiBlockUI.unblockContainer(); 
        }
        }, dataString);

    }

}

function ChangeBtnDelete() {
    $('button[id|="btn-del"]').each(function (index) {
        $(this).button({
            icons: {
                primary: "ui-icon-close"
            },
            text: false
        });
    });
}

function removePublicacion(publicacionid) {

    var apiMessages = new MessageApi();
    var message = "Se eliminar&aacute;n los datos de la sugerencia <br />¿Desea Continuar?";
    apiMessages.CreateQuestionMessage(message, function () {
        $("#dialog-question").dialog("close");
        removePublicacionFromServer(publicacionid);

    });

    apiMessages.ShowQuestion();

}
function removePublicacionFromServer(publicacionid) {

    var data = { dto: { "publicacionid": publicacionid} };
    var dataString = $.toJSON(data);

    myApiBlockUI.process();
    apiPub.RemovePub({ success: function (result) { removePublicacionFromUI(result); myApiBlockUI.unblockContainer(); } }, dataString);
}
function removePublicacionFromUI(data) {
    var dataString = $.toJSON(data)
    if (data.d.success != null && data.d.success == true) {
        var obj = document.getElementById(data.d.publicacionid);
        $(obj).remove();
    } else {
        $(hdnmessageinputid).val(data.d.error);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}

function getFilesUrl() {
    var randomnumber = Math.floor(Math.random() * 10000)
    return "../Files/ImagenUsuario/ImagenPerfil.aspx?r=" + randomnumber;
}

function reportarAbusoPublicacionFromServer(reportableid) {

    var oReporteAbuso = new ReporteAbusoApi();
    var data = { dto: {
        "reportableid": reportableid,
        "tiporeporte": 1
    }
    };
var dataString = $.toJSON(data);
myApiBlockUI.send();
oReporteAbuso.AddReporteAbuso({ success: function (result) { removeElementoReportadaFromUI(reportableid); myApiBlockUI.unblockContainer(); }, error: function (result) {
        $(hdnmessageinputid).val(reportableid);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
        myApiBlockUI.unblockContainer(); 
        return;
    }
    }, dataString);

}

function reportarAbusoComentarioFromServer(reportableid) {

    var oReporteAbuso = new ReporteAbusoApi();

    var data = { dto: {
        "reportableid": reportableid,
        "tiporeporte": 2
    }
    };
    var dataString = $.toJSON(data);
    oReporteAbuso.AddReporteAbuso({ success: function (result) { removeElementoReportadaFromUI(reportableid); }, error: function (result) {

        $(hdnmessageinputid).val(reportableid);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
        return;

    }
    }, dataString);


}

function reportarAbusoComentario(reportableid) {


    var oReporteAbuso = new ReporteAbusoApi();

    oReporteAbuso.validateReportarAbuso({ success: function (result) {

        if (result != null && result.d) {
            if (!result.d.success) {

                var apiMessages = new MessageApi();

                var message = result.d.error;
                apiMessages.CreateMessage(message, "ERROR");

                apiMessages.Show();
            } else {
                //solicitar confirmación del reporte de abuso
                var apiMessages = new MessageApi();
                var message = "Se reportar&aacute; como abuso el elemento seleccionado <br />¿Desea Continuar?";
                apiMessages.CreateQuestionMessage(message, function () {
                    $("#dialog-question").dialog("close");
                    reportarAbusoComentarioFromServer(reportableid);
                });

                apiMessages.ShowQuestion();

            }


        }
    }, error: function (result) {

        if (result.d && result.d.length > 0 && result.d.error.length > 0) {
            $(hdnmessageinputid).val(result.d.error);
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
            return;
        }

    }
    });



}


function reportarAbusoPublicacion(reportableid) {


    var oReporteAbuso = new ReporteAbusoApi();

    oReporteAbuso.validateReportarAbuso({ success: function (result) {

        if (result != null && result.d) {
            if (!result.d.success) {

                var apiMessages = new MessageApi();

                var message = result.d.error;
                apiMessages.CreateMessage(message, "ERROR");

                apiMessages.Show();
            } else {
                //solicitar confirmación del reporte de abuso
                var apiMessages = new MessageApi();
                var message = "Se reportar&aacute; como abuso el elemento seleccionado <br />¿Desea Continuar?";
                apiMessages.CreateQuestionMessage(message, function () {
                    $("#dialog-question").dialog("close");
                    reportarAbusoPublicacionFromServer(reportableid);
                });

                apiMessages.ShowQuestion();

            }


        }
    }, error: function (result) {

        if (result.d && result.d.length > 0 && result.d.error.length > 0) {
            $(hdnmessageinputid).val(result.d.error);
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
            return;
        }

    }
    });

}

function removeElementoReportadaFromUI(reportableid) {

    if (reportableid) {
        var obj = document.getElementById(reportableid);
        $(obj).remove();
        if (place == VIEW_PUB) {
            window.location = defaulturl.val();
        }

    }
}
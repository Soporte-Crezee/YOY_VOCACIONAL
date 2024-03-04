$(document).ready(initPage);
var api = new PublicacionApi();
function initPage() {

    $.get('../Scripts/tmpl/pubs.tmpl.htm', function (templates) {
        $('body').append(templates);
        $("#postit").css("display", "none");
        $("#txtPublicacion").autoResize({
            onResize: function () {
                $(this).css({ opacity: 0.9 });
            },
            animateCallback: function () {
                $(this).css({ opacity: 1 });
            }, animateDuration: 200,
            extraSpace: 5
        });
        curpage = 1;
        $("#txtPublicacion").focus(function () { $("#postit").css("display", "block"); });
        $("#txtPublicacion").blur(function () { var value = $("#txtPublicacion").val().trim(); if (value == "") $("#postit").css("display", "none"); });
        $("#btnShare").bind("click", compartir);

        $("#dialogpub").dialog({ autoOpen: "false" });
        $("#dialogpub").dialog("close");
        $("#dialogcom").dialog({ autoOpen: "false" });
        $("#dialogcom").dialog("close");


        LoadPubs();
    });
    setInterval("ReloadPubs()", 70000);



}



function compartir() {
    var contenido = $("#txtPublicacion").val();
    var identificadorCompartir = $("#hdn-identificador-contenido-pub").val();
    contenido = $.trim(contenido);
    if (contenido != '' || identificadorCompartir != '') {
        var tipoContenido = $("#hdn-tipo-contenido-pub").val();
        var tipo = identificadorCompartir != '' ? 3 : 1;
        contenido = contenido != '' ? contenido : 'Compartió el siguiente recurso';
        var data = {
            publicaciondto: {
                "contenido": contenido,
                "usuariosocialid": usrse.val(),
                "socialhubid": hub.val(),
                "tipocompartido": tipoContenido,
                "compartidoid": identificadorCompartir,
                "tipo": tipo
            }
        };

        var dataString = $.toJSON(data);
        myApiBlockUI.send();
        api.Share({ success: function (result) { $("#txtPublicacion").css("height", "54px"); showPublicacion(result.d); myApiBlockUI.unblockContainer(); myApiCompartir.quitarContenido('pub'); } }, dataString);
    }
}

function getContenidoCompartir() {
    
}

function getOption(id, option) {
    if (option == 'delcom') {
        return "<input type=\"button\" id=\"btnDelete\" title=\"Eliminar Comentario\" onclick=\"javascript:removeComentario('" + id + "');\" class=\"operDel icon icon_cross floatRight ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only\"/>"
    } else if (option == 'delpub') {
        return "<input type=\"button\" id=\"btnDelete\" title=\"Eliminar Publicación\" onclick=\"javascript:removePublicacion('" + id + "');\" class=\"operDel icon icon_cross floatRight ui-button ui-widget ui-state-default ui-corner-all ui-button-icon-only\"/>";
    } else {
        return '';
    }
}

function LoadPubs() {
    var data = { dto: {
        "pagesize": 10,
        "currentpage": curpage,
        "socialhubid": hub.val(),
        "usuariosocialid": usr.val(),
        "estatus": 1
    }
    };
    var dataString = $.toJSON(data);
    myApiBlockUI.loading();
    api.LoadPubsMuroDocente({
        success: function (result) { printPublicaciones(result); myApiBlockUI.unblockContainer(); }
    }, dataString);

}


function printPublicaciones(data) {

    var dataString = $.toJSON(data);
    if (data.d.length <= 0) {
        if (curpage == 1) {
            var htm = '<span>No se ha compartido ninguna publicaci&oacute;n</span>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s publicaciones</span>';
            $("#more").empty();
            $("#more").html(htm);
        }
    } else {

        for (var x = 0; x < data.d.length; x++) {
            var pub = document.getElementById(data.d[x].publicacionid);
            if (!pub) {
                $("#pubTmpl").tmpl(data.d[x],
                { getOption: getOption,
                    getTipoTexto: getTipoTexto,
                    urlFiles: getFilesUrl
                }).appendTo("#PublicacionStream");
            } else {
                var html_pub = $("#pubTmpl").tmpl(data.d[x], { getTipoTexto: getTipoTexto,
                    urlFiles: getFilesUrl
                }).html();

                $("#" + data.d[x].publicacionid).html(html_pub);
            }
        }
        $("div#PublicacionStream div").each(function () { $(this).fadeIn(1000); });
        $('button[id|="btn-del"]').each(function (index) {
            $(this).button({
                icons: {
                    primary: "ui-icon-close"
                },
                text: false
            });
        });

        $('button[id|=btn-reportar]').each(function (index) {
            $(this).button({ icons: { primary: "ui-icon-flag" }, text: false
            });
        });

        ChangeBtnAbuso();
        if (data.d.length < 10) {
            var htm = '<span>No existen m&aacute;s publicaciones</span>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            $("#more").html('<a class="link_blue" onclick="javascript:LoadPubs();" >Mostrar m&aacute;s publicaciones</a>');

        }
    }
    curpage++;

}
function showPublicacion(data) {
    $("#pubTmpl").tmpl(data, { getOption: getOption, getTipoTexto: getTipoTexto, urlFiles: getFilesUrl
    }).prependTo("#PublicacionStream");
    $("#txtPublicacion").val("");
    $("div#PublicacionStream div:first").fadeIn(100);
    $("#more").html('<a class="link_blue" onclick="javascript:LoadPubs();" >Mostrar m&aacute;s publicaciones</a>');
    ChangeBtnDelete();
    ChangeBtnAbuso();

}
function removePublicacion(publicacionid) {
    var apiMessages = new MessageApi();
    var message = "Se eliminar&aacute; la publicaci&oacute;n y sus comentarios<br />¿Desea Continuar?";
    apiMessages.CreateQuestionMessage(message, function () {
        removePublicacionFromServer(publicacionid);

    });

    apiMessages.ShowQuestion();

}
function removePublicacionFromServer(publicacionid) {
    var data = { dto: { "publicacionid": publicacionid} };
    var dataString = $.toJSON(data);
    myApiBlockUI.send();
    $("#btnRechazarMensaje").click();
    api.RemovePub({ success: function (result) { removePublicacionFromUI(result); myApiBlockUI.unblockContainer(); } }, dataString);
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

function removeComentario(commentid) {

    var apiMessages = new MessageApi();
    var message = "Se eliminar&aacute; el comentario<br />¿Desea Continuar?";
    apiMessages.CreateQuestionMessage(message, function () {
        removeComentarioFromServer(commentid);

    });

    apiMessages.ShowQuestion();

}

function removeComentarioFromServer(comentarioid) {
    var data = { dto: { "comentarioid": comentarioid} };
    var dataString = $.toJSON(data);
    myApiBlockUI.send();
    $("#btnRechazarMensaje").click();
    api.RemoveComment({ success: function (result) { removeComentarioFromUI(result); myApiBlockUI.unblockContainer(); } }, dataString);
}

function removeComentarioFromUI(data) {
    if (data.d.success != null && data.d.success == true) {
        var obj = document.getElementById(data.d.comentarioid);
        $(obj).remove();
    } else {
        $(hdnmessageinputid).val(data.d.error);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}

function commentenable(pubid) {
    var pub = document.getElementById(pubid);
    var data = { publicacionid: pubid };
    var dataString = $.toJSON(data);

    $("#panelcomment").remove().fadeIn("slow");

    if (TIPO_FUENTE_PUBLICACION.val() == "D")
        $("#areacomShareTmpl").tmpl(data, { urlFiles: getFilesUrl }).appendTo(pub).fadeIn(1000);
    else
        $("#areacomTmpl").tmpl(data, { urlFiles: getFilesUrl }).appendTo(pub).fadeIn(1000);

    var txtarea = $(pub).find("#panelcomment").find("textarea");
    $(txtarea).autoResize({
        onResize: function () { $(this).css({ opacity: 0.9 }); }, animateCallback: function () { $(this).css({ opacity: 1 }); }, animateDuration: 200,
        extraSpace: 1
    });
    var options = {
        'maxCharacterSize': 200,
        'originalStyle': 'display_info_textarea',
        'warningStyle': 'display_warning_textarea',
        'warningNumber': 200,
        'displayFormat': '#left caracteres restantes de #max max.'
    };
    $(txtarea).textareaCount(options);
    txtarea.focus();
    $("#pnl-contenido-seleccionado-com").hide();
    $('button[id|="btn-add-contenido"]').each(function (index) {
        $(this).button();
    });
    $("#btn-delete-contenido-com").button();
}

function cancelComment() {
    $("#panelcomment").remove().fadeIn("slow");
}

function sendcomment(pubid, commentdata) {
    var contenido = $("#txtComment").val();
    var identificadorCompartir = $("#hdn-identificador-contenido-com").val();
    contenido = $.trim(contenido);
    if ((contenido != '' || identificadorCompartir != '') && pubid != '') {
        var tipoContenido = $("#hdn-tipo-contenido-com").val();
        var tipo = identificadorCompartir != '' ? 3 : 1;
        contenido = contenido != '' ? contenido : 'Compartió el siguiente recurso';
        var data = {
            dto: {
                "publicacionid": pubid,
                "usuariosocialidcom": usrse.val(),
                "contenidocom": contenido,
                "tipocompartido": tipoContenido,
                "compartidoid": identificadorCompartir,
                "tipo": tipo
            }
        };
        var dataString = $.toJSON(data);
        myApiBlockUI.send();
        api.Comment(
            { success: function (result) { showComment(result); myApiBlockUI.unblockContainer(); } }, dataString);

    }
}
function showComment(data) {
    var obj = document.getElementById(data.d.publicacionid);
    $(obj).find("#panelcomment").remove();
    $(obj).find(".opcomentar").remove();
    var dataString = $.toJSON(data);
    var pub = document.getElementById(data.d.publicacionid + "_comments");
    $("#comTmpl").tmpl(data.d, { getOption: getOption, getTipoTexto: getTipoTexto, urlFiles: getFilesUrl
    }).appendTo(pub).fadeIn(100);
    ChangeBtnDelete();
    ChangeBtnAbuso();
}

var index_li = 0;
var size_li = 0;
function ReloadPubs() {
    index_li = 1;
    size_li = $("#PublicacionStream > div").size();
    ToReloadPubs(1);
}

function ToReloadPubs(page_c) {
    var data = { dto: {
        "pagesize": 10,
        "currentpage": page_c,
        "socialhubid": hub.val(),
        "usuariosocialid": usr.val(),
        "estatus": 1
    }
    };
    var dataString = $.toJSON(data);

    api.LoadPubsMuroDocente({
        success: function (result) {

            for (var x = 0; x < result.d.length; x++) {
                RefreshPublication(result.d[x], index_li);
                index_li++;
            }

            if (page_c < curpage - 1) {
                page_c++;
                ToReloadPubs(page_c);
            } else {

                if (index_li <= $("#PublicacionStream > div").size()) {
                    var size_temp = $("#PublicacionStream > div").size();
                    for (var j = index_li; j <= size_temp; j++) {
                        $("#PublicacionStream > div:nth-child(" + j + ")").remove();
                    }

                    $("#more").html('<a class="link_blue" onclick="javascript:LoadPubsSocial();" >Mostrar m&aacute;s publicaciones</a>');
                }
            }
        }
    }, dataString);

}
function RefreshPublication(data, index) {

    if (index > size_li) {

        $("#pubTmpl").tmpl(data, { getOption: getOption, getTipoTexto: getTipoTexto,
            urlFiles: getFilesUrl
        }).appendTo("#PublicacionStream").fadeIn(100);
        ChangeBtnDelete();
        ChangeBtnAbuso();
    } else {
        var liItem = $("#PublicacionStream > div:nth-child(" + index + ")");
        liItem.attr('id', data.publicacionid);
        if (!liItem) {
            var html_pub = $("#pubTmpl").tmpl(data, { getTipoTexto: getTipoTexto,
                urlFiles: getFilesUrl
            }).html();
            liItem.html(html_pub);
        } else {
            var panelcom = $(liItem).find("#panelcomment");
            if (!panelcom.html()) {

                var requestComplete = document.getElementById("request_complete_" + data.publicacionid);
                if (requestComplete != null) {

                    LoadPubComplete(data.publicacionid)
                } else {
                    var html_pub = $("#pubTmpl").tmpl(data, { getTipoTexto: getTipoTexto,
                        urlFiles: getFilesUrl
                    }).html();
                    liItem.html(html_pub);
                    ChangeBtnDelete();
                    ChangeBtnAbuso();
                }

            }
        }
    }
}

function LoadPubComplete(publicacionid) {
    if (publicacionid != null && publicacionid != '') {
        var data = { dto: { "publicacionid": publicacionid, "complete": true} };
        var dataString = $.toJSON(data);

        api.GetPub({ success: function (result) {
            var pub = document.getElementById(result.d.publicacionid);
            $("#" + publicacionid).html("");
            var pubcomplete = $("#pubTmpl").tmpl(result.d, { getTipoTexto: getTipoTexto, urlFiles: getFilesUrl }).html();
            $("#" + publicacionid).html(pubcomplete);
            ChangeBtnDelete();
            ChangeBtnAbuso();
        }
        }, dataString);
    }
}

function showoption(panel, option) {
    $(panel).append(option);
}

function LikePub(publicacionid, type) {
    if (publicacionid != null && publicacionid != '') {
        var data = { dto: { "publicacionid": publicacionid, "usuarioid": usrse.val(), "vote": type} };
        var dataString = $.toJSON(data);
        myApiBlockUI.send();
        api.LikePub({ success: function (result) { updateVotesPublicacion(result); myApiBlockUI.unblockContainer(); } }, dataString);
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

function LikeComment(comentarioid, type) {
    if (comentarioid != null && comentarioid != '') {
        var data = { dto: { "comentarioid": comentarioid, "usuariosocialidcom": usrse.val(), "vote": type} };
        var dataString = $.toJSON(data);
        myApiBlockUI.send();
        api.LikeComment({ success: function (result) { updateVotesComentario(result); myApiBlockUI.unblockContainer(); } }, dataString);

    }
}

function updateVotesComentario(data) {
    if (data.d != null) {
        $("#votes0_" + data.d.comentarioid).attr('title', data.d.numvotes1 + " excelente");
        $("#votes1_" + data.d.comentarioid).attr('title', data.d.numvotes2 + " bien");
        $("#votes2_" + data.d.comentarioid).attr('title', data.d.numvotes3 + " regular");
        $("#votes0_" + data.d.comentarioid).attr("value", "(" + data.d.numvotes1 + ")");
        $("#votes1_" + data.d.comentarioid).attr("value", "(" + data.d.numvotes2 + ")");
        $("#votes2_" + data.d.comentarioid).attr("value", "(" + data.d.numvotes3 + ")");
    }
}

function showPeople(rankingID, type) {
    if (rankingID != '') {
        var data = { dto: { "rankingid": rankingID, "vote": type} };
        var dataString = $.toJSON(data);
        myApiBlockUI.loading();
        api.GetPeople({ success: function (result) {


            $("#people-stream").empty();
            $(".modal-title").empty();
            $("#message").empty();
            $("#message").hide();
            $("#emergentes").show();

            if (result.d.peoples.length > 0) {
                var typeTitle = result.d.vote == '0' ? 'Excelente' : (result.d.vote == '1' ? 'Bien' : 'Regular');
                var classHeader = "modal-header color-info";
                $("#modalHeader").addClass(classHeader);
                var modalTitle = typeTitle;
                $(".modal-title").html(modalTitle);
                var message = $("#peopleTmpl").tmpl(result.d, {
                    urlFiles: getFilesUrl
                }).appendTo("#people-stream");

                $("#emergentes").html(message);
                var btns = '<button type="button" class="btn-cancel" data-dismiss="modal" id="btnRechazarMensaje" >Cerrar</button>';
                $("#modalFooter").html(btns);

                $('#updPanelMaster').on('shown.bs.modal', function () {
                    $('#btnAceptarMensaje').focus()
                });
                $('#updPanelMaster').modal('show');
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

    $('button[id|=btn-reportar]').each(function (index) {
        $(this).button({ icons: { primary: "ui-icon-flag" }, text: false
        });
    });
}

function ChangeBtnAbuso() {
    $('button[id|="btn-reportar"]').each(function (index) {
        $(this).button({
            icons: {
                primary: "ui-icon-flag"
            },
            text: false
        });
    });
}

function getTipoTexto() {
    return TIPO_PUBLICACION_TEXTO.val();
}

function loadPublicacionComplete() {
    if (publicacionID.val() != null && publicacionID.val() != '') {
        var data = { dto: { "publicacionid": publicacionID.val(), "complete": true} };
        var dataString = $.toJSON(data);

        api.GetPub({ success: function (result) {
            $("#pubTmpl").tmpl(result.d,
                { getOption: getOption,
                    getTipoTexto: getTipoTexto, urlFiles: getFilesUrl
                }).appendTo("#PublicacionStream");
            $("div#PublicacionStream div").each(function () { $(this).fadeIn(1000); });

            ChangeBtnDelete();
            ChangeBtnAbuso();
        }
        }, dataString);
    }
}

function getFilesUrl() {
    var randomnumber = 1; //Math.floor(Math.random() * 10000);
    return "../Files/ImagenUsuario/ImagenPerfil.aspx?r=" + randomnumber;
}

var page_dudas = 1;
function LoadDudas() {
    var data = { dto: {
        "pagesize": 10,
        "currentpage": page_dudas,
        "socialhubid": hub.val(),
        "usuariosocialid": usr.val(),
        "estatus": 1
    }
    };
var dataString = $.toJSON(data);
myApiBlockUI.loading(); 
    api.LoadDudasMuroDocente({
        success: function (result) { printDudas(result); myApiBlockUI.unblockContainer(); }
    }, dataString);

}

function printDudas(data) {
    var dataString = $.toJSON(data);
    if (data.d.length <= 0) {
        if (page_dudas == 1) {
            var htm = '<span>No se ha compartido ninguna duda</span>';
            $("#more-dudas").empty();
            $("#more-dudas").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s dudas publicadas</span>';
            $("#more-dudas").empty();
            $("#more-dudas").html(htm);
        }
    } else {

        for (var x = 0; x < data.d.length; x++) {
            var pub = document.getElementById(data.d[x].publicacionid);
            if (!pub) {
                $("#pubTmpl").tmpl(data.d[x],
                { getOption: getOption,
                    getTipoTexto: getTipoTexto,
                    urlFiles: getFilesUrl
                }).appendTo("#DudasStream");
            } else {
                var html_pub = $("#pubTmpl").tmpl(data.d[x], { getTipoTexto: getTipoTexto,
                    urlFiles: getFilesUrl
                }).html();

                $("#" + data.d[x].publicacionid).html(html_pub);
            }
        }
        $("div#DudasStream div").each(function () { $(this).fadeIn(1000); });
        ChangeBtnDelete();
        ChangeBtnAbuso();
        if (data.d.length < 10) {
            var htm = '<span>No existen m&aacute;s dudas publicadas</span>';
            $("#more-dudas").empty();
            $("#more-dudas").html(htm);
        } else {
            $("#more-dudas").html('<a class="link_blue" onclick="javascript:LoadDudas();" >Mostrar m&aacute;s dudas publicadas</a>');

        }
    }
    page_dudas++;

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

function reportarAbusoComentario(reportableid) {


    var oReporteAbuso = new ReporteAbusoApi();
    myApiBlockUI.process(); 
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
        myApiBlockUI.unblockContainer(); 
    }, error: function (result) {
        if (result.d && result.d.length > 0 && result.d.error.length > 0) {

            $(hdnmessageinputid).val(result.d.error);
            $(hdnmessagetypeinputid).val('1');
            mostrarError();
            return;
        }

        myApiBlockUI.unblockContainer(); 
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
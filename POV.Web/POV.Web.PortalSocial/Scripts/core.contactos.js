$(document).ready(initPage);
var contactosApi = new ContactosApi();
var group;
function initPage() {
    pubs_current = 1;
    $("#postit").css("display", "none");

    $.get('../Scripts/tmpl/contacto.tmpl.htm', function (templates) {
        $('body').append(templates);
        LoadContactos();
    });
}

function LoadContactos() {

    var data = {
        dto: {
            "pagesize": 12,
            "currentpage": pubs_current
        }
    };

    var dataString = $.toJSON(data);
    myApiBlockUI.loading();
    contactosApi.GetContactos({ success: function (result) { printContactos(result); myApiBlockUI.unblockContainer(); } }, dataString);

}

function printContactos(data) {

    var dataString = $.toJSON(data);
    if (data.d.length <= 0) {
        if (pubs_current == 1) {
            var htm = '<div class="more"><span>No se encontraron resultados.</span></div>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            var htm = '<span>No existen m&aacute;s contactos</span>';
            $("#more").empty();
            $("#more").html(htm);
        }

    } else {

        for (var i = 0; i < data.d.length; i++) {
            var pub = document.getElementById(data.d[i].usuariosocialid);
            if (!pub) {
                $("#contactosTemp").tmpl(data.d[i]).appendTo("#ContactosStream");
            } else {
                var html_pub = $("#contactosTemp").tmpl(data.d[i]).html();
                $("#" + data.d[i].contactoid).html(html_pub);
            }
        }

        $("div#ContactosStream div").each(function () { $(this).fadeIn(1000); });




        if (data.d.length < 7) {
            var htm = '<span>No existen m&aacute;s contactos</span>';
            $("#more").empty();
            $("#more").html(htm);
        } else {
            $("#more").html('<a class="link_blue pconfirmacorreo" onclick="javascript:LoadContactos();" >Mostrar m&aacute;s contactos</a>');
        }
        pubs_current++;

    }


}



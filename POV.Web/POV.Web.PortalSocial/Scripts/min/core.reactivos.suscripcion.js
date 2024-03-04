function suscribirse(reactivoID) {
    if (reactivoID != null && reactivoID != '') {
        var data = {
            dto: {
                "reactivoid": reactivoID
            }
        }

        var dataString = $.toJSON(data);
        api.Join({ success: function (result) { processResultSuscribir(result); } }, dataString);
        
    }
}

function processResultSuscribir(data) {
    if (data.d.success == true) {
        $("#txtRedirect").val('');
        $("#option_" + data.d.reactivoid).html(getOption(data.d.reactivoid, true));
        $(hdnmessageinputid).val("Este reactivo se ha incluido en tu mochila.");
        $(hdnmessagetypeinputid).val('1');
        mostrarInfo();

    } else if (data.d.success == false) {
        $("#txtRedirect").val('');
        $(hdnmessageinputid).val($.toJSON(data.d.errors));
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    } else {
        $("#txtRedirect").val('../Default.aspx');
        $(hdnmessageinputid).val("Error inesperado. Intentalo de nuevo en unos minutos.");
        $(hdnmessagetypeinputid).val('1');
        mostrarError();
    }
}



function getOption(id, option) {
    if (option == true) {
        return "<div class=\"boton_link\"><input type=\"button\" value=\"¡Resolver!\" onclick=\"javascript:redirectReactivo('" + id + "');\" /></div>"
    } else {
        return "<div class=\"boton_link\"><input type=\"button\"  value=\"¡Suscribirse!\" onclick=\"javascript:suscribirse('" + id + "');\"/></div>";
    }
}

function redirectReactivo(id) {
    window.location = "../Reactivos/MuroReactivo.aspx?id=" + id;
}
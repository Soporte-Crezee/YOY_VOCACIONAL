
var apiConfirmacion = new ConfirmacionApi();

jQuery.validator.addMethod("whitespace", function (value, element) {

    var isValid = true;
    var largo = value.length;
    if (largo > 1) {

        var temp = value.indexOf(" ", 0); 
        if (temp != -1 )
            isValid = false;
    }
    else
        isValid = false;


    return this.optional(element) || isValid;
}, "No puede tener espacios en blanco");


$(function () {
    $(".boton").button();
    loadInformacion();
    $("#frmMain").validate();
});


function loadInformacion() {
    apiConfirmacion.GetInfoConfirmacion({ success: function (result) { onLoadInformacionComplete(result.d); } }, null);
}

function onLoadInformacionComplete(data) {

    $("#txtNombreUsuario").val(data.nombreusuario);
    $("#escuelas-tmpl").tmpl(data.escuelas).appendTo("#InformacionEscuelas");
    
}

function confirmar() {
    var bValid = isValidForm();

    if (bValid) {
        var nombreusuario = $("#txtNombreUsuario").val();

        var escuelas = [];
        var index = 0;
        $('input[id|="esc"]').each(function (index) {
            var escuelaid = $(this).val();
            var ccid = $("#hdn-cc-" + escuelaid).val();

            var numpcs = parseInt($("#numpc-" + escuelaid).val());
            var responsable = $("#responsablecc-" + escuelaid).val();
            var telefonocc = parseInt($("#telefonocc-" + escuelaid).val());
            var anchobanda = $("#anchobanda-" + escuelaid).val() != '' ? parseFloat($("#anchobanda-" + escuelaid).val()) : 0;
            var proveedor = $("#proveedor-" + escuelaid).val();
            var tipoc = $("#tipoc-" + escuelaid).val();

            var tienecc = $("#tienecc-" + escuelaid + "-si:checked").val() ? true : false;


            var tieneinet = $("#tieneinet-" + escuelaid + "-si:checked").val() ? true : false; ;


            var centroComputo = {
                "centrocomputoid": ccid != '' ? parseInt(ccid) : null,
                "tienecentro": tienecc,
                "tieneinternet": tieneinet,
                "anchobanda": anchobanda,
                "proveedor": proveedor,
                "tipocontrato": tipoc,
                "responsable": responsable,
                "numpcs": numpcs,
                "telefono": telefonocc
            };

            var escuela = {
                "escuelaid": escuelaid,
                "centrocomputo": centroComputo
            }
            escuelas[index] = escuela;
            index++;
        });


        var data = {
            dto: {
                "escuelas": escuelas,
                "nombreusuario": nombreusuario
            }
        }

        var dataString = $.toJSON(data);
        apiConfirmacion.ConfirmarInformacionUniversidad({ success: function (result) { onConfirmarUniversidadComplete(result); } }, dataString);
    }
}

function onConfirmarUniversidadComplete(data) {
    if (data.d.success != null && data.d.success) {
        window.location = data.d.urlredirect;
    } else if (data.d.errormsg != null) {
        $(hdnmessageinputid).val(data.d.errormsg);
        $(hdnmessagetypeinputid).val('1');
        mostrarError();

    }
}
function isValidForm() {
    return $("#frmMain").validate().form();
}

function mostrarCamposInet(escuelaid) {
    

    if ($("#anchobanda-" + escuelaid).val() == '') {
        $("#anchobanda-" + escuelaid).val(0)
    }

    $("#anchobanda-" + escuelaid).addClass("required number");
    $(".inet-" + escuelaid).show();
}

function ocultarCamposInet(escuelaid) {
    $(".inet-" + escuelaid).hide();
    $("#anchobanda-" + escuelaid).removeClass("required number");
}
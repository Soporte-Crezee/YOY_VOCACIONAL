/* File Created: agosto 24, 2012 */
$(document).ready(initPage);

function initPage() {
    $(".boton").button();

    $("#txtFechaContrato").datepicker({changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy' }).keydown(function () { return false; });
    $("#txtFechaInicio").datepicker({ changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy' }).keydown(function () { return false; });
    $("#txtFechaFinalizacion").datepicker({ changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy' }).keydown(function () { return false; });

    $("#frmMain").validate();
    
    $("#cbPais").rules('add', { required: true });
    $("#cbPais").rules('add', { min: 1, messages: { min: "Este campo es obligatorio."} });
    
    $("#cbEstado").rules('add', { required: true, min: 1 });
    $("#cbEstado").rules('add', { min: 1, messages: { min: "Este campo es obligatorio."} });

    $("#txtFechaContrato").rules('add', { required: true });

    $("#txtClave").rules('add', { required: true });
    $("#txtClave").rules('add', { maxlength: 10 });

    $("#txtFechaInicio").rules('add', { required: true });

    $("#txtFechaFinalizacion").rules('add', { required: true });

    $("#txtNumeroLicencias").rules('add', { maxlength: 9 });
    $("#txtNumeroLicencias").rules('add', { digits: true });

    $("#txtNombreCte").rules('add', { required: true });
    $("#txtNombreCte").rules('add', { maxlength: 100 });

    $("#txtDireccionCte").rules('add', { required: true });
    $("#txtDireccionCte").rules('add', { maxlength: 150 });

    $("#txtRepresentante").rules('add', { required: true });
    $("#txtRepresentante").rules('add', { maxlength: 100 });

    $("#txtTelefono").rules('add', { required: true });
    $("#txtTelefono").rules('add', { maxlength: 20 });

    $("#chkIlimitadas").click(function () {
        $("#txtNumeroLicencias").val("");
        if ($(this).is(":checked"))
            $("#txtNumeroLicencias").attr('disabled', 'disabled');
        else
            $("#txtNumeroLicencias").removeAttr('disabled');
    });

    $("#txtNumeroLicencias").keydown(function () { $("#chkIlimitadas").removeAttr("checked"); });
}
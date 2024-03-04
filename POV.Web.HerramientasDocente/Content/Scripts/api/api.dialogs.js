var MessageApi = function () { };

MessageApi.prototype.PrepareDialog
    = function () {
        $("#dialog:ui-dialog").dialog("destroy");
        $("#dialog-question").dialog({ autoOpen: false });
        $("#dialog").dialog({
            modal: true,
            autoOpen: false,
            width: 400,
            closeOnEscape: false,
            buttons: {
                Cerrar: function () {

                    $(this).dialog("close");


                }
            },
            close: function (event, ui) {
                if ($("#txtRedirect").length) {
                    if ($("#txtRedirect").val() != "")
                        location.href = $("#txtRedirect").val();
                }
            }
        });

    };
MessageApi.prototype.PrepareQuestionDialog = function (action) {

};
MessageApi.prototype.CreateMessage =
    function (message, type) {
        var band = 0;
        var icon = "";
        var classHeader = "modal-header ";
        switch (type) {
            case "ERROR":
                icon = "";
                classHeader += 'color-error';
                break;
            case "WARNING":
                icon = 'glyphicon glyphicon-warning-sign';
                classHeader += 'color-warning';
                break;
            case "INFO":
                icon = 'glyphicon glyphicon-info-sign';
                classHeader += 'color-info';
                break;
            case "OK":
                icon = 'glyphicon glyphicon-ok-sign';
                classHeader += 'color-success';
                break;
            default: band = 1; alert("Tipo de mensaje no soportado");
        }
        $("#modalHeader").addClass(classHeader);
        message = '<span class="' + icon + '"></span>&nbsp;' + message;
        $("#message").html(message);
        var btnCerrar = '<button type="button" class="btn btn-cancel" data-dismiss="modal" id="modalCloseButton">Cerrar</button>';
        $("#modalFooter").html(btnCerrar);
        return;
    };
MessageApi.prototype.CreateQuestionMessage =
        function (message, action) {
            var classHeader = "modal-header color-info";

            $("#modalHeader").addClass(classHeader);
            message = '<span class="glyphicon glyphicon-question-sign"></span>&nbsp;' + message;
            $("#message").html(message);
            var btns = '<button type="button" class="btn btn-primary" id="btnAceptarMensaje" ' + action + '>S&iacute;</button>' +
                       '<button type="button" class="btn btn-danger" data-dismiss="modal" id="btnRechazarMensaje" >No</button>';
            $("#modalFooter").html(btns);

            return;
        };
MessageApi.prototype.Show =
function () {
    $('#updPanelMaster').on('shown.bs.modal', function () {
        $('#modalCloseButton').focus()
    });
    $('#updPanelMaster').modal('show');
};
MessageApi.prototype.ShowQuestion =
function () {
    $('#updPanelMaster').on('shown.bs.modal', function () {
        $('#btnAceptarMensaje').focus()
    });
    $('#updPanelMaster').modal('show');
};


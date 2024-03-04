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
    function (message, type, redirect) {

        var band = 0;
        var icon = "";
        var classHeader = "modal-header ";
        switch (type) {
            case "ERROR":
                icon = "";
                classHeader += "color-error";
                break;
            case "WARNING":
                icon = "";
                classHeader += "color-warning";
                break;
            case "INFO":
                icon = "";
                classHeader += "color-info";
                break;
            case "INFO":
                icon = "";
                classHeader += "color-success";
            default: 
                band = 1; alert("Tipo de mensaje no soportado");
        }
        $("#modalHeader").attr('class', classHeader);
        message = '<span class="' + icon + '"></span>&nbsp;' + message;
        $("#message").html(message);
        var link = '';
        if (typeof redirect !== "undefined") {
            link = redirect;
        }

        var btnCerrar = '<button type="button" class="btn-cancel" data-link="' + link + '" data-dismiss="modal" id="modalCloseButton">Cerrar</button>';
        $("#modalFooter").html(btnCerrar);
        return;
    };
MessageApi.prototype.CreateQuestionMessage =
        function (message, action) {
            var classHeader = "modal-header color-info";

            $("#modalHeader").addClass(classHeader);
            message = '<span class="glyphicon glyphicon-question-sing"></span>&nbsp;' + message;
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
    $('#updPanelMaster').on('hide.bs.modal', function () {

        var link = $('#modalCloseButton').data('link');
        if (link != '') { window.location = link }
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


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
        switch (type) {
            case "ERROR": icon = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 50px 0;"></span>'; break;
            case "WARNING": icon = '<span class="ui-icon ui-icon-notice" style="float:left; margin:0 7px 50px 0;"></span>'; break;
            case "INFO": icon = '<span class="ui-icon ui-icon-info" style="float:left; margin:0 7px 50px 0;"></span>'; break;
            default: band = 1; alert("Tipo de mensaje no soportado");
        }
        var html = icon + message;
        $("#message").html(html);
        return;
    };
MessageApi.prototype.CreateQuestionMessage =
        function (message, action) {
            $("#dialog-question").dialog({ modal: true, autoOpen: false, width: 400, show: "drop",
                buttons: { "Sí": action,
                    "No": function () {
                        $(this).dialog("close");
                    }
                }
            });

            var icon = '<span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 50px 0;"></span>';

            var html = icon + message;
            $("#message_question").html(html);


            return;
        };
MessageApi.prototype.Show =
function () { $("#dialog").dialog('open'); };
MessageApi.prototype.ShowQuestion =
function () { $("#dialog-question").dialog('open'); };


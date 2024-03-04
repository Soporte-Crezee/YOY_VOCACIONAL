var AyudaApi = function (opciones) {
    this.myConfigs = $.extend({
        popup_container_id: '',
        url_contenido_popup: '',
        height: 500,
        width: 870,
        tituloPopup: 'Ayuda'
    }, opciones || {});

    this.initPopup = function () {
        var containerid = this.myConfigs.popup_container_id;
        var width = this.myConfigs.width;
        var height = this.myConfigs.height;
        var titulo = this.myConfigs.tituloPopup;
        $.get(this.myConfigs.url_contenido_popup, function (templates) {

            $("#" + containerid).html(templates);
            $("#" + containerid).dialog({
                modal: true,
                autoOpen: false,
                width: width,
                height: height,
                title: titulo,
                closeOnEscape: false,
                buttons: {
                    Cerrar: function () {

                        $(this).dialog("close");
                    }
                },
                close: function (event, ui) {

                }
            });
        });

    };

    this.showPopup = function () {
        $("#" + this.myConfigs.popup_container_id).dialog('open');
    };
};

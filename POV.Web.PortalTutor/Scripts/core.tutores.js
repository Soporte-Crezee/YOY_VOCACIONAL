var CoreTutores = {};

CoreTutores.ValidacionesTutor = {

    init: function (elements) {
        var self = this;
        this.elements = elements;
        self.bindEvents();

    },

    validateUsuario: function () {
        var self = CoreTutores.ValidacionesTutor;
        //e.preventDefault();

        var nombreusuario = $(self.elements.txtusername).val();

        //if (nombreusuario.length <= 0) {
        //    $(self.elements.btnguardar).unbind(e).click();
        //    return;
        //}
        var data = { dto: { nombreusuario: nombreusuario } };

        var datastring = $.toJSON(data);
        var apitutores = new TutoresApi();

        apitutores.ValidateTutor({
            success: function (result) {
                var id = $("#userimg").length;
                if (id > 0) {
                    $("#userimg").remove();
                }

                if (result != null && result.d != null && result.d.usuarioid != null) {                   
                    $(".username").append('<img src="/images/hr.gif" id="userimg" title="nombre de usuario no disponible" >');
                    alertify.set('notifier', 'position', 'bottom-left');
                    alertify.set('notifier', 'delay', 5);
                    alertify.error("El nombre de usuario está en uso");
                }
                else {
                    $(".username").append('<img src="/images/tick.png"  id="userimg" title="nombre de usuario disponible" >');
                }
            }
        }, datastring);
    },

    bindEvents: function () {
        var self = CoreTutores.ValidacionesTutor;

        $(self.elements.buttons).button();
        $(self.elements.btnguardar).on('click', function (e, sender) {

        });
    }
};
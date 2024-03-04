var CoreUsuarios = {};

CoreUsuarios.ValidacionesUsuario = {

    init: function (elements) {
        var self = this;
        this.elements = elements;
    },

    validateUsuario: function (key, activo) {       
        var self = CoreUsuarios.ValidacionesUsuario;    
        var data = {};
        var dto = {};

        var nombreusuario = $(self.elements.uiusername).val().trim();
        var nombreantusuario = $(self.elements.uiantusername).val().trim();
        var esactivo = activo;

        var text = "Dato no disponible";

        switch (key) {
            case 1:
                key = "usuario";
                text = "Nombre de usuario";
                if (nombreusuario.length <= 0) {
                    
                    return;
                }
                else{ 
                    dto.nombreusuario = nombreusuario;
                    dto.nombreantusuario = nombreantusuario;
                    dto.esactivo = esactivo;
                }
                break;
            case 4:
                if (esactivo.length <= 0) {
                    
                    return;
                }
                else dto.esactivo = esactivo;
                break;
            default:
                if (nombreusuario.length <= 0) {
                    
                    return;
                }
                else {
                    dto.nombreusuario = nombreusuario;
                    dto.nombreantusuario = nombreantusuario;
                    dto.esactivo = esactivo;
                }
                break;
        }

        data.dto = dto;

        var datastring = $.toJSON(data);
        var apiusuarios = new UsuariosApi();

        apiusuarios.ValidateUsuario({
            success: function (result) {
                self.userValid(key, result, text);
            }
        }, datastring);
    },

    userValid: function (key, result, text) {
        if ($("#" + key + "img").length > 0)
            $("#" + key + "img").remove();        

        if ($("#" + key + "imgError").length > 0) 
            $("#" + key + "imgError").remove();        

        if ($("#" + key.trim() + "error").length > 0)
            $("#" + key.trim() + "error").remove();
        if (result != null && result.d != null && result.d.usuarioid != null) {
            $("." + key).append('<img src="../images/hr.gif" id="' + key.trim() + 'imgError" title="El ' + text.toLowerCase().trim() + ' ya está en uso por otra cuenta" >');                
            var labelError =
                [
                    '<label class="error" id="' + key.trim() + 'error" for="MainContent_txt' + key.trim() + '" generated="true">',
                    text.trim() + ' no disponible',
                    '</label>'
                ].join('');
            $("." + key.trim() + "1 input").attr('class', 'error form-control');
            $("." + key.trim() + "1").append(labelError);
            $(".btn-info-valida").attr("disabled", true);
        }
        else {
            $("." + key.trim() + "1 input").removeClass('error')
            $("." + key).append('<img src="../images/tick.png"  id="' + key.trim() + 'img" title="El ' + text.toLowerCase().trim() + ' está disponible para está cuenta" >');
            $(".btn-info-valida").removeAttr("disabled");
        }
    },

    bindEvents: function () {
        var self = CoreUsuarios.ValidacionesUsuario;
        $(self.elements.buttons).button();
        $(self.elements.btnguardar).on('click', function (e, sender) {
        });
    }
};